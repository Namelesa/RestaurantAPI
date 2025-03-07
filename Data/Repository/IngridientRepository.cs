using Data.Data;
using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

public class IngridientRepository : IIngridientRepository
{
    private readonly AppDbContext _db;

    public IngridientRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Ingridient>> GetAllAsync()
    {
        return await _db.Ingridients.ToListAsync();
    }

    public async Task<Ingridient> GetByIdAsync(int id)
    {
        var ingridient = await _db.Ingridients.FindAsync(id);
        if (ingridient != null)
        {
            return ingridient;
        }

        return null;
    }

    public async Task AddAsync(Ingridient entity)
    {
        await _db.Ingridients.AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public void Update(Ingridient entity)
    {
        _db.Ingridients.Update(entity);
        _db.SaveChanges();
    }

    public void Delete(Ingridient entity)
    {
        _db.Ingridients.Remove(entity);
        _db.SaveChanges();
    }

    public async Task<Ingridient> GetByName(string name)
    {
        var ingridient = _db.Ingridients.FirstOrDefault(u => u.Name == name);
        return ingridient;
    }
    
    public async Task<List<Ingridient>> GetIngridientsByDishId(int dishId)
    {
        var ingridients = await _db.DishIngridients
            .Where(di => di.DishId == dishId)
            .Select(di => di.Ingridient)
            .ToListAsync();

        return ingridients;
    }
    
    public async Task RemoveIngredientFromDishAsync(int dishId, int ingredientId)
    {
        var dish = await _db.Dishes
            .Include(d => d.Ingridients)
            .FirstOrDefaultAsync(d => d.Id == dishId);

        if (dish != null)
        {
            var ingredientToRemove = dish.Ingridients.FirstOrDefault(i => i.Id == ingredientId);
            if (ingredientToRemove != null)
            {
                dish.Ingridients.Remove(ingredientToRemove);
                var dishingr = _db.DishIngridients.FirstOrDefault(u=> u.IngridientId == ingredientToRemove.Id);
                _db.DishIngridients.Remove(dishingr);
            }
        }

        await _db.SaveChangesAsync();
    }
    
    public async Task<List<DishIngridient>> GetDishIngridientByIdAsync(int ingrId)
    {
        var dishes = await _db.DishIngridients
            .Where(u => u.IngridientId == ingrId)
            .Include(u => u.Dish) 
            .ToListAsync();

        return dishes;
    }
}