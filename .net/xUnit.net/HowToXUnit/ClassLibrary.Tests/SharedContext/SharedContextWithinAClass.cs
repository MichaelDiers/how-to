namespace ClassLibrary.Tests.SharedContext;

public class SharedContextWithinAClass(ClassFixture classFixture, ITestOutputHelper testOutputHelper)
    : IClassFixture<ClassFixture>
{
    /// <summary>
    ///     Tests <see cref="TestA" /> and <see cref="TestB" /> share the same context <see cref="ClassFixture" /> and
    ///     therefore the test output is identical.
    /// </summary>
    [Fact]
    public void TestA()
    {
        testOutputHelper.WriteLine(classFixture.ADisposableObject.Guid.ToString());
    }

    /// <summary>
    ///     Tests <see cref="TestA" /> and <see cref="TestB" /> share the same context <see cref="ClassFixture" /> and
    ///     therefore the test output is identical.
    /// </summary>
    [Fact]
    public void TestB()
    {
        testOutputHelper.WriteLine(classFixture.ADisposableObject.Guid.ToString());
    }
}
