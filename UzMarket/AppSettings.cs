using UzMarket.ServiceLayer.Services.Integration.Tax;

namespace UzMarket.WebApi
{
    public class AppSettings
    {
        public static AppSettings Instance { get; set; } = null!;
        public UzasboSetting UzasboSetting { get; set; } = null!;

        public static void Init(AppSettings instance)
        {
            Instance = instance;
        }
    }
}
