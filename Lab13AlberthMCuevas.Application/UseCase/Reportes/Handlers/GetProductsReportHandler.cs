using Lab13AlberthMCuevas.Application.UseCase.Reportes.DTOs;
using Lab13AlberthMCuevas.Application.UseCase.Reportes.Queries;
using Lab13AlberthMCuevas.Domain.Ports;
using Lab13AlberthMCuevas.Infrastructure;
using MediatR;

namespace Lab13AlberthMCuevas.Application.UseCase.Reportes.Handlers;

public class GetProductsReportHandler : IRequestHandler<GetProductsReportQuery, byte[]>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExcelService _excelService;

    public GetProductsReportHandler(IUnitOfWork unitOfWork, IExcelService excelService)
    {
        _unitOfWork = unitOfWork;
        _excelService = excelService;
    }

    public async Task<byte[]> Handle(GetProductsReportQuery request, CancellationToken cancellationToken)
    {
        // Obtener datos de la base de datos
        var products = await _unitOfWork.Repository<Product>().GetAll();
        var orderDetails = await _unitOfWork.Repository<Orderdetail>().GetAll();

        // Transformar a DTOs con la lógica de negocio
        var reportData = products.Select(p => new ProductReportDto
        {
            ProductId = p.ProductId,
            Name = p.Name ?? string.Empty,
            Description = p.Description ?? string.Empty,
            Price = p.Price,
            TotalQuantity = orderDetails.Where(od => od.ProductId == p.ProductId).Sum(od => od.Quantity),
            TotalRevenue = orderDetails.Where(od => od.ProductId == p.ProductId).Sum(od => od.Quantity) * p.Price
        })
        .OrderByDescending(p => p.TotalQuantity)
        .ToList();

        // Generar archivo Excel
        var excelBytes = _excelService.GenerateProductsReport(reportData);

        return excelBytes;
    }
}
