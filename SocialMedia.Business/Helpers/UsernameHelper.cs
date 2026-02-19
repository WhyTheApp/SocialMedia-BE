using System.Text.RegularExpressions;

namespace SocialMedia.Business.Helpers;

public static class UsernameHelper
{
    public static bool IsValidUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            return false;

        const string pattern = @"^[a-zA-Z][a-zA-Z0-9_]{2,29}$";

        return Regex.IsMatch(username, pattern);
    }
}