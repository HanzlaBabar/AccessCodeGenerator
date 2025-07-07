namespace AccessCodeGenerator.API.Models
{
    public class ValidateResponse
    {
        public bool Valid { get; set; }
        public bool Expired { get; set; }
        public Dictionary<string, string>? Payload { get; set; }
    }
}
