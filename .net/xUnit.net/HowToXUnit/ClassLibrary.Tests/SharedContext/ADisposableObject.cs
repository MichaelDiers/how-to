namespace ClassLibrary.Tests.SharedContext;

internal class ADisposableObject : IDisposable
{
    public Guid Guid { get; } = Guid.NewGuid();

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        // dispose something
    }
}
