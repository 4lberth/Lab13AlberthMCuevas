using ClosedXML.Excel;
using Lab13AlberthMCuevas.Domain.Ports;

namespace Lab13AlberthMCuevas.Infrastructure.Services;

public class ExcelService : IExcelService
{
    public byte[] GenerateClientsReport(IEnumerable<dynamic> data)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Clientes");

        // Encabezados
        worksheet.Cell(1, 1).Value = "Cliente ID";
        worksheet.Cell(1, 2).Value = "Nombre";
        worksheet.Cell(1, 3).Value = "Email";
        worksheet.Cell(1, 4).Value = "Total Órdenes";
        worksheet.Cell(1, 5).Value = "Total Gastado";

        // Estilo para encabezados
        var headerRange = worksheet.Range(1, 1, 1, 5);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;

        // Datos
        int row = 2;
        foreach (var item in data)
        {
            worksheet.Cell(row, 1).Value = item.ClientId;
            worksheet.Cell(row, 2).Value = item.Name;
            worksheet.Cell(row, 3).Value = item.Email;
            worksheet.Cell(row, 4).Value = item.TotalOrders;
            worksheet.Cell(row, 5).Value = item.TotalSpent;
            row++;
        }

        // Ajustar columnas
        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public byte[] GenerateProductsReport(IEnumerable<dynamic> data)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Productos");

        // Encabezados
        worksheet.Cell(1, 1).Value = "Producto ID";
        worksheet.Cell(1, 2).Value = "Nombre";
        worksheet.Cell(1, 3).Value = "Descripción";
        worksheet.Cell(1, 4).Value = "Precio";
        worksheet.Cell(1, 5).Value = "Cantidad Vendida";
        worksheet.Cell(1, 6).Value = "Total Ingresos";

        // Estilo para encabezados
        var headerRange = worksheet.Range(1, 1, 1, 6);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGreen;

        // Datos
        int row = 2;
        foreach (var item in data)
        {
            worksheet.Cell(row, 1).Value = item.ProductId;
            worksheet.Cell(row, 2).Value = item.Name;
            worksheet.Cell(row, 3).Value = item.Description;
            worksheet.Cell(row, 4).Value = item.Price;
            worksheet.Cell(row, 5).Value = item.TotalQuantity;
            worksheet.Cell(row, 6).Value = item.TotalRevenue;
            row++;
        }

        // Ajustar columnas
        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
