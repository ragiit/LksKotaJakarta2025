using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Namatara.API.Models;

namespace Namatara.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourismAttractionsController(ApplicationDbContext context) : ControllerBase
    {
        // GET: api/TourismAttractions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<_ApiResponse<TourismAttraction>>> GetTourismAttraction(Guid id)
        {
            var attraction = await context.TourismAttractions
                    .Include(x => x.Ratings)
                    .FirstOrDefaultAsync(x => x.Id == id);
            if (attraction == null)
            {
                return NotFound(new _ApiResponse<object>
                (
                    statusCode: StatusCodes.Status404NotFound,
                    message: "Id is not found."
                ));
            }

            // Hitung rata-rata rating
            var averageRating = attraction.Ratings.Count != 0
                ? attraction.Ratings.Average(r => r.Rating)
                : 0;

            return Ok(new _ApiResponse<object>
            (
                data: new
                {
                    attraction.Id,
                    attraction.CategoryId,
                    attraction.Price,
                    attraction.Name,
                    attraction.Description,
                    attraction.Location,
                    attraction.OpeningHours,
                    Rating = averageRating,
                    attraction.ImageUrl
                }
            ));
        }

        [HttpPost("{attractionId}/ratings")]
        [Authorize]
        public async Task<ActionResult<_ApiResponse<object>>> AddRating(Guid attractionId, [FromBody] TourismAttractionUserRating request)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new _ApiResponse<object>(
                    statusCode: StatusCodes.Status401Unauthorized,
                    message: "Invalid user authentication."
                ));
            }

            // Check if the attraction exists
            var attraction = await context.TourismAttractions.FindAsync(attractionId);
            if (attraction == null)
            {
                return NotFound(new _ApiResponse<object>(
                    statusCode: StatusCodes.Status404NotFound,
                    message: "Tourism attraction not found."
                ));
            }

            // Validate if the user has already rated this attraction
            var existingRating = await context.TourismAttractionRatings
                .FirstOrDefaultAsync(r => r.TourismAttractionId == attractionId && r.UserId == userId);

            if (existingRating != null)
            {
                return BadRequest(new _ApiResponse<object>(
                    statusCode: StatusCodes.Status400BadRequest,
                    message: "You have already rated this attraction."
                ));
            }

            // Add the new rating
            var newRating = new TourismAttractionRating
            {
                Id = Guid.NewGuid(),
                TourismAttractionId = attractionId,
                UserId = userId,
                Rating = request.Rating,
                Review = request.Review
            };

            context.TourismAttractionRatings.Add(newRating);
            await context.SaveChangesAsync();

            return Ok(new _ApiResponse<object>(
                message: "Rating added successfully."
            ));
        }
    }
}