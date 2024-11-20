namespace Data.Models;

public class DishIngridient : BaseModel
{
    public int DishId { get; set; }
    public Dish Dish { get; set; }

    public int IngridientId { get; set; }
    public Ingridient Ingridient { get; set; }
}