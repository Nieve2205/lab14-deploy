using Lab13_parte2;
using Lab13_parte2.Models;
using Lab13_parte2.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddScoped<IRepository<Order>, Repository<Order>>();
builder.Services.AddScoped<IRepository<Orderdetail>, Repository<Orderdetail>>();
builder.Services.AddScoped<IRepository<Product>, Repository<Product>>();

// Register services
builder.Services.AddScoped<ExcelReportService>();

// Add controllers and Swagger for API documentation
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Build and run the app
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty; // This will serve the Swagger UI at the root
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();