namespace Lab13AlberthMCuevas.Application.UseCase.Reportes.DTOs;

public class ProductReportDto
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int TotalQuantity { get; set; }
    public decimal TotalRevenue { get; set; }
}
