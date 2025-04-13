using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Namatara.API.Models;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Namatara.API.Controllers
{
    /// <summary>
    /// Controller for authentication.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="configuration"></param>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(ApplicationDbContext context, IConfiguration configuration) : ControllerBase
    {
        /// <summary>
        /// Sign in a user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] LoginRequest request)
        {
            var usr = await context.Users.FirstOrDefaultAsync(x => x.Username == request.Username);

            if (usr is null)
                return NotFound(new _ApiResponse<object>
                (
                    statusCode: StatusCodes.Status404NotFound,
                    message: "The user is not found."
                ));

            if (!new PasswordHelper().VerifyPassword(usr.Password, request.Password))
                return NotFound(new _ApiResponse<object>
                (
                    statusCode: StatusCodes.Status404NotFound,
                    message: "The user is not found."
                ));

            return Ok(new _ApiResponse<object>
            (
                data: new
                {
                    Token = GenerateJwtToken(usr)
                }
            ));
        }

        /// <summary>
        /// Sign up a new user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] UserSignUpRequest request)
        {
            var checkUsername = await context.Users.AnyAsync(x => x.Username == request.Username);
            if (checkUsername)
                return Conflict(new _ApiResponse<object>
                (
                    statusCode: StatusCodes.Status409Conflict,
                    message: "Username already used."
                ));

            await context.Users.AddAsync(new Models.User
            {
                Username = request.Username,
                Password = new PasswordHelper().HashPassword(request.Password),
                DateOfBirth = request.DateOfBirth,
                FullName = request.FullName,
                Role = UserRole.User,
                ImageUrl = ImageHelper.GetRandomImageUrl()
            });
            await context.SaveChangesAsync();

            return Ok(new _ApiResponse<object>
            (
                message: "Sign up successfully."
            ));
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()) // Tambahkan klaim role
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? ""));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(10),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}