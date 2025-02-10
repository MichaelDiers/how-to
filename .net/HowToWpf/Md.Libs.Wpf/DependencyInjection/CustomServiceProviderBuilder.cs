namespace Md.Libs.Wpf.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Create a <see cref="ServiceProvider" /> that is used for dependency injection.
/// </summary>
public static class CustomServiceProviderBuilder
{
    /// <summary>
    ///     Build a new <see cref="ServiceProvider" /> and add the given dependencies <paramref name="addDependencies" /> to
    ///     the provider.
    /// </summary>
    /// <param name="addDependencies">Dependencies that are added to the new <see cref="ServiceProvider" />.</param>
    /// <returns>The initialized <see cref="ServiceProvider" />.</returns>
    public static IServiceProvider Build(params Func<IServiceCollection, IServiceCollection>[] addDependencies)
    {
        var services = new ServiceCollection();
        foreach (var addDependency in addDependencies)
        {
            addDependency(services);
        }

        return services.BuildServiceProvider();
    }
}
