namespace ClassLibrary;

internal class CalcService : ICalcService
{
    public IModel Add(IModel model, int value)
    {
        return new Model(model.Value + value);
    }
}
