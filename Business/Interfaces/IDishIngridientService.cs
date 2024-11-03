using Data.Models;

namespace Business.Interfaces;

public interface IDishIngridientService : ICrud<DishIngridient>
{
    Task<DishIngridient> GetByDishId(int id);
    Task<Ingridient> GetByName(string name);
}