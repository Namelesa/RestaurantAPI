using Business.Interfaces;
using Data.Models;
using Lab_7.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Lab_7.Controllers;

[Route("api/[controller]")]
public class DishController : ControllerBase
{
    private readonly IDishService _dishService;
    private readonly IDishSizeService _dishSizeService;
    private readonly IIngridientService _ingridientService;

    public DishController(IDishService dishService, IDishSizeService dishSizeService, IIngridientService ingridientService)
    {
        _dishService = dishService;
        _dishSizeService = dishSizeService;
        _ingridientService = ingridientService;
    }

// Get -----------------------------------------------------------------------------------------------
    [HttpGet("GetAllDish")]
    public async Task<ActionResult<IEnumerable<Dish>>> GetAllDish()
    {
        var dishes = await _dishService.GetAllAsync();
        var sizes = await _dishSizeService.GetAllAsync();
        
        foreach (var dish in dishes)
        {
            dish.DishSize = sizes.FirstOrDefault(s => s.Id == dish.DishSizeId);
        }
        
        return Ok(dishes);
    }

    [HttpGet("GetDishById")]
    public async Task<ActionResult<Dish>> GetDishById(int id)
    {
        var dish = await _dishService.GetAllInfo(id);
        return dish != null ? Ok(dish) : BadRequest("Find by id is not ok");
    }

    [HttpGet("GetAllDishSize")]
    public async Task<ActionResult<IEnumerable<DishSize>>> GetAllDishSize()
    {
        return Ok(await _dishSizeService.GetAllAsync());
    }

    [HttpGet("GetDishSizeById")]
    public async Task<ActionResult<DishSize>> GetDishSizeById(int id)
    {
        var dishSize = await _dishSizeService.GetByIdAsync(id);
        return dishSize != null ? Ok(dishSize) : NotFound("No dish with this id");
    }
    
// Put -----------------------------------------------------------------------------------------------   
    [HttpPut("UpdateDish")]
    public async Task<IActionResult> UpdateDish(int id, [FromBody] DishDto dishDto)
    {
        var currentDish = await _dishService.GetByIdAsync(id);
        if (currentDish == null)
            return NotFound("Dish not found");

        currentDish.Name = string.IsNullOrWhiteSpace(dishDto.Name) ? currentDish.Name : dishDto.Name;
        currentDish.Price = dishDto.Price > 0 ? dishDto.Price : currentDish.Price;
        currentDish.Image = string.IsNullOrWhiteSpace(dishDto.Image) ? currentDish.Image : dishDto.Image;

        if (dishDto.DishIngridientsNames?.Any() == true)
        {
            foreach (var name in dishDto.DishIngridientsNames)
            {
                var ingredient = await _ingridientService.FindByName(name);
                if (ingredient != null)
                {
                    currentDish.Ingridients.Add(ingredient);
                }
            }
        }

        await _dishService.UpdateAsync(currentDish);
        return Ok(new { message = "Dish updated successfully" });
    }
    
    [HttpPut("UpdateDishSize")]
    public async Task<IActionResult> UpdateDishSize(int id, [FromBody] DishSizeDto dishSizeDto)
    {
        var currentDishSize = await _dishSizeService.GetByIdAsync(id);
    
        if (currentDishSize == null)
        {
            return NotFound("Dish size not found");
        }
        
        var existingSize = await _dishSizeService.FindByName(dishSizeDto.Size);
        var existingPrice = await _dishSizeService.FindByPrice(dishSizeDto.Price);

        if ((dishSizeDto.Size != currentDishSize.Size && existingSize != null) || 
            (dishSizeDto.Price != currentDishSize.Price && existingPrice != null))
        {
            return BadRequest("You already have this size or price for size");
        }
        
        currentDishSize.Size = dishSizeDto.Size != currentDishSize.Size ? dishSizeDto.Size : currentDishSize.Size;
        currentDishSize.Price = dishSizeDto.Price != currentDishSize.Price ? dishSizeDto.Price : currentDishSize.Price;
        currentDishSize.Image = dishSizeDto.Image ?? currentDishSize.Image;
        
        await _dishSizeService.UpdateAsync(currentDishSize);
    
        return Ok(new { message = "Dish size updated successfully" });
    }


// Post -----------------------------------------------------------------------------------------------
    [HttpPost("AddDish")]
    public async Task<IActionResult> AddDish([FromBody] DishDto dishDto)
    {
        if (dishDto == null || string.IsNullOrEmpty(dishDto.Name) || dishDto.Price <= 0)
            return BadRequest("Invalid dish data. Ensure all required fields are provided.");

        var size = await _dishSizeService.FindByName("S");
        if (size == null)
            return NotFound("Dish size 'S' not found.");
        
        var dish = new Dish
        {
            DishSize = size,
            DishSizeId = size.Id,
            Name = dishDto.Name,
            Price = dishDto.Price,
            Image = dishDto.Image,
        };

        if (dishDto.DishIngridientsNames?.Any() == true)
        {
            var ingredients = new List<Ingridient>();
            foreach (var name in dishDto.DishIngridientsNames)
            {
                var ingredient = await _ingridientService.FindByName(name);
                if (ingredient != null)
                {
                    ingredients.Add(ingredient);
                }
            }
            dish.Ingridients = ingredients;
        }
        
        await _dishService.AddAsync(dish);
        return Ok(new { message = "You have added a new dish." });
    }

    [HttpPost("AddDishSize")]
    public async Task<IActionResult> AddDishSize([FromBody] DishSizeDto dishSizeDto)
    {
        if ((await _dishSizeService.FindByName(dishSizeDto.Size) != null || await _dishSizeService.FindByPrice(dishSizeDto.Price) != null))
            return BadRequest("You already have this size or price for size");

        var dishSize = new DishSize
        {
            Price = dishSizeDto.Price,
            Size = dishSizeDto.Size,
            Image = dishSizeDto.Image
        };

        await _dishSizeService.AddAsync(dishSize);
        return Ok(new { message = "You add a new Size" });
    }

// Delete ---------------------------------------------------------------------------------------------
    [HttpDelete("DeleteDish")]
    public async Task<IActionResult> DeleteDish(int id)
    {
        var dish = await _dishService.GetByIdAsync(id);
        if (dish == null)
            return NotFound("Not Found this Dish");

        await _dishService.DeleteAsync(dish);
        return Ok(new { message = "Delete Dish is Ok" });
    }

    [HttpDelete("DeleteDishSize")]
    public async Task<IActionResult> DeleteDishSize(int id)
    {
        var size = await _dishSizeService.GetByIdAsync(id);
        if (size == null)
            return NotFound("Not Found this Size");

        await _dishSizeService.DeleteAsync(size);
        return Ok(new { message = "Delete Size is Ok" });
    }
}
