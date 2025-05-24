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

        [Fact]
        public async Task GetBlogPost_ShouldReturn()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Get, $"/api/blog/post/{SeedDataConstants.BlogPostId}"));

            // Assert
            response.EnsureSuccessStatusCode();

            ApiResponse<BlogPost>? result = await TestHelpers.ReadResponse<ApiResponse<BlogPost>>(response);

            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(SeedDataConstants.BlogPostId);
        }

        [Fact]
        public async Task GetBlogPost_ShouldNotReturn()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Get, $"/api/blog/post/{Guid.NewGuid()}"));

            // Assert
            ApiResponse<BlogPost>? result = await TestHelpers.ReadResponse<ApiResponse<BlogPost>>(response);

            result.Should().NotBeNull();
            result.Code.Should().Be(StatusCodes.Status404NotFound);
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task CreateBlogPost_GoodValue()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            CreateBlogPostRequest createBlogPostRequest = new CreateBlogPostRequest()
            {
                Title = "Test Title",
                Content = "This is test content that must be atleast 20 characters"
            };

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Post, $"/api/blog/post", createBlogPostRequest));

            // Assert
            response.EnsureSuccessStatusCode();

            ApiResponse<BlogPost>? result = await TestHelpers.ReadResponse<ApiResponse<BlogPost>>(response);

            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.Title.Should().Be(createBlogPostRequest.Title);
            result.Data.Content.Should().Be(createBlogPostRequest.Content);
        }

        [Fact]
        public async Task CreateBlogPost_BadValue()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            UpdateBlogPostRequest createBlogPostRequest = new UpdateBlogPostRequest()
            {
                Title = new string('*', 50),
                Content = new string('*', 5000),
            };

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Post, $"/api/blog/post", createBlogPostRequest));

            // Assert
            ApiResponse<BlogPost>? result = await TestHelpers.ReadResponse<ApiResponse<BlogPost>>(response);

            result.Should().NotBeNull();
            result.Code.Should().Be(StatusCodes.Status400BadRequest);
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task UpdateBlogPost_FeatureNotAvailable()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            UpdateBlogPostRequest updateBlogPostRequest = new UpdateBlogPostRequest()
            {
                Id = SeedDataConstants.BlogPostId,
                Title = "Updated Test Title",
                Content = "Updated Test Content"
            };

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Patch, $"/api/blog/post/{SeedDataConstants.BlogPostId}", updateBlogPostRequest));

            // Assert
            ApiResponse<BlogPost>? result = await TestHelpers.ReadResponse<ApiResponse<BlogPost>>(response);

            result.Should().NotBeNull();
            result.Code.Should().Be(StatusCodes.Status501NotImplemented);
        }

        [Fact]
        public async Task DeleteBlogPost_FeatureNotAvailable()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Delete, $"/api/blog/post/{SeedDataConstants.BlogPostId}"));

            // Assert
            ApiResponse<bool?>? result = await TestHelpers.ReadResponse<ApiResponse<bool?>>(response);

            result.Should().NotBeNull();
            result.Code.Should().Be(StatusCodes.Status501NotImplemented);
        }
    }
}
