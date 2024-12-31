namespace ClassLibrary.Tests.ParallelAndSequential;

/// <summary>
///     Tests within the same class run always sequential, like <see cref="Add" /> and <see cref="Subtract" />.
///     Tests within the same <see cref="CollectionAttribute" />, like <see cref="TestCollectionName.TestCollectionA" />
///     run sequential, like <see cref="TestsA" /> and <see cref="TestsB" />.
///     Tests in <see cref="TestsC" /> run in parallel to the collection <see cref="TestCollectionName.TestCollectionA" />.
/// </summary>
[Collection(nameof(TestCollectionName.TestCollectionA))]
public class TestsA
{
    public const int Delay = 2000;

    [Fact]
    public async Task Add()
    {
        await Task.Delay(
            TestsA.Delay,
            TestContext.Current.CancellationToken);

        Assert.Equal(
            2,
            1 + 1);
    }

    [Fact]
    public async Task Subtract()
    {
        await Task.Delay(
            TestsA.Delay,
            TestContext.Current.CancellationToken);

        Assert.Equal(
            0,
            1 - 1);
    }
}
