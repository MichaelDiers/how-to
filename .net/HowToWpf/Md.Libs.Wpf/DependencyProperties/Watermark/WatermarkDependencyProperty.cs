namespace Md.Libs.Wpf.DependencyProperties.Watermark;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

/// <summary>
///     Add a watermark to <see cref="ComboBox" />, <see cref="PasswordBox" /> and <see cref="TextBox" />.
/// </summary>
public static class WatermarkDependencyProperty
{
    /// <summary>
    ///     The attached watermark dependency property.
    /// </summary>
    public static DependencyProperty WatermarkProperty = DependencyProperty.RegisterAttached(
        nameof(WatermarkDependencyProperty.WatermarkProperty)[..^8],
        typeof(string),
        typeof(WatermarkDependencyProperty),
        new PropertyMetadata(WatermarkDependencyProperty.OnPropertyChanged));

    /// <summary>
    ///     Gets the text of the watermark.
    /// </summary>
    /// <param name="obj">The object to that the watermark is attached.</param>
    /// <returns>The text of the watermark.</returns>
    public static string GetWatermark(DependencyObject obj)
    {
        return (string) obj.GetValue(WatermarkDependencyProperty.WatermarkProperty);
    }

    /// <summary>
    ///     Sets the text of the watermark.
    /// </summary>
    /// <param name="obj">The object to that the watermark is attached.</param>
    /// <param name="watermark">The text of the watermark.</param>
    public static void SetWatermark(DependencyObject obj, string watermark)
    {
        obj.SetValue(
            WatermarkDependencyProperty.WatermarkProperty,
            watermark);
    }

    /// <summary>
    ///     Adds the watermark adorner to the <paramref name="control" />.
    /// </summary>
    /// <param name="control">The control to that the <see cref="WatermarkAdorner" /> is added.</param>
    private static void AddWatermark(Control control)
    {
        // check if the control has an adorner layer
        var adornerLayer = AdornerLayer.GetAdornerLayer(control);
        if (adornerLayer is null)
        {
            return;
        }

        // check if the adorner is already added
        var adorners = adornerLayer.GetAdorners(control);
        if (adorners is not null && adorners.Any(adorner => adorner.GetType() == typeof(WatermarkAdorner)))
        {
            return;
        }

        // add the adorner
        adornerLayer.Add(
            new WatermarkAdorner(
                control,
                WatermarkDependencyProperty.GetWatermark(control)));
    }

    /// <summary>
    ///     Checks the display conditions of the <see cref="WatermarkAdorner" /> for the supported controls. Handles adding and
    ///     removing the <see cref="WatermarkAdorner" />.
    /// </summary>
    /// <param name="sender">The sender that has raised an event that can change the display condition of the watermark.</param>
    private static void HandleWatermark(object? sender)
    {
        // reject non-controls
        if (sender is not Control control)
        {
            return;
        }

        // reject if adorner layer is not available
        var adornerLayer = AdornerLayer.GetAdornerLayer(control);
        if (adornerLayer is null)
        {
            return;
        }

        // control is not visible
        if (!control.IsVisible)
        {
            WatermarkDependencyProperty.RemoveWatermark(control);
            return;
        }

        // check display conditions for controls
        var addWatermark = control is ComboBox {IsDropDownOpen: false, SelectedItem: null}
            or TextBox {IsFocused: false, Text: null or ""}
            or PasswordBox {IsFocused: false, Password: null or ""};

        // add the watermark
        if (addWatermark)
        {
            WatermarkDependencyProperty.AddWatermark(control);
        }
        else
        {
            // remove the watermark
            WatermarkDependencyProperty.RemoveWatermark(control);
        }
    }

    /// <summary>
    ///     Called when an event is raised that could change the watermark visibility.
    /// </summary>
    /// <param name="sender">The sender that raised the event.</param>
    /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
    private static void OnHandleEvent(object sender, EventArgs e)
    {
        WatermarkDependencyProperty.HandleWatermark(sender);
    }

    /// <summary>
    ///     Called when an event is raised that could change the watermark visibility.
    /// </summary>
    /// <param name="sender">The sender that raised the event.</param>
    /// <param name="e">
    ///     The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event
    ///     data.
    /// </param>
    private static void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        WatermarkDependencyProperty.HandleWatermark(sender);
    }

    /// <summary>
    ///     Called when the dependency property <see cref="WatermarkProperty" /> changed.
    /// </summary>
    /// <param name="d">The dependency object to the <see cref="WatermarkProperty" /> is attached.</param>
    /// <param name="e">
    ///     The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event
    ///     data.
    /// </param>
    private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Control control)
        {
            return;
        }

        control.Loaded += WatermarkDependencyProperty.OnHandleEvent;
        control.Unloaded += WatermarkDependencyProperty.OnUnloaded;

        control.IsVisibleChanged += WatermarkDependencyProperty.OnIsVisibleChanged;

        switch (control)
        {
            case ComboBox comboBox:
                comboBox.SelectionChanged += WatermarkDependencyProperty.OnHandleEvent;
                break;
            case PasswordBox passwordBox:
                passwordBox.GotFocus += WatermarkDependencyProperty.OnHandleEvent;
                passwordBox.LostFocus += WatermarkDependencyProperty.OnHandleEvent;
                passwordBox.PasswordChanged += WatermarkDependencyProperty.OnHandleEvent;
                break;
            case TextBox textBox:
                textBox.GotFocus += WatermarkDependencyProperty.OnHandleEvent;
                textBox.LostFocus += WatermarkDependencyProperty.OnHandleEvent;
                textBox.TextChanged += WatermarkDependencyProperty.OnHandleEvent;
                break;
        }
    }

    /// <summary>
    ///     Called when the control to that the <see cref="WatermarkProperty" /> is attached is unloaded.
    /// </summary>
    /// <param name="sender">The sender that raised the event.</param>
    /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
    private static void OnUnloaded(object sender, RoutedEventArgs e)
    {
        if (sender is not Control control)
        {
            return;
        }

        control.Loaded -= WatermarkDependencyProperty.OnHandleEvent;
        control.Unloaded -= WatermarkDependencyProperty.OnUnloaded;

        control.IsVisibleChanged -= WatermarkDependencyProperty.OnIsVisibleChanged;

        switch (control)
        {
            case ComboBox comboBox:
                comboBox.SelectionChanged -= WatermarkDependencyProperty.OnHandleEvent;
                break;
            case TextBox textBox:
                textBox.GotFocus -= WatermarkDependencyProperty.OnHandleEvent;
                textBox.LostFocus -= WatermarkDependencyProperty.OnHandleEvent;
                textBox.TextChanged -= WatermarkDependencyProperty.OnHandleEvent;
                break;
            case PasswordBox passwordBox:
                passwordBox.GotFocus -= WatermarkDependencyProperty.OnHandleEvent;
                passwordBox.LostFocus -= WatermarkDependencyProperty.OnHandleEvent;
                passwordBox.PasswordChanged -= WatermarkDependencyProperty.OnHandleEvent;
                break;
        }
    }

    /// <summary>
    ///     Removes the watermark from the adorner layer.
    /// </summary>
    /// <param name="control">The control from that the adorner is removed.</param>
    private static void RemoveWatermark(Control control)
    {
        var adornerLayer = AdornerLayer.GetAdornerLayer(control);
        if (adornerLayer is null)
        {
            return;
        }

        var adorners = adornerLayer.GetAdorners(control);
        if (adorners is null)
        {
            return;
        }

        foreach (var adorner in adorners.Where(adorner => adorner.GetType() == typeof(WatermarkAdorner)))
        {
            adornerLayer.Remove(adorner);
        }
    }
}
