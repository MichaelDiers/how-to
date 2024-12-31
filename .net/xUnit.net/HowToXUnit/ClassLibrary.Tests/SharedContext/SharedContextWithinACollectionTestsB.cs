namespace ClassLibrary.Tests.SharedContext;

[Collection(CustomCollectionDefinition.CollectionName)]
public class SharedContextWithinACollectionTestsB(ClassFixture classFixture, ITestOutputHelper testOutputHelper)
{
    /// <summary>
    ///     Tests <see cref="SharedContextWithinACollectionTestsA.TestA" />,
    ///     <see cref="SharedContextWithinACollectionTestsA.TestB" />,
    ///     <see cref="SharedContextWithinACollectionTestsB.TestC" /> and
    ///     <see cref="SharedContextWithinACollectionTestsB.TestD" /> use the same test context <see cref="ClassFixture" /> and
    ///     therefore the output is identical.
    /// </summary>
    [Fact]
    public void TestC()
    {
        testOutputHelper.WriteLine(classFixture.ADisposableObject.Guid.ToString());
    }

    /// <summary>
    ///     Tests <see cref="SharedContextWithinACollectionTestsA.TestA" />,
    ///     <see cref="SharedContextWithinACollectionTestsA.TestB" />,
    ///     <see cref="SharedContextWithinACollectionTestsB.TestC" /> and
    ///     <see cref="SharedContextWithinACollectionTestsB.TestD" /> use the same test context <see cref="ClassFixture" /> and
    ///     therefore the output is identical.
    /// </summary>
    [Fact]
    public void TestD()
    {
        testOutputHelper.WriteLine(classFixture.ADisposableObject.Guid.ToString());
    }
}
