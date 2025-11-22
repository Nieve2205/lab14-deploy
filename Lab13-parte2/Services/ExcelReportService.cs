using Lab13_parte2.Models;

namespace Lab13_parte2.Services;

using ClosedXML.Excel;

public class ExcelReportService
{
    private readonly IRepository<Order> _orderRepo;
    private readonly IRepository<Orderdetail> _orderDetailRepo;
    private readonly IRepository<Product> _productRepo;

    public ExcelReportService(IRepository<Order> orderRepo, IRepository<Orderdetail> orderDetailRepo, IRepository<Product> productRepo)
    {
        _orderRepo = orderRepo;
        _orderDetailRepo = orderDetailRepo;
        _productRepo = productRepo;
    }

    public async Task GenerateMonthlySalesReport(string filePath)
    {
        var orders = await _orderRepo.GetAllAsync();
        var orderDetails = await _orderDetailRepo.GetAllAsync();
        var products = await _productRepo.GetAllAsync();

        var workbook = new XLWorkbook();
        var worksheet = workbook.AddWorksheet("Ventas Mensuales");

        // Agregar encabezados
        worksheet.Cell(1, 1).Value = "Cliente";
        worksheet.Cell(1, 2).Value = "Producto";
        worksheet.Cell(1, 3).Value = "Cantidad";
        worksheet.Cell(1, 4).Value = "Precio Unitario";
        worksheet.Cell(1, 5).Value = "Total";

        var row = 2;

        foreach (var order in orders)
        {
            foreach (var detail in orderDetails.Where(d => d.Orderid == order.Orderid))
            {
                var product = products.First(p => p.Productid == detail.Productid);
                worksheet.Cell(row, 1).Value = order.Clientid; // Cliente
                worksheet.Cell(row, 2).Value = product.Name; // Producto
                worksheet.Cell(row, 3).Value = detail.Quantity; // Cantidad
                worksheet.Cell(row, 4).Value = product.Price; // Precio
                worksheet.Cell(row, 5).Value = detail.Quantity * product.Price; // Total
                row++;
            }
        }

        workbook.SaveAs(filePath);
    }
}
