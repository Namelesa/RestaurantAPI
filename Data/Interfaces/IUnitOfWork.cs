namespace Data.Interfaces;

public interface IUnitOfWork
{
    public IDishRepository DishRepository { get; }
    public IIngridientRepository IngridientRepository { get; }
    public IOrderRepository OrderRepository { get; }
    public IOrderDetailRepository OrderDetailRepository { get; }
    public IDishSizeRepository DishSizeRepository { get; }
    Task SaveAsync();
}