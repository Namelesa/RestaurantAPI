using Business.Interfaces;
using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab_7.Controllers;

[Route("api/[controller]")]
public class IngridientController : ControllerBase
{
    private readonly IIngridientService _ingridientService;
    private readonly IDishService _dishService;

    public IngridientController(IIngridientService ingridientService, IDishService dishService)
    {
        _ingridientService = ingridientService;
        _dishService = dishService;
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
    

    // Post ----------------------------------------------------------------------------------------------    
    [HttpPost]
    [Route("AddIngridient")]
    public async Task<ActionResult> AddIngridient(string name, string? image)
    {
        var existingIngridients = await _ingridientService.FindByName(name);

        if (existingIngridients == null)
        {
            var ingridient = new Ingridient { Name = name, Image = image };

            await _ingridientService.AddAsync(ingridient);

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

        if (dish == null || ingridient == null)
        {
            return NotFound("Dish or ingredient not found.");
        }
        
        dish.Ingridients ??= new List<Ingridient>();

        dish.Ingridients.Add(ingridient);
        await _dishService.UpdateAsync(dish);
    
        return Ok(new { message = $"Added {ingridient.Name} to {dish.Name}" });
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
    
    [HttpDelete]
    [Route("RemoveIngridientFromDish")]
    public async Task<ActionResult> RemoveIngridientFromDish(int dishId, int ingridientId)
    {
        var dish = await _dishService.GetAllInfo(dishId);
        var ingridient = await _ingridientService.GetByIdAsync(ingridientId);

        if (dish == null || ingridient == null)
        {
            return NotFound("Dish or ingredient not found.");
        }
        
        var ingredientExists = dish.Ingridients.Any(i => i.Id == ingridientId);
        if (!ingredientExists)
        {
            return BadRequest("Ingredient is not part of the dish.");
        }

        await _ingridientService.RemoveIngredientFromDishAsync(dishId, ingridientId);

        return Ok(new { message = $"Removed {ingridient.Name} from {dish.Name}" });
    }
}
