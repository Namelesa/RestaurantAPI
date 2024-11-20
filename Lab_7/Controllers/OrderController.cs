using Business.Interfaces;
using Data.Models;
using Lab_7.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Lab_7.Controllers;

[Route("api/[controller]")]
public class OrderController(
    IOrderService orderService,
    IOrderDetailService orderDetailService,
    IDishService dishService,
    IDishSizeService dishSizeService)
    : ControllerBase
{
    // Get -----------------------------------------------------------------------------------------------

    [HttpGet("GetAllOrders")]
    public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
    {
        var orders = await orderService.GetAllAsync();
        return Ok(orders);
    }
    
    [HttpGet("GetOrderById")]
    public async Task<ActionResult<Order>> GetOrderById(int id)
    {
        var order = await orderService.GetByIdAsync(id);
        if (order == null)
        {
            return NotFound("Order not found");
        }

        var orderDetails = await orderDetailService.FindByOrderId(id);

        foreach (var detail in orderDetails)
        {
            detail.Dish = await dishService.GetAllInfo(detail.DishId);
        }
        
        order.OrderDetails = orderDetails;
        
        return Ok(order);
    }

    [HttpGet("GetOrderDetailsById")]
    public async Task<ActionResult<OrderDto>> GetOrderDetailsById(int orderId)
    {
        var order = await orderService.GetByIdAsync(orderId);
        if (order == null)
        {
            return NotFound("Order not found");
        }

        var orderDetails = await orderDetailService.FindByOrderId(orderId);
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
    [HttpPut("UpdateOrderDetail")]
    public async Task<ActionResult> UpdateOrderDetail(int orderDetailId, int? dishId = null, int? quantity = null, string? size = null)
    {
        var currentOrderDetail = await orderDetailService.GetByIdAsync(orderDetailId);
        if (currentOrderDetail == null)
            return NotFound("Order detail not found");

        var dish = currentOrderDetail.Dish ?? await dishService.GetByIdAsync(currentOrderDetail.DishId);
        decimal baseDishPrice = dish?.Price ?? 0;
        decimal sizePrice = 0;

        if (dishId.HasValue && dishId != currentOrderDetail.DishId)
        {
            dish = await dishService.GetByIdAsync(dishId.Value);
            if (dish == null)
                return NotFound("Dish not found");

            currentOrderDetail.DishId = dishId.Value;
            baseDishPrice = dish.Price;
        }

        if (!string.IsNullOrEmpty(size) && size != currentOrderDetail.Size)
        {
            var dishSize = await dishSizeService.FindByName(size);
            if (dishSize == null)
                return BadRequest("Invalid size for the selected dish");

            currentOrderDetail.Size = size;
            sizePrice = dishSize.Price;
        }
        else if (!string.IsNullOrEmpty(currentOrderDetail.Size))
        {
            var currentSize = await dishSizeService.FindByName(currentOrderDetail.Size);
            sizePrice = currentSize?.Price ?? 0;
        }

        if (quantity.HasValue && quantity > 0)
            currentOrderDetail.Quantity = quantity.Value;

        currentOrderDetail.Price = (baseDishPrice + sizePrice) * currentOrderDetail.Quantity;
        
        var currentOrder = await orderService.GetByIdAsync(currentOrderDetail.OrderId);
        if (currentOrder == null)
        {
            return NotFound("Order not found");
        }

        var test = currentOrder.Total; 
        var total = await orderDetailService.FindByOrderId(currentOrderDetail.OrderId);
        foreach (var item in total)
        {
            currentOrder.Total += item.Price;
        }
        
        currentOrder.OrderDate = DateTime.UtcNow;
        currentOrder.Total -= test;

        Task.WhenAll(
            orderService.UpdateAsync(currentOrder),
            orderDetailService.UpdateAsync(currentOrderDetail)
        );

        return Ok(new{message = "Order detail updated successfully"});
    }

    [HttpPut("AddDishToOrder")]
    public async Task<ActionResult> AddDishToOrder(int orderId, int dishId, int quantity, string size)
    {
        if (orderId == null || dishId == null || quantity == null || quantity <= 0 || size == null)
        {
            return BadRequest(new{ message = "You can`t add item with null params"});
        }

        var currentOrder = await orderService.GetByIdAsync(orderId);
        if (currentOrder == null)
        {
            return BadRequest(new{ message = "You don`t have order with this id"});
        }

        var currentDish = await dishService.GetByIdAsync(dishId);
        if (currentDish == null)
        {
            return BadRequest(new{ message = "You don`t have dish with this id"});
        }
        
        var currentDishSize = await dishSizeService.FindByName(size);
        if (currentDishSize == null)
        {
            return BadRequest(new{ message = "You don`t have size with this name"});
        }

        decimal price = (currentDish.Price + currentDishSize.Price) * quantity;
        OrderDetail newOrderDish = new OrderDetail()
        {
            OrderId = orderId,
            Order = currentOrder,
            DishId = dishId,
            Dish = currentDish,
            Price = price,
            Size = currentDishSize.Size,
            Quantity = quantity
        };
        
        await orderDetailService.AddAsync(newOrderDish);
        
        return Ok(new
        {
            message = "Dish successfully added to order.",
            orderDetail = new
            {
                newOrderDish.Id,
                newOrderDish.OrderId,
                newOrderDish.DishId,
                newOrderDish.Price,
                newOrderDish.Quantity,
                newOrderDish.Size
            }
        });
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
        await orderService.AddAsync(order);
        return Ok(new {id = order.Id});
    }
    
    [HttpPost("AddOrderDetails")]
    public async Task<ActionResult> AddOrderDetails([FromBody] List<OrderDetailDto> orderDetailsDto)
    {
        try
        {
            foreach (var orderDetailDto in orderDetailsDto)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = orderDetailDto.OrderId,
                    Quantity = orderDetailDto.Quantity,
                    DishId = orderDetailDto.DishId,
                    Price = orderDetailDto.Price,
                    Size = orderDetailDto.Size
                    
                };
                await orderDetailService.AddAsync(orderDetail);
            }

            return Ok(new { message = "Order details added successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"Problem in adding order details: {ex.Message}" });
        }
    }


    // Delete --------------------------------------------------------------------------------------------
    
    [HttpDelete("DeleteOrder")]
    public async Task<ActionResult> DeleteOrder(int id)
    {
        var currentOrder = await orderService.GetByIdAsync(id);
        if (currentOrder == null)
        {
            return NotFound("Order not found");
        }

        var currentDetails =  orderDetailService.FindByOrderId(id).Result;

        foreach (var detail in currentDetails)
        {
            orderDetailService.DeleteAsync(detail);
        }

        orderService.DeleteAsync(currentOrder);
        return Ok(new{message = "Order deleted successfully"});
    }
    
    [HttpDelete("DeleteOrderDetail")]
    public async Task<ActionResult> DeleteOrderDetail(int detailId)
    {
        var currentDetails = await orderDetailService.GetByIdAsync(detailId);
        if (currentDetails == null)
        {
            return NotFound("Order detail not found");
        }

        orderDetailService.DeleteAsync(currentDetails);
        return Ok(new { message = "Order detail deleted successfully"});
    }
}
