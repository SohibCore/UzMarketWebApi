namespace UzMarket.ServiceLayer.Security
{
    public interface IAccountService
    {
        bool IsAuthenticated { get; }
        long UserId { get; }
        string UserName { get; }
        string FullName { get; }
    }
}
