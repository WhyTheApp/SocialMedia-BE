namespace SocialMedia.Business.Services.Authentication;

public interface IEmailerService
{
    Task SendVerificationEmailAsync(string toName, string toEmail, string code);
}