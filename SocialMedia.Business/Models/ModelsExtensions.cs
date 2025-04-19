using SocialMedia.Business.Models.Registers;
using SocialMedia.Data.Models;

namespace SocialMedia.Business.Models;

public static class ModelsExtensions
{
    public static Register ToRegisterDataModel(this AddRegisterDTO registerDTO) =>
        new Register
        {
            Email = registerDTO.Email,
            Feedback = registerDTO.Feedback,
        };

    public static RegisterResponse ToRegisterResponse(this Register register) =>
        new RegisterResponse
        {
            Id = register.Id,
            Email = register.Email,
            Feedback = register.Feedback,
        };
}