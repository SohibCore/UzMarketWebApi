namespace UzMarket.Core.Common
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }

        public PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = pageSize;
            Items = items;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }
        public bool HasPreviousPage => PageNumber > 1; // Oldinda page bo'lsa - true aks holda false
        public bool HasNextPage => PageNumber < TotalPages; // Biz turgan page oxirgi bo'lmasa - true
    }
}
