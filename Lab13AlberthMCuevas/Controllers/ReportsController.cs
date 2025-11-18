using Lab13AlberthMCuevas.Application.UseCase.Reportes.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lab13AlberthMCuevas.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Genera reporte de clientes con total de órdenes y monto gastado
    /// </summary>
    [HttpGet("clients")]
    public async Task<IActionResult> GetClientsReport()
    {
        var query = new GetClientsReportQuery();
        var excelBytes = await _mediator.Send(query);

        return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteClientes.xlsx");
    }

    /// <summary>
    /// Genera reporte de productos más vendidos con cantidad y revenue
    /// </summary>
    [HttpGet("products")]
    public async Task<IActionResult> GetProductsReport()
    {
        var query = new GetProductsReportQuery();
        var excelBytes = await _mediator.Send(query);

        return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteProductos.xlsx");
    }
}
