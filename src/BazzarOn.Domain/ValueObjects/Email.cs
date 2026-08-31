using System.Text.RegularExpressions;
using BazzarOn.Domain.Exception;

namespace BazzarOn.Domain.ValueObjects;

public sealed record Email
{
    private static readonly Regex EmailRegex = new
        (@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new EmailEmptyException();

        value = value.Trim().ToLowerInvariant();

        if (value.Length is < 5 or > 50)
            throw new InvalidEmailLengthException();

        Value = value;
    }

    public override string ToString() => Value;
}