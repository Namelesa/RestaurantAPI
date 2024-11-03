using Data.Interfaces;
using Data.Repository;

namespace Data.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;

    public UnitOfWork(AppDbContext db)
    {
        _db = db;
    }
    
    private IDishRepository _dishRepository;
    public IDishRepository DishRepository
    {
        get
        {
            return _dishRepository ??= new DishRepository(_db);
        }
    }

    private IDishSizeRepository _dishSizeRepository;
    public IDishSizeRepository DishSizeRepository
    {
        get
        {
            return _dishSizeRepository ??= new DishSizeRepository(_db);
        }
    }
    
    private IDishIngridientRepository _dishIngridientRepository;
    public IDishIngridientRepository DishIngridientRepository
    {
        get
        {
            return _dishIngridientRepository ??= new DishIngridientRepository(_db);
        }
    }
    
    private IIngridientRepository _ingridientRepository;
    public IIngridientRepository IngridientRepository
    {
        get
        {
            return _ingridientRepository ??= new IngridientRepository(_db);
        }
    }
    
    private IOrderRepository _orderRepository;
    public IOrderRepository OrderRepository
    {
        get
        {
            return _orderRepository ??= new OrderRepository(_db);
        }
    }
    
    private IOrderDetailRepository _orderDetailRepository;
    public IOrderDetailRepository OrderDetailRepository {
        get
        {
            return _orderDetailRepository ??= new OrderDetailRepository(_db);
        }
    }
    
    public Task SaveAsync()
    { 
        return _db.SaveChangesAsync();
    }
}