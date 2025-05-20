using SocialMedia.Business.Models;
using SocialMedia.Business.Models.Registers;
using SocialMedia.Data;

namespace SocialMedia.Business.Services.Registers;

public class RegisterService : IRegisterService
{
    SocialMediaDbContext _dbContext;

    public RegisterService(SocialMediaDbContext dbContext)
    {
        _dbContext = dbContext;   
    }
    
    public async Task AddRegister(AddRegisterDTO request)
    {
        await _dbContext.Registers.AddAsync(request.ToRegisterDataModel());
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<RegisterResponse>> GetAllRegisters()
    {
        var registers = _dbContext.Registers
            .Select(register => register.ToRegisterResponse())
            .ToList();

        return registers;
    }
}