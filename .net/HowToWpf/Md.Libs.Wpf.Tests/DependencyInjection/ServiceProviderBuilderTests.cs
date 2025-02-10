namespace Md.Libs.Wpf.Tests.DependencyInjection;

using Md.Libs.Wpf.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Test for <see cref="CustomServiceProviderBuilder" />.
/// </summary>
public class ServiceProviderBuilderTests
{
    /// <summary>
    ///     The initialized <see cref="IServiceProvider" />.
    /// </summary>
    private readonly IServiceProvider serviceProvider =
        CustomServiceProviderBuilder.Build(ServiceCollectionExtensions.AddDependencies);

    /// <summary>
    ///     Try to get a service of a given type.
    /// </summary>
    [Fact]
    public void GetRequiredService()
    {
        var service = this.serviceProvider.GetRequiredService<ITestService>();

        Assert.NotNull(service);
    }
}
