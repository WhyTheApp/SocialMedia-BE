using SocialMedia.Business.Models.Authentication;
using SocialMedia.Data.Models;

namespace SocialMedia.Business.Services.Authentication;

public interface IAuthenticationService
{
    Task<LoginResponse> LoginLocalUserAsync(LocalLoginRequestDTO request);
    Task<LoginResponse> RegisterLocalUser(LocalRegisterRequestDTO request);
    Task<LoginResponse> GoogleLogin(OauthRequestDTO request);
    Task<RefreshTokenResponse> RefreshJWTToken(string refreshToken);
    Task<LoginResponse> VerifyEmail(VerifyEmailRequestDTO request);

    string GenerateAndSaveRefreshToken(Guid userId, int size = 32);
}



