using System.Text;
using Konscious.Security.Cryptography;

namespace Service;

public class Argon2idPasswordHashAlgorithm : PasswordHashAlgorithm
{
    public const string name = "argon2id";

    public override string getName() => name;

    public override string hashPassword(string password, string salt)
    {
        using var hashAlgo = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = decode(salt),
            MemorySize = 12288,
            Iterations = 10,
            DegreeOfParallelism = 1,
        };
        return encode(hashAlgo.GetBytes(256));
    }

    public override bool verifyHashedPassword(string password, string hash, string salt)
    {
        return hashPassword(password, salt).SequenceEqual(hash);
    }
}