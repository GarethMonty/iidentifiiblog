namespace IIdentifii.Blog.Tests
{
    public class TagControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        [Fact]
        public async Task GetTags_ShouldReturn()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });
            await TestHelpers.PreAuthAsync(client);

            //Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Get, $"/api/blog/post/{SeedDataConstants.BlogPostId}/tag"));

            // Assert
            response.EnsureSuccessStatusCode();

            ApiResponse<List<Tag>>? result = await TestHelpers.ReadResponse<ApiResponse<List<Tag>>>(response);

            result.Should().NotBeNull();
            result.Data.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetTag_ShouldReturn()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Get, $"/api/blog/post/{SeedDataConstants.BlogPostId}/tag/{SeedDataConstants.TagId}"));

            // Assert
            response.EnsureSuccessStatusCode();

            ApiResponse<Tag>? result = await TestHelpers.ReadResponse<ApiResponse<Tag>>(response);

            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(SeedDataConstants.TagId);
        }

        [Fact]
        public async Task GetTag_ShouldNotReturn()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Get, $"/api/blog/post/{SeedDataConstants.BlogPostId}/tag/{Guid.NewGuid()}"));

            // Assert
            ApiResponse<Tag>? result = await TestHelpers.ReadResponse<ApiResponse<Tag>>(response);

            result.Should().NotBeNull();
            result.Code.Should().Be(StatusCodes.Status404NotFound);
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task CreateTag_GoodValue()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client, SeedDataConstants.ModeratorEmail, SeedDataConstants.ModeratorPassword);

            CreateTagRequest createTagRequest = new CreateTagRequest()
            {
                Type = TagType.Misleading,
            };

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Post, $"/api/blog/post/{SeedDataConstants.BlogPostId}/tag", createTagRequest));

            // Assert
            response.EnsureSuccessStatusCode();

            ApiResponse<Tag>? result = await TestHelpers.ReadResponse<ApiResponse<Tag>>(response);

            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.Type.Should().Be(createTagRequest.Type);
        }

        [Fact]
        public async Task CreateTag_WrongRole()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            CreateTagRequest createTagRequest = new CreateTagRequest()
            {
                Type = TagType.Misleading,
            };

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Post, $"/api/blog/post/{SeedDataConstants.BlogPostId}/tag", createTagRequest));

            // Assert
            ApiResponse<Tag>? result = await TestHelpers.ReadResponse<ApiResponse<Tag>>(response);

            result.Should().NotBeNull();
            result.Code.Should().Be(StatusCodes.Status401Unauthorized);
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task UpdateTag_GoodValue()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client, SeedDataConstants.ModeratorEmail, SeedDataConstants.ModeratorPassword);

            UpdateTagRequest updateTagRequest = new UpdateTagRequest()
            {
                Id = SeedDataConstants.TagId,
                Type = TagType.Misleading,
            };

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Patch, $"/api/blog/post/{SeedDataConstants.BlogPostId}/tag", updateTagRequest));

            // Assert
            // Assert
            response.EnsureSuccessStatusCode();

            ApiResponse<Tag>? result = await TestHelpers.ReadResponse<ApiResponse<Tag>>(response);

            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.Type.Should().Be(updateTagRequest.Type);
        }

        [Fact]
        public async Task UpdateTag_WrongRole()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            UpdateTagRequest updateTagRequest = new UpdateTagRequest()
            {
                Type = TagType.Misleading,
            };

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Patch, $"/api/blog/post/{SeedDataConstants.BlogPostId}/tag", updateTagRequest));

            // Assert
            ApiResponse<Tag>? result = await TestHelpers.ReadResponse<ApiResponse<Tag>>(response);

            result.Should().NotBeNull();
            result.Code.Should().Be(StatusCodes.Status401Unauthorized);
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task DeleteTag_GoodValue()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client, SeedDataConstants.ModeratorEmail, SeedDataConstants.ModeratorPassword);

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Delete, $"/api/blog/post/{SeedDataConstants.BlogPostId}/tag/{SeedDataConstants.TagId}"));

            // Assert
            response.EnsureSuccessStatusCode();

            ApiResponse<bool>? result = await TestHelpers.ReadResponse<ApiResponse<bool>>(response);

            result.Should().NotBeNull();
            result.Data.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteTag_WrongRole()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Delete, $"/api/blog/post/{SeedDataConstants.BlogPostId}/tag/{Guid.NewGuid()}"));

            // Assert
            ApiResponse<bool?>? result = await TestHelpers.ReadResponse<ApiResponse<bool?>>(response);

            result.Should().NotBeNull();
            result.Code.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task DeleteTag_BadValue()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client, SeedDataConstants.ModeratorEmail, SeedDataConstants.ModeratorPassword);

            // Act
            HttpResponseMessage response = await client.SendAsync(TestHelpers.JsonRequest(HttpMethod.Delete, $"/api/blog/post/{SeedDataConstants.BlogPostId}/tag/{Guid.NewGuid()}"));

            // Assert
            ApiResponse<bool>? result = await TestHelpers.ReadResponse<ApiResponse<bool>>(response);

            result.Should().NotBeNull();
            result.Code.Should().Be(StatusCodes.Status404NotFound);
        }
    }
}
