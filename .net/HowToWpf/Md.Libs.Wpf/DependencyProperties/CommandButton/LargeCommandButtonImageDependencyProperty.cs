namespace Md.Libs.Wpf.DependencyProperties.CommandButton;

using System.Windows;
using System.Windows.Controls;

/// <summary>
///     Attaches an image property to a control. Sets the style of the control if the control is a <see cref="Button" />.
/// </summary>
public static class LargeCommandButtonImageDependencyProperty
{
    /// <summary>
    ///     The resource name of the <see cref="Button.Style" />.
    /// </summary>
    private const string LargeCommandButtonStyle =
        nameof(LargeCommandButtonImageDependencyProperty.LargeCommandButtonStyle);

    /// <summary>
    ///     The attached image property.
    /// </summary>
    public static DependencyProperty ImageProperty = DependencyProperty.RegisterAttached(
        "Image",
        typeof(string),
        typeof(LargeCommandButtonImageDependencyProperty),
        new PropertyMetadata(LargeCommandButtonImageDependencyProperty.PropertyChangedCallback));

    /// <summary>
    ///     Gets the value of the image property.
    /// </summary>
    /// <param name="obj">The property is attached to this <see cref="DependencyObject" />.</param>
    /// <returns>The path to the image.</returns>
    public static string GetImage(DependencyObject obj)
    {
        return (string) obj.GetValue(LargeCommandButtonImageDependencyProperty.ImageProperty);
    }

    /// <summary>
    ///     Sets the value of the image property.
    /// </summary>
    /// <param name="obj">The property is attached to this <see cref="DependencyObject" />.</param>
    /// <param name="value">The new path of the image.</param>
    public static void SetImage(DependencyObject obj, string value)
    {
        obj.SetValue(
            LargeCommandButtonImageDependencyProperty.ImageProperty,
            value);
    }

    /// <summary>
    ///     Called if <see cref="ImageProperty" /> changed.
    /// </summary>
    /// <param name="d">The <see cref="DependencyObject" /> the <see cref="DependencyProperty" /> is attached to.</param>
    /// <param name="e">The event args.</param>
    private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Button button &&
            Application.Current.FindResource(LargeCommandButtonImageDependencyProperty.LargeCommandButtonStyle) is Style
                style)
        {
            button.Style = style;
        }
    }
}
