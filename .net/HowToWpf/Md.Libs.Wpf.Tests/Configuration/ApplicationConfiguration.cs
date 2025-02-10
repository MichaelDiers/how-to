namespace Md.Libs.Wpf.Tests.Configuration;

/// <summary>
///     The application configuration that describes the <c>appsettings.json</c> file.
/// </summary>
/// <param name="key">The value of the <see cref="Key" /> property.</param>
internal class ApplicationConfiguration(string key)
{
    /// <summary>
    ///     Gets the key value.
    /// </summary>
    public string Key => key;
}
