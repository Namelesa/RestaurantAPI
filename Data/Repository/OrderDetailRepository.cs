using Data.Data;
using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

public class OrderDetailRepository : IOrderDetailRepository
{
    private readonly AppDbContext _db;

    public OrderDetailRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<OrderDetail>> GetAllAsync()
    {
        return await _db.OrderDetails.ToListAsync();
    }

    public async Task<OrderDetail> GetByIdAsync(int id)
    {
        var details = await _db.OrderDetails.FindAsync(id);
        return details;
    }

    public async Task AddAsync(OrderDetail entity)
    {
        await _db.OrderDetails.AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public void Update(OrderDetail entity)
    {
        _db.OrderDetails.Update(entity);
        _db.SaveChangesAsync();
    }

    public void Delete(OrderDetail entity)
    {
        _db.OrderDetails.Remove(entity);
        _db.SaveChangesAsync();
    }

    public async Task<ICollection<OrderDetail>> FindByOrderId(int id)
    {
        return await _db.OrderDetails
            .Where(u => u.OrderId == id)
            .ToListAsync(); 
    }
}