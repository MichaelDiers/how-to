namespace Crypto;

using System.Security.Cryptography;

/// <summary>
///     Encrypt data using <see cref="Aes" /> and encrypt the <see cref="SymmetricAlgorithm.Key" /> using
///     <see cref="RSA" />.
/// </summary>
public class RsaAesHowTo
{
    /// <summary>
    ///     The used algorithms are included in the header of the encrypted data.
    /// </summary>
    private const byte AlgorithmRsaAes = 244;

    /// <summary>
    ///     The supported version used for encrypting the data.
    /// </summary>
    private const byte SupportedVersion = 30;

    /// <summary>
    ///     Reads the header from <paramref name="data" /> and decrypts the aes key using rsa. The data is decrypted using the
    ///     aes key.
    /// </summary>
    /// <param name="privateKeyPem">The private rsa key pem.</param>
    /// <param name="data">The data that is decrypted.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>The encrypted data.</returns>
    public async Task<byte[]> DecryptAsync(string privateKeyPem, byte[] data, CancellationToken cancellationToken)
    {
        // initialize input and output stream
        using var inputStream = new MemoryStream(data);
        using var outputStream = new MemoryStream();

        // check header and read encrypted aes key
        var encryptedAesKey = await RsaAesHowTo.ProcessHeaderAsync(
            inputStream,
            RsaAesHowTo.SupportedVersion,
            RsaAesHowTo.AlgorithmRsaAes,
            cancellationToken);

        // decrypt aes key
        var aesKey = new RsaHowTo().Decrypt(
            privateKeyPem,
            encryptedAesKey,
            cancellationToken);

        // decrypt data
        await new AesHowTo().DecryptAsync(
            aesKey,
            inputStream,
            outputStream,
            cancellationToken);

        // return decrypted data
        return outputStream.ToArray();
    }

    /// <summary>
    ///     Encrypts the <paramref name="data" /> using aes and encrypts the aes key using rsa key
    ///     <paramref name="publicKeyPem" />.
    /// </summary>
    /// <param name="publicKeyPem">The public rsa key pem.</param>
    /// <param name="data">The data that is encrypted using aes.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>The encrypted data using aes and an additional header.</returns>
    public async Task<byte[]> EncryptAsync(string publicKeyPem, byte[] data, CancellationToken cancellationToken)
    {
        // create aes key for encrypting the data
        var aes = Aes.Create();
        aes.KeySize = 256;
        aes.GenerateKey();

        // encrypt aes key using rsa
        var rsa = new RsaHowTo();
        var encryptedAesKey = rsa.Encrypt(
            publicKeyPem,
            aes.Key,
            cancellationToken);

        // add header information
        using var outputMemoryStream = new MemoryStream();
        await RsaAesHowTo.WriteHeaderAsync(
            outputMemoryStream,
            RsaAesHowTo.SupportedVersion,
            RsaAesHowTo.AlgorithmRsaAes,
            encryptedAesKey,
            cancellationToken);

        // encrypt using aes
        using var inputMemoryStream = new MemoryStream(data);
        var aesHowTo = new AesHowTo();
        await aesHowTo.EncryptAsync(
            aes.Key,
            inputMemoryStream,
            outputMemoryStream,
            cancellationToken);

        // return the encrypted data and header
        return outputMemoryStream.ToArray();
    }

    /// <summary>
    ///     Processes the header, checks the information and extracts the encrypted aes key.
    /// </summary>
    /// <param name="stream">The encrypted input stream.</param>
    /// <param name="version">The encrypted data is processed using this version.</param>
    /// <param name="algorithm">The information about the used crypto algorithms.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task{T}" /> whose result is the encrypted aes key.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the header is invalid.</exception>
    private static async Task<byte[]> ProcessHeaderAsync(
        Stream stream,
        byte version,
        byte algorithm,
        CancellationToken cancellationToken
    )
    {
        var temp = new byte[1];

        // check version
        var tempLength = await stream.ReadAsync(
            temp,
            cancellationToken);
        if (tempLength == 0 || temp[0] != version)
        {
            throw new InvalidOperationException();
        }

        // check algorithms
        tempLength = await stream.ReadAsync(
            temp,
            cancellationToken);
        if (tempLength == 0 || temp[0] != algorithm)
        {
            throw new InvalidOperationException();
        }

        // read how many bytes are reserved for the length of the encrypted aes key
        tempLength = await stream.ReadAsync(
            temp,
            cancellationToken);
        if (tempLength == 0 || temp[0] == 0)
        {
            throw new InvalidOperationException();
        }

        var encryptedKeyLength = temp[0];

        var totalLengthBytes = new byte[encryptedKeyLength];
        tempLength = await stream.ReadAsync(
            totalLengthBytes,
            cancellationToken);
        if (tempLength != totalLengthBytes.Length)
        {
            throw new InvalidOperationException();
        }

        var totalLength = totalLengthBytes.Select(x => (int) x).Sum();

        // read encrypted key
        var encryptedKey = new byte[totalLength];
        tempLength = await stream.ReadAsync(
            encryptedKey,
            cancellationToken);
        if (tempLength != encryptedKey.Length)
        {
            throw new InvalidOperationException();
        }

        return encryptedKey;
    }

    /// <summary>
    ///     Writes the header to the <paramref name="outputStream" />. The header includes information about the used
    ///     algorithms and the key for decrypting the data part.
    /// </summary>
    /// <param name="outputStream">The output stream that includes a header and the encrypted data.</param>
    /// <param name="version">The encrypted data is processed using this version.</param>
    /// <param name="algorithm">The information about the used crypto algorithms.</param>
    /// <param name="encryptedKey">The encrypted key that is used for the data part.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <exception cref="InvalidOperationException">Thrown if header information is invalid.</exception>
    private static async Task WriteHeaderAsync(
        Stream outputStream,
        byte version,
        byte algorithm,
        byte[] encryptedKey,
        CancellationToken cancellationToken
    )
    {
        // add version and algorithm 
        outputStream.WriteByte(version);
        outputStream.WriteByte(algorithm);

        // add length of the encrypted key
        // example: encryptedKey.Length = 519
        // byte 0: 3       the next 3 bytes include the length of the encrypted key
        // byte 1: 255     the sum of byte 1 to byte 3 equals the total ke length
        // byte 2: 255
        // byte 3: 9
        var requiredBytesForLength = encryptedKey.Length / byte.MaxValue + 1;
        if (requiredBytesForLength / byte.MaxValue + 1 > byte.MaxValue)
        {
            throw new InvalidOperationException();
        }

        outputStream.WriteByte((byte) requiredBytesForLength);

        for (var i = encryptedKey.Length; i > 0; i -= byte.MaxValue)
        {
            if (i >= byte.MaxValue)
            {
                outputStream.WriteByte(byte.MaxValue);
            }
            else
            {
                outputStream.WriteByte((byte) i);
            }
        }

        // add encrypted key
        await outputStream.WriteAsync(
            encryptedKey,
            cancellationToken);
    }
}
