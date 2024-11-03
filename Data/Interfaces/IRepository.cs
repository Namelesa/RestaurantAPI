namespace Data.Interfaces;

public interface IRepository<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    
    Task<TEntity> GetByIdAsync(int id);
    
    Task AddAsync(TEntity entity);
    
    void Update(TEntity entity);

    void Delete(TEntity entity);
}