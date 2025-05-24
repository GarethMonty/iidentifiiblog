namespace IIdentifii.Blog.Tests
{
    public class ReactionControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        [Fact]
        public async Task GetReactions_Like_ShouldReturnAllForPost()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Get, $"/api/blog/post/{SeedDataConstants.BlogPostId}/reaction/like"));

            // Assert
            response.EnsureSuccessStatusCode();

            PagedApiResponse<Reaction>? result = await TestHelpers.ReadResponse<PagedApiResponse<Reaction>>(response);

            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetReactionCount_ShouldReturn()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Get, $"/api/blog/post/{SeedDataConstants.BlogPostId}/reaction/like/count"));

            // Assert
            response.EnsureSuccessStatusCode();

            ApiResponse<int>? result = await TestHelpers.ReadResponse<ApiResponse<int>>(response);

            result.Should().NotBeNull();
            result.Data.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task CreateReaction_GoodValue()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Post, $"/api/blog/post/{SeedDataConstants.BlogPostId}/reaction/like"));

            // Assert
            response.EnsureSuccessStatusCode();

            ApiResponse<Reaction>? result = await TestHelpers.ReadResponse<ApiResponse<Reaction>>(response);

            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.Type.Should().Be(ReactionType.Like);
        }

        [Fact]
        public async Task CreateReaction_BadValue()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Post, $"/api/blog/post/{SeedDataConstants.BlogPostId}/reaction/bob"));

            // Assert
            ApiResponse<Reaction>? result = await TestHelpers.ReadResponse<ApiResponse<Reaction>>(response);

            result.Should().NotBeNull();
            result.Code.Should().Be(StatusCodes.Status400BadRequest);
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task ChangeReaction_GoodValue()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Patch, $"/api/blog/post/{SeedDataConstants.BlogPostId}/reaction/love?previous=like"));

            // Assert
            ApiResponse<Reaction>? result = await TestHelpers.ReadResponse<ApiResponse<Reaction>>(response);

            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.Type.Should().Be(ReactionType.Love);
        }

        [Fact]
        public async Task DeleteReaction_GoodValue()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Delete, $"/api/blog/post/{SeedDataConstants.BlogPostId}/reaction/like"));

            // Assert

            ApiResponse<bool>? result = await TestHelpers.ReadResponse<ApiResponse<bool>>(response);

            result.Should().NotBeNull();
            result.Data.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteReaction_BadValue()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Delete, $"/api/blog/post/{SeedDataConstants.BlogPostId}/reaction/love"));

            // Assert

            ApiResponse<bool>? result = await TestHelpers.ReadResponse<ApiResponse<bool>>(response);

            result.Should().NotBeNull();
            result.Data.Should().BeFalse();
        }
    }
}
