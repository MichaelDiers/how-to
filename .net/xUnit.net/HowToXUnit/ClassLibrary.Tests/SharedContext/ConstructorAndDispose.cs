namespace ClassLibrary.Tests.SharedContext;

public class ConstructorAndDispose(ITestOutputHelper testOutputHelper) : IDisposable
{
    private readonly ADisposableObject aDisposableObject = new();

    public void Dispose()
    {
        this.aDisposableObject.Dispose();
    }

    /// <summary>
    ///     Tests <see cref="TestA" /> and <see cref="TestB" /> do not share the same <see cref="aDisposableObject" />: For
    ///     each test in a class a new instance of the class is created.
    /// </summary>
    [Fact]
    public void TestA()
    {
        testOutputHelper.WriteLine(this.aDisposableObject.Guid.ToString());
    }

    /// <summary>
    ///     Tests <see cref="TestA" /> and <see cref="TestB" /> do not share the same <see cref="aDisposableObject" />: For
    ///     each test in a class a new instance of the class is created.
    /// </summary>
    [Fact]
    public void TestB()
    {
        testOutputHelper.WriteLine(this.aDisposableObject.Guid.ToString());
    }
}
