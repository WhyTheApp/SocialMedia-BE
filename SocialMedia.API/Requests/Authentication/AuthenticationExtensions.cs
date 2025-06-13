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
            Email = request.Email,
            Password = request.Password,
        };

    public static LocalLoginRequestDTO ToLocalLoginRequestDTO(this LocalLoginRequest request) =>
        new LocalLoginRequestDTO
        {
            EmailOrUsername = request.EmailOrUsername,
            Password = request.Password,
        };
}