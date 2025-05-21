using FluentValidation;
using SocialMedia.Business.Helpers;

namespace SocialMedia.API.Requests;

public class AddRegisterRequest
{
    public string Email { get; set; }
    public string Feedback { get; set; }
}

public class AddRegisterRequestValidator : AbstractValidator<AddRegisterRequest>
{
    public AddRegisterRequestValidator()
    {
        RuleFor(request => request.Email).Must(EmailHelper.IsValidEmail);
        RuleFor(request => request.Feedback).Must(FeedbackHelper.isValidFeedback);
    }
}