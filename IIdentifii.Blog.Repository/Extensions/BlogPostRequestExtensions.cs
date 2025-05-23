namespace IIdentifii.Blog.Shared
{
    internal static class BlogPostRequestExtensions
    {
        public static (IQueryable<BlogPostModel>, IFilterable) BeginFilter(
            this IQueryable<BlogPostModel> query,
            BlogPostRequest blogPostRequest)
        {
            query = query
                .AsNoTracking()
                .Include(x => x.Author)
                .Include(x => x.Tags)
                    .ThenInclude(t=> t.Moderator)
                .Include(x => x.ReactionAggregates);

            return (query, blogPostRequest);
        }

        public static (IQueryable<BlogPostModel>, IFilterable) ApplyAuthorFilter(
            this (IQueryable<BlogPostModel> Query, IFilterable Request) filterChain)
        {
            if(filterChain.Request is not IAuthorFilterable authorFilterable)
            {
                return filterChain;
            }

            if(authorFilterable.AuthorId is null)
            {
                return filterChain;
            }

            filterChain.Query = filterChain.Query
                .Where(x => x.AuthorId == authorFilterable.AuthorId);

            return filterChain;
        }

        public static (IQueryable<BlogPostModel>, IFilterable) ApplyDateFilter(
            this (IQueryable<BlogPostModel> Query, IFilterable Request) filterChain)
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
                    .Where(x => x.PostedAt >= dateFilterable.DateFilter.From);
            }

            if (dateFilterable.DateFilter.To.HasValue)
            {
                filterChain.Query = filterChain.Query
                    .Where(x => x.PostedAt <= dateFilterable.DateFilter.To);
            }

            return filterChain;
        }

        public static (IQueryable<BlogPostModel>, IFilterable) ApplyTextQueryFilter(
            this (IQueryable<BlogPostModel> Query, IFilterable Request) filterChain)
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
                .Where(x => x.Title.Contains(queryText) || x.Content.Contains(queryText));

            return filterChain;
        }

        public static (IQueryable<BlogPostModel>, IFilterable) ApplyReactionFilter(
            this (IQueryable<BlogPostModel> Query, IFilterable Request) filterChain)
        {
            if (filterChain.Request is not IReactionFilterable reactionFilterable)
            {
                return filterChain;
            }

            if (reactionFilterable.ReactionFilter is null)
            {
                return filterChain;
            }

            if (reactionFilterable.ReactionFilter.IncludeEntities)
            {
                filterChain.Query = filterChain.Query
                    .Include(x => x.Reactions);

                if (reactionFilterable.ReactionFilter.ExcludedTypes?.Any() == true)
                {
                    filterChain.Query = filterChain.Query
                        .IncludeFilter(x => x.Reactions
                        .Where(r => !reactionFilterable.ReactionFilter.ExcludedTypes.Contains(r.Type)));
                }
            }

            if (reactionFilterable.ReactionFilter.ExcludedTypes?.Any() == true)
            {
                filterChain.Query = filterChain.Query
                    .IncludeFilter(x => x.ReactionAggregates
                    .Where(r => !reactionFilterable.ReactionFilter.ExcludedTypes.Contains(r.Type)));
            }

            return filterChain;
        }

        public static (IQueryable<BlogPostModel>, IFilterable) ApplyTagFilter(
            this (IQueryable<BlogPostModel> Query, IFilterable Request) filterChain)
        {
            if (filterChain.Request is not ITagFilterable tagFilterable)
            {
                return filterChain;
            }

            if (tagFilterable.TagFilter is null || tagFilterable.TagFilter.Tags.Count == 0)
            {
                return filterChain;
            }

            filterChain.Query = filterChain.Query
                .Where(post => post.Tags.Any(tag => tagFilterable.TagFilter.Tags.Contains(tag.Type)));

            return filterChain;
        }

        public static (IQueryable<BlogPostModel>, IFilterable) ApplySort(
            this (IQueryable<BlogPostModel> Query, IFilterable Request) filterChain)
        {
            if (filterChain.Request is not ISortable sortable)
            {
                return filterChain;
            }

            if (sortable.Sort is null)
            {
                return filterChain;
            }

            bool descending = sortable.Sort.SortOrder == SortOrderType.Descending;

            filterChain.Query = sortable.Sort.SortBy switch
            {
                SortByType.Title => descending
                    ? filterChain.Query.OrderByDescending(x => x.Title)
                    : filterChain.Query.OrderBy(x => x.Title),

                SortByType.Reactions => descending
                    ? filterChain.Query.OrderByDescending(x => x.ReactionAggregates.Sum(r => r.Count))
                    : filterChain.Query.OrderBy(x => x.ReactionAggregates.Sum(r => r.Count)),

                SortByType.Likes => descending
                    ? filterChain.Query.OrderByDescending(x => x.ReactionAggregates
                        .Where(r => r.Type == ReactionType.Like)
                        .Sum(r => r.Count))
                    : filterChain.Query.OrderBy(x => x.ReactionAggregates
                        .Where(r => r.Type == ReactionType.Like)
                        .Sum(r => r.Count)),

                SortByType.Tags => descending
                    ? filterChain.Query.OrderByDescending(x => x.Tags.Count)
                    : filterChain.Query.OrderBy(x => x.Tags.Count),

                _ => descending
                    ? filterChain.Query.OrderByDescending(x => x.PostedAt)
                    : filterChain.Query.OrderBy(x => x.PostedAt)
            };

            return filterChain;
        }

        public static (IQueryable<BlogPostModel>, IFilterable) ApplyPaging(
            this (IQueryable<BlogPostModel> Query, IFilterable Request) filterChain)
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

        public static IQueryable<BlogPostModel> BuildFilter(
            this (IQueryable<BlogPostModel> Query, IFilterable Request) filterChain)
        {
            return filterChain.Query;
        }

    }
}
