namespace ClassLibrary.Tests;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class UsingIAsyncLifetime : IAsyncLifetime
{
    private ICalcService? calcService;
    private Func<int, IModel>? createModel;

    [Theory]
    [InlineData(
        20,
        10)]
    [InlineData(
        0,
        0)]
    public async Task AddAsync(int a, int b)
    {
        Assert.NotNull(this.calcService);
        Assert.NotNull(this.createModel);

        var result = await this.calcService.AddAsync(
            this.createModel(a),
            b,
            CancellationToken.None);

        Assert.Equal(
            a + b,
            result.Value);
    }

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources
    ///     asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    public async ValueTask DisposeAsync()
    {
        Assert.NotNull(this.calcService);
        await this.calcService.DisposeAsync();
    }

    /// <summary>
    ///     Called immediately after the class has been created, before it is used.
    /// </summary>
    public async ValueTask InitializeAsync()
    {
        var builder = new HostApplicationBuilder();
        builder.Services.TryAddDependencies();
        using var host = builder.Build();

        this.createModel = host.Services.GetRequiredService<Func<int, IModel>>();
        this.calcService = host.Services.GetRequiredService<ICalcService>();

        await this.calcService.InitAsync(CancellationToken.None);
    }
}
