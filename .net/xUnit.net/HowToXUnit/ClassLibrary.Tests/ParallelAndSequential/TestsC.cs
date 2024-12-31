namespace ClassLibrary.Tests.ParallelAndSequential;

/// <summary>
///     Tests within the same class run always sequential, like <see cref="Bar" /> and <see cref="Foo" />.
///     Tests within the same <see cref="CollectionAttribute" />, like <see cref="TestCollectionName.TestCollectionA" />
///     run sequential, like <see cref="TestsA" /> and <see cref="TestsB" />.
///     Tests in <see cref="TestsC" /> run in parallel to the collection <see cref="TestCollectionName.TestCollectionA" />.
/// </summary>
public class TestsC
{
    [Fact]
    public async Task Bar()
    {
        await Task.Delay(
            TestsA.Delay,
            TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task Foo()
    {
        await Task.Delay(
            TestsA.Delay,
            TestContext.Current.CancellationToken);
    }
}
