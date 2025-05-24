using IIdentifii.Blog.Shared;

namespace IIdentifii.Blog.Tests
{
    public class CommentControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        [Fact]
        public async Task GetComments_ShouldReturnAll_WhenNoFilters()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            CommentRequest request = new CommentRequest();

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Get, $"/api/blog/post/{SeedDataConstants.BlogPostId}/comment", request));

            // Assert
            response.EnsureSuccessStatusCode();

            PagedApiResponse<Comment>? result = await TestHelpers.ReadResponse<PagedApiResponse<Comment>>(response);

            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetComments_ShouldFilterByAuthor()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            Guid userId = SeedDataConstants.UserId;
            CommentRequest request = new CommentRequest
            {
                UserId = userId
            };

            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Get, $"/api/blog/post/{SeedDataConstants.BlogPostId}/comment", request));

            response.EnsureSuccessStatusCode();

            PagedApiResponse<Comment>? result = await TestHelpers.ReadResponse<PagedApiResponse<Comment>>(response);
            result.Data.Should().OnlyContain(p => p.User.Id == userId);
        }

        [Fact]
        public async Task GetComments_ShouldFilterByDateRange()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            CommentRequest request = new CommentRequest
            {
                DateFilter = new DateFilterRequest
                {
                    From = DateTime.UtcNow.AddDays(-7),
                    To = DateTime.UtcNow.AddDays(1)
                }
            };

            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Get, $"/api/blog/post/{SeedDataConstants.BlogPostId}/comment", request));

            response.EnsureSuccessStatusCode();

            PagedApiResponse<Comment>? result = await TestHelpers.ReadResponse<PagedApiResponse<Comment>>(response);
            result.Data.Should().OnlyContain(p => p.CreatedAt >= request.DateFilter.From && p.CreatedAt <= request.DateFilter.To);
        }

        [Fact]
        public async Task GetComments_ShouldReturnBadRequest_WhenFromAfterTo()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            CommentRequest request = new CommentRequest
            {
                DateFilter = new DateFilterRequest
                {
                    From = DateTime.UtcNow,
                    To = DateTime.UtcNow.AddDays(-1)
                }
            };

            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Get, $"/api/blog/post/{SeedDataConstants.BlogPostId}/comment", request));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetComments_ShouldPageResults()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            CommentRequest request = new CommentRequest
            {
                Paging = new PagingRequest
                {
                    Page = 1,
                    PageSize = 1
                }
            };

            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Get, $"/api/blog/post/{SeedDataConstants.BlogPostId}/comment", request));

            response.EnsureSuccessStatusCode();

            var result = await TestHelpers.ReadResponse<PagedApiResponse<Comment>>(response);
            result.Data.Count.Should().BeLessOrEqualTo(1);
        }

        [Fact]
        public async Task GetComments_ShouldReturnBadRequest_WhenPageNumberIsZero()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            var request = new CommentRequest
            {
                Paging = new PagingRequest
                {
                    Page = 0,
                    PageSize = 10
                }
            };

            var response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Get, $"/api/blog/post/{SeedDataConstants.BlogPostId}/comment", request));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetComment_ShouldReturn()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Get, $"/api/blog/post/{SeedDataConstants.BlogPostId}/comment/{SeedDataConstants.CommentId}"));

            // Assert
            response.EnsureSuccessStatusCode();

            ApiResponse<Comment>? result = await TestHelpers.ReadResponse<ApiResponse<Comment>>(response);

            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(SeedDataConstants.CommentId);
        }

        [Fact]
        public async Task GetComment_ShouldNotReturn()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Get, $"/api/blog/post/{SeedDataConstants.BlogPostId}/comment/{Guid.NewGuid()}"));

            // Assert
            ApiResponse<Comment>? result = await TestHelpers.ReadResponse<ApiResponse<Comment>>(response);

            result.Should().NotBeNull();
            result.Code.Should().Be(StatusCodes.Status404NotFound);
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task CreateComment_GoodValue()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            CreateCommentRequest createCommentRequest = new CreateCommentRequest()
            {
                Content = "This is test content that must be atleast 1 characters"
            };

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Post, $"/api/blog/post/{SeedDataConstants.BlogPostId}/comment", createCommentRequest));

            // Assert
            response.EnsureSuccessStatusCode();

            ApiResponse<Comment>? result = await TestHelpers.ReadResponse<ApiResponse<Comment>>(response);

            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.Content.Should().Be(createCommentRequest.Content);
        }

        [Fact]
        public async Task CreateComment_BadValue()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            UpdateCommentRequest createCommentRequest = new UpdateCommentRequest()
            {
                Content = new string('*', 5000),
            };

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Post, $"/api/blog/post/{SeedDataConstants.BlogPostId}/comment", createCommentRequest));

            // Assert
            ApiResponse<Comment>? result = await TestHelpers.ReadResponse<ApiResponse<Comment>>(response);

            result.Should().NotBeNull();
            result.Code.Should().Be(StatusCodes.Status400BadRequest);
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task UpdateComment_GoodValue()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            UpdateCommentRequest updateCommentRequest = new UpdateCommentRequest()
            {
                Content = "Updated Test Content"
            };

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Patch, $"/api/blog/post/{SeedDataConstants.BlogPostId}/comment", updateCommentRequest));

            // Assert
            // Assert
            response.EnsureSuccessStatusCode();

            ApiResponse<Comment>? result = await TestHelpers.ReadResponse<ApiResponse<Comment>>(response);

            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.Content.Should().Be(updateCommentRequest.Content);
        }

        [Fact]
        public async Task UpdateComment_BadValue()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            UpdateCommentRequest updateCommentRequest = new UpdateCommentRequest()
            {
                Content = new string('*', 500),
            };

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Patch, $"/api/blog/post/{SeedDataConstants.BlogPostId}/comment", updateCommentRequest));

            // Assert
            ApiResponse<Comment>? result = await TestHelpers.ReadResponse<ApiResponse<Comment>>(response);

            result.Should().NotBeNull();
            result.Code.Should().Be(StatusCodes.Status400BadRequest);
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task DeleteComment_GoodValue()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Delete, $"/api/blog/post/{SeedDataConstants.BlogPostId}/comment/{SeedDataConstants.CommentId}"));

            // Assert
            response.EnsureSuccessStatusCode();

            ApiResponse<bool>? result = await TestHelpers.ReadResponse<ApiResponse<bool>>(response);

            result.Should().NotBeNull();
            result.Data.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteComment_BadValue()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Delete, $"/api/blog/post/{SeedDataConstants.BlogPostId}/comment/{Guid.NewGuid()}"));

            // Assert
            ApiResponse<bool>? result = await TestHelpers.ReadResponse<ApiResponse<bool>>(response);

            result.Should().NotBeNull();
            result.Code.Should().Be(StatusCodes.Status400BadRequest);
            result.Data.Should().BeFalse();
        }
    }
}
