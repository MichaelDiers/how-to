namespace Md.Libs.Wpf.Configuration;

using System.IO;
using Microsoft.Extensions.Configuration;

/// <summary>
///     Build the configuration from an <c>appsettings.json</c> file.
/// </summary>
public static class CustomConfigurationBuilder
{
    /// <summary>
    ///     The default name of the settings file.
    /// </summary>
    private const string AppSettingsJson = "appsettings.json";

    /// <summary>
    ///     Read the settings file and build the configuration.
    /// </summary>
    /// <typeparam name="T">The type of the configuration.</typeparam>
    /// <returns>The requested configuration of type <typeparamref name="T" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the configuration initialization fails.</exception>
    public static T Build<T>()
    {
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(
                CustomConfigurationBuilder.AppSettingsJson,
                false,
                false);

        var configuration = builder.Build();

        return configuration.Get<T>() ?? throw new InvalidOperationException($"Cannot initialize {typeof(T).Name}.");
    }
}
