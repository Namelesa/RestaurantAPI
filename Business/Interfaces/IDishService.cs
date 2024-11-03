using Data.Models;

namespace Business.Interfaces;

public interface IDishService : ICrud<Dish>
{ 
    Task<Dish> GetAllInfo(int id);
}