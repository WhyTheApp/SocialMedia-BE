namespace SocialMedia.Business.Helpers;

using System.Text.RegularExpressions;

public class PasswordHelper
{
    public static bool IsValidPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
            return false;

        const int minLength = 8;
        const int maxLength = 128;

        if (password.Length < minLength || password.Length > maxLength)
            return false;

        if (!HasUppercase(password))
            return false;

        if (!HasLowercase(password))
            return false;

        if (!HasDigit(password))
            return false;

        if (!HasSpecialChar(password))
            return false;

        return true;
    }

    private static bool HasUppercase(string password)
    {
        return password.Any(char.IsUpper);
    }

    private static bool HasLowercase(string password)
    {
        return password.Any(char.IsLower);
    }

    private static bool HasDigit(string password)
    {
        return password.Any(char.IsDigit);
    }

    private static bool HasSpecialChar(string password)
    {
        var specialChars = 
            "!@#$%^&*(),.?" +    // common punctuation
            "\"{}|<>_" +         // quotes, braces, pipe, angle brackets, underscore
            "\\-+=[]" +          // backslash, dash, plus, equals, brackets
            "\\:;'/~`";          // colon, semicolon, single quote, slash, tilde, backtick

        var specialCharPattern = new Regex("[" + specialChars + "]");
        return specialCharPattern.IsMatch(password);
    }
}
