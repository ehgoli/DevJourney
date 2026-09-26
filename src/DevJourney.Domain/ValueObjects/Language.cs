using DevJourney.Domain.Exceptions;

namespace DevJourney.Domain.ValueObjects;

public readonly record struct Language
{
    public string Code { get; }

    private Language(string code)
    {
        Code = code;
    }

    public static Language Create(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new DomainException(
                "Language cannot be empty.");

        var normalizedCode = code.Trim().ToLowerInvariant();

        if (normalizedCode.Length != 2)
            throw new DomainException(
                "Language code must contain exactly two characters.");

        if (!normalizedCode.All(char.IsLetter))
            throw new DomainException(
                "Language code must contain only letters.");

        return new Language(normalizedCode);
    }

    public override string ToString()
    {
        return Code;
    }
}