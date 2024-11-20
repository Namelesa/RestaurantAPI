namespace Data.Models;

public class Ingridient : BaseModel
{
    public string Name { get; set; }
    public string? Image { get; set; }
    
    public IList<Dish> Dishes { get; set; }
    
}