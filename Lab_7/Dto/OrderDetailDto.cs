namespace Lab_7.Dto;

public class OrderDetailDto
{
    public int OrderId { get; set; }
    public int DishId { get; set; }
    public int Quantity { get; set; }

    public decimal Price { get; set; }
    
    public string Size { get; set; }
}