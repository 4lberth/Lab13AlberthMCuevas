using MediatR;

namespace Lab13AlberthMCuevas.Application.UseCase.Reportes.Queries;

public class GetClientsReportQuery : IRequest<byte[]>
{
    // Esta query no necesita parámetros, devuelve el reporte completo
}
