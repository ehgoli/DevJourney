using System.Security.Cryptography;
using DevJourney.Application.Interfaces.Infrastructure.Identity;

namespace DevJourney.Infrastructure.Identity.Password;

public sealed class PasswordHasher : IPasswordHasher
{
    private const string Algorithm = "PBKDF2-SHA256";
    private const string Version = "v1";

    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 600_000;

    public string Hash(string password)
    {
        ArgumentException.ThrowIfNullOrEmpty(password);
        
        var salt = RandomNumberGenerator.GetBytes(SaltSize);

        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password.AsSpan(),
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            HashSize);

        return string.Join(
            '$',
            Algorithm,
            Version,
            Iterations,
            Convert.ToBase64String(salt),
            Convert.ToBase64String(hash));
    }

    public bool Verify(string password, string passwordHash)
    {
        ArgumentException.ThrowIfNullOrEmpty(password);
        ArgumentException.ThrowIfNullOrEmpty(passwordHash);

        if (!TryParse(
                passwordHash,
                out var iterations,
                out var salt,
                out var expectedHash))
        {
            return false;
        }

        var actualHash = Rfc2898DeriveBytes.Pbkdf2(
            password.AsSpan(),
            salt,
            iterations,
            HashAlgorithmName.SHA256,
            expectedHash.Length);

        return CryptographicOperations.FixedTimeEquals(
            actualHash,
            expectedHash);
    }

    private static bool TryParse(
        string passwordHash,
        out int iterations,
        out byte[] salt,
        out byte[] expectedHash)
    {
        iterations = default;
        salt = [];
        expectedHash = [];

        var parts = passwordHash.Split('$');

        if (parts.Length != 5)
            return false;

        if (!string.Equals(parts[0], Algorithm, StringComparison.Ordinal) ||
            !string.Equals(parts[1], Version, StringComparison.Ordinal))
        {
            return false;
        }

        if (!int.TryParse(parts[2], out iterations))
            return false;

        if (iterations <= 0)
            return false;

        try
        {
            salt = Convert.FromBase64String(parts[3]);
            expectedHash = Convert.FromBase64String(parts[4]);
        }
        catch (FormatException)
        {
            return false;
        }

        return salt.Length == SaltSize &&
               expectedHash.Length == HashSize;
    }
}