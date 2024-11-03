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

    public OrderController(IOrderService orderService, IOrderDetailService orderDetailService)
    {
        _orderDetailService = orderDetailService;
        _orderService = orderService;
    }
// Get -----------------------------------------------------------------------------------------------

    [HttpGet]
    [Route("/GetAllOrders")]
    public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
    {
        var orders = await _orderService.GetAllAsync();
        return Ok(orders);
    }
    
    [HttpGet]
    [Route("/GetOrderById")]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrderById(int id)
    {
        var order = await _orderService.GetByIdAsync(id);
        return Ok(order);
    }
    
// Put -----------------------------------------------------------------------------------------------

    [HttpPut]
    [Route("/UpdateOrder")]
    public async Task<ActionResult> UpdateOrder([FromBody] OrderDto orderDto, int orderId)
    {
        var currentOrder = _orderService.GetByIdAsync(orderId).Result;
        if (currentOrder != null)
        {
            currentOrder.OrderDate = DateTime.UtcNow;
            currentOrder.Total = orderDto.Total;
        }
        await _orderService.UpdateAsync(currentOrder);
        return Ok("Update an order");
    }
    
    [HttpPut]
    [Route("/UpdateOrderDetail")]
    public async Task<ActionResult> UpdateOrderDetail(int orderDetailId, int dishId, int quantity)
    {
        var currentOrderDetail = _orderDetailService.GetByIdAsync(orderDetailId).Result;
        if (currentOrderDetail != null)
        {
            currentOrderDetail.Quantity = quantity;
            currentOrderDetail.DishId = dishId;
        }
        await _orderDetailService.UpdateAsync(currentOrderDetail);
        return Ok("Update an order detail");
    }
    

// Post ----------------------------------------------------------------------------------------------

    [HttpPost]
    [Route("/AddOrder")]
    public async Task<ActionResult> AddOrder([FromBody] OrderDto orderDto)
    {
        Order order = new Order()
        {
            OrderDate = DateTime.UtcNow,
            Total = orderDto.Total,
        };
        await _orderService.AddAsync(order);
        return Ok("Add a new order");
    }
    
    [HttpPost]
    [Route("/AddOrderDetail")]
    public async Task<ActionResult> AddOrderDetail([FromBody] OrderDetailDto orderDto)
    {
        try
        {
            OrderDetail order = new OrderDetail()
            {
                OrderId = orderDto.OrderId,
                Quantity = orderDto.Quantity,
                DishId = orderDto.DishId
            };
            await _orderDetailService.AddAsync(order);
            return Ok("Add a new order detail");
        }
        catch
        {
            return BadRequest("Problem in adding");
        }
    }
    
// Delete --------------------------------------------------------------------------------------------
    [HttpDelete]
    [Route("/DeleteOrder")]
    public async Task<ActionResult> DeleteOrder(int id)
    {
        try
        {
            var current = _orderService.GetByIdAsync(id).Result;
            var currentDetails = _orderDetailService.FindByOrderId(current.Id);

            if (current != null && currentDetails != null)
            {
                foreach (var det in currentDetails.Result)
                {
                    _orderDetailService.DeleteAsync(det);
                }

                _orderService.DeleteAsync(current);
            }

            return Ok("You delete an order");
        }
        catch
        {
            return BadRequest("You have a problem with Delete");
        }
    }
    
    [HttpDelete]
    [Route("/DeleteOrderDetail")]
    public async Task<ActionResult> DeleteOrderDetail(int detailId)
    {
        var currentDetails = _orderDetailService.GetByIdAsync(detailId).Result;
        if (currentDetails != null)
        {
            await _orderDetailService.DeleteAsync(currentDetails);
            return Ok("You delete an order detail");
        }
        else
        {
            return NotFound("No one OrderDetail with this Id");
        }
    }
}