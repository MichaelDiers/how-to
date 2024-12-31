namespace ClassLibrary;

public interface ICalcService : IAsyncDisposable
{
    Task<IModel> AddAsync(IModel model, int value, CancellationToken cancellationToken);
    Task InitAsync(CancellationToken cancellationToken);
}
