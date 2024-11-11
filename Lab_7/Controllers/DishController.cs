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
    private readonly IDishIngridientService _dishIngridientService;
    private readonly IIngridientService _ingridientService;

    public DishController(IDishService dishService, IDishSizeService dishSizeService, IDishIngridientService dishIngridientService, IIngridientService ingridientService)
    {
        _dishService = dishService;
        _dishSizeService = dishSizeService;
        _dishIngridientService = dishIngridientService;
        _ingridientService = ingridientService;
    }
// Get -----------------------------------------------------------------------------------------------
    [HttpGet]
    [Route("/GetAllDish")]
    public async Task<ActionResult<IEnumerable<Dish>>> GetAllDish()
    {
        var dishes = await _dishService.GetAllAsync();
        var sizes = await _dishSizeService.GetAllAsync();
        foreach (var d in dishes)
        {
            foreach (var s in sizes)
            {
                d.DishSize = s;
            }
        }
        return Ok(dishes);
    }
    
    [HttpGet]
    [Route("/GetDishById")]
    public async Task<ActionResult<IEnumerable<Dish>>> GetDishById(int id)
    {
        try
        {
            var dish = _dishService.GetAllInfo(id).Result;
            return Ok(dish);
        }
        catch
        {
            return BadRequest("Find by id is not ok");
        }
    }
    
    [HttpGet]
    [Route("/GetAllDishSize")]
    public async Task<ActionResult<IEnumerable<DishSize>>> GetAllDishSize()
    {
        var dishes = await _dishSizeService.GetAllAsync();
        return Ok(dishes);
    }
    
    [HttpGet]
    [Route("/GetDishSizeById")]
    public async Task<ActionResult<IEnumerable<DishSize>>> GetDishSizeById(int id)
    {
        try
        {
            var dishSize = _dishSizeService.GetByIdAsync(id).Result;
            if (dishSize != null)
            {
                return Ok(dishSize);
            }

            return NotFound("No dish with this id");

        }
        catch
        {
            return BadRequest("Find by id is not ok");
        }
    }
    
// Put -----------------------------------------------------------------------------------------------   

    [HttpPut]
    [Route("/UpdateDish")]
    public async Task<IActionResult> UpdateDish([FromBody] DishDto dishDto, int id)
    {
        try
        {
            var currentDish = await _dishService.GetByIdAsync(id);

            if (currentDish == null)
            {
                return NotFound("Dish not found");
            }
            
            if (!string.IsNullOrWhiteSpace(dishDto.Name))
            {
                currentDish.Name = dishDto.Name;
            }
            
            if (dishDto.Price > 0)
            {
                currentDish.Price = dishDto.Price;
            }

            if (!string.IsNullOrWhiteSpace(dishDto.Image))
            {
                currentDish.Image = dishDto.Image;
            }

            if (dishDto.DishIngridientsNames != null && dishDto.DishIngridientsNames.Any())
            {
                currentDish.DishIngridientsIds.Clear();
                foreach (var name in dishDto.DishIngridientsNames)
                {
                    var ingredient = await _ingridientService.FindByName(name);
                    if (ingredient != null)
                    {
                        currentDish.DishIngridientsIds.Add(ingredient.Id);
                    }
                }
            }
            
            await _dishService.UpdateAsync(currentDish);
            return Ok(new { message = "Dish updated successfully"});
        }
        catch (Exception ex)
        {
            return BadRequest($"Error updating dish: {ex.Message}");
        }
    }
    
    [HttpPut]
    [Route("/UpdateDishSize")]
    public IActionResult UpdateDishSize(string name, decimal price, int id, string? image)
    {
        try
        {
            var currentDishSizePrice = _dishSizeService.FindByPrice(price).Result;
            var currentDishSizeName = _dishSizeService.FindByName(name).Result;
            var currentDishSize = _dishSizeService.GetByIdAsync(id).Result;

            if (currentDishSizeName != null || currentDishSizePrice != null)
            {
                return BadRequest("You already have this size or price for size");
            }

            currentDishSize.Size = name;
            currentDishSize.Price = price;
            currentDishSize.Image = image;
            _dishSizeService.UpdateAsync(currentDishSize);
            return Ok(new{ message = "You update a Size"});
        }
        catch
        {
            return BadRequest("You already have this size or price for size");
        }
    }

// Post -----------------------------------------------------------------------------------------------

    [HttpPost]
    [Route("/AddDish")]
    public async Task<IActionResult> AddDish([FromBody] DishDto dishDto)
    {
        try
        {
            if (dishDto == null)
            {
                return BadRequest("Dish data is missing.");
            }

            if (string.IsNullOrEmpty(dishDto.Name) || dishDto.Price <= 0)
            {
                return BadRequest("Invalid dish data. Ensure all required fields are provided.");
            }

            var size = await _dishSizeService.FindByName("S");
            if (size == null)
            {
                return NotFound("Dish size 'S' not found.");
            }
            
            Dish dish = new Dish()
            {
                DishSize = size,
                DishSizeId = size.Id,
                Name = dishDto.Name,
                Price = dishDto.Price,
                Image = dishDto.Image
            };

            List<Ingridient> ingridients = new List<Ingridient>();
            List<int> ingridientsIds = new List<int>();

            if (dishDto.DishIngridientsNames == null)
            {
                dish.DishIngridientsIds = new List<int>();
            }
            else
            {
                foreach (var name in dishDto.DishIngridientsNames)
                {
                    var ingredient = await _dishIngridientService.GetByName(name);
                    ingridients.Add(ingredient);
                    ingridientsIds.Add(ingredient.Id);
                }

                dish.Ingridients = ingridients;
                dish.DishIngridientsIds = ingridientsIds;
            }
            
            
            
           

            await _dishService.AddAsync(dish);

            return Ok(new {message = "You have added a new dish."});
        }
        catch (Exception ex)
        {
            // В случае ошибки возвращаем более подробную информацию
            return BadRequest($"An error occurred while adding the dish: {ex.Message}");
        }
    }

    
    [HttpPost]
    [Route("/AddDishSize")]
    public IActionResult AddDishSize([FromBody] DishSizeDto dishSizeDto)
    {
        try
        {
            var currentDishSizePrice = _dishSizeService.FindByPrice(dishSizeDto.Price).Result;
            var currentDishSizeName = _dishSizeService.FindByName(dishSizeDto.Size).Result;

            if (currentDishSizeName != null || currentDishSizePrice != null)
            {
                return BadRequest("You already have this size or price for size");
            }

            DishSize dishSize = new DishSize()
            {
                Price = dishSizeDto.Price,
                Size = dishSizeDto.Size,
                Image = dishSizeDto.Image
            };
            _dishSizeService.AddAsync(dishSize);
            return Ok(new { message = "You add a new Size"});
        }
        catch
        {
            return BadRequest("You already have this size or price for size");
        }
    }

// Delete ---------------------------------------------------------------------------------------------

    [HttpDelete]
    [Route("/DeleteDish")]
    public ActionResult DeleteDish(int id)
    {
        try
        {
            var dish = _dishService.GetByIdAsync(id).Result;
            _dishService.DeleteAsync(dish);
            return Ok(new { message = "Delete Dish is Ok" });
        }
        catch
        {
            return NotFound("Not Found this Dish");
        }
    }

    [HttpDelete]
    [Route("/DeleteDishShize")]
    public IActionResult DeleteDishSize(int id)
    {
        try
        {
            var size = _dishSizeService.GetByIdAsync(id);
            _dishSizeService.DeleteAsync(size.Result);
            return Ok(new{ message = "Delete Size is Ok"});
        }
        catch
        {
            return NotFound("Not Found this Size");
        }
    }

}