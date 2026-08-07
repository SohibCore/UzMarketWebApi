namespace UzMarket.ServiceLayer.Services.Integration.Tax
{
    public class UzasboSetting
    {
        public string BaseUrl { get; set; } = default!;
        public string Login { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string BasicToken { get { return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{Login}:{Password}")); } }

    }
}
