namespace Data.Models;

public class DishSize : BaseModel
{
    public string Size { get; set; }
    public string? Image { get; set; }
    public decimal Price { get; set; } 
}