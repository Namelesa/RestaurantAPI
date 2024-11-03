using Business.Interfaces;
using Data.Interfaces;
using Data.Models;

namespace Business.Services;

public class DishIngridientService : IDishIngridientService
{
    private readonly IUnitOfWork _unitOfWork;

    public DishIngridientService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    
    public async Task<IEnumerable<DishIngridient>> GetAllAsync()
    {
       return await _unitOfWork.DishIngridientRepository.GetAllAsync();
    }

    public Task<DishIngridient> GetByIdAsync(int id)
    {
        return _unitOfWork.DishIngridientRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(DishIngridient model)
    {
        await _unitOfWork.DishIngridientRepository.AddAsync(model);
        await _unitOfWork.SaveAsync();
    }

    public async Task UpdateAsync(DishIngridient model)
    { 
        _unitOfWork.DishIngridientRepository.Update(model);
    }

    public async Task DeleteAsync(DishIngridient model)
    {
        _unitOfWork.DishIngridientRepository.Delete(model);
    }

    public Task<DishIngridient> GetByDishId(int id)
    {
        return _unitOfWork.DishIngridientRepository.GetByDishIdAsync(id);
    }

    public async Task<Ingridient> GetByName(string name)
    {
        var ingridient = _unitOfWork.IngridientRepository.GetByName(name).Result;
        return ingridient;
    }
}