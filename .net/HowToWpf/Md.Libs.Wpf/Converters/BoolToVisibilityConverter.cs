namespace Md.Libs.Wpf.Converters;

using System.Globalization;
using System.Windows;
using System.Windows.Data;

/// <summary>
///     A <see cref="IValueConverter" /> that converts <see cref="bool" /> to <see cref="Visibility" />. The values
///     <c>null</c> and <c>false</c> are converted to <see cref="Visibility.Collapsed" />; <c>true</c> is converted to
///     <see cref="Visibility.Visible" />.
/// </summary>
/// <remarks>The method <see cref="IValueConverter.ConvertBack" /> is not implemented.</remarks>
public class BoolToVisibilityConverter : IValueConverter
{
    /// <summary>Converts a value.</summary>
    /// <param name="value">The value produced by the binding source.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
    public object Convert(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        if (value is not bool boolValue)
        {
            return Visibility.Collapsed;
        }

        return boolValue ? Visibility.Visible : Visibility.Collapsed;
    }

    /// <summary>Converts a value.</summary>
    /// <param name="value">The value that is produced by the binding target.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
    /// <exception cref="NotImplementedException">Thrown if the method is called.</exception>
    /// <remarks>The method is not implemented.</remarks>
    public object ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        throw new NotImplementedException();
    }
}
