using SocialMedia.Business.Models.Authentication;
using SocialMedia.Data.Models;

namespace SocialMedia.Business.Services.Authentication;

public interface IAuthenticationService
{
    Task<LoginResponse> LoginLocalUserAsync(LocalLoginRequestDTO request);
    Task<LoginResponse> RegisterLocalUser(LocalRegisterRequestDTO request);
    Task<RefreshTokenResponse> RefreshJWTToken(string refreshToken);
    string GenerateAndSaveRefreshToken(Guid userId, int size = 32);
}



