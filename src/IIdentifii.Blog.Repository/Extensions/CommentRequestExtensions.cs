namespace IIdentifii.Blog.Repository
{
    internal static class CommentRequestExtensions
    {
        public static (IQueryable<CommentModel>, IFilterable) BeginFilter(
            this IQueryable<CommentModel> query,
            CommentRequest commentRequest)
        {
            query = query
                .AsNoTracking()
                .Include(c => c.User)
                .AsQueryable();

            return (query, commentRequest);
        }
        public static (IQueryable<CommentModel>, IFilterable) ApplyBlogPostFilter(
            this (IQueryable<CommentModel> Query, IFilterable Request) filterChain)
        {
            if (filterChain.Request is not IBlobPostFilterable blobPostFilterable)
            {
                return filterChain;
            }

            if (blobPostFilterable.BlogPostId is null)
            {
                return filterChain;
            }

            filterChain.Query = filterChain.Query
                .Where(x => x.BlogPostId == blobPostFilterable.BlogPostId);

            return filterChain;
        }

        public static (IQueryable<CommentModel>, IFilterable) ApplyUserFilter(
            this (IQueryable<CommentModel> Query, IFilterable Request) filterChain)
        {
            if (filterChain.Request is not IUserFilterable userFilterable)
            {
                return filterChain;
            }

            if (userFilterable.UserId is null)
            {
                return filterChain;
            }

            filterChain.Query = filterChain.Query
                .Where(x => x.UserId == userFilterable.UserId);

            return filterChain;
        }

        public static (IQueryable<CommentModel>, IFilterable) ApplyDateFilter(
            this (IQueryable<CommentModel> Query, IFilterable Request) filterChain)
        {
            if (filterChain.Request is not IDateFilterable dateFilterable)
            {
                return filterChain;
            }

            if (dateFilterable.DateFilter is null)
            {
                return filterChain;
            }

            if (dateFilterable.DateFilter.From.HasValue)
            {
                filterChain.Query = filterChain.Query
                    .Where(x => x.CreatedAt >= dateFilterable.DateFilter.From);
            }

            if (dateFilterable.DateFilter.To.HasValue)
            {
                filterChain.Query = filterChain.Query
                    .Where(x => x.CreatedAt <= dateFilterable.DateFilter.To);
            }

            return filterChain;
        }

        public static (IQueryable<CommentModel>, IFilterable) ApplyTextQueryFilter(
            this (IQueryable<CommentModel> Query, IFilterable Request) filterChain)
        {
            if (filterChain.Request is not ITextFilterable textFilterable)
            {
                return filterChain;
            }

            if (string.IsNullOrWhiteSpace(textFilterable.Filter?.Query))
            {
                return filterChain;
            }

            string queryText = textFilterable.Filter.Query;

            filterChain.Query = filterChain.Query
                .Where(x => x.Content.Contains(queryText));

            return filterChain;
        }

        public static (IQueryable<CommentModel>, IFilterable) ApplyPaging(
            this (IQueryable<CommentModel> Query, IFilterable Request) filterChain)
        {
            if (filterChain.Request is not IPagingFilterable pagingFilterable)
            {
                return filterChain;
            }

            if (pagingFilterable.Paging is null)
            {
                return filterChain;
            }

            filterChain.Query = filterChain.Query
                .Skip((pagingFilterable.Paging.Page - 1) * pagingFilterable.Paging.PageSize)
                .Take(pagingFilterable.Paging.PageSize);

            return filterChain;
        }

        public static IQueryable<CommentModel> BuildFilter(
            this (IQueryable<CommentModel> Query, IFilterable Request) filterChain)
        {
            return filterChain.Query;
        }

    }
}
