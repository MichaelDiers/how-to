namespace Md.Libs.Wpf.Tests.Configuration;

using Md.Libs.Wpf.Configuration;

/// <summary>
///     Tests for <see cref="CustomConfigurationBuilder" />.
/// </summary>
public class CustomConfigurationBuilderTests
{
    /// <summary>
    ///     Build a configuration from an <c>appsettings.json</c> file.
    /// </summary>
    [Fact]
    public void Build()
    {
        var configuration = CustomConfigurationBuilder.Build<ApplicationConfiguration>();

        Assert.NotNull(configuration);
        Assert.IsType<ApplicationConfiguration>(configuration);
    }
}
