using BazzarOn.Domain.Exception;

namespace BazzarOn.Domain.ValueObjects;

public sealed record Username
{
    public string Value { get; }

    public Username(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new UsernameEmptyException();

        value = value.Trim();

        if (value.Length is < 3 or > 30)
            throw new InvalidUsernameLengthException();

        if (!value.All(c => char.IsLetterOrDigit(c) || c is '_' or '.'))
            throw new InvalidUsernameFormatException();

        Value = value;
    }
    
    public override string ToString() => Value;
    
    public static implicit operator string(Username username) => username.Value;
}