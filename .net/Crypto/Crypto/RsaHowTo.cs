namespace Crypto;

using System.Security.Cryptography;

/// <summary>
///     Encrypt and decrypt data using <see cref="RSA" />.
/// </summary>
public class RsaHowTo
{
    /// <summary>
    ///     Decrypts the <paramref name="data" /> using the <paramref name="privateKeyPem" />.
    /// </summary>
    /// <param name="privateKeyPem">The private rsa key pem.</param>
    /// <param name="data">The encrypted data.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>The decrypted <paramref name="data" />.</returns>
    public byte[] Decrypt(string privateKeyPem, byte[] data, CancellationToken cancellationToken)
    {
        var rsa = RSA.Create();
        rsa.ImportFromPem(privateKeyPem);
        return rsa.Decrypt(
            data,
            RSAEncryptionPadding.Pkcs1);
    }

    /// <summary>
    ///     Encrypts the <paramref name="data" /> using the <paramref name="publicKeyPem" />.
    /// </summary>
    /// <param name="publicKeyPem">The public rsa key pem.</param>
    /// <param name="data">The data that gets encrypted.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>The encrypted <paramref name="data" />.</returns>
    public byte[] Encrypt(string publicKeyPem, byte[] data, CancellationToken cancellationToken)
    {
        var rsa = RSA.Create();
        rsa.ImportFromPem(publicKeyPem);
        return rsa.Encrypt(
            data,
            RSAEncryptionPadding.Pkcs1);
    }
}
