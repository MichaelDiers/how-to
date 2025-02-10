namespace Md.Libs.Wpf.Tests.DependencyInjection;

/// <summary>
///     A service that is added as a dependency.
/// </summary>
/// <seealso cref="ServiceProviderBuilderTests" />
public interface ITestService;

/// <summary>
///     A service that is added as a dependency.
/// </summary>
/// <seealso cref="ServiceProviderBuilderTests" />
/// <seealso cref="ITestService" />
internal class TestService : ITestService;
