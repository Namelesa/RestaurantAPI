using Business.Interfaces;
using Business.Services;
using Data.Data;
using Data.Interfaces;
using Data.Models;
using Data.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IDishRepository, DishRepository>();
builder.Services.AddScoped<IDishSizeRepository, DishSizeRepository>();
builder.Services.AddScoped<IDishIngridientRepository, DishIngridientRepository>();
builder.Services.AddScoped<IIngridientRepository, IngridientRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();

builder.Services.AddScoped<IDishSizeService, DishSizeService>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<IIngridientService, IngridientService>();
builder.Services.AddScoped<IDishIngridientService, DishIngridientService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",policyBuilder =>
    {
        policyBuilder
            .WithOrigins("http://localhost:4200")
            .SetIsOriginAllowedToAllowWildcardSubdomains()
            .AllowCredentials()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.Run();
