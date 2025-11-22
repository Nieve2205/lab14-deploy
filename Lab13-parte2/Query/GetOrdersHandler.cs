using Lab13_parte2.Models;

namespace Lab13_parte2.Query;

public class GetOrdersHandler
{
    private readonly IRepository<Order> _orderRepo;

    public GetOrdersHandler(IRepository<Order> orderRepo)
    {
        _orderRepo = orderRepo;
    }

    public async Task<IEnumerable<Order>> Handle(GetOrdersQuery query)
    {
        return await _orderRepo.GetAllAsync(); // Puedes agregar filtros aquí
    }
}