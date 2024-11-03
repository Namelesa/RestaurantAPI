using Data.Models;

namespace Data.Interfaces;

public interface IDishIngridientRepository : IRepository<DishIngridient>
{
    Task<DishIngridient> GetByDishIdAsync(int id);
}