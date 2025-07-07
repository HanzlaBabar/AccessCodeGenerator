using AccessCodeGenerator.API.Models;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AccessCodeGenerator.API.Services
{
    public class AccessCodeService
    {
        private const string SecretKey = "kzPvTxRBiGrfc1dj5XwK1k+iGV+jk7xu2n8dvnDhENM=\r\n";
        private readonly HMACSHA256 _hmac = new(Encoding.UTF8.GetBytes(SecretKey));

        public GenerateResponse GenerateCode(GenerateRequest request)
        {
            var expiresAt = DateTime.UtcNow.AddSeconds(request.ExpiresInSeconds);
            var payload = new
            {
                exp = expiresAt,
                data = request.Payload
            };

            var json = JsonSerializer.Serialize(payload);
            var payloadBytes = Encoding.UTF8.GetBytes(json);
            var encodedPayload = Base64UrlEncoder.Encode(payloadBytes);

            var signature = ComputeHmac(encodedPayload);
            var code = $"{encodedPayload}.{signature}";

            return new GenerateResponse
            {
                Code = code,
                ExpiresAt = expiresAt
            };
        }

        public ValidateResponse ValidateCode(string code)
        {
            var parts = code.Split('.');
            if (parts.Length != 2)
                return new ValidateResponse { Valid = false, Expired = false };

            var encodedPayload = parts[0];
            var providedSignature = parts[1];
            var expectedSignature = ComputeHmac(encodedPayload);

            if (providedSignature != expectedSignature)
                return new ValidateResponse { Valid = false, Expired = false };

            string json = Base64UrlEncoder.Decode(encodedPayload);
            using var doc = JsonDocument.Parse(json);

            var exp = doc.RootElement.GetProperty("exp").GetDateTime();
            var isExpired = exp < DateTime.UtcNow;

            Dictionary<string, string>? data = null;
            if (doc.RootElement.TryGetProperty("data", out var dataProp))
            {
                data = new();
                foreach (var prop in dataProp.EnumerateObject())
                    data[prop.Name] = prop.Value.GetString() ?? "";
            }

            return new ValidateResponse
            {
                Valid = !isExpired,
                Expired = isExpired,
                Payload = isExpired ? null : data
            };
        }

        private string ComputeHmac(string data)
        {
            var hash = _hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return Base64UrlEncoder.Encode(hash);
        }
    }
}
