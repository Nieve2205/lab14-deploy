using Lab13_parte2.Models;

namespace Lab13_parte2.Command;

public class CreateOrderCommand
{
    public int ClientId { get; set; }
    public DateTime OrderDate { get; set; }
    public List<Orderdetail> OrderDetails { get; set; }
}