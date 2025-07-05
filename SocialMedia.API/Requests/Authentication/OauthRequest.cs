namespace SocialMedia.API.Requests.Authentication;

public class OauthRequest
{
    public string Code { get; set; }
    public string Verifier { get; set; }
    public string RedirectUri { get; set; }
}