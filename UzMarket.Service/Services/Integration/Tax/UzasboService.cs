using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using UzMarket.ServiceLayer.Services.Integration.Dtos;
using UzMarket.ServiceLayer.Services.Integration.Interfaces;

namespace UzMarket.ServiceLayer.Services.Integration.Tax
{
    public class UzasboService : IUzasboService
    {
        private readonly HttpClient _httpClient;
        private readonly UzasboSetting _settings;
        public UzasboService(HttpClient httpClient, UzasboSetting settings)
        {
            _httpClient = httpClient;
            _settings = settings;
            Init();
        }

        private void Init()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", _settings.BasicToken);
        }

        private async Task LoginAsync(CancellationToken cancellation)
        {
            var payload = new { login = _settings.Login, password = _settings.Password };

            var response = await _httpClient.PostAsJsonAsync($"{_settings.BaseUrl}/login", payload, cancellation);
            response.EnsureSuccessStatusCode();
        }

        public async Task<TaxPersonInfoDto> GetPersonInfoAsync(string pinfl, CancellationToken cancellation)
        {
            string url = $"{_settings.BaseUrl}/Uzasbo2/GetLegOrPhysicalByTinPinfl2?innOrPinfl={pinfl}";

            var response = await _httpClient.GetAsync(url, cancellation);

            //if (response.StatusCode == HttpStatusCode.Unauthorized)
            //{
            //    _isLoggedIn = false;
            //    await LoginAsync();
            //    response = await _httpClient.GetAsync(url, cancellation);
            //}

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(cancellation);

            var result = JsonSerializer.Deserialize<TaxPersonInfoDto>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result ?? throw new Exception("Ma'lumot topilmadi");
        }
    }
}
