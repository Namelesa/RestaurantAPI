namespace Data.Models;

public class Dish : BaseModel
{
    public string Name { get; set; }
    public string? Image { get; set; }
    
    public List<Ingridient> Ingridients { get; set; }

    public int DishSizeId { get; set; }

    public virtual DishSize DishSize { get; set; }

    public decimal Price { get; set; }
    
}