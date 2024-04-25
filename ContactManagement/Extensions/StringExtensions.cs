using System.Globalization;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using static System.String;
using static System.Text.RegularExpressions.Regex;

namespace ContactManagement.Extensions;
public static class StringExtensions
{

    public static string Truncate(this string value, int maxLength)
    {
        if (IsNullOrEmpty(value)) { return value; }
        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }

    public static string ToPostcode(this string str)
    {
        if (str.Length < 3)
        {
            return str;
        }

        StringBuilder stringBuilder = new(8);
        var postCodeNoSpaces = str.Replace(" ", "").ToUpperInvariant();
        var firstPart = postCodeNoSpaces[..^3];
        var secondPart = postCodeNoSpaces.Substring(postCodeNoSpaces.Length - 3, 3);
        return stringBuilder.Append(firstPart + " " + secondPart).ToString();
    }

    /*
     * Remove all whitespace from string.
     */
    public static string ToWhitespaceRemoved(this string str)
    {
        if (IsNullOrEmpty(str))
        {
            return str;
        }

        str = Replace(str, @"\s", "");
        return str;
    }

    public static string ToLeadingCapital(this string input)
    {
        if (IsNullOrWhiteSpace(input))
        {
            return input;
        }

        return input[..1].ToUpper(CultureInfo.CurrentCulture) +
               input.Substring(1, input.Length - 1);
    }

    public static bool IsValidPhoneNumber(this string phone)
    {
        // In a real application it would be better to use a library such as https://github.com/twcclegg/libphonenumber-csharp or an API service equivalent (to ensure up-to-date checks)
        // These include functionality to tell us whether it is a mobile or landline number 
        // For this test project I am using a regex  with some basic unit tests
        if (IsNullOrWhiteSpace(phone))
        {
            return false;
        }
        const string pattern = """^(?:(?:\(?(?:0(?:0|11)\)?[\s-]?\(?|\+)44\)?[\s-]?(?:\(?0\)?[\s-]?)?)|(?:\(?0))(?:(?:\d{5}\)?[\s-]?\d{4,5})|(?:\d{4}\)?[\s-]?(?:\d{5}|\d{3}[\s-]?\d{3}))|(?:\d{3}\)?[\s-]?\d{3}[\s-]?\d{3,4})|(?:\d{2}\)?[\s-]?\d{4}[\s-]?\d{4}))(?:[\s-]?(?:x|ext\.?|\#)\d{3,4})?$""";
        return Match(phone, pattern, RegexOptions.IgnoreCase).Success;
    }

    public static bool IsValidEmail(this string email)
    {
        // Using a simple check for this test project. 
        // In a real project we should also test by sending an email and using a check that the email was delivered
        // Can be done by reading response from the email sender e.g. AWS SES. This tests not just the format but 
        // also whether the email exists. Repeat sends to non-existent email addresses can damage email send reputation and lead to 
        // emails being blocked by Email Providers. Better to do single send and then prevent future sends to non-existent or invalid email addresses
        return MailAddress.TryCreate(email, out _);
    }

    public static string NullToEmpty(this string? input)
    {
        return input ?? Empty;
    }
}