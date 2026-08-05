namespace UzMarket.ServiceLayer.Services.RegisterServices.Interfaces
{
    public interface IEmailSender
    {
        Task SendAsync(string to, string subject, string body);
    }
}
