namespace Lab13AlberthMCuevas.Application.UseCase.Reportes.DTOs;

public class ClientReportDto
{
    public int ClientId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int TotalOrders { get; set; }
    public decimal TotalSpent { get; set; }
}
