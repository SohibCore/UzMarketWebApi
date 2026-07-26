namespace UzMarket.ServiceLayer.MediatorServices.CategoryServices.Dtos
{
    public class CategoryDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public long? ParentCategoryId { get; set; }
    }
}
