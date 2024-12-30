namespace ClassLibrary;

public interface ICalcService
{
    Task<IModel> AddAsync(IModel model, int value, CancellationToken cancellationToken);
}
