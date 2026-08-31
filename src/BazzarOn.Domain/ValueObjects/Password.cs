using BazzarOn.Domain.Exception;

namespace BazzarOn.Domain.ValueObjects;

public sealed record Password
{
    public string Value { get; }

    public Password(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new PasswordEmptyException();

        var trimmedValue = value.Trim();

        if (trimmedValue.Length is < 7 or > 40)
            throw new InvalidPasswordLengthException();

        Value = trimmedValue;
    }

    public static Password Create(string rawPassword) => new(rawPassword);

    public override string ToString() => Value;

    public static implicit operator string(Password password) => password.Value;
}