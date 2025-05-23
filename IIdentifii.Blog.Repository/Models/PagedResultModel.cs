namespace IIdentifii.Blog.Repository
{
    public record PagedResultModel<T>
    {
        public List<T> Items { get; set; } = new List<T>();

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public static PagedResultModel<T> CreateFromRequest(
            PagingRequest? pagingRequest, 
            List<T>? items = null, 
            int totalCount = 0)
        {
            return new PagedResultModel<T>
            {
                Items = items ?? new List<T>(),
                Page = pagingRequest?.Page ?? 1,
                PageSize = pagingRequest?.PageSize ?? 20,
                TotalCount = totalCount,
            };
        }
    }
}
