using MotorbikeRental.Application.Interface.IExternalServices.ISecurityServices;

namespace MotorbikeRental.Infrastructure.ExternalServices.SecurityService
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}