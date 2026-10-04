using System.Collections;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using projekt_inzynierski.Server.Users.Application.Interfaces;
using projekt_inzynierski.Server.Users.Domain.Models;
using AdminModel = projekt_inzynierski.Server.Users.Domain.Models.Admin;

namespace projekt_inzynierski.Server.Users.Infrastructures.Services
{
    public class HashService : IPasswordHasher
    {
        public string Hash(string password, User user)
        {
            var passwordHasher = new PasswordHasher<User>();
            return passwordHasher.HashPassword(user, password);
        }

        public  string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sb = new StringBuilder();
                foreach (var b in bytes)
                    sb.Append(b.ToString("x2")); 
                return sb.ToString();
            }
        }

        public string HashAdminPassword(string password, AdminModel admin)
        {
            return new PasswordHasher<AdminModel>().HashPassword(admin, password);
        }

        // Admin rows created before the PBKDF2 migration hold an unsalted SHA-256 hex digest.
        // Those still verify (in constant time) and are flagged for rehashing.
        public bool VerifyAdminPassword(string password, AdminModel admin, out bool needsRehash)
        {
            needsRehash = false;
            var stored = admin.Password ?? string.Empty;

            if (IsLegacySha256Hex(stored))
            {
                var expected = Encoding.UTF8.GetBytes(stored.ToLowerInvariant());
                var actual = Encoding.UTF8.GetBytes(HashPassword(password));
                var ok = CryptographicOperations.FixedTimeEquals(expected, actual);
                needsRehash = ok;
                return ok;
            }

            var result = new PasswordHasher<AdminModel>().VerifyHashedPassword(admin, stored, password);
            needsRehash = result == PasswordVerificationResult.SuccessRehashNeeded;
            return result != PasswordVerificationResult.Failed;
        }

        private static bool IsLegacySha256Hex(string value) =>
            value.Length == 64 && value.All(Uri.IsHexDigit);

        public bool VerifyPassword(string password, User user)
        {
            var passwordHasher = new PasswordHasher<User>();

            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            return result==PasswordVerificationResult.Success;
        }
    }
}
