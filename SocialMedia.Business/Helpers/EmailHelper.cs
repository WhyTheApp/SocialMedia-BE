using System.Text.RegularExpressions;

namespace SocialMedia.Business.Helpers;

public class EmailHelper
{
    public static bool IsValidEmail(string email)
    {
        const string emailSeparator = "@";
        const int localPartIndex = 0;
        const int domainPartIndex = 1;

        if (email == string.Empty)
            return false;

        if (!IsMatchRegex(email))
            return false;

        var emailParts = email.Split(emailSeparator);
        if (!HasValidEmailParts(emailParts))
            return false;

        var localPart = emailParts[localPartIndex];
        var domainPart = emailParts[domainPartIndex];

        if (!IsLengthValid(localPart, domainPart))
            return false;

        if (HasInvalidBoundaryChars(domainPart))
            return false;

        if (HasEmptyDomainParts(domainPart))
            return false;

        return true;
    }

    private static bool IsMatchRegex(string email)
    {
        const string localPartAllowedChars = @"[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+";
        const string atSeparator = "@";
        const string domainLabel = @"[a-zA-Z0-9-]+";
        const string domainSeparator = @"\.";
        const string repeatZeroOrMore = "*";

        var domain = $"{domainLabel}({domainSeparator}{domainLabel}){repeatZeroOrMore}";
        var fullPattern = $"^{localPartAllowedChars}{atSeparator}{domain}$";

        var emailRegex = new Regex(
            fullPattern,
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        return emailRegex.IsMatch(email);

    }

    private static bool HasValidEmailParts(string[] emailParts)
    {
        const int validLength = 2;
        
        return emailParts.Length == validLength;
    }

    private static bool IsLengthValid(string localPart, string domainPart)
    {
        const int localPartMaxLength = 64;
        const int domainPartMaxLength = 255;

        return localPart.Length <= localPartMaxLength && domainPart.Length <= domainPartMaxLength;
    }

    private static bool HasInvalidBoundaryChars(string domainPart)
    {
        var invalidBoundaryCharacters = new[] { '-' };

        foreach (var invalidChar in invalidBoundaryCharacters)
            if (domainPart.StartsWith(invalidChar) || domainPart.EndsWith(invalidChar))
                return true;

        return false;
    }

    private static bool HasEmptyDomainParts(string domainPart)
    {
        const string domainSeparator = ".";

        if (!domainPart.Contains(domainSeparator))
            return true;
        
        var domainParts = domainPart.Split(domainSeparator);

        return domainParts.Any(splitDomainPart => splitDomainPart == string.Empty);
    }
}