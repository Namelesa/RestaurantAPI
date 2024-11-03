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
    public IActionResult UpdateDish([FromBody] DishDto dishDto, int id)
    {
        try
        {
            var currentDish = _dishService.GetByIdAsync(id).Result;

            if (currentDish != null && currentDish.Name != dishDto.Name)
            {
                currentDish.DishSizeId = _dishSizeService.FindByName(dishDto.Size).Result.Id;
                currentDish.Price = dishDto.Price;
                currentDish.Name = dishDto.Name;
                foreach (var name in dishDto.DishIngridientsNames)
                {
                    currentDish.DishIngridientsIds.Add(_ingridientService.FindByName(name).Result.Id);
                }
                _dishService.UpdateAsync(currentDish);
                return Ok("You update a Dish");
            }
            return BadRequest("You already have this dish");
        }
        catch
        {
            return BadRequest("You have a problem in update");
        }
    }

    [HttpPut]
    [Route("/UpdateDishSize")]
    public IActionResult UpdateDishSize(string name, decimal price, int id)
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
            _dishSizeService.UpdateAsync(currentDishSize);
            return Ok("You update a Size");
        }
        catch
        {
            return BadRequest("You already have this size or price for size");
        }
    }

// Post -----------------------------------------------------------------------------------------------

    [HttpPost]
    [Route("/AddDish")]
    public IActionResult AddDish([FromBody] DishDto dishDto)
    {
        try
        {
            var size = _dishSizeService.FindByName(dishDto.Size).Result;

            List<Ingridient> ingridients = new List<Ingridient>();
            List<int> ingridientsIds = new List<int>();
            foreach (var names in dishDto.DishIngridientsNames)
            {
                ingridients.Add(_dishIngridientService.GetByName(names).Result);
                ingridientsIds.Add(_dishIngridientService.GetByName(names).Result.Id);
            }

            if (dishDto != null && size != null)
            {
                Dish dish = new Dish()
                {
                    DishSize = size,
                    DishSizeId = size.Id,
                    Name = dishDto.Name,
                    Price = dishDto.Price,
                    Ingridients = ingridients,
                    DishIngridientsIds = ingridientsIds
                };

                _dishService.AddAsync(dish);
                
                return Ok("You add a new dish");
            }

            return NotFound("Not found ingridients or size for your dish");
        }
        catch
        {
            return BadRequest("You have a problems with adding a dish");
        }
    }
    
    [HttpPost]
    [Route("/AddDishSize")]
    public IActionResult AddDishSize([FromBody] DishSizeDto dishSizeDto)
    {
        try
        {
            var currentDishSizePrice = _dishSizeService.FindByPrice(dishSizeDto.Price).Result;
            var currentDishSizeName = _dishSizeService.FindByName(dishSizeDto.Name).Result;

            if (currentDishSizeName != null || currentDishSizePrice != null)
            {
                return BadRequest("You already have this size or price for size");
            }

            DishSize dishSize = new DishSize()
            {
                Price = dishSizeDto.Price,
                Size = dishSizeDto.Name
            };
            _dishSizeService.AddAsync(dishSize);
            return Ok("You add a new Size");
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
            var dish = _dishService.GetByIdAsync(id);
            _dishService.DeleteAsync(dish.Result);
            return Ok("Delete Dish is Ok");
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
            return Ok("Delete Size is Ok");
        }
        catch
        {
            return NotFound("Not Found this Size");
        }
    }

}