namespace UzMarket.RepositoryLayer.Dtos.CategoryDtos
{
    public class UpdateCategoryDlDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public long? ParentCategoryId { get; set; }
    }
}
