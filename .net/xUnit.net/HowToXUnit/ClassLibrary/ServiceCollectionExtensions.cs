namespace ClassLibrary;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection TryAddDependencies(this IServiceCollection services)
    {
        services.TryAddSingleton<ICalcService, CalcService>();

        return services;
    }
}
