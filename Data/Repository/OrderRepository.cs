using Data.Data;
using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _db;

    public OrderRepository(AppDbContext db)
    {
        _db = db;
    }


    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _db.Orders.ToListAsync();
    }

    public async Task<Order> GetByIdAsync(int id)
    {
        return await _db.Orders.FindAsync(id);
    }

    public async Task AddAsync(Order entity)
    {
        await _db.Orders.AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public void Update(Order entity)
    {
        _db.Orders.Update(entity);
        _db.SaveChanges();
    }

    public void Delete(Order entity)
    {
        _db.Orders.Remove(entity);
        _db.SaveChanges();
    }
}