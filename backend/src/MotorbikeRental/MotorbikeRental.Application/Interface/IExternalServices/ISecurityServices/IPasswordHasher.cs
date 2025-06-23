namespace MotorbikeRental.Application.Interface.IExternalServices.ISecurityServices
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash);
    }
}