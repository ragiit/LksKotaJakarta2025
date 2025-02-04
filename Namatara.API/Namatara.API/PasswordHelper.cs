using Microsoft.AspNetCore.Identity;
using System;

namespace Namatara.API
{
    public class PasswordHelper
    {
        private readonly PasswordHasher<object> _passwordHasher;

        public PasswordHelper()
        {
            _passwordHasher = new PasswordHasher<object>();
        }

        // Meng-hash kata sandi
        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null, password);
        }

        // Memverifikasi kata sandi
        public bool VerifyPassword(string hashedPassword, string password)
        {
            var result = _passwordHasher.VerifyHashedPassword(null, hashedPassword, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}