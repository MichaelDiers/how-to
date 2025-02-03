namespace Crypto.Tests;

using System.Security.Cryptography;
using System.Text;
using Aes = System.Security.Cryptography.Aes;

/// <summary>
///     Tests for <see cref="RsaAesHowTo" />: Encrypt and decrypt data using a combination of <see cref="RSA" /> and
///     <see cref="Aes" />.
/// </summary>
public class RsaAesTests
{
    /// <summary>
    ///     The test data include different key sizes and data sizes.
    /// </summary>
    public static TheoryData<int, int> RsaAesTestData = new MatrixTheoryData<int, int>(
        [
            2048,
            4096
        ],
        [
            0,
            63,
            64,
            65,
            1024
        ]);

    /// <summary>
    ///     The rsa test data include different key sizes.
    /// </summary>
    public static IEnumerable<TheoryDataRow<int>> RsaKeySizes =
    [
        new(2048),
        new(4096)
    ];

    /// <summary>
    ///     The <see cref="RsaAesHowTo" /> provides operations for encrypting and decrypting data using <see cref="RSA" /> and
    ///     <see cref="Aes" />.
    /// </summary>
    private readonly RsaAesHowTo rsaAesHowTo = new();

    /// <summary>
    ///     Encrypts data of size <paramref name="dataSize" /> using a key of length
    ///     <paramref name="keySize" />.
    /// </summary>
    /// <param name="keySize">The size of the rsa key.</param>
    /// <param name="dataSize">The size of the test data.</param>
    [Theory]
    [MemberData(nameof(RsaAesTests.RsaAesTestData))]
    public async Task EncryptAsync(int keySize, int dataSize)
    {
        // init rsa
        var rsa = RSA.Create(keySize);
        var publicKeyPem = rsa.ExportRSAPublicKeyPem();

        // init test data using random values
        var data = new byte[dataSize];
        RandomNumberGenerator.Create().GetBytes(data);

        // encrypt
        var encrypted = await this.rsaAesHowTo.EncryptAsync(
            publicKeyPem,
            data,
            TestContext.Current.CancellationToken);

        Assert.NotEqual(
            data,
            encrypted);
    }

    /// <summary>
    ///     Encrypts and decrypts data of size <paramref name="dataSize" /> using a key of
    ///     length <paramref name="keySize" />.
    /// </summary>
    /// <param name="keySize">The size of the rsa key.</param>
    /// <param name="dataSize">The size of the test data.</param>
    [Theory]
    [MemberData(nameof(RsaAesTests.RsaAesTestData))]
    public async Task EncryptDecryptAsync(int keySize, int dataSize)
    {
        // init rsa
        var rsa = RSA.Create(keySize);
        var publicKeyPem = rsa.ExportRSAPublicKeyPem();
        var privateKeyPem = rsa.ExportRSAPrivateKeyPem();

        // init test data using random values
        var data = new byte[dataSize];
        RandomNumberGenerator.Create().GetBytes(data);

        // encrypt
        var encrypted = await this.rsaAesHowTo.EncryptAsync(
            publicKeyPem,
            data,
            TestContext.Current.CancellationToken);

        Assert.NotEqual(
            data,
            encrypted);

        // decrypt
        var decrypted = await this.rsaAesHowTo.DecryptAsync(
            privateKeyPem,
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
    /// <param name="keySize">The size of the rsa key.</param>
    [Theory]
    [MemberData(nameof(RsaAesTests.RsaKeySizes))]
    public async Task EncryptDecryptLoremIpsumAsync(int keySize)
    {
        // init rsa
        var rsa = RSA.Create(keySize);
        var publicKeyPem = rsa.ExportRSAPublicKeyPem();
        var privateKeyPem = rsa.ExportRSAPrivateKeyPem();

        // use lorem ipsum as test input
        var data = TestData.LoremIpsum;
        var byteData = Encoding.UTF8.GetBytes(data);

        // encrypt
        var encrypted = await this.rsaAesHowTo.EncryptAsync(
            publicKeyPem,
            byteData,
            TestContext.Current.CancellationToken);

        Assert.NotEqual(
            byteData,
            encrypted);

        // decrypt
        var decrypted = await this.rsaAesHowTo.DecryptAsync(
            privateKeyPem,
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
    [MemberData(nameof(RsaAesTests.RsaKeySizes))]
    public async Task EncryptLoremIpsumAsync(int keySize)
    {
        // init rsa
        var rsa = RSA.Create(keySize);
        var publicKeyPem = rsa.ExportRSAPublicKeyPem();

        // use lorem ipsum as test input
        var data = TestData.LoremIpsum;
        var byteData = Encoding.UTF8.GetBytes(data);

        // encrypt
        var encrypted = await this.rsaAesHowTo.EncryptAsync(
            publicKeyPem,
            byteData,
            TestContext.Current.CancellationToken);

        Assert.NotEqual(
            byteData,
            encrypted);
    }
}
