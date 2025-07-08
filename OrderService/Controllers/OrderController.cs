using Microsoft.AspNetCore.Mvc;
using OrderService.DTO;
using OrderService.Model;
using OrderService.Services;

namespace OrderService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] OrderCreateRequest request)
    {
        var order = await _orderService.CreateOrderAsync(request);
        return order is null ? NotFound() : Ok(order);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        return order is null ? NotFound() : Ok(order);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(string id, [FromBody] OrderStatus newStatus)
    {
        var updatedOrder = await _orderService.UpdateOrderStatusAsync(id, newStatus);
        return updatedOrder is null ? NotFound() : Ok(updatedOrder);
    }
}
