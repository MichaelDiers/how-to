namespace StreamPipeline.Tests;

public abstract class PackerStreamCommonTests
{
    [Fact]
    public void CanSeek()
    {
        using var memoryStream = new MemoryStream();
        using var packerStream = this.InitializePackerStream(memoryStream);

        Assert.False(packerStream.CanSeek);
    }

    [Fact]
    public void Flush()
    {
        using var memoryStream = new MemoryStream();
        using var packerStream = this.InitializePackerStream(memoryStream);

        packerStream.Flush();
    }

    [Fact]
    public void Length()
    {
        var memoryStream = new MemoryStream();
        var packerStream = this.InitializePackerStream(memoryStream);

        Assert.Throws<NotSupportedException>(() => packerStream.Length);

        packerStream.Dispose();
        memoryStream.Dispose();
    }

    [Fact]
    public void Position()
    {
        var memoryStream = new MemoryStream();
        var packerStream = this.InitializePackerStream(memoryStream);

        Assert.Throws<NotSupportedException>(() => packerStream.Position);

        packerStream.Dispose();
        memoryStream.Dispose();
    }

    [Fact]
    public void Seek()
    {
        var memoryStream = new MemoryStream();
        var packerStream = this.InitializePackerStream(memoryStream);

        Assert.Throws<NotSupportedException>(
            () => packerStream.Seek(
                1,
                SeekOrigin.Begin));

        packerStream.Dispose();
        memoryStream.Dispose();
    }

    [Fact]
    public void SetLength()
    {
        var memoryStream = new MemoryStream();
        var packerStream = this.InitializePackerStream(memoryStream);

        Assert.Throws<NotSupportedException>(() => packerStream.SetLength(1));

        packerStream.Dispose();
        memoryStream.Dispose();
    }

    protected abstract PackerStream InitializePackerStream(Stream stream);
}
