using Data.Data;
using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

public class DishIngridientRepository : IDishIngridientRepository
{
    private readonly AppDbContext _db;

    public DishIngridientRepository(AppDbContext db)
    {
        _db = db;
    }
    
    public async Task<IEnumerable<DishIngridient>> GetAllAsync()
    {
        return await _db.DishIngridients.ToListAsync();
    }

    public async Task<DishIngridient> GetByIdAsync(int id)
    {
        var dishIngridient = _db.DishIngridients.FindAsync(id).Result;
        if (dishIngridient != null)
        {
            return dishIngridient;
        }

        return null;
    }

    public async Task<DishIngridient> GetByDishIdAsync(int id)
    {
        var dishIngr = _db.DishIngridients.FirstOrDefault(u => u.DishId == id);
        if (dishIngr != null)
        {
            return dishIngr;
        }

        return null;
    }
    
    public async Task AddAsync(DishIngridient entity)
    {
        await _db.DishIngridients.AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public void Update(DishIngridient entity)
    {
        _db.DishIngridients.Update(entity);
        _db.SaveChanges();
    }

    public void Delete(DishIngridient entity)
    {
        _db.DishIngridients.Remove(entity);
        _db.SaveChanges();
    }
    
    
}