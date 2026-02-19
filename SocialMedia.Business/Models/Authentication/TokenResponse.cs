using System.Text.Json.Serialization;

namespace SocialMedia.Business.Models.Authentication;

public class TokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("id_token")]
    public string IdToken { get; set; }
}