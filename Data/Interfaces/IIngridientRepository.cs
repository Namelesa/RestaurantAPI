using Data.Models;

namespace Data.Interfaces;

public interface IIngridientRepository : IRepository<Ingridient>
{
    Task<Ingridient> GetByName(string name);
    Task<List<Ingridient>> GetIngridientsByDishId(int dishId);
}