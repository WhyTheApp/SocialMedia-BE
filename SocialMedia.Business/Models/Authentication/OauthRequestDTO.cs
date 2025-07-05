namespace SocialMedia.Business.Models.Authentication;

public class OauthRequestDTO
{
    public string Code { get; set; }
    public string Verifier { get; set; }
    public string RedirectUri { get; set; }
}