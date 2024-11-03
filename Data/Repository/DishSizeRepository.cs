using Data.Data;
using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

public class DishSizeRepository : IDishSizeRepository
{
    private readonly AppDbContext _db;

    public DishSizeRepository(AppDbContext db)
    {
        _db = db;
    }


    public async Task<IEnumerable<DishSize>> GetAllAsync()
    {
        return await _db.DishSizes.ToListAsync();
    }

    public async Task<DishSize> GetByIdAsync(int id)
    {
        var size = await _db.DishSizes.FindAsync(id);
        if (size != null)
        {
            return size;
        }

        return null;
    }

    public async Task AddAsync(DishSize entity)
    {
        await _db.DishSizes.AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public void Update(DishSize entity)
    {
        _db.DishSizes.Update(entity);
        _db.SaveChanges();
    }

    public void Delete(DishSize entity)
    {
        _db.DishSizes.Remove(entity);
        _db.SaveChanges();
    }
    
    public async Task<DishSize> FindByName(string name)
    {
        var size = _db.DishSizes.FirstOrDefault(u => u.Size == name);
        if (size != null)
        {
            return size;
        }
        return null;
    }

    public async Task<DishSize> FindByPrice(decimal price)
    {
        var size = _db.DishSizes.FirstOrDefault(u => u.Price == price);
        if (size != null)
        {
            return size;
        }
        return null;
    }
}