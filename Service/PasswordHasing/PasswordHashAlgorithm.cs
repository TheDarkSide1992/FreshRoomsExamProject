using System.Security.Cryptography;

namespace Service;

public abstract class PasswordHashAlgorithm
{
    const string PreferredAlgorithmName = Argon2idPasswordHashAlgorithm.name;

    public static PasswordHashAlgorithm create(string algorithmName = PreferredAlgorithmName)
    {
        switch (algorithmName)
        {
            case Argon2idPasswordHashAlgorithm.name:
                return new Argon2idPasswordHashAlgorithm();
            default:
                throw new NotImplementedException();
        }
    }

    public abstract string getName();

    public abstract string hashPassword(string password, string salt);

    public abstract bool verifyHashedPassword(string password, string hash, string salt);

    public string generateSalt()
    {
        return encode(RandomNumberGenerator.GetBytes(128));
    }

    protected byte[] decode(string value)
    {
        return Convert.FromBase64String(value);
    }

    protected string encode(byte[] value)
    {
        return Convert.ToBase64String(value);
    }
}