namespace ClassLibrary.Tests;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal static class ServiceInitializer
{
    private static ICalcService? calcService;

    private static Func<int, IModel>? modelGenerator;

    public static ICalcService GetCalcService()
    {
        if (ServiceInitializer.calcService is null)
        {
            ServiceInitializer.Initialize();
        }

        Assert.NotNull(ServiceInitializer.calcService);
        return ServiceInitializer.calcService;
    }

    public static Func<int, IModel> GetModelGenerator()
    {
        if (ServiceInitializer.modelGenerator is null)
        {
            ServiceInitializer.Initialize();
        }

        Assert.NotNull(ServiceInitializer.modelGenerator);
        return ServiceInitializer.modelGenerator;
    }

    private static void Initialize()
    {
        var builder = new HostApplicationBuilder();
        builder.Services.TryAddDependencies();

        var host = builder.Build();
        ServiceInitializer.calcService = host.Services.GetRequiredService<ICalcService>();
        ServiceInitializer.modelGenerator = host.Services.GetRequiredService<Func<int, IModel>>();
    }
}
