using SocialMedia.Business.Models.Registers;

namespace SocialMedia.API.Requests;

public static class RegistersExtensions
{
    public static AddRegisterDTO ToAddRegisterDTO(this AddRegisterRequest request) =>
        new AddRegisterDTO
        {
            Email = request.Email,
            Feedback = request.Feedback,
        };
}