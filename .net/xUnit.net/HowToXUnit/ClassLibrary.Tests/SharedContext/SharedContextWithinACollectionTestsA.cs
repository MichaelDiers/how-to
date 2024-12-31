namespace ClassLibrary.Tests.SharedContext;

[Collection(CustomCollectionDefinition.CollectionName)]
public class SharedContextWithinACollectionTestsA(ClassFixture classFixture, ITestOutputHelper testOutputHelper)
{
    /// <summary>
    ///     Tests <see cref="TestA" />, <see cref="TestB" />, <see cref="SharedContextWithinACollectionTestsB.TestC" /> and
    ///     <see cref="SharedContextWithinACollectionTestsB.TestD" /> use the same test context <see cref="ClassFixture" /> and
    ///     therefore the output is identical.
    /// </summary>
    [Fact]
    public void TestA()
    {
        testOutputHelper.WriteLine(classFixture.ADisposableObject.Guid.ToString());
    }

    /// <summary>
    ///     Tests <see cref="TestA" />, <see cref="TestB" />, <see cref="SharedContextWithinACollectionTestsB.TestC" /> and
    ///     <see cref="SharedContextWithinACollectionTestsB.TestD" /> use the same test context <see cref="ClassFixture" /> and
    ///     therefore the output is identical.
    /// </summary>
    [Fact]
    public void TestB()
    {
        testOutputHelper.WriteLine(classFixture.ADisposableObject.Guid.ToString());
    }
}
