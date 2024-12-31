namespace ClassLibrary.Tests.SharedContext;

public class ClassFixture : IDisposable
{
    public ADisposableObject ADisposableObject { get; } = new();

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        this.ADisposableObject.Dispose();
    }
}
