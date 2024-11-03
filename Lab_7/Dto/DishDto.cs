namespace Lab_7.Dto;

public class DishDto
{
    public string Name { get; set; }
    public string Size { get; set; }
    public decimal Price { get; set; }
    public IList<string>? DishIngridientsNames { get; set; }
    
}