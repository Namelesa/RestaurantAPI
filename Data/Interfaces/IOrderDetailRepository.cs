using Data.Models;

namespace Data.Interfaces;

public interface IOrderDetailRepository : IRepository<OrderDetail>
{
    Task<ICollection<OrderDetail>> FindByOrderId(int id);
}