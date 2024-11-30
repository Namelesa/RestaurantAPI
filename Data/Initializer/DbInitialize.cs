using Data.Data;
using Microsoft.EntityFrameworkCore;

namespace Data.Initializer;

public class DbInitialize : IDbInitializer
{
    private readonly AppDbContext _db;

    public DbInitialize(AppDbContext db)
    {
        _db = db;
    }
    
    public void Initialize()
    {
        try
        {
            if (_db.Database.GetPendingMigrations().Any())
            {
                _db.Database.Migrate();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error during migration: " + ex.Message);
        }
    }
}