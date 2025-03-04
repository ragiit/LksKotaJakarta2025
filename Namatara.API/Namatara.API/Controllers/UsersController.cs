using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Namatara.API.Controllers
{
    /// <summary>
    /// Controller for users.
    /// </summary>
    /// <param name="context"></param>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(ApplicationDbContext context) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<_ApiResponse<object>>> GetUsers()
        {
            return Ok(new _ApiResponse<object>
            (
                data: await context.Users.Select(x => new
                    {
                        x.Id,
                        x.FullName, 
                        x.Username,
                        x.DateOfBirth,
                        x.ImageUrl
                    })
                    .OrderBy(x => x.Username)
                    .AsNoTracking()
                    .ToListAsync()
            ));
        }
    }
}