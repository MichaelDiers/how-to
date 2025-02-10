namespace Md.Libs.Wpf.Tests.Converters;

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Md.Libs.Wpf.Converters;

/// <summary>
///     Tests for <see cref="BoolToVisibilityConverter" />.
/// </summary>
public class BoolToVisibilityConverterTests
{
    /// <summary>
    ///     The initialized converter under test.
    /// </summary>
    private readonly IValueConverter converter = new BoolToVisibilityConverter();

    /// <summary>
    ///     Convert <see cref="bool" /> to <see cref="Visibility" />.
    /// </summary>
    /// <param name="value">The <see cref="bool" /> value that is converted to <see cref="Visibility" />.</param>
    /// <param name="expectedVisibility">The expected result of <see cref="IValueConverter.Convert" />.</param>
    [Theory]
    [InlineData(
        null,
        Visibility.Collapsed)]
    [InlineData(
        false,
        Visibility.Collapsed)]
    [InlineData(
        true,
        Visibility.Visible)]
    public void Convert(bool? value, Visibility expectedVisibility)
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
}
