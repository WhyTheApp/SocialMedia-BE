namespace SocialMedia.API.Requests.Authentication;

public class LocalLoginRequest
{
    public string EmailOrUsername { get; set; }
    public string Password { get; set; }
}