namespace UzMarket.RepositoryLayer.Dtos.CategoryDtos
{
    public class CreateCategoryDlDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public long? ParentCategoryId { get; set; }
    }
}
