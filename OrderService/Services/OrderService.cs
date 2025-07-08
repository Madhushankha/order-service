using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.DTO;
using OrderService.Model;

namespace OrderService.Services;

public class OrderService : IOrderService
{
    private readonly OrderDbContext _context;

    public OrderService(OrderDbContext context)
    {
        _context = context;
    }

    public async Task<OrderModel> CreateOrderAsync(OrderCreateRequest request)
    {
        var order = new OrderModel
        {
            Id = Guid.NewGuid().ToString(),
            CustomerId = request.CustomerId.ToString(),
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            Items = request.Items.Select(i => new OrderItemModel
            {
                Id = Guid.NewGuid().ToString(),
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                UnitPrice = i.UnitPrice,
                Quantity = i.Quantity
            }).ToList()
        };

        order.TotalPrice = order.Items.Sum(x => x.UnitPrice * x.Quantity);

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return order;
    }

    public async Task<OrderModel?> GetOrderByIdAsync(string id)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<OrderModel?> UpdateOrderStatusAsync(string id, OrderStatus newStatus)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null) return null;

        order.Status = newStatus;
        await _context.SaveChangesAsync();

        return order;
    }
}
