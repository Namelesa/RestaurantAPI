using Business.Interfaces;
using Data.Interfaces;
using Data.Models;

namespace Business.Services;

public class OrderDetailService : IOrderDetailService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderDetailService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<OrderDetail>> GetAllAsync()
    {
        return await _unitOfWork.OrderDetailRepository.GetAllAsync();
    }

    public Task<OrderDetail> GetByIdAsync(int id)
    {
        return _unitOfWork.OrderDetailRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(OrderDetail model)
    {
        await _unitOfWork.OrderDetailRepository.AddAsync(model);
        await _unitOfWork.SaveAsync();
    }

    public async Task UpdateAsync(OrderDetail model)
    {
        _unitOfWork.OrderDetailRepository.Update(model);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(OrderDetail model)
    {
        _unitOfWork.OrderDetailRepository.Delete(model);
        await _unitOfWork.SaveAsync();
    }

    public Task<ICollection<OrderDetail>> FindByOrderId(int id)
    {
        return _unitOfWork.OrderDetailRepository.FindByOrderId(id);
    }
}