using Lab13AlberthMCuevas.Application.UseCase.Reportes.DTOs;
using Lab13AlberthMCuevas.Application.UseCase.Reportes.Queries;
using Lab13AlberthMCuevas.Domain.Ports;
using Lab13AlberthMCuevas.Infrastructure;
using MediatR;

namespace Lab13AlberthMCuevas.Application.UseCase.Reportes.Handlers;

public class GetClientsReportHandler : IRequestHandler<GetClientsReportQuery, byte[]>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExcelService _excelService;

    public GetClientsReportHandler(IUnitOfWork unitOfWork, IExcelService excelService)
    {
        _unitOfWork = unitOfWork;
        _excelService = excelService;
    }

    public async Task<byte[]> Handle(GetClientsReportQuery request, CancellationToken cancellationToken)
    {
        // Obtener datos de la base de datos
        var clients = await _unitOfWork.Repository<Client>().GetAll();
        var orders = await _unitOfWork.Repository<Order>().GetAll();
        var orderDetails = await _unitOfWork.Repository<Orderdetail>().GetAll();
        var products = await _unitOfWork.Repository<Product>().GetAll();

        // Transformar a DTOs con la lógica de negocio
        var reportData = clients.Select(c => new ClientReportDto
        {
            ClientId = c.ClientId,
            Name = c.Name ?? string.Empty,
            Email = c.Email ?? string.Empty,
            TotalOrders = orders.Count(o => o.ClientId == c.ClientId),
            TotalSpent = orders
                .Where(o => o.ClientId == c.ClientId)
                .SelectMany(o => orderDetails.Where(od => od.OrderId == o.OrderId))
                .Sum(od =>
                {
                    var product = products.FirstOrDefault(p => p.ProductId == od.ProductId);
                    return product != null ? od.Quantity * product.Price : 0;
                })
        }).ToList();

        // Generar archivo Excel
        var excelBytes = _excelService.GenerateClientsReport(reportData);

        return excelBytes;
    }
}
