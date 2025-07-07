namespace AccessCodeGenerator.API.Models
{
    public class GenerateRequest
    {
        public int ExpiresInSeconds { get; set; }
        public Dictionary<string, string> Payload { get; set; }
    }
}
