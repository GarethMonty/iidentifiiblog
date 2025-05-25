namespace IIdentifii.Blog.Shared
{
    public class LoginResponse200Example : IExamplesProvider<LoginResponse>
    {
        public LoginResponse GetExamples() => new()
        {
            Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImI2NjczNGNjLTg1ODQtNDk0Yy02YjI5LTA4ZGQ5OTFmODFlYiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ1c2VyQGV4YW1wbGUuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZW1haWxhZGRyZXNzIjoidXNlckBleGFtcGxlLmNvbSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlVzZXIiLCJleHAiOjE3NDgwMTEzOTUsImlzcyI6IklJZGVudGlmaWkuQmxvZyIsImF1ZCI6IkJsb2cifQ.aD8cZdrw3fAP64hd9O50Wus2BgnOvS-F76YoODShT4w",
            ValidTo = DateTime.UtcNow.AddHours(3),
        };
    }
}
