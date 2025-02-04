namespace StreamPipeline.Tests;

using System.Security.Cryptography;
using System.Text;

/// <summary>
///     Tests for <see cref="PackerStream" />.
/// </summary>
public class PackerStreamTests
{
    /// <summary>
    ///     The used cancellation token for async operations.
    /// </summary>
    private readonly CancellationToken cancellationToken = CancellationToken.None;

    [Fact]
    public async Task PackAndUnpackAsync()
    {
        // generate a new aes key
        var aes = Aes.Create();

        // init test data
        const string data = TestData.LoremIpsum;
        var byteData = Encoding.UTF8.GetBytes(data);

        using var packedStream = new MemoryStream();
        await using (var packer = await PackerStream.InitializePackStreamAsync(
                         packedStream,
                         aes.Key,
                         this.cancellationToken))
        {
            await packer.WriteAsync(
                byteData,
                this.cancellationToken);
        }

        var encryptedAndCompressed = packedStream.ToArray();

        Assert.NotEqual(
            byteData,
            encryptedAndCompressed);

        using var input = new MemoryStream(encryptedAndCompressed);
        using var unpackStream = new MemoryStream();
        await using (var packer = await PackerStream.InitializeUnPackStreamAsync(
                         input,
                         aes.Key,
                         this.cancellationToken))
        {
            await packer.CopyToAsync(
                unpackStream,
                this.cancellationToken);
        }

        var decryptedAndDecompressed = unpackStream.ToArray();

        Assert.Equal(
            byteData,
            decryptedAndDecompressed);
        Assert.Equal(
            data,
            Encoding.UTF8.GetString(decryptedAndDecompressed));
    }

    [Fact]
    public async Task PackAsync()
    {
        // generate a new aes key
        var aes = Aes.Create();

        // init test data
        const string data = TestData.LoremIpsum;
        var byteData = Encoding.UTF8.GetBytes(data);

        using var output = new MemoryStream();
        await using (var packer = await PackerStream.InitializePackStreamAsync(
                         output,
                         aes.Key,
                         this.cancellationToken))
        {
            await packer.WriteAsync(
                byteData,
                this.cancellationToken);
        }

        var encryptedAndCompressed = output.ToArray();

        Assert.NotEqual(
            byteData,
            encryptedAndCompressed);
    }
}
