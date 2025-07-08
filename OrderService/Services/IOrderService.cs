using OrderService.DTO;
using OrderService.Model;

namespace OrderService.Services;

public interface IOrderService
{
    Task<OrderModel> CreateOrderAsync(OrderCreateRequest request);
    Task<OrderModel?> GetOrderByIdAsync(string id);
    Task<OrderModel?> UpdateOrderStatusAsync(string id, OrderStatus newStatus);
}
