
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Online.Models;
using Online.Models.DTO;
using Online.Interfaces;
using Online.Contexts;
using Online.Services;
using System.Windows.Markup;

namespace Online.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FunctionController : ControllerBase
    {
        private readonly IFunctionServices _otherService;
        private readonly IRepository<int, Model> _modelrepo;
        private readonly IRepository<int, Color> _colorrepo;

        private readonly IRepository<int, Category> _categrepo;

        public FunctionController(IFunctionServices otherService, IRepository<int, Model> modelrepo, IRepository<int, Color> colorrepo, IRepository<int, Category> categrepo)
        {
            _otherService = otherService;
            _modelrepo = modelrepo;
            _colorrepo = colorrepo;
            _categrepo = categrepo;
        }

        //Colors:
        #region Color
        [HttpPost("add-color")]
        public async Task<IActionResult> AddColor([FromQuery] string colorname)
        {
            try
            {
                Color c = new();
                c.ColorName = colorname.ToLower();

                var col = await _otherService.GetColorByName(c.ColorName);
                if (col == null)
                {
                    await _colorrepo.AddAsync(c);
                    return Ok($"Color {colorname} added successfully");
                }
                return BadRequest($"Color {colorname} already exists");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in OtherController : AddColor, please try again later.", ex.Message });
            }

        }

        [HttpGet("color-by-name")]
        public async Task<IActionResult> GetColorByName([FromQuery] string colorname)
        {
            try
            {
                var col = await _otherService.GetColorByName(colorname.ToLower());
                if (col == null)
                {
                    return BadRequest($"{colorname} doesnt exist!");
                }
                return Ok(new { message = "Color found!", col });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in OtherController : GetColorByName, please try again later.", ex.Message });
            }
        }

        [HttpGet("get-all-color")]
        public async Task<IActionResult> GetAllColor()
        {
            try
            {
                var col = await _colorrepo.GetAllAsync();
                if (col == null)
                {
                    return BadRequest("No colors found!");
                }
                return Ok(new { message = "Colors found!", col });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in OtherController : GetAllColor , please try again later.", ex.Message });
            }
        }
        #endregion

        #region Category
        [HttpPost("add-Category")]
        public async Task<IActionResult> AddCategory([FromQuery] string CategoryName)
        {
            try
            {
                var cat = new Category { Name = CategoryName.ToLower() };
                var res = await _otherService.GetCategoryByName(cat.Name);
                if (res == null)
                {
                    await _categrepo.AddAsync(cat);
                    return Ok($"Color {CategoryName} added successfully");
                }
                return BadRequest($"Color {CategoryName} already exists");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in OtherController : AddCategory, please try again later.", ex.Message });
            }
        }

        [HttpGet("get-category-by-name")]
        public async Task<IActionResult> GetCategoryByName([FromQuery] string CategoryName)
        {
            try
            {
                var cat = await _otherService.GetCategoryByName(CategoryName.ToLower());
                if (cat == null)
                {
                    return BadRequest($"Category {CategoryName} not found!");
                }
                return Ok(new { message = "Category found!", cat });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in OtherController : GetCategoryByName, please try again later.", ex.Message });
            }
        }

        [HttpGet("get-all-categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var cats = await _categrepo.GetAllAsync();
                if (cats == null)
                {
                    return BadRequest("No categories found!");
                }
                return Ok(new { message = "Categories found!", cats });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in OtherController : GetAllCategories, please try again later.", ex.Message });
            }
        }
        #endregion


        #region Model
        [HttpPost("add-model")]
        public async Task<IActionResult> AddModel([FromQuery] string modelname)
        {
            try
            {
                var mod = new Model { ModelName = modelname.ToLower() };
                var res = await _otherService.GetModelByName(modelname.ToLower());
                if (res == null)
                {
                    await _modelrepo.AddAsync(mod);
                    return Ok(new { message = $"Model {modelname} added successfully!", mod });
                }
                return BadRequest($"Model {modelname} already exists!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in OtherController : AddModel, please try again later.", ex.Message });
            }
        }

        [HttpGet("get-model-by-name")]
        public async Task<IActionResult> GetModelByName([FromQuery] string modelname)
        {
            try
            {
                var mod = await _otherService.GetModelByName(modelname.ToLower());
                if (mod != null)
                {
                    return Ok(new { message = $"Model {modelname} found!", mod });
                }
                return NotFound($"Model {modelname} not found!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in OtherController : GetModelByName, please try again later.", ex.Message });
            }
        }

        [HttpGet("get-all-model")]
        public async Task<IActionResult> GetAllModel()
        {
            try
            {
                var mods = await _modelrepo.GetAllAsync();
                if (mods == null)
                {
                    return BadRequest("No models found!");
                }
                return Ok(new { message = "Models found!", mods });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in OtherController : GetAllModel, please try again later.", ex.Message });
            }
        }
        #endregion
    }
}