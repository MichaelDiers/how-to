namespace ClassLibrary.Tests;

public class UsingFact
{
    private readonly ICalcService calcService = ServiceInitializer.GetCalcService();
    private readonly Func<int, IModel> modelGenerator = ServiceInitializer.GetModelGenerator();

    [Fact]
    public async Task AddAsync()
    {
        const int a = 10;
        const int b = 22;
        const int expected = a + b;

        var actual = await this.calcService.AddAsync(
            this.modelGenerator(a),
            b,
            CancellationToken.None);

        Assert.Equal(
            expected,
            actual.Value);
    }
}
