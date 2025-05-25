namespace IIdentifii.Blog.Shared
{
    /// <summary>
    /// Represents a filter that defines a date range.
    /// </summary>
    public record DateFilterRequest
    {
        /// <summary>
        /// The start date for filtering.
        /// </summary>
        [JsonPropertyName("from")]
        public DateTime? From { get; set; }

        /// <summary>
        /// The end date for filtering.
        /// </summary>
        [JsonPropertyName("to")]
        public DateTime? To { get; set; }

        /// <summary>
        /// Validates the date filter logic (at least one value must be set, and From cannot be after To).
        /// </summary>
        public void Validate()
        {
            if (From is null && To is null)
            {
                throw IIdentifiiException.Bad("At least one of From or To must be provided.");
            }
            else if (From is not null && To is not null && From > To)
            {
                throw IIdentifiiException.Bad("From date cannot be greater than To date.");
            }
        }
    }
}
