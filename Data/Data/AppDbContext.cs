using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<Ingridient> Ingridients { get; set; }
    public DbSet<DishSize> DishSizes { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<DishIngridient> DishIngridients { get; set; }
}