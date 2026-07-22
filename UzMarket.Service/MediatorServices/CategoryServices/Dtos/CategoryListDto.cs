namespace UzMarket.ServiceLayer.MediatorServices.CategoryServices.Dtos
{
    public class CategoryListDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public long? ParentCategoryId { get; set; }
    }
}
