using Data.Models;

namespace Business.Interfaces;

public interface IOrderDetailService : ICrud<OrderDetail>
{
    Task<IEnumerable<OrderDetail>> FindByOrderId (int id);
}