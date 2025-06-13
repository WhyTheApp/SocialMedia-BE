namespace SocialMedia.Business.Models.Authentication;

public class LocalLoginRequestDTO
{
    public string EmailOrUsername { get; set; }
    public string Password { get; set; }
}