using SocialMedia.Business.Models.Registers;

namespace SocialMedia.Business.Services.Example;

public interface IRegisterService
{
    Task AddRegister(AddRegisterDTO request);
    Task<List<RegisterResponse>> GetAllRegisters();
}