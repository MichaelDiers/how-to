namespace Md.Libs.Wpf.Tests.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
///     An extension to <see cref="IServiceCollection" />.
/// </summary>
internal static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds the <see cref="ITestService" /> dependency.
    /// </summary>
    /// <param name="services">The dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        services.TryAddScoped<ITestService, TestService>();
        return services;
    }
}
