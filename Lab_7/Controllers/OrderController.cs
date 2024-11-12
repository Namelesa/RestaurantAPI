using Business.Interfaces;
using Data.Models;
using Lab_7.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Lab_7.Controllers;

[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IOrderDetailService _orderDetailService;
    private readonly IDishService _dishService;

    public OrderController(IOrderService orderService, IOrderDetailService orderDetailService, IDishService dishService)
    {
        _orderService = orderService;
        _orderDetailService = orderDetailService;
        _dishService = dishService;
    }

    // Get -----------------------------------------------------------------------------------------------

    [HttpGet("GetAllOrders")]
    public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
    {
        var orders = await _orderService.GetAllAsync();
        return Ok(orders);
    }
    
    [HttpGet("GetOrderById")]
    public async Task<ActionResult<Order>> GetOrderById(int id)
    {
        var order = await _orderService.GetByIdAsync(id);
        if (order == null)
        {
            return NotFound("Order not found");
        }

        var orderDetails = await _orderDetailService.FindByOrderId(id);

        foreach (var detail in orderDetails)
        {
            detail.Dish = await _dishService.GetAllInfo(detail.DishId);
        }
        
        order.OrderDetails = orderDetails;
        
        return Ok(order);
    }

    [HttpGet("GetOrderDetailsById")]
    public async Task<ActionResult<OrderDto>> GetOrderDetailsById(int orderId)
    {
        var order = await _orderService.GetByIdAsync(orderId);
        if (order == null)
        {
            return NotFound("Order not found");
        }

        var orderDetails = await _orderDetailService.FindByOrderId(orderId);
        var orderDetailsDto = orderDetails.Select(od => new OrderDetailDto
        {
            OrderId = od.OrderId,
            DishId = od.DishId,
            Quantity = od.Quantity
        }).ToList();

        var orderDto = new OrderDto
        {
            Total = order.Total,
            OrderDetails = orderDetailsDto
        };

        return Ok(orderDto);
    }

    // Put -----------------------------------------------------------------------------------------------

    [HttpPut("UpdateOrder")]
    public async Task<ActionResult> UpdateOrder([FromBody] OrderDto orderDto, int orderId)
    {
        var currentOrder = await _orderService.GetByIdAsync(orderId);
        if (currentOrder == null)
        {
            return NotFound("Order not found");
        }

        currentOrder.OrderDate = DateTime.UtcNow;
        currentOrder.Total = orderDto.Total;

        await _orderService.UpdateAsync(currentOrder);
        return Ok("Order updated successfully");
    }
    
    [HttpPut("UpdateOrderDetail")]
    public async Task<ActionResult> UpdateOrderDetail(int orderDetailId, int dishId, int quantity)
    {
        var currentOrderDetail = await _orderDetailService.GetByIdAsync(orderDetailId);
        if (currentOrderDetail == null)
        {
            return NotFound("Order detail not found");
        }

        currentOrderDetail.Quantity = quantity;
        currentOrderDetail.DishId = dishId;

        await _orderDetailService.UpdateAsync(currentOrderDetail);
        return Ok("Order detail updated successfully");
    }

    // Post ----------------------------------------------------------------------------------------------

    [HttpPost("AddOrder")]
    public async Task<ActionResult> AddOrder([FromBody] OrderDto orderDto)
    {
        var order = new Order
        {
            OrderDate = DateTime.UtcNow,
            Total = orderDto.Total,
        };
        await _orderService.AddAsync(order);
        return Ok("New order added successfully");
    }
    
    [HttpPost("AddOrderDetail")]
    public async Task<ActionResult> AddOrderDetail([FromBody] OrderDetailDto orderDto)
    {
        try
        {
            var orderDetail = new OrderDetail
            {
                OrderId = orderDto.OrderId,
                Quantity = orderDto.Quantity,
                DishId = orderDto.DishId
            };
            await _orderDetailService.AddAsync(orderDetail);
            return Ok("New order detail added successfully");
        }
        catch (Exception ex)
        {
            return BadRequest($"Problem in adding order detail: {ex.Message}");
        }
    }

    // Delete --------------------------------------------------------------------------------------------
    
    [HttpDelete("DeleteOrder")]
    public async Task<ActionResult> DeleteOrder(int id)
    {
        var currentOrder = await _orderService.GetByIdAsync(id);
        if (currentOrder == null)
        {
            return NotFound("Order not found");
        }

        var currentDetails = await _orderDetailService.FindByOrderId(id);

        foreach (var detail in currentDetails)
        {
            await _orderDetailService.DeleteAsync(detail);
        }

        await _orderService.DeleteAsync(currentOrder);
        return Ok("Order deleted successfully");
    }
    
    [HttpDelete("DeleteOrderDetail")]
    public async Task<ActionResult> DeleteOrderDetail(int detailId)
    {
        var currentDetails = await _orderDetailService.GetByIdAsync(detailId);
        if (currentDetails == null)
        {
            return NotFound("Order detail not found");
        }

        await _orderDetailService.DeleteAsync(currentDetails);
        return Ok("Order detail deleted successfully");
    }
}
