﻿namespace StreamPipeline;

using System.IO.Compression;
using System.Security.Cryptography;

/// <summary>
///     A custom stream that supports <see cref="BrotliStream" /> compression and <see cref="Aes" /> encryption.
/// </summary>
/// <seealso cref="System.IO.Stream" />
public class PackerStream : Stream
{
    /// <summary>
    ///     The brotli stream is used for compression and decompression.
    /// </summary>
    private readonly BrotliStream brotliStream;

    /// <summary>
    ///     Indicates whether the stream is writable (compress/encrypt) or readable (decompress/decrypt).
    /// </summary>
    private readonly bool canRead;

    /// <summary>
    ///     The crypto stream is used for <see cref="Aes" /> encryption and decryption.
    /// </summary>
    private readonly CryptoStream cryptoStream;

    /// <summary>
    ///     Initializes a new instance of the <see cref="PackerStream" /> class.
    /// </summary>
    /// <param name="brotliStream">The brotli stream is used for compression and decompression.</param>
    /// <param name="cryptoStream">The crypto stream is used for <see cref="Aes" /> encryption and decryption.</param>
    /// <param name="canRead">Indicates whether the stream is writable (compress/encrypt) or readable (decompress/decrypt).</param>
    private PackerStream(BrotliStream brotliStream, CryptoStream cryptoStream, bool canRead)
    {
        this.cryptoStream = cryptoStream;
        this.canRead = canRead;
        this.brotliStream = brotliStream;
    }

    /// <summary>When overridden in a derived class, gets a value indicating whether the current stream supports reading.</summary>
    /// <returns>
    ///     <see langword="true" /> if the stream supports reading; otherwise, <see langword="false" />.
    /// </returns>
    public override bool CanRead => this.canRead;

    /// <summary>When overridden in a derived class, gets a value indicating whether the current stream supports seeking.</summary>
    /// <returns>
    ///     <see langword="true" /> if the stream supports seeking; otherwise, <see langword="false" />.
    /// </returns>
    public override bool CanSeek => false;

    /// <summary>When overridden in a derived class, gets a value indicating whether the current stream supports writing.</summary>
    /// <returns>
    ///     <see langword="true" /> if the stream supports writing; otherwise, <see langword="false" />.
    /// </returns>
    public override bool CanWrite => !this.canRead;

    /// <summary>When overridden in a derived class, gets the length in bytes of the stream.</summary>
    /// <exception cref="T:System.NotSupportedException">
    ///     A class derived from <see langword="Stream" /> does not support
    ///     seeking and the length is unknown.
    /// </exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <returns>A long value representing the length of the stream in bytes.</returns>
    public override long Length => throw new NotSupportedException();

    /// <summary>When overridden in a derived class, gets or sets the position within the current stream.</summary>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support seeking.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <returns>The current position within the stream.</returns>
    public override long Position
    {
        get => throw new NotSupportedException();
        set => throw new NotSupportedException();
    }

    /// <summary>
    ///     When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be
    ///     written to the underlying device.
    /// </summary>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    public override void Flush()
    {
        this.brotliStream.Flush();
        this.cryptoStream.Flush();
    }

    /// <summary>
    ///     Initializes the pack stream to first compress and then encrypt.
    /// </summary>
    /// <param name="writeableStream">The packer stream writes the output to this stream.</param>
    /// <param name="aesKey">The aes key that used to encrypt the stream data.</param>
    /// <returns>A new <see cref="PackerStream" /> instance.</returns>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <exception cref="ArgumentException">Stream is not writeable - writeableStream</exception>
    public static async Task<PackerStream> InitializePackStreamAsync(
        Stream writeableStream,
        byte[] aesKey,
        CancellationToken cancellationToken
    )
    {
        if (!writeableStream.CanWrite)
        {
            throw new ArgumentException(
                "Stream is not writeable",
                nameof(writeableStream));
        }

        // initialize aes and the encryptor
        using var aes = Aes.Create();
        aes.Padding = PaddingMode.PKCS7;
        aes.Key = aesKey;
        aes.GenerateIV();
        var cryptoTransform = aes.CreateEncryptor();

        // write the iv to the stream
        await writeableStream.WriteAsync(
            aes.IV,
            cancellationToken);

        // set the aes encrypt stream
        var cryptoStream = new CryptoStream(
            writeableStream,
            cryptoTransform,
            CryptoStreamMode.Write);

        // compress using brotli
        var brotliStream = new BrotliStream(
            cryptoStream,
            CompressionMode.Compress);

        // the packer stream disposes brotliStream and cryptoStream
        return new PackerStream(
            brotliStream,
            cryptoStream,
            false);
    }

    /// <summary>
    ///     Initializes the pack stream to first decrypt and then decompress.
    /// </summary>
    /// <param name="readableStream">The packer stream reads the input from this stream.</param>
    /// <param name="aesKey">The aes key that used to decrypt the stream data.</param>
    /// <returns>A new <see cref="PackerStream" /> instance.</returns>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <exception cref="ArgumentException">Stream is not readable - readableStream</exception>
    public static async Task<PackerStream> InitializeUnPackStreamAsync(
        Stream readableStream,
        byte[] aesKey,
        CancellationToken cancellationToken
    )
    {
        if (!readableStream.CanRead)
        {
            throw new ArgumentException(
                "Stream is not readable",
                nameof(readableStream));
        }

        // initialize aes and the decryptor
        using var aes = Aes.Create();
        aes.Padding = PaddingMode.PKCS7;
        aes.Key = aesKey;

        var iv = new byte[aes.IV.Length];
        var actualIvLength = await readableStream.ReadAsync(
            iv,
            cancellationToken);
        if (actualIvLength != iv.Length)
        {
            throw new InvalidOperationException();
        }

        aes.IV = iv;

        var cryptoTransform = aes.CreateDecryptor();

        // set the aes encrypt stream
        var cryptoStream = new CryptoStream(
            readableStream,
            cryptoTransform,
            CryptoStreamMode.Read);

        // compress using brotli
        var brotliStream = new BrotliStream(
            cryptoStream,
            CompressionMode.Decompress);

        // the packer stream disposes brotliStream and cryptoStream
        return new PackerStream(
            brotliStream,
            cryptoStream,
            true);
    }

    /// <summary>
    ///     When overridden in a derived class, reads a sequence of bytes from the current stream and advances the
    ///     position within the stream by the number of bytes read.
    /// </summary>
    /// <param name="buffer">
    ///     An array of bytes. When this method returns, the buffer contains the specified byte array with the
    ///     values between <paramref name="offset" /> and (<paramref name="offset" /> + <paramref name="count" /> - 1) replaced
    ///     by the bytes read from the current source.
    /// </param>
    /// <param name="offset">
    ///     The zero-based byte offset in <paramref name="buffer" /> at which to begin storing the data read
    ///     from the current stream.
    /// </param>
    /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
    /// <exception cref="T:System.ArgumentException">
    ///     The sum of <paramref name="offset" /> and <paramref name="count" /> is
    ///     larger than the buffer length.
    /// </exception>
    /// <exception cref="T:System.ArgumentNullException">
    ///     <paramref name="buffer" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///     <paramref name="offset" /> or <paramref name="count" /> is negative.
    /// </exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <returns>
    ///     The total number of bytes read into the buffer. This can be less than the number of bytes requested if that
    ///     many bytes are not currently available, or zero (0) if <paramref name="count" /> is 0 or the end of the stream has
    ///     been reached.
    /// </returns>
    public override int Read(byte[] buffer, int offset, int count)
    {
        if (!this.canRead)
        {
            throw new NotSupportedException();
        }

        return this.brotliStream.Read(
            buffer,
            offset,
            count);
    }

    /// <summary>When overridden in a derived class, sets the position within the current stream.</summary>
    /// <param name="offset">A byte offset relative to the <paramref name="origin" /> parameter.</param>
    /// <param name="origin">
    ///     A value of type <see cref="T:System.IO.SeekOrigin" /> indicating the reference point used to
    ///     obtain the new position.
    /// </param>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.NotSupportedException">
    ///     The stream does not support seeking, such as if the stream is
    ///     constructed from a pipe or console output.
    /// </exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <returns>The new position within the current stream.</returns>
    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotSupportedException();
    }

    /// <summary>When overridden in a derived class, sets the length of the current stream.</summary>
    /// <param name="value">The desired length of the current stream in bytes.</param>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.NotSupportedException">
    ///     The stream does not support both writing and seeking, such as if the
    ///     stream is constructed from a pipe or console output.
    /// </exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    public override void SetLength(long value)
    {
        throw new NotSupportedException();
    }

    /// <summary>
    ///     When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current
    ///     position within this stream by the number of bytes written.
    /// </summary>
    /// <param name="buffer">
    ///     An array of bytes. This method copies <paramref name="count" /> bytes from
    ///     <paramref name="buffer" /> to the current stream.
    /// </param>
    /// <param name="offset">
    ///     The zero-based byte offset in <paramref name="buffer" /> at which to begin copying bytes to the
    ///     current stream.
    /// </param>
    /// <param name="count">The number of bytes to be written to the current stream.</param>
    /// <exception cref="T:System.ArgumentException">
    ///     The sum of <paramref name="offset" /> and <paramref name="count" /> is
    ///     greater than the buffer length.
    /// </exception>
    /// <exception cref="T:System.ArgumentNullException">
    ///     <paramref name="buffer" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///     <paramref name="offset" /> or <paramref name="count" /> is negative.
    /// </exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred, such as the specified file cannot be found.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">
    ///     <see cref="M:System.IO.Stream.Write(System.Byte[],System.Int32,System.Int32)" /> was called after the stream was
    ///     closed.
    /// </exception>
    public override void Write(byte[] buffer, int offset, int count)
    {
        if (!this.CanWrite)
        {
            throw new NotSupportedException();
        }

        this.brotliStream.Write(
            buffer,
            offset,
            count);
    }

    /// <summary>
    ///     Releases the unmanaged resources used by the <see cref="T:System.IO.Stream" /> and optionally releases the
    ///     managed resources.
    /// </summary>
    /// <param name="disposing">
    ///     <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only
    ///     unmanaged resources.
    /// </param>
    protected override void Dispose(bool disposing)
    {
        this.brotliStream.Dispose();
        this.cryptoStream.Dispose();
        base.Dispose(disposing);
    }
}
