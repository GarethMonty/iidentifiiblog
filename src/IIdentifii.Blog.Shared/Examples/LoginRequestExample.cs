namespace IIdentifii.Blog.Shared
{
    public class LoginRequestExample : IExamplesProvider<LoginRequest>
    {
        public LoginRequest GetExamples() => new()
        {
            Email = SeedDataConstants.UserEmail,
            Password = SeedDataConstants.UserPassword,
        };
    }
}
