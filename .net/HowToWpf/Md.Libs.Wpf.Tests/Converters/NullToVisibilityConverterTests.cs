namespace Md.Libs.Wpf.Tests.Converters;

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Md.Libs.Wpf.Converters;

/// <summary>
///     Tests for <see cref="NullToVisibilityConverter" />.
/// </summary>
public class NullToVisibilityConverterTests
{
    /// <summary>
    ///     The initialized converter under test.
    /// </summary>
    private readonly IValueConverter converter = new NullToVisibilityConverter();

    /// <summary>
    ///     The method <see cref="IValueConverter.ConvertBack" /> is not implemented and should throw
    ///     <see cref="NotImplementedException" /> if called.
    /// </summary>
    [Fact]
    public void ConvertBack()
    {
        foreach (var visibility in Enum.GetValues<Visibility>())
        {
            Assert.Throws<NotImplementedException>(
                () => this.converter.ConvertBack(
                    visibility,
                    typeof(object),
                    null,
                    CultureInfo.CurrentUICulture));
        }
    }

    /// <summary>
    ///     Convert <c>null</c> and non-<c>null</c> to <see cref="Visibility" />.
    /// </summary>
    /// <param name="value">The value that is converted to <see cref="Visibility" />.</param>
    /// <param name="expectedVisibility">The expected result of <see cref="IValueConverter.Convert" />.</param>
    [Theory]
    [InlineData(
        null,
        Visibility.Collapsed)]
    [InlineData(
        false,
        Visibility.Visible)]
    [InlineData(
        true,
        Visibility.Visible)]
    public void ConvertNullableBool(bool? value, Visibility expectedVisibility)
    {
        var actualVisibility = this.converter.Convert(
            value,
            typeof(object),
            null,
            CultureInfo.CurrentUICulture);

        Assert.Equal(
            expectedVisibility,
            actualVisibility);
    }

    /// <summary>
    ///     Convert <c>null</c> and non-<c>null</c> to <see cref="Visibility" />.
    /// </summary>
    /// <param name="value">The value that is converted to <see cref="Visibility" />.</param>
    /// <param name="expectedVisibility">The expected result of <see cref="IValueConverter.Convert" />.</param>
    [Theory]
    [InlineData(
        null,
        Visibility.Collapsed)]
    [InlineData(
        "string",
        Visibility.Visible)]
    public void ConvertNullableString(string? value, Visibility expectedVisibility)
    {
        var actualVisibility = this.converter.Convert(
            value,
            typeof(object),
            null,
            CultureInfo.CurrentUICulture);

        Assert.Equal(
            expectedVisibility,
            actualVisibility);
    }
}
