namespace UzMarket.ServiceLayer.Services.RegisterServices.Interfaces
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string password, string hash);
    }
}
