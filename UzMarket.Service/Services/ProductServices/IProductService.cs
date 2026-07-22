using UzMarket.RepositoryLayer.Dtos.ProductDtos;
using UzMarket.ServiceLayer.MediatorServices.ProductServices.Dtos;

namespace UzMarket.ServiceLayer.Services.ProductServices
{
    public interface IProductService
    {
        Task<List<ProductListDto>> GetListAsync(ProductFilterDto filter);

        Task<ProductDto> GetAsync(long Id);

        Task<ProductDto> CreateAsync(CreateProductDlDto dto, CancellationToken cancellationToken);

        Task<ProductDto> UpdateAsync(UpdateProductDlDto dto, CancellationToken cancellationToken);

        Task<string> DeleteAsync(long id, CancellationToken cancellationToken);
    }
}