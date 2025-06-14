namespace SocialMedia.Business.Models.Authentication;

public class RefreshTokenResponse
{
    public string JwtToken { get; set; }
    public string RefreshToken { get; set; }
}