using Data.Models;

namespace Data.Interfaces;

public interface IIngridientRepository : IRepository<Ingridient>
{
    Task<Ingridient> GetByName(string name);
    Task<List<Ingridient>> GetIngridientsByDishId(int dishId);
    public Task RemoveIngredientFromDishAsync(int dishId, int ingredientId);
    public Task<List<DishIngridient>> GetDishIngridientByIdAsync(int ingrId);
}