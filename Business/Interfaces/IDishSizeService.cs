using Data.Models;

namespace Business.Interfaces;

public interface IDishSizeService : ICrud<DishSize>
{
    Task<DishSize> FindByName(string name);
    Task<DishSize> FindByPrice(decimal price);
}