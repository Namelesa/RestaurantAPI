using Data.Models;

namespace Lab_7.Dto;

public class OrderDto
{
    public decimal Total { get; set; }
    public List<OrderDetailDto> OrderDetails { get; set; }
}