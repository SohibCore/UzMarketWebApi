using Microsoft.EntityFrameworkCore;
using UzMarket.Core;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.RepositoryLayer.Dtos.ProductDtos;
using UzMarket.RepositoryLayer.Entity;
using UzMarket.ServiceLayer.Security;
using UzMarket.ServiceLayer.Services.ProductServices.QueryObejcts;

namespace UzMarket.ServiceLayer.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public ProductService(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }

        public async Task<List<ProductListDto>> GetListAsync(ProductFilterDto filter)
        {
            var product = await _context.Products
                .Include(x => x.Tables)
                .Select(x => new ProductListDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    StockQuantity = x.StockQuantity,
                    CategoryId = x.CategoryId,
                    SupplierId = x.SupplierId,
                    Tables = x.Tables.Select(img => new ProductImageDto
                    {
                        Id = img.Id,
                        ImageUrl = img.ImageUrl,
                        MainPic = img.MainPic,
                        ProductId = img.ProductId,
                        SortOrder = img.SortOrder,
                    }).ToList(),
                }).SortFilter(filter)
            .ToListAsync();

            if (product == null)
                throw new Exception("Product not found");

            return product;
        }

        public async Task<ProductDto> GetAsync(long Id)
        {
            var product = await _context.Products
                .Include(x => x.Tables)
                .FirstOrDefaultAsync(x => x.Id == Id);

            if (product == null)
                throw new Exception($"Product not found : {Id}");

            return new ProductDto
            {
                Id = Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                SupplierId = product.SupplierId,
                Tables = product.Tables.Select(x => new ProductImageDto
                {
                    Id = x.Id,
                    ImageUrl = x.ImageUrl,
                    MainPic = x.MainPic,
                    ProductId = x.ProductId,
                    SortOrder = x.SortOrder,
                }).ToList()
            };
        }

        public async Task<ProductDto> CreateAsync(CreateProductDlDto dto, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                CategoryId = dto.CategoryId,
                SupplierId = _service.UserId,

                CreatedAt = DateTime.UtcNow,

                Tables = dto.Tables.Select(x => new ProductImage
                {
                    ImageUrl = x.ImageUrl,
                    MainPic = x.MainPic,
                    //ProductId = x.ProductId,
                    SortOrder = x.SortOrder,
                }).ToList()
            };

            await _context.Products.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new ProductDto
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                SupplierId = product.SupplierId,
                Tables = product.Tables.Select(x => new ProductImageDto
                {
                    Id = x.Id,
                    ImageUrl = x.ImageUrl,
                    MainPic = x.MainPic,
                    ProductId = x.ProductId,
                    SortOrder = x.SortOrder,
                }).ToList()
            };
        }

        public async Task<ProductDto> UpdateAsync(UpdateProductDlDto dto, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == dto.Id, cancellationToken);

            if (product == null)
                throw new Exception($"Product not found : {dto.Id}");

            if (product.Name != dto.Name)
                product.Name = dto.Name;

            if (product.Description != dto.Description)
                product.Description = dto.Description;

            if (product.Price != dto.Price)
                product.Price = dto.Price;

            if (product.StockQuantity != dto.StockQuantity)
                product.StockQuantity = dto.StockQuantity;

            if (product.CategoryId != dto.CategoryId)
                product.CategoryId = dto.CategoryId;

            if (product.SupplierId != dto.SupplierId)
                product.SupplierId = dto.SupplierId;

            product.Tables = dto.Tables.Select(x => new ProductImage
            {
                ImageUrl = x.ImageUrl,
                MainPic = x.MainPic,
                ProductId = x.ProductId,
                SortOrder = x.SortOrder,
            }).ToList();

            return new ProductDto
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                SupplierId = product.SupplierId,
                Tables = product.Tables.Select(x => new ProductImageDto
                {
                    Id = x.Id,
                    ImageUrl = x.ImageUrl,
                    MainPic = x.MainPic,
                    ProductId = x.ProductId,
                    SortOrder = x.SortOrder,
                }).ToList()
            };
        }

        public async Task<string> DeleteAsync(long Id, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == Id, cancellationToken);

            if (product == null)
                throw new Exception($"Product not found : {Id}");

            if (product.StatusId == (int)StatusIdConst.DELETED)
                throw new Exception($"Product with ID {Id} is already deleted.");

            product.SupplierId = (int)StatusIdConst.DELETED;
            await _context.SaveChangesAsync(cancellationToken);

            return $"Product with ID {product.Id} has been deleted successfully.";
        }
    }
}
