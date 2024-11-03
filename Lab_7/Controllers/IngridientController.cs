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
    [Route("/GetAllIngridient")]
    public async Task<ActionResult<IEnumerable<Ingridient>>> GetAllIngridient()
    {
        var ingridients = await _ingridientService.GetAllAsync();
        return Ok(ingridients);
    }
    
    [HttpGet]
    [Route("/GetAllIngridientByName")]
    public async Task<ActionResult<IEnumerable<Ingridient>>> GetIngridientByName(string name)
    {
        var ingridients = await _ingridientService.FindByName(name);
        return Ok(ingridients);
    }
    
    [HttpGet]
    [Route("/GetIngridientById")]
    public async Task<ActionResult<IEnumerable<Ingridient>>> GetById(int id)
    {
        var ingridient = await _ingridientService.GetByIdAsync(id);
        return Ok(ingridient);
    }
    
    [HttpGet]
    [Route("/GetAllDishIngridients")]
    public async Task<ActionResult<IEnumerable<Ingridient>>> GetAllDishIngridients()
    {
        var dishIngridients = await _dishIngridientService.GetAllAsync();
        
        return Ok(dishIngridients);
    }
// Post ----------------------------------------------------------------------------------------------    
    [HttpPost]
    [Route("/AddIngridient")]
    public async Task<ActionResult> AddIngridient(string name)
    {
        Ingridient ingridient = new Ingridient()
        {
            Name = name
        };
        DishIngridient dishIngridient = new DishIngridient()
        {
            Ingridient = ingridient,
            IngridientId = ingridient.Id
        };
        await _ingridientService.AddAsync(ingridient);
        await _dishIngridientService.AddAsync(dishIngridient);
        return Ok($"You add a new ingridient {ingridient.Name}");
    }
    
    [HttpPost]
    [Route("/AddIngridientToDish")]
    public async Task<ActionResult> AddIngridientToDish(int dishId, int ingridientId)
    {
        var dish = _dishService.GetByIdAsync(dishId).Result;
        var ingridient  = _ingridientService.GetByIdAsync(dishId).Result;
        if (dish != null && ingridient != null)
        {
            DishIngridient dishIngridient = new DishIngridient()
            {
                DishId = dishId,
                Dish = dish,
                IngridientId = ingridientId,
                Ingridient = ingridient
            };
            
            await _dishIngridientService.AddAsync(dishIngridient);
            dish.DishIngridientsIds?.Add(dishIngridient.Id);
            await _dishService.UpdateAsync(dish);
            return Ok($"You add a new ingridient {ingridient.Name}");
        }

        return NotFound("Not fount dish or ingridient");
    }
    
// Put -----------------------------------------------------------------------------------------------
    
    [HttpPut]
    [Route("/UpdateIngridient")]
    public async Task<ActionResult> UpdateIngridient(string name, int id)
    {
        var current = await _ingridientService.GetByIdAsync(id);
        var currentInfo = _ingridientService.FindByName(name).Result;
        if (currentInfo == null && current != null)
        {
            current.Name = name;
            await _ingridientService.UpdateAsync(current);
            return Ok("Update is ok");
        }

        return BadRequest("Update is not ok");
    }
    
// Delete ---------------------------------------------------------------------------------------------    
    [HttpDelete]
    [Route("/DeleteIngridient")]
    public async Task<ActionResult> DeleteIngridient(int id)
    {
        try
        {
            var ingridient = _ingridientService.GetByIdAsync(id).Result;
            await _ingridientService.DeleteAsync(ingridient);
            return Ok("Delete Ingridient");
        }
        catch (Exception e)
        {
            return NotFound($"No ingridient for delete {e}");
        }
        
    }
    
}