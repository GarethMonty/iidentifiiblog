using System.Text.Json.Serialization;

namespace IIdentifii.Blog.Shared
{
    public record AuthSettings
    {
        public const string Key = nameof(AuthSettings);

        [JsonPropertyName("secret")]
        public string Secret { get; set; }

        [JsonPropertyName("issuer")]
        public string Issuer { get; set; }

        [JsonPropertyName("audience")]
        public string Audience { get; set; }

        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; } = new List<string>();

        [JsonPropertyName("allowedUserNameCharacters")]
        public string AllowedUserNameCharacters { get; set; }

        [JsonPropertyName("loginPath")]
        public string LoginPath { get; set; }

        [JsonPropertyName("accessDeniedPath")]
        public string AccessDeniedPath { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Secret))
            {
                throw new ArgumentException("Secret cannot be null or empty.", nameof(Secret));
            }
            else if (string.IsNullOrWhiteSpace(Issuer))
            {
                throw new ArgumentException("Issuer cannot be null or empty.", nameof(Issuer));
            }
            else if (string.IsNullOrWhiteSpace(Audience))
            {
                throw new ArgumentException("Audience cannot be null or empty.", nameof(Audience));
            }
            else if (Roles == null || Roles.Count == 0)
            {
                throw new ArgumentException("Roles cannot be null or empty.", nameof(Roles));
            }
            else if (string.IsNullOrWhiteSpace(AllowedUserNameCharacters))
            {
                throw new ArgumentException("AllowedUserNameCharacters cannot be null or empty.", nameof(AllowedUserNameCharacters));
            }
            else if (string.IsNullOrWhiteSpace(LoginPath))
            {
                throw new ArgumentException("LoginPath cannot be null or empty.", nameof(LoginPath));
            }
            else if (string.IsNullOrWhiteSpace(AccessDeniedPath))
            {
                throw new ArgumentException("AccessDeniedPath cannot be null or empty.", nameof(AccessDeniedPath));
            }
        }
    }
}
