using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Namatara.API.Models;

namespace Namatara.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ApplicationDbContext context) : ControllerBase
    {
        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<_ApiResponse<Category>>> GetCategories()
        {
            return Ok(new _ApiResponse<object>
            (
                data: await context.Categories.Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Description,
                })
                .OrderBy(x => x.Name)
                .AsNoTracking()
                .ToListAsync()
            ));
        }

        // GET: api/Categories/d99e5c64-f169-4b3c-8be5-502ca48d66d5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(Guid id)
        {
            var category = await context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound(new _ApiResponse<object>
                (
                    statusCode: StatusCodes.Status404NotFound,
                    message: "Id is not found."
                ));
            }

            return Ok(new _ApiResponse<object>
            (
                data: new
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                }
            ));
        }

        [HttpGet("{id}/attractions")]
        public async Task<ActionResult<TourismAttraction>> GetCategoryWithAttractions(Guid id)
        {
            var category = await context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound(new _ApiResponse<object>
                (
                    statusCode: StatusCodes.Status404NotFound,
                    message: "Id is not found."
                ));
            }

            return Ok(new _ApiResponse<object>
            (
                data: await context.TourismAttractions.Where(x => x.CategoryId == id).Select(x =>
                new
                {
                    x.Id,
                    x.CategoryId,
                    x.Price,
                    x.Name,
                    x.Description,
                    x.Location,
                    x.OpeningHours,
                    x.Rating,
                    x.ImageUrl
                })
                .OrderBy(x => x.Name)
                .AsNoTracking()
                .ToListAsync()
            ));
        }
    }
}