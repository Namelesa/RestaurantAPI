using Business.Interfaces;
using Data.Interfaces;
using Data.Models;

namespace Business.Services;

public class IngridientService : IIngridientService
{
    private readonly IUnitOfWork _unitOfWork;

    public IngridientService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<IEnumerable<Ingridient>> GetAllAsync()
    {
        return await _unitOfWork.IngridientRepository.GetAllAsync();
    }

    public Task<Ingridient> GetByIdAsync(int id)
    {
        return _unitOfWork.IngridientRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(Ingridient model)
    {
        await _unitOfWork.IngridientRepository.AddAsync(model);
        await _unitOfWork.SaveAsync();
    }

    public async Task UpdateAsync(Ingridient model)
    {
        _unitOfWork.IngridientRepository.Update(model);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(Ingridient model)
    {
        _unitOfWork.IngridientRepository.Delete(model);
        await _unitOfWork.SaveAsync();
    }

    public Task<Ingridient> FindByName(string name)
    {
        return _unitOfWork.IngridientRepository.GetByName(name);
    }
}