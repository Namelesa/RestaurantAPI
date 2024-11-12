using Data.Models;

namespace Business.Interfaces;

public interface IOrderDetailService : ICrud<OrderDetail>
{
    Task<ICollection<OrderDetail>> FindByOrderId (int id);
}