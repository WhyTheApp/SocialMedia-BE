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
        var group1 = "!@#$%^&*(),.?";      // common punctuation
        var group2 = "\"{}|<>_";           // quotes, braces, pipe, angle brackets, underscore
        var group3 = "-+=[]";              // dash, plus, equals, brackets
        var group4 = ":;'/~`\\";           // colon, semicolon, single quote, slash, tilde, backtick, backslash

        var allSpecialChars = group1 + group2 + group3 + group4;

        string escaped = allSpecialChars
            .Replace(@"\", @"\\")  // escape backslash
            .Replace("-", @"\-")   // escape dash
            .Replace("[", @"\[")   // escape [
            .Replace("]", @"\]");  // escape ]

        var specialCharPattern = new Regex("[" + escaped + "]");

        return specialCharPattern.IsMatch(password);
    }
}
