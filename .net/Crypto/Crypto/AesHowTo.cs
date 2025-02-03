namespace Crypto;

using System.Security.Cryptography;

/// <summary>
///     Encrypt and decrypt data using Aes.
/// </summary>
/// <remarks>
///     The used header included in the encrypted data is a custom header. If the header is omitted the header
///     information needs to be stored somewhere else.
/// </remarks>
public class AesHowTo
{
    /// <summary>
    ///     The supported custom version used in the custom header.
    /// </summary>
    private const byte SupportedVersion = 4;

    /// <summary>
    ///     Decrypts the given <paramref name="data" /> using the given <paramref name="key" />. The first bytes
    ///     of <paramref name="data" /> include a version number and the <see cref="Aes.IV" />.
    /// </summary>
    /// <param name="key">The aes key.</param>
    /// <param name="data">
    ///     The data is encrypted by <paramref name="key" /> and the <see cref="Aes.IV" /> that is included as a
    ///     <paramref name="data" /> header.
    /// </param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task{T}" /> whose result is the decrypted <paramref name="data" />.</returns>
    public async Task<byte[]> DecryptAsync(byte[] key, byte[] data, CancellationToken cancellationToken)
    {
        // create input and output streams
        using var inputStream = new MemoryStream(data);
        using var outputStream = new MemoryStream();

        // decrypt the data
        await this.DecryptAsync(
            key,
            inputStream,
            outputStream,
            cancellationToken);

        return outputStream.ToArray();
    }

    /// <summary>
    ///     Decrypts the given <paramref name="inputStream" /> using the given <paramref name="key" />. The first bytes
    ///     of the <paramref name="inputStream" /> include a version number and the <see cref="Aes.IV" />.
    /// </summary>
    /// <param name="key">The aes key.</param>
    /// <param name="inputStream">
    ///     The data is encrypted by <paramref name="key" /> and the <see cref="Aes.IV" /> that is
    ///     included in the first bytes of the stream.
    /// </param>
    /// <param name="outputStream">The decrypted data is written to the <paramref name="outputStream" />.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
    public async Task DecryptAsync(
        byte[] key,
        Stream inputStream,
        Stream outputStream,
        CancellationToken cancellationToken
    )
    {
        // read the header information including the Aes.IV
        var iv = await AesHowTo.ReadHeaderAsync(
            inputStream,
            cancellationToken);

        // initialize Aes and the decryptor
        var aes = Aes.Create();
        aes.Padding = PaddingMode.PKCS7;
        aes.Key = key;
        aes.IV = iv;
        var cryptoTransform = aes.CreateDecryptor();

        // The cryptoStream has to be disposed before calling outputStream.ToArray(),
        // otherwise the final block gets invalid due to Aes.Padding issues.
        await using var cryptoStream = new CryptoStream(
            outputStream,
            cryptoTransform,
            CryptoStreamMode.Write);

        await inputStream.CopyToAsync(
            cryptoStream,
            cancellationToken);
    }

    /// <summary>
    ///     Encrypts the given <paramref name="data" /> using the <paramref name="key" /> and a new <see cref="Aes.IV" />. The
    ///     encrypted result will include a header including <see cref="Aes.IV" /> information.
    /// </summary>
    /// <param name="key">The aes key.</param>
    /// <param name="data">The data that is encrypted using the <paramref name="key" />.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task{T}" /> whose result includes a non-encrypted header and the encrypted data.</returns>
    public async Task<byte[]> EncryptAsync(byte[] key, byte[] data, CancellationToken cancellationToken)
    {
        // initialize input and encrypted output streams
        using var inputStream = new MemoryStream(data);
        using var outputStream = new MemoryStream();

        // encrypt the data
        await this.EncryptAsync(
            key,
            inputStream,
            outputStream,
            cancellationToken);

        return outputStream.ToArray();
    }

    /// <summary>
    ///     Encrypts the given <paramref name="inputStream" /> using the <paramref name="key" /> and a new
    ///     <see cref="Aes.IV" />. The encrypted result will include a header including <see cref="Aes.IV" /> information.
    /// </summary>
    /// <param name="key">The aes key.</param>
    /// <param name="inputStream">The data that is encrypted using the <paramref name="key" />.</param>
    /// <param name="outputStream">The stream includes a non-encrypted header and the encrypted data.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
    public async Task EncryptAsync(
        byte[] key,
        Stream inputStream,
        Stream outputStream,
        CancellationToken cancellationToken
    )
    {
        // initialize aes and the encryptor
        var aes = Aes.Create();
        aes.Padding = PaddingMode.PKCS7;
        aes.Key = key;
        aes.GenerateIV();
        var cryptoTransform = aes.CreateEncryptor();

        // add header information to the output stream
        await AesHowTo.WriteHeaderAsync(
            outputStream,
            aes.IV,
            cancellationToken);

        // The cryptoStream has to be disposed before calling outputStream.ToArray(),
        // otherwise the final block gets invalid due to Aes.Padding issues.
        await using var cryptoStream = new CryptoStream(
            outputStream,
            cryptoTransform,
            CryptoStreamMode.Write);

        await inputStream.CopyToAsync(
            cryptoStream,
            cancellationToken);
    }

    /// <summary>
    ///     Reads the custom header from the <paramref name="stream" /> and executes header checks.
    /// </summary>
    /// <param name="stream">The input stream that includes the encrypted data.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task{T}" /> whose result is the <see cref="Aes.IV" />.</returns>
    /// <exception cref="InvalidOperationException">Throw if header is invalid.</exception>
    /// <example>
    ///     Header format:
    ///     - byte 0:              a custom version
    ///     - byte 1:              the length of the iv
    ///     - byte 2 - length + 1: the iv
    ///     - byte length + 2:     data
    ///     - byte length + 3:     data
    ///     - ...
    /// </example>
    /// >
    private static async Task<byte[]> ReadHeaderAsync(Stream stream, CancellationToken cancellationToken)
    {
        // check version
        var version = new byte[1];
        var actualLength = await stream.ReadAsync(
            version,
            cancellationToken);
        if (actualLength != 1 || version[0] != AesHowTo.SupportedVersion)
        {
            throw new InvalidOperationException();
        }

        // read length of iv
        var ivLength = new byte[1];
        actualLength = await stream.ReadAsync(
            ivLength,
            cancellationToken);
        if (actualLength != ivLength.Length)
        {
            throw new InvalidOperationException();
        }

        // read iv
        var iv = new byte[ivLength[0]];
        actualLength = await stream.ReadAsync(
            iv,
            cancellationToken);
        if (actualLength != iv.Length)
        {
            throw new InvalidOperationException();
        }

        return iv;
    }

    /// <summary>
    ///     Writes the custom header to the <paramref name="stream" />.
    /// </summary>
    /// <param name="stream">The output stream that will include the encrypted data.</param>
    /// <param name="iv">The <see cref="Aes.IV" /> specific for the encrypted data.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <exception cref="InvalidOperationException">Throw if <paramref name="iv" /> has an unexpected length.</exception>
    /// <example>
    ///     Header format:
    ///     - byte 0:              a custom version
    ///     - byte 1:              the length of the iv
    ///     - byte 2 - length + 1: the iv
    ///     - byte length + 2:     data
    ///     - byte length + 3:     data
    ///     - ...
    /// </example>
    private static async Task WriteHeaderAsync(Stream stream, byte[] iv, CancellationToken cancellationToken)
    {
        // add a version information
        stream.WriteByte(AesHowTo.SupportedVersion);

        // the length should be 16
        if (iv.Length > byte.MaxValue)
        {
            throw new InvalidOperationException();
        }

        // add the length 
        stream.WriteByte((byte) iv.Length);

        // add the iv
        await stream.WriteAsync(
            iv,
            cancellationToken);
    }
}
