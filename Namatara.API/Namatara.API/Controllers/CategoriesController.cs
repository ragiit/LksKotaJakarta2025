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
        public async Task<ActionResult<_ApiResponse<Category>>> GetCategories(string search = "")
        {
            // Memfilter berdasarkan query 'search' jika ada
            var categoriesQuery = context.Categories
                .Where(x => string.IsNullOrEmpty(search) || x.Name.Contains(search) || x.Description.Contains(search))
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Description,
                    x.ImageUrl
                })
                .OrderBy(x => x.Name)
                .AsNoTracking();

            var categories = await categoriesQuery.ToListAsync();

            return Ok(new _ApiResponse<object>
            (
                data: categories
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
                    ImageUrl = category.ImageUrl
                }
            ));
        }

        [HttpGet("{id}/attractions")]
        public async Task<ActionResult<TourismAttraction>> GetCategoryWithAttractions(Guid id, string search = "")
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

            var attractionsQuery = context.TourismAttractions
                .Where(x => x.CategoryId == id)
                .AsQueryable(); // Mulai sebagai IQueryable agar bisa diubah sesuai search

            // Tambahkan filter berdasarkan search jika ada
            if (!string.IsNullOrEmpty(search))
            {
                attractionsQuery = attractionsQuery
                    .Where(x => x.Name.Contains(search) || x.Description.Contains(search));
            }

            var attractions = await attractionsQuery
                .Select(x => new
                {
                    x.Id,
                    x.CategoryId,
                    x.Price,
                    x.Name,
                    x.Description,
                    x.Location,
                    x.OpeningHours,
                    AverageRating = x.Ratings.Count != 0 ? x.Ratings.Average(r => r.Rating) : 0,
                    x.ImageUrl
                })
                .OrderBy(x => x.Name)
                .AsNoTracking()
                .ToListAsync();

            return Ok(new _ApiResponse<object>(data: attractions));
        }


        [HttpGet("sssss")]
        public async Task<ActionResult<_ApiResponse<object>>> GetCategorieszz()
        {
            return Ok("Nyoba");
        }
    }
}