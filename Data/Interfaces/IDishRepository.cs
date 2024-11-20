using Data.Models;

namespace Data.Interfaces;

public interface IDishRepository : IRepository<Dish>
{
    public Task AddIngridientsToDish(List<Ingridient> ingridientNames, Dish dish);
}