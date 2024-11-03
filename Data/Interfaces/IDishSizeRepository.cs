using Data.Models;

namespace Data.Interfaces;

public interface IDishSizeRepository : IRepository<DishSize>
{
    Task<DishSize> FindByName(string name);
    Task<DishSize> FindByPrice(decimal price);
}