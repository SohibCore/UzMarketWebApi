namespace UzMarket.ServiceLayer.MediatorServices.CategoryServices.Dtos
{
    public class CategoryFilterDto
    {
        public long? Id { get; set; }
        public string? Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
