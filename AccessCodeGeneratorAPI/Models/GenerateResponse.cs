namespace AccessCodeGenerator.API.Models
{
    public class GenerateResponse
    {
        public string Code { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
