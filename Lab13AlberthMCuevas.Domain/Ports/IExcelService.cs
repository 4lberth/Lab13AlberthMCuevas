namespace Lab13AlberthMCuevas.Domain.Ports;

public interface IExcelService
{
    byte[] GenerateClientsReport(IEnumerable<dynamic> data);
    byte[] GenerateProductsReport(IEnumerable<dynamic> data);
}
