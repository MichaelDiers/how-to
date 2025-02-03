namespace Crypto.Tests;

using System.Security.Cryptography;
using System.Text;
using Aes = System.Security.Cryptography.Aes;

/// <summary>
///     Tests for <see cref="AesBrotliHowTo" />: Compress data by brotli. Encrypt and decrypt data using <see cref="Aes" />
///     .
/// </summary>
public class AesBrotliTests
{
    /// <summary>
    ///     The aes test data include different key sizes and data sizes.
    /// </summary>
    public static TheoryData<int, int> AesTestData = new MatrixTheoryData<int, int>(
        [
            128,
            192,
            256
        ],
        [
            0,
            63,
            64,
            65,
            1024
        ]);

    /// <summary>
    ///     The <see cref="AesBrotliHowTo" /> provides operations for encrypting and decrypting data using <see cref="Aes" />.
    /// </summary>
    private readonly AesBrotliHowTo aesBrotliHowTo = new();

    /// <summary>
    ///     Encrypts data of size <paramref name="dataSize" /> using a <see cref="SymmetricAlgorithm.Key" /> of length
    ///     <paramref name="keySize" />.
    /// </summary>
    /// <param name="keySize">The size of the aes key.</param>
    /// <param name="dataSize">The size of the test data.</param>
    [Theory]
    [MemberData(nameof(AesBrotliTests.AesTestData))]
    public async Task EncryptAsync(int keySize, int dataSize)
    {
        // init aes
        var aes = Aes.Create();
        aes.KeySize = keySize;
        aes.GenerateKey();

        // init test data using random values
        var data = new byte[dataSize];
        RandomNumberGenerator.Create().GetBytes(data);

        // encrypt
        var encrypted = await this.aesBrotliHowTo.EncryptAsync(
            aes.Key,
            data,
            TestContext.Current.CancellationToken);

        Assert.NotEqual(
            data,
            encrypted);
    }

    /// <summary>
    ///     Encrypts and decrypts data of size <paramref name="dataSize" /> using a <see cref="SymmetricAlgorithm.Key" /> of
    ///     length <paramref name="keySize" />.
    /// </summary>
    /// <param name="keySize">The size of the aes key.</param>
    /// <param name="dataSize">The size of the test data.</param>
    [Theory]
    [MemberData(nameof(AesTests.AesTestData))]
    public async Task EncryptDecryptAsync(int keySize, int dataSize)
    {
        // init aes
        var aes = Aes.Create();
        aes.KeySize = keySize;
        aes.GenerateKey();

        // init test data using random values
        var data = new byte[dataSize];
        RandomNumberGenerator.Create().GetBytes(data);

        // encrypt
        var encrypted = await this.aesBrotliHowTo.EncryptAsync(
            aes.Key,
            data,
            TestContext.Current.CancellationToken);

        Assert.NotEqual(
            data,
            encrypted);

        // decrypt
        var decrypted = await this.aesBrotliHowTo.DecryptAsync(
            aes.Key,
            encrypted,
            TestContext.Current.CancellationToken);

        Assert.NotEqual(
            encrypted,
            decrypted);
        Assert.Equal(
            data,
            decrypted);
    }

    /// <summary>
    ///     Encrypts and decrypt a part of lorem ipsum using different key sizes.
    /// </summary>
    /// <param name="keySize">The size of the aes key.</param>
    [Theory]
    [InlineData(128)]
    [InlineData(192)]
    [InlineData(256)]
    public async Task EncryptDecryptLoremIpsumAsync(int keySize)
    {
        // init aes
        var aes = Aes.Create();
        aes.KeySize = keySize;
        aes.GenerateKey();

        // use lorem ipsum as test input
        var data = TestData.LoremIpsum;
        var byteData = Encoding.UTF8.GetBytes(data);

        // encrypt
        var encrypted = await this.aesBrotliHowTo.EncryptAsync(
            aes.Key,
            byteData,
            TestContext.Current.CancellationToken);

        Assert.NotEqual(
            byteData,
            encrypted);

        // decrypt
        var decrypted = await this.aesBrotliHowTo.DecryptAsync(
            aes.Key,
            encrypted,
            TestContext.Current.CancellationToken);

        Assert.Equal(
            byteData,
            decrypted);

        Assert.Equal(
            data,
            Encoding.UTF8.GetString(decrypted));
    }

    /// <summary>
    ///     Encrypts a part of lorem ipsum using different key sizes.
    /// </summary>
    /// <param name="keySize">The size of the aes key.</param>
    [Theory]
    [InlineData(128)]
    [InlineData(192)]
    [InlineData(256)]
    public async Task EncryptLoremIpsumAsync(int keySize)
    {
        // init aes
        var aes = Aes.Create();
        aes.KeySize = keySize;
        aes.GenerateKey();

        // use lorem ipsum as test input
        var data = TestData.LoremIpsum;
        var byteData = Encoding.UTF8.GetBytes(data);

        // encrypt
        var encrypted = await this.aesBrotliHowTo.EncryptAsync(
            aes.Key,
            byteData,
            TestContext.Current.CancellationToken);

        Assert.NotEqual(
            byteData,
            encrypted);
    }
}
