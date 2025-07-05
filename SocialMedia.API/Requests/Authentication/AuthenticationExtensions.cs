using SocialMedia.API.Requests.Articles;
using SocialMedia.API.Requests.PagingAndFiltering;
using SocialMedia.Business.Models.Articles;
using SocialMedia.Business.Models.Authentication;
using SocialMedia.Business.Models.PagingAndFiltering;

namespace SocialMedia.API.Requests.Authentication;

public static class AuthenticationExtensions
{
    public static LocalRegisterRequestDTO ToLocalRegisterRequestDTO(this LocalRegisterRequest request) =>
        new LocalRegisterRequestDTO
        {
            Username = request.Username,
            Name = request.Name,
            Email = request.Email,
            Password = request.Password,
        };

    public static LocalLoginRequestDTO ToLocalLoginRequestDTO(this LocalLoginRequest request) =>
        new LocalLoginRequestDTO
        {
            EmailOrUsername = request.EmailOrUsername,
            Password = request.Password,
            KeepMeLoggedIn = request.KeepMeLoggedIn,
        };

    public static VerifyEmailRequestDTO ToVerifyEmailRequestDTO(this VerifyEmailRequest request) =>
        new VerifyEmailRequestDTO
        {
            UserId = request.UserId,
            Code = request.Code,
        };

    public static OauthRequestDTO ToOauthRequestDTO(this OauthRequest request) =>
        new OauthRequestDTO
        {
            Code = request.Code,
            Verifier = request.Verifier,
            RedirectUri = request.RedirectUri   
        };
}