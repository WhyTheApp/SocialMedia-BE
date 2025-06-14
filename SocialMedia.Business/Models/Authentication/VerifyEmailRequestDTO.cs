namespace SocialMedia.Business.Models.Authentication;

public class VerifyEmailRequestDTO
{
    public Guid UserId { get; set; }
    public string Code { get; set; }
}