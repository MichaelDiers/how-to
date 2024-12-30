namespace ClassLibrary.Tests.ClassDataTests;

public class DataProviderArrayOfObjectAsync : IAsyncEnumerable<object[]>
{
    /// <summary>Returns an enumerator that iterates asynchronously through the collection.</summary>
    /// <param name="cancellationToken">
    ///     A <see cref="T:System.Threading.CancellationToken" /> that may be used to cancel the
    ///     asynchronous iteration.
    /// </param>
    /// <returns>An enumerator that can be used to iterate asynchronously through the collection.</returns>
    public async IAsyncEnumerator<object[]> GetAsyncEnumerator(CancellationToken cancellationToken = new())
    {
        yield return await Task.FromResult<object[]>(["foo", 5, new Data("bar")]);
        yield return await Task.FromResult<object[]>(["foobar", 5, new Data("baz")]);
    }
}
