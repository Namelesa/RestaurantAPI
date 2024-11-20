using System.Text.Json.Serialization;

namespace Data.Models;

public class OrderDetail : BaseModel
{
    public int OrderId { get; set; }
    [JsonIgnore]
    public Order Order { get; set; }

    public int DishId { get; set; }
    public Dish Dish { get; set; }

    public decimal Price { get; set; }

    public string Size { get; set; }
    
    public int Quantity { get; set; }
}