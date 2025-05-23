using System.Text.Json;
using System.Text.Json.Serialization;

namespace IIdentifii.Blog.Tests
{
    public class BlogControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {

        public BlogControllerTests()
        {
        }

        [Fact]
        public async Task GetBlogPosts_ShouldReturnSuccess()
        {
            // Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient(services =>
            {
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                db.SaveChanges();
            });

            await TestHelpers.PreAuthAsync(client);

            BlogPostRequest blogPostRequest = new BlogPostRequest();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "/api/blog/post")
            {
                Content = JsonContent.Create(blogPostRequest)
            };

            HttpResponseMessage response = await client.SendAsync(request, CancellationToken.None);

            response.EnsureSuccessStatusCode();

            JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new JsonStringEnumConverter());

            PagedApiResponse<BlogPost>? pagedResponse = await response.Content.ReadFromJsonAsync<PagedApiResponse<BlogPost>>(jsonOptions);

            pagedResponse.Should().NotBeNull();
        }
    }
}
