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
    private readonly CancellationToken cancellationToken = TestContext.Current.CancellationToken;

    [Fact]
    public async Task PackAndUnpackAsync()
    {
        // generate a new aes key
        using var aes = Aes.Create();

        // init test data
        const string data = TestData.LoremIpsum;
        var byteData = Encoding.UTF8.GetBytes(data);

        using var packedStream = new MemoryStream();
        await using (var packer = new PackerStream(
                         packedStream,
                         PackerStreamMode.Pack,
                         aes.Key))
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
        await using (var packer = new PackerStream(
                         input,
                         PackerStreamMode.Unpack,
                         aes.Key))
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
        using var aes = Aes.Create();

        // init test data
        const string data = TestData.LoremIpsum;
        var byteData = Encoding.UTF8.GetBytes(data);

        using var output = new MemoryStream();
        await using (var packer = new PackerStream(
                         output,
                         PackerStreamMode.Pack,
                         aes.Key))
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
