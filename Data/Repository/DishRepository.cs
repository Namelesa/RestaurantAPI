using Data.Data;
using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

public class DishRepository : IDishRepository
{
    private readonly AppDbContext _db;
    public DishRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Dish>> GetAllAsync()
    {
        return await _db.Dishes.ToListAsync();
    }

    public Task<Dish> GetByIdAsync(int id)
    {
        var dish = _db.Dishes.FindAsync(id).Result;
        if (dish != null)
        {
            return Task.FromResult(dish);
        }

        return Task.FromResult<Dish>(null);
    }

    public async Task AddAsync(Dish entity)
    {
        _db.Dishes.Add(entity); 
        _db.SaveChanges();
    }

    public void Update(Dish entity)
    {
        _db.Dishes.Update(entity);
        _db.SaveChanges();
    }

    public void Delete(Dish entity)
    {
        _db.Dishes.Remove(entity);
        _db.SaveChanges();
    }

    public async Task AddIngridientsToDish(List<Ingridient> ingridientNames, Dish dish)
    {
        dish.Ingridients.AddRange(ingridientNames);
    }
    
}