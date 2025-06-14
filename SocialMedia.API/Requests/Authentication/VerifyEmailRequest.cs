namespace SocialMedia.API.Requests.Authentication;

public class VerifyEmailRequest
{
    public Guid UserId { get; set; }
    public string Code { get; set; }
}