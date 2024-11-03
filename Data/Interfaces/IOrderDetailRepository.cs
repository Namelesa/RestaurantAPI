using Data.Models;

namespace Data.Interfaces;

public interface IOrderDetailRepository : IRepository<OrderDetail>
{
    Task<IEnumerable<OrderDetail>> FindByOrderId(int id);
}