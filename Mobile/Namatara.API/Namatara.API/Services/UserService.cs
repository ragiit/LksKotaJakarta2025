using System.Security.Claims;

namespace Namatara.API.Services
{
    public interface IUserService
    {
        Guid GetCurrentUserId();
    }

    public class UserService(IHttpContextAccessor httpContextAccessor) : IUserService
    {
        public Guid GetCurrentUserId()
        {
            // Anda bisa mengubah ini sesuai dengan cara autentikasi yang digunakan (misalnya, mengambil ID pengguna dari klaim token JWT)
            var userIdClaim = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                return Guid.Parse(userIdClaim.Value);
            }

            return Guid.Empty; // Jika tidak ada pengguna yang terautentikasi
        }
    }
}