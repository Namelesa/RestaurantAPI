using Business.Interfaces;
using Data.Interfaces;
using Data.Models;

namespace Business.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _unitOfWork.OrderRepository.GetAllAsync();
    }

    public Task<Order> GetByIdAsync(int id)
    {
        return _unitOfWork.OrderRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(Order model)
    {
        await _unitOfWork.OrderRepository.AddAsync(model);
        await _unitOfWork.SaveAsync();
    }

    public async Task UpdateAsync(Order model)
    {
        _unitOfWork.OrderRepository.Update(model);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(Order model)
    {
        _unitOfWork.OrderRepository.Delete(model);
        await _unitOfWork.SaveAsync();
    }
}