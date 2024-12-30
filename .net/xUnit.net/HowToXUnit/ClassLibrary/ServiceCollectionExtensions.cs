namespace ClassLibrary;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection TryAddDependencies(this IServiceCollection services)
    {
        services.TryAddSingleton<ICalcService, CalcService>();
        services.TryAddSingleton<Func<int, IModel>>(_ => value => new Model(value));

        return services;
    }
}
