using UzMarket.ServiceLayer.Services.Integration.Dtos;

namespace UzMarket.ServiceLayer.Services.Integration.Interfaces
{
    public interface IUzasboService
    {
        Task<TaxPersonInfoDto> GetPersonInfoAsync(string pinfl, CancellationToken cancellation);
    }
}
