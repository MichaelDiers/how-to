namespace ClassLibrary;

internal class CalcService : ICalcService
{
    public async Task<IModel> AddAsync(IModel model, int value, CancellationToken cancellationToken)
    {
        return await Task.FromResult(new Model(model.Value + value));
    }
}
