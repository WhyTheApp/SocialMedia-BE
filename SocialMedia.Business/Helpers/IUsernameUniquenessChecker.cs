namespace SocialMedia.Business.Helpers;

public interface IUsernameUniquenessChecker
{
    bool IsUnique(string username);
}