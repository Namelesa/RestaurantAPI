using Business.Interfaces;
using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab_7.Controllers;

[Route("api/[controller]")]
public class IngridientController : ControllerBase
{
    private readonly IIngridientService _ingridientService;
    private readonly IDishService _dishService;
    private readonly IDishIngridientService _dishIngridientService;

    public IngridientController(IIngridientService ingridientService, IDishService dishService, IDishIngridientService dishIngridientService)
    {
        _ingridientService = ingridientService;
        _dishService = dishService;
        _dishIngridientService = dishIngridientService;
    }

    // Get -----------------------------------------------------------------------------------------------    
    [HttpGet]
    [Route("GetAllIngridient")]
    public async Task<ActionResult<IEnumerable<Ingridient>>> GetAllIngridient()
    {
        var ingridients = await _ingridientService.GetAllAsync();
        return Ok(ingridients);
    }
    
    [HttpGet]
    [Route("GetAllIngridientByName")]
    public async Task<ActionResult<IEnumerable<Ingridient>>> GetIngridientByName(string name)
    {
        var ingridient = await _ingridientService.FindByName(name);
        
        return ingridient != null ? Ok(ingridient) : NotFound($"Ingredient with name {name} not found.");
    }
    
    [HttpGet]
    [Route("GetIngridientById")]
    public async Task<ActionResult<Ingridient>> GetById(int id)
    {
        var ingridient = await _ingridientService.GetByIdAsync(id);
        return ingridient != null ? Ok(ingridient) : NotFound($"Ingredient with ID {id} not found.");
    }
    
    [HttpGet]
    [Route("GetAllDishIngridients")]
    public async Task<ActionResult<IEnumerable<DishIngridient>>> GetAllDishIngridients()
    {
        var dishIngridients = await _dishIngridientService.GetAllAsync();
        return Ok(dishIngridients);
    }

    // Post ----------------------------------------------------------------------------------------------    
    [HttpPost]
    [Route("AddIngridient")]
    public async Task<ActionResult> AddIngridient(string name, string? image)
    {
        var existingIngridients = await _ingridientService.FindByName(name);

        if (existingIngridients == null)
        {
            var ingridient = new Ingridient { Name = name, Image = image };
            var dishIngridient = new DishIngridient { Ingridient = ingridient, IngridientId = ingridient.Id };

            await _ingridientService.AddAsync(ingridient);
            await _dishIngridientService.AddAsync(dishIngridient);

            return Ok(new { message = $"Added new ingredient {ingridient.Name}" });
        }

        return BadRequest("An ingredient with this name already exists.");
    }
    
    [HttpPost]
    [Route("AddIngridientToDish")]
    public async Task<ActionResult> AddIngridientToDish(int dishId, int ingridientId)
    {
        var dish = await _dishService.GetByIdAsync(dishId);
        var ingridient = await _ingridientService.GetByIdAsync(ingridientId);

        if (dish != null && ingridient != null)
        {
            var dishIngridient = new DishIngridient
            {
                DishId = dishId,
                Dish = dish,
                IngridientId = ingridientId,
                Ingridient = ingridient
            };

            await _dishIngridientService.AddAsync(dishIngridient);
            dish.DishIngridientsIds?.Add(dishIngridient.Id);
            await _dishService.UpdateAsync(dish);

            return Ok(new { message = $"Added ingredient {ingridient.Name} to dish." });
        }

        return NotFound("Dish or ingredient not found.");
    }

    // Put -----------------------------------------------------------------------------------------------
    
    [HttpPut]
    [Route("UpdateIngridient")]
    public async Task<ActionResult> UpdateIngridient(string name, int id, string? image)
    {
        var current = await _ingridientService.GetByIdAsync(id);

        if (current == null)
        {
            return NotFound("Ingredient not found.");
        }

        var existingIngridientWithName = await _ingridientService.FindByName(name);

        if (existingIngridientWithName == null || existingIngridientWithName.Id == id)
        {
            current.Name = name;
            current.Image = image;
            await _ingridientService.UpdateAsync(current);

            return Ok(new { message = "Ingredient updated successfully." });
        }

        return BadRequest("An ingredient with this name already exists.");
    }
    
    // Delete ---------------------------------------------------------------------------------------------    
    [HttpDelete]
    [Route("DeleteIngridient")]
    public async Task<ActionResult> DeleteIngridient(int id)
    {
        var ingridient = await _ingridientService.GetByIdAsync(id);

        if (ingridient == null)
        {
            return NotFound($"No ingredient found with ID {id} to delete.");
        }

        await _ingridientService.DeleteAsync(ingridient);
        return Ok(new { message = "Ingredient deleted successfully." });
    }
}
