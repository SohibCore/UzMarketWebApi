using UzMarket.ServiceLayer.Security.RegisterServices.Interfaces;

namespace UzMarket.ServiceLayer.Security.RegisterServices.Services
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool Verify(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
