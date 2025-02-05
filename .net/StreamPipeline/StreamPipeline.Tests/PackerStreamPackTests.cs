namespace StreamPipeline.Tests;

using System.Security.Cryptography;

public class PackerStreamPackTests : PackerStreamCommonTests
{
    [Fact]
    public void CanRead()
    {
        var memoryStream = new MemoryStream();
        var packerStream = this.InitializePackerStream(memoryStream);

        Assert.False(packerStream.CanRead);

        packerStream.Dispose();
        memoryStream.Dispose();
    }

    [Fact]
    public void CanWrite()
    {
        var memoryStream = new MemoryStream();
        var packerStream = this.InitializePackerStream(memoryStream);

        Assert.True(packerStream.CanWrite);

        packerStream.Dispose();
        memoryStream.Dispose();
    }

    [Fact]
    public void Read_Buffer_Offset_Count()
    {
        var memoryStream = new MemoryStream();
        var packerStream = this.InitializePackerStream(memoryStream);

        var exception = Assert.Throws<NotSupportedException>(
            () => packerStream.Read(
                new byte[10],
                1,
                3));

        Assert.Equal(
            "Stream is not readable.",
            exception.Message);

        packerStream.Dispose();
        memoryStream.Dispose();
    }

    /*
    [Fact]
    public async Task AAA()
    {
        var x = await PackerStream.InitializePackStreamAsync(
            new MemoryStream(),
            new byte[1],
            TestContext.Current.CancellationToken);

        var z1 = x.Length;
        var z2 = x.CanRead;
        var z3 = x.CanSeek;
        var z4 = x.CanWrite;
        var z5 = x.Position;
        var z6 = x.WriteTimeout;
        var z7 = x.CanTimeout;
        var z8 = x.ReadTimeout;
        x.Read(new byte[0]);
        x.Flush();
        x.Seek(
            10,
            SeekOrigin.Begin);
        x.SetLength(10);
    }

    [Theory]
    [MemberData(nameof(PackerStreamPackTests.PackerStreamAsync))]
    public async Task CanRead(PackerStream packerStream, MemoryStream memoryStream, bool readable)
    {
        Assert.Equal(
            readable,
            packerStream.CanRead);

        await packerStream.DisposeAsync();
        await memoryStream.DisposeAsync();
    }

    [Theory]
    [MemberData(nameof(PackerStreamPackTests.PackerStreamAsync))]
    public async Task CanSeek(PackerStream packerStream, MemoryStream memoryStream, bool readable)
    {
        Assert.False(packerStream.CanSeek);

        await packerStream.DisposeAsync();
        await memoryStream.DisposeAsync();
    }

    [Theory]
    [MemberData(nameof(PackerStreamPackTests.PackerStreamAsync))]
    public async Task CanTimeout(PackerStream packerStream, MemoryStream memoryStream, bool readable)
    {
        Assert.False(packerStream.CanTimeout);

        await packerStream.DisposeAsync();
        await memoryStream.DisposeAsync();
    }

    [Theory]
    [MemberData(nameof(PackerStreamPackTests.PackerStreamAsync))]
    public async Task CanWrite(PackerStream packerStream, MemoryStream memoryStream, bool readable)
    {
        Assert.Equal(
            !readable,
            packerStream.CanWrite);

        await packerStream.DisposeAsync();
        await memoryStream.DisposeAsync();
    }

    [Theory]
    [MemberData(nameof(PackerStreamPackTests.PackerStreamAsync))]
    public async Task Flush(PackerStream packerStream, MemoryStream memoryStream, bool readable)
    {
        packerStream.Flush();

        await packerStream.DisposeAsync();
        await memoryStream.DisposeAsync();
    }

    [Theory]
    [MemberData(nameof(PackerStreamPackTests.PackerStreamAsync))]
    public async Task Length(PackerStream packerStream, MemoryStream memoryStream, bool readable)
    {
        Assert.Throws<NotSupportedException>(() => packerStream.Length);

        await packerStream.DisposeAsync();
        await memoryStream.DisposeAsync();
    }

    public static async IAsyncEnumerable<TheoryDataRow<PackerStream, MemoryStream, bool>> PackerStreamAsync()
    {
        using var aes = Aes.Create();
        var writeableStream = new MemoryStream();

        yield return (await PackerStream.InitializePackStreamAsync(
            writeableStream,
            aes.Key,
            TestContext.Current.CancellationToken), writeableStream, false);

        var data = new byte[aes.IV.Length + 1 + aes.IV.Length];
        data[0] = (byte) aes.IV.Length;
        aes.IV.CopyTo(
            data,
            1);
        aes.IV.CopyTo(
            data,
            aes.IV.Length + 1);
        var readableStream = new MemoryStream(data);

        yield return (await PackerStream.InitializeUnpackStream(
            readableStream,
            aes.Key,
            TestContext.Current.CancellationToken), readableStream, true);
    }

    [Theory]
    [MemberData(nameof(PackerStreamPackTests.PackerStreamAsync))]
    public async Task Position(PackerStream packerStream, MemoryStream memoryStream, bool readable)
    {
        Assert.Throws<NotSupportedException>(() => packerStream.Position = 10);
        Assert.Throws<NotSupportedException>(() => packerStream.Position);

        await packerStream.DisposeAsync();
        await memoryStream.DisposeAsync();
    }

    [Theory]
    [MemberData(nameof(PackerStreamPackTests.PackerStreamAsync))]
    public async Task Read(PackerStream packerStream, MemoryStream memoryStream, bool readable)
    {
        _ = packerStream.Read([1]);

        await packerStream.DisposeAsync();
        await memoryStream.DisposeAsync();
    }

    [Theory]
    [MemberData(nameof(PackerStreamPackTests.PackerStreamAsync))]
    public async Task Read_Offset_Count(PackerStream packerStream, MemoryStream memoryStream, bool readable)
    {
        _ = packerStream.Read(
            new byte[3],
            1,
            2);

        await packerStream.DisposeAsync();
        await memoryStream.DisposeAsync();
    }

    [Theory]
    [MemberData(nameof(PackerStreamPackTests.PackerStreamAsync))]
    public async Task ReadTimeout(PackerStream packerStream, MemoryStream memoryStream, bool readable)
    {
        Assert.Throws<NotSupportedException>(() => packerStream.ReadTimeout = 10);
        Assert.Throws<NotSupportedException>(() => packerStream.ReadTimeout);

        await packerStream.DisposeAsync();
        await memoryStream.DisposeAsync();
    }

    [Theory]
    [MemberData(nameof(PackerStreamPackTests.PackerStreamAsync))]
    public async Task Seek(PackerStream packerStream, MemoryStream memoryStream, bool readable)
    {
        Assert.Throws<NotSupportedException>(
            () => packerStream.Seek(
                10,
                SeekOrigin.Begin));

        await packerStream.DisposeAsync();
        await memoryStream.DisposeAsync();
    }

    [Theory]
    [MemberData(nameof(PackerStreamPackTests.PackerStreamAsync))]
    public async Task SetLength(PackerStream packerStream, MemoryStream memoryStream, bool readable)
    {
        Assert.Throws<NotSupportedException>(() => packerStream.SetLength(10));

        await packerStream.DisposeAsync();
        await memoryStream.DisposeAsync();
    }

    [Theory]
    [MemberData(nameof(PackerStreamPackTests.PackerStreamAsync))]
    public async Task WriteTimeout(PackerStream packerStream, MemoryStream memoryStream, bool readable)
    {
        Assert.Throws<NotSupportedException>(() => packerStream.WriteTimeout = 10);
        Assert.Throws<NotSupportedException>(() => packerStream.WriteTimeout);

        await packerStream.DisposeAsync();
        await memoryStream.DisposeAsync();
    }
    */
    protected override PackerStream InitializePackerStream(Stream stream)
    {
        using var aes = Aes.Create();
        return new PackerStream(
            stream,
            PackerStreamMode.Pack,
            aes.Key);
    }
}
