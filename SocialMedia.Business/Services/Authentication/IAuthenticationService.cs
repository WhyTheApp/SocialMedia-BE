using SocialMedia.Business.Models.Authentication;

namespace SocialMedia.Business.Services.Authentication;

public interface IAuthenticationService
{
    Task<LoginResponse> LoginLocalUserAsync(LocalLoginRequestDTO request);
    Task<LoginResponse> RegisterLocalUser(LocalRegisterRequestDTO request);
}


