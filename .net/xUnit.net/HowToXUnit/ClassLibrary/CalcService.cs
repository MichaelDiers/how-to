namespace ClassLibrary;

internal class CalcService : ICalcService
{
    public async Task<IModel> AddAsync(IModel model, int value, CancellationToken cancellationToken)
    {
        return await Task.FromResult(new Model(model.Value + value));
    }

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources
    ///     asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    public async ValueTask DisposeAsync()
    {
        await Task.CompletedTask;
    }

    public async Task InitAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}
