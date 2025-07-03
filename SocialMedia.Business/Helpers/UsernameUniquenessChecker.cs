using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;

namespace SocialMedia.Business.Helpers;

public class UsernameUniquenessChecker : IUsernameUniquenessChecker
{
    private readonly SocialMediaDbContext _context;

    public UsernameUniquenessChecker(SocialMediaDbContext context)
    {
        _context = context;
    }

    public bool IsUnique(string username)
    {
        return !_context.Users.Any(user => user.Username == username.ToLower());
    }
}
