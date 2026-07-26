namespace UzMarket.ServiceLayer.Security.AccountServices
{
    public interface IAccountService
    {
        bool IsAuthenticated { get; }
        long UserId { get; }
        string UserName { get; }
        string FullName { get; }
    }
}
