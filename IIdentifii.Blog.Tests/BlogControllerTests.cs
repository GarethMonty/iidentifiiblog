namespace IIdentifii.Blog.Tests
{
    public class BlogControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        [Fact]
        public async Task GetBlogPosts_ShouldReturnAll_WhenNoFilters()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            BlogPostRequest request = new BlogPostRequest();

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Get, "/api/blog/post", request));

            // Assert
            response.EnsureSuccessStatusCode();

            PagedApiResponse<BlogPost>? result = await TestHelpers.ReadResponse<PagedApiResponse<BlogPost>>(response);

            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetBlogPosts_ShouldFilterByAuthor()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            Guid authorId = SeedDataConstants.UserId;
            BlogPostRequest request = new BlogPostRequest
            {
                AuthorId = authorId
            };

            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Get, "/api/blog/post", request));

            response.EnsureSuccessStatusCode();

            PagedApiResponse<BlogPost>? result = await TestHelpers.ReadResponse<PagedApiResponse<BlogPost>>(response);
            result.Data.Should().OnlyContain(p => p.Author.Id == authorId);
        }

        [Fact]
        public async Task GetBlogPosts_ShouldFilterByDateRange()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            BlogPostRequest request = new BlogPostRequest
            {
                DateFilter = new DateFilterRequest
                {
                    From = DateTime.UtcNow.AddDays(-7),
                    To = DateTime.UtcNow.AddDays(1)
                }
            };

            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Get, "/api/blog/post", request));

            response.EnsureSuccessStatusCode();

            PagedApiResponse<BlogPost>? result = await TestHelpers.ReadResponse<PagedApiResponse<BlogPost>>(response);
            result.Data.Should().OnlyContain(p => p.PostedAt >= request.DateFilter.From && p.PostedAt <= request.DateFilter.To);
        }

        [Fact]
        public async Task GetBlogPosts_ShouldReturnBadRequest_WhenFromAfterTo()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            BlogPostRequest request = new BlogPostRequest
            {
                DateFilter = new DateFilterRequest
                {
                    From = DateTime.UtcNow,
                    To = DateTime.UtcNow.AddDays(-1)
                }
            };

            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Get, "/api/blog/post", request));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetBlogPosts_ShouldPageResults()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            BlogPostRequest request = new BlogPostRequest
            {
                Paging = new PagingRequest
                {
                    Page = 1,
                    PageSize = 1
                }
            };

            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Get, "/api/blog/post", request));

            response.EnsureSuccessStatusCode();

            var result = await TestHelpers.ReadResponse<PagedApiResponse<BlogPost>>(response);
            result.Data.Count.Should().BeLessOrEqualTo(1);
        }

        [Fact]
        public async Task GetBlogPosts_ShouldSortByTitleAscending()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            BlogPostRequest request = new BlogPostRequest
            {
                Sort = new SortRequest
                {
                    SortBy = SortByType.Title,
                    SortOrder = SortOrderType.Ascending
                }
            };

            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Get, "/api/blog/post", request));

            response.EnsureSuccessStatusCode();

            PagedApiResponse<BlogPost>? result = await TestHelpers.ReadResponse<PagedApiResponse<BlogPost>>(response);

            result.Data.Should().BeInAscendingOrder(p => p.Title);
        }

        [Fact]
        public async Task GetBlogPosts_ShouldReturnBadRequest_WhenPageNumberIsZero()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            var request = new BlogPostRequest
            {
                Paging = new PagingRequest
                {
                    Page = 0,
                    PageSize = 10
                }
            };

            var response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Get, "/api/blog/post", request));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
