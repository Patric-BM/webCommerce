
using System;
using System.Security.Cryptography;
using Domain.Options;
using Microsoft.Extensions.Options;

namespace Application.Services;

public interface IHashingService
{
    string Hash(string password);
    bool Verify(string password, string passwordHashed);
}

public class HashingService : IHashingService
{
    private readonly int SaltByteSize;
    private readonly int HashByteSize;
    private readonly string HashAlgorithm;
    private readonly int MinimumIterations;
    private readonly int MaximumIterations;
    private const int IterationIndex = 0;
    private const int SaltIndex = 1;
    private const int Pbkdf2Index = 2;

    public HashingService(IOptions<PasswordHashOptions> options)
    {

        SaltByteSize = options.Value.SaltByteSize;
        HashByteSize = options.Value.HashByteSize;
        HashAlgorithm = options.Value.HashingAlgorithm!;
        MinimumIterations = options.Value.MinIteration;
        MaximumIterations = options.Value.MaxIteration;

    }

    public string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltByteSize);
        int iterations = Random.Shared.Next(MinimumIterations, MaximumIterations);

        var hash = GetPbkdf2Bytes(password, salt, iterations, HashByteSize);
        return $"{iterations}:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
    }

    public bool Verify(string password, string passwordHashed)
    {
        char[] delimiter = { ':' };
        var split = passwordHashed.Split(delimiter);
        var iterations = int.Parse(split[IterationIndex]);
        var salt = Convert.FromBase64String(split[SaltIndex]);
        var hash = Convert.FromBase64String(split[Pbkdf2Index]);

        var testHash = GetPbkdf2Bytes(password, salt, iterations, hash.Length);
        return SlowEquals(hash, testHash);
    }

    private byte[] GetPbkdf2Bytes(string password, byte[] salt, int iterations, int outputBytes)
    {
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, new HashAlgorithmName(HashAlgorithm));
        return pbkdf2.GetBytes(outputBytes);
    }

    private bool SlowEquals(byte[] a, byte[] b)
    {
        uint diff = (uint)a.Length ^ (uint)b.Length;
        for (int i = 0; i < a.Length && i < b.Length; i++)
        {
            diff |= (uint)(a[i] ^ b[i]);
        }
        return diff == 0;
    }

}