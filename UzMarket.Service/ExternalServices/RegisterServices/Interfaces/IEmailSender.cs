namespace UzMarket.ServiceLayer.Security.RegisterServices.Interfaces
{
    public interface IEmailSender
    {
        Task SendAsync(string to, string subject, string body);
    }
}
