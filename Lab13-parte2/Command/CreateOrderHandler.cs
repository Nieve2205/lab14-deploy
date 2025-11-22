using Lab13_parte2.Models;

namespace Lab13_parte2.Command;

public class CreateOrderHandler
{
    private readonly IRepository<Order> _orderRepo;
    private readonly IRepository<Orderdetail> _orderDetailRepo;

    public CreateOrderHandler(IRepository<Order> orderRepo, IRepository<Orderdetail> orderDetailRepo)
    {
        _orderRepo = orderRepo;
        _orderDetailRepo = orderDetailRepo;
    }

    public async Task Handle(CreateOrderCommand command)
    {
        var order = new Order { Clientid = command.ClientId, Orderdate = command.OrderDate };
        await _orderRepo.AddAsync(order);
        foreach (var detail in command.OrderDetails)
        {
            detail.Orderid = order.Orderid;
            await _orderDetailRepo.AddAsync(detail);
        }
        // Guardar cambios utilizando el repositorio
        await _orderRepo.SaveChangesAsync(); 
    }
}
