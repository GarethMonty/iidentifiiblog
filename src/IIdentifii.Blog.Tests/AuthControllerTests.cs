namespace IIdentifii.Blog.Tests
{
    public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        [Fact]
        public async Task GetAuthResponse_ShouldReturnSuccess()
        {
            //Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient();

            LoginRequest loginRequest = new LoginRequest
            {
                Email = SeedDataConstants.UserEmail,
                Password = SeedDataConstants.UserPassword
            };

            HttpResponseMessage response = await client.PostAsJsonAsync("/api/auth/login", loginRequest, CancellationToken.None);

            response.EnsureSuccessStatusCode();

            //Act
            ApiResponse<LoginResponse>? loginResponse = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();

            //Assert
            loginResponse.Should().NotBeNull();
            loginResponse.Code.Should().Be(StatusCodes.Status200OK);
            loginResponse.HasData.Should().BeTrue();
            loginResponse.ErrorMessage.Should().BeNull();
            loginResponse.Exception.Should().BeNull();
            loginResponse.Data.Should().NotBeNull();
            loginResponse.Data.Token.Should().NotBeNullOrEmpty();
            loginResponse.Data.ValidTo.Should().BeAfter(DateTime.UtcNow);
        }

        [Fact]
        public async Task GetAuthResponse_ShouldReturnBadRequest()
        {
            //Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient();

            object wrongObject = new
            {
                WrongProperty = "WrongValue"
            };
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/auth/login", wrongObject, CancellationToken.None);

            //Act
            ApiResponse<LoginResponse>? loginResponse = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();

            //Assert
            loginResponse.Should().NotBeNull();
            loginResponse.Code.Should().Be(StatusCodes.Status400BadRequest);
            loginResponse.HasData.Should().BeFalse();
            loginResponse.Exception.Should().BeNull();
        }

        [Fact]
        public async Task GetAuthResponse_ShouldReturnUnauthrorized()
        {
            //Arrange
            (CustomWebApplicationFactory<Program> factory, HttpClient client) = TestHelpers.GetClient();

            LoginRequest loginRequest = new LoginRequest
            {
                Email = "random@example.com",
                Password = "DoesNotExist"
            };
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/auth/login", loginRequest, CancellationToken.None);

            //Act
            ApiResponse<LoginResponse>? loginResponse = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();

            //Assert
            loginResponse.Should().NotBeNull();
            loginResponse.Code.Should().Be(StatusCodes.Status401Unauthorized);
            loginResponse.HasData.Should().BeFalse();
            loginResponse.ErrorMessage.Should().Be("Invalid credentials");
            loginResponse.Exception.Should().BeNull();
        }

    }
}
