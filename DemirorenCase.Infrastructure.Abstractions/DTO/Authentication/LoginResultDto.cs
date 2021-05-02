using Newtonsoft.Json;

namespace DemirorenCase.Infrastructure.Abstractions.DTO.Authentication
{
    public class LoginResultDto
    {
        [JsonProperty("access_token")] public string Token { get; set; }
        [JsonProperty("expires_in")] public int ExpiresIn { get; set; }
    }
}