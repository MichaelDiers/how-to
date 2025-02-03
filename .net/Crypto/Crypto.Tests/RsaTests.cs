namespace Crypto.Tests;

using System.Security.Cryptography;
using System.Text;

/// <summary>
///     Tests for <see cref="RsaHowTo" />: Encrypt and decrypt data using <see cref="RSA" />.
/// </summary>
public class RsaTests
{
    /// <summary>
    ///     The rsa test data include different key sizes.
    /// </summary>
    public static IEnumerable<TheoryDataRow<int>> RsaKeySizes =
    [
        new(2048),
        new(4096)
    ];

    /// <summary>
    ///     The rsa test data include different key sizes and data sizes.
    /// </summary>
    public static TheoryData<int, int> RsaTestData = new MatrixTheoryData<int, int>(
        [
            2048,
            4096
        ],
        [
            0,
            63,
            64,
            65
        ]);

    /// <summary>
    ///     The <see cref="RsaHowTo" /> provides operations for encrypting and decrypting data using <see cref="RSA" />.
    /// </summary>
    private readonly RsaHowTo rsaHowTo = new();

    /// <summary>
    ///     Encrypts data of size <paramref name="dataSize" /> using a public of length
    ///     <paramref name="keySize" />.
    /// </summary>
    /// <param name="keySize">The size of the rsa key.</param>
    /// <param name="dataSize">The size of the test data.</param>
    [Theory]
    [MemberData(nameof(RsaTests.RsaTestData))]
    public void Encrypt(int keySize, int dataSize)
    {
        // init rsa
        var rsa = RSA.Create(keySize);
        var publicKeyPem = rsa.ExportRSAPublicKeyPem();

        // init test data using random values
        var data = new byte[dataSize];
        RandomNumberGenerator.Create().GetBytes(data);

        // encrypt
        var encrypted = this.rsaHowTo.Encrypt(
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
    [MemberData(nameof(RsaTests.RsaTestData))]
    public void EncryptDecrypt(int keySize, int dataSize)
    {
        // init rsa
        var rsa = RSA.Create(keySize);
        var publicKeyPem = rsa.ExportRSAPublicKeyPem();
        var privateKeyPem = rsa.ExportRSAPrivateKeyPem();

        // init test data using random values
        var data = new byte[dataSize];
        RandomNumberGenerator.Create().GetBytes(data);

        // encrypt
        var encrypted = this.rsaHowTo.Encrypt(
            publicKeyPem,
            data,
            TestContext.Current.CancellationToken);

        Assert.NotEqual(
            data,
            encrypted);

        // decrypt
        var decrypted = this.rsaHowTo.Decrypt(
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
    [MemberData(nameof(RsaTests.RsaKeySizes))]
    public void EncryptDecryptLoremIpsum(int keySize)
    {
        // init rsa
        var rsa = RSA.Create(keySize);
        var publicKeyPem = rsa.ExportRSAPublicKeyPem();
        var privateKeyPem = rsa.ExportRSAPrivateKeyPem();

        // use lorem ipsum subset as test input: process larger inputs using a combination of aes and rsa
        var data = TestData.LoremIpsum[..100];
        var byteData = Encoding.UTF8.GetBytes(data);

        // encrypt
        var encrypted = this.rsaHowTo.Encrypt(
            publicKeyPem,
            byteData,
            TestContext.Current.CancellationToken);

        Assert.NotEqual(
            byteData,
            encrypted);

        // decrypt
        var decrypted = this.rsaHowTo.Decrypt(
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
    /// <param name="keySize">The size of the rsa key.</param>
    [Theory]
    [MemberData(nameof(RsaTests.RsaKeySizes))]
    public void EncryptLoremIpsum(int keySize)
    {
        // init rsa
        var rsa = RSA.Create(keySize);
        var publicKeyPem = rsa.ExportRSAPublicKeyPem();

        // use lorem ipsum subset as test input: process larger inputs using a combination of aes and rsa
        var data = TestData.LoremIpsum[..100];
        var byteData = Encoding.UTF8.GetBytes(data);

        // encrypt
        var encrypted = this.rsaHowTo.Encrypt(
            publicKeyPem,
            byteData,
            TestContext.Current.CancellationToken);

        Assert.NotEqual(
            byteData,
            encrypted);
    }
}
