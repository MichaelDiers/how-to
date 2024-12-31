namespace ClassLibrary.Tests.ParallelAndSequential;

/// <summary>
///     Tests within the same class run always sequential, like <see cref="Divide" /> and <see cref="Multiply" />.
///     Tests within the same <see cref="CollectionAttribute" />, like <see cref="TestCollectionName.TestCollectionA" />
///     run sequential, like <see cref="TestsA" /> and <see cref="TestsB" />.
///     Tests in <see cref="TestsC" /> run in parallel to the collection <see cref="TestCollectionName.TestCollectionA" />.
/// </summary>
[Collection(nameof(TestCollectionName.TestCollectionA))]
public class TestsB
{
    [Fact]
    public async Task Divide()
    {
        await Task.Delay(
            TestsA.Delay,
            TestContext.Current.CancellationToken);

        Assert.Equal(
            1,
            1 / 1);
    }

    [Fact]
    public async Task Multiply()
    {
        await Task.Delay(
            TestsA.Delay,
            TestContext.Current.CancellationToken);

        Assert.Equal(
            1,
            1 * 1);
    }
}
