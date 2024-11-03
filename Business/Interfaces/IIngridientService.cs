using Data.Models;

namespace Business.Interfaces;

public interface IIngridientService : ICrud<Ingridient>
{
    Task<Ingridient> FindByName(string name);
}