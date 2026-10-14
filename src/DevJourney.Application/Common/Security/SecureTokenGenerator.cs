using System.Security.Cryptography;

namespace DevJourney.Application.Common.Security;

public static class SecureTokenGenerator
{
    private const string Digits = "0123456789";
    private const string HexCharacters = "0123456789ABCDEF";

    public static string Generate(int length = 64)
        => Generate(length, HexCharacters);

    public static string GenerateVerificationCode(int length = 5)
        => Generate(length, Digits);

    private static string Generate(int length, string characters)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(length);

        if (string.IsNullOrEmpty(characters))
            throw new ArgumentException(
                "Character set cannot be empty.",
                nameof(characters));

        var result = new char[length];

        for (var i = 0; i < length; i++)
        {
            var index = RandomNumberGenerator.GetInt32(characters.Length);
            result[i] = characters[index];
        }

        return new string(result);
    }
}