using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IIdentifii.Blog.Tests
{
    internal static class TestHelpers
    {
        public static (CustomWebApplicationFactory<Program>, HttpClient) GetClient(
            Action<IServiceProvider>? seedCallback = null)
        {
            CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>
            {
                SeedCallback = seedCallback
            };

            HttpClient client = factory.CreateClient();

            return (factory, client);
        }

        public static async Task PreAuthAsync(
            HttpClient client, 
            string email = SeedDataConstants.UserEmail, 
            string password = SeedDataConstants.UserPassword)
        {
            LoginRequest loginRequest = new LoginRequest
            {
                Email = email,
                Password = password
            };
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/auth/login", loginRequest, CancellationToken.None);

            response.EnsureSuccessStatusCode();

            ApiResponse<LoginResponse>? loginResponse = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();

            loginResponse.Should().NotBeNull();
            loginResponse.Code.Should().Be(StatusCodes.Status200OK);
            loginResponse.HasData.Should().BeTrue();
            loginResponse.ErrorMessage.Should().BeNull();
            loginResponse.Exception.Should().BeNull();
            loginResponse.Data.Should().NotBeNull();
            loginResponse.Data.Token.Should().NotBeNullOrEmpty();
            loginResponse.Data.ValidTo.Should().BeAfter(DateTime.UtcNow);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse?.Data.Token);
        }

        public static async Task<T?> ReadResponse<T>(HttpResponseMessage response)
        {
            JsonSerializerOptions options = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };

            return await response.Content.ReadFromJsonAsync<T>(options);
        }

        public static HttpRequestMessage JsonRequest(
            HttpMethod method, 
            string url, 
            object? body = null)
        {
            if(body == null)
            {
                return new HttpRequestMessage(method, url);
            }

            return new HttpRequestMessage(method, url)
            {
                Content = JsonContent.Create(body, options: new JsonSerializerOptions
                {
                    Converters = { new JsonStringEnumConverter() }
                })
            };
        }

    }
}
