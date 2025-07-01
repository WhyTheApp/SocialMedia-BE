using FluentValidation;
using SocialMedia.Business.Helpers;

namespace SocialMedia.API.Requests.Authentication;

public class LocalRegisterRequest
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LocalRegisterRequestValidator : AbstractValidator<LocalRegisterRequest>
{

    public LocalRegisterRequestValidator()
    {
        RuleFor(registerRequest => registerRequest.Email).Must(EmailHelper.IsValidEmail);
        RuleFor(registerRequest => registerRequest.Password).Must(PasswordHelper.IsValidPassword);
    }
}