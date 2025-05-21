namespace IIdentifii.Blog.Shared
{
    public record DateFilterRequest
    {
        [JsonPropertyName("from")]
        public DateTime? From { get; set; }

        [JsonPropertyName("to")]
        public DateTime? To { get; set; }

        public void Validate()
        {
            if (From is null && To is null)
            {
                throw new ArgumentException("At least one of From or To must be provided.");
            }
            else if (From is not null && To is not null && From > To)
            {
                throw new ArgumentException("From date cannot be greater than To date.");
            }
        }
    }
}
