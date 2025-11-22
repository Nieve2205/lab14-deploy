using Lab13_parte2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab13_parte2.Services;  // Si estás usando el servicio ExcelReportService

namespace Lab13_parte2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ExcelReportService _excelReportService;  // Si usas este servicio

        // Inyección de AppDbContext en el constructor
        public ReportsController(AppDbContext db, ExcelReportService excelReportService)
        {
            _db = db;
            _excelReportService = excelReportService;
        }

        // GET: api/reports/ventas-mensuales
        [HttpGet("ventas-mensuales")]
        public async Task<IActionResult> GetVentasMensuales()
        {
            var data = await _db.Orders
                .Join(_db.Orderdetails, o => o.Orderid, d => d.Orderid, (o, d) => new { o, d })
                .Join(_db.Products, od => od.d.Productid, p => p.Productid, (od, p) => new { od.o.Orderdate, od.d.Quantity, p.Price })
                .ToListAsync();  // Traer los datos a memoria

            var result = data
                .GroupBy(x => x.Orderdate.ToString("yyyy-MM"))
                .Select(g => new {
                    Mes = g.Key,
                    Monto = g.Sum(x => x.Quantity * x.Price)
                })
                .OrderBy(x => x.Mes)
                .ToList();

            return Ok(result);
        }

        // GET: api/reports/excel/ventas-mensuales -> genera Excel con ClosedXML
        [HttpGet("excel/ventas-mensuales")]
        public async Task<IActionResult> GetExcelVentasMensuales()
        {
            // Define a path in the application's root directory
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "VentasMensuales.xlsx");

            // Ensure the directory exists before generating the file
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Generate the report
            await _excelReportService.GenerateMonthlySalesReport(filePath);

            // Return the generated file
            return PhysicalFile(filePath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "VentasMensuales.xlsx");
        }

    }
}
