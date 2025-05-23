using Microsoft.AspNetCore.Builder;

namespace IIdentifii.Blog.Tests
{
    public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AuthControllerTests(
            CustomWebApplicationFactory<Program> factory)
        {
            factory.SeedCallback = services =>
            {
                SeedTestData(services).GetAwaiter().GetResult();
            };

            _client = factory.CreateClient();
        }

        private static async Task SeedTestData(IServiceProvider serviceProvider)
        {
            UserManager<IIdentifiiUser> userManager = serviceProvider.GetRequiredService<UserManager<IIdentifiiUser>>();
            RoleManager<IdentityRole<Guid>> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            AppDbContext db = serviceProvider.GetRequiredService<AppDbContext>();
        }


        [Fact]
        public async Task GetAuthResponse_ShouldReturnSuccess()
        {
            LoginRequest loginRequest = new LoginRequest
            {
                Email = SeedDataConstants.UserEmail,
                Password = SeedDataConstants.UserPassword
            };
            HttpResponseMessage response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest, CancellationToken.None);

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
        }
    }
}
