using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Namatara.API.Controllers;

/// <summary>
/// Controller for user profile.
/// </summary>
/// <param name="context"></param>
/// <param name="configuration"></param>
[Route("api/[controller]")]
[ApiController]
public class MeController(ApplicationDbContext context, IConfiguration configuration) : ControllerBase
{
    /// <summary>
    /// Get current user data.
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet()]
    public async Task<IActionResult> GetCurrentUser()
    {
        var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        if (string.IsNullOrEmpty(username))
        {
            return Unauthorized(new _ApiResponse<object>(
                statusCode: StatusCodes.Status401Unauthorized,
                message: "Invalid token or user not found."
            ));
        }

        var user = await context.Users.FirstOrDefaultAsync(x => x.Username == username);

        if (user is null)
        {
            return NotFound(new _ApiResponse<object>(
                statusCode: StatusCodes.Status404NotFound,
                message: "User data not found."
            ));
        }

        return Ok(new _ApiResponse<object>(
            data: new
            {
                user.Id,
                user.Username,
                user.FullName,
                user.DateOfBirth,
                user.Role,
                user.ImageUrl
            }
        ));
    }

    /// <summary>
    /// Get current user attraction ratings.
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet("attraction-ratings")]
    public async Task<IActionResult> GetCurrentUserAttractionRatings()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "";

        return Ok(new _ApiResponse<object>(
            data: await context.TourismAttractionRatings.Where(x => x.UserId == Guid.Parse(userId)).Select(x => new
            {
                Id = x.TourismAttractionId,
                Name = x.TourismAttraction == null ? "" : x.TourismAttraction.Name,
                ImageUrl = x.TourismAttraction == null ? "" : x.TourismAttraction.ImageUrl,
                Rating = x.Rating
            }).AsNoTracking().ToListAsync()
        ));
    }

    /// <summary>
    /// Get current user attraction bookmarks.
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet("bookmarks")]
    public async Task<IActionResult> GetCurrentUserAttractionBookmarks()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "";

        return Ok(new _ApiResponse<object>(
            data: await context.TourismAttractionBookmarks.Where(x => x.UserId == Guid.Parse(userId)).Select(x =>
                new
                {
                    Id = x.TourismAttractionId,
                    Name = x.TourismAttraction == null ? "-" : x.TourismAttraction.Name,
                    ImageUrl = x.TourismAttraction == null ? "-" : x.TourismAttraction.ImageUrl,
                }).AsNoTracking().ToListAsync()
        ));
    }
}