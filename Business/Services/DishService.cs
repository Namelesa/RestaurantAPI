using Business.Interfaces;
using Data.Data;
using Data.Interfaces;
using Data.Models;

namespace Business.Services;

public class DishService : IDishService
{
    private readonly IUnitOfWork _unitOfWork;

    public DishService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<Dish>> GetAllAsync()
    {
        return await _unitOfWork.DishRepository.GetAllAsync();
    }

    public Task<Dish> GetByIdAsync(int id)
    {
        return _unitOfWork.DishRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(Dish model)
    {
        await _unitOfWork.DishRepository.AddAsync(model);
        await _unitOfWork.SaveAsync();
    }

    public async Task UpdateAsync(Dish model)
    {
        _unitOfWork.DishRepository.Update(model);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(Dish model)
    {
        _unitOfWork.DishRepository.Delete(model);
        await _unitOfWork.SaveAsync();
    }

    public async Task<Dish> GetAllInfo(int id)
    {
        var dish = await _unitOfWork.DishRepository.GetByIdAsync(id);
        
        dish.DishSize = await _unitOfWork.DishSizeRepository.GetByIdAsync(dish.DishSizeId);

        var ingredientsList = await _unitOfWork.IngridientRepository.GetIngridientsByDishId(id);
        
        dish.Ingridients = ingredientsList;

        return dish;
    }
    
    public async Task AddIngridientsToDish(List<string> ingridientNames, Dish dish)
    {
        var ingredients = new List<Ingridient>();
        foreach (var name in ingridientNames)
        {
            var ingredient = await _unitOfWork.IngridientRepository.GetByName(name);
            if (ingredient != null)
            {
                ingredients.Add(ingredient);
            }
        }
        dish.Ingridients = ingredients;
        await _unitOfWork.DishRepository.AddIngridientsToDish(ingredients, dish);
        await _unitOfWork.SaveAsync();
    }
    

}