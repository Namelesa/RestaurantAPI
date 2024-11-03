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
}