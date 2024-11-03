using Business.Interfaces;
using Data.Interfaces;
using Data.Models;

namespace Business.Services;

public class DishSizeService : IDishSizeService
{
    private readonly IUnitOfWork _unitOfWork;

    public DishSizeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<DishSize>> GetAllAsync()
    {
        var sizes = await _unitOfWork.DishSizeRepository.GetAllAsync();
        return sizes;
    }

    public Task<DishSize> GetByIdAsync(int id)
    {
        return _unitOfWork.DishSizeRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(DishSize model)
    {
        await _unitOfWork.DishSizeRepository.AddAsync(model);
        await _unitOfWork.SaveAsync();
    }

    public async Task UpdateAsync(DishSize model)
    {
        _unitOfWork.DishSizeRepository.Update(model);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(DishSize model)
    {
        _unitOfWork.DishSizeRepository.Delete(model);
        await _unitOfWork.SaveAsync();
    }
    
    public async Task<DishSize> FindByName(string name)
    {
        var result = _unitOfWork.DishSizeRepository.FindByName(name).Result;
        return result;
    }
    
    public async Task<DishSize> FindByPrice(decimal price)
    {
        var result = _unitOfWork.DishSizeRepository.FindByPrice(price).Result;
        return result;
    }
    
    
}