namespace Md.Libs.Wpf.DependencyProperties.Watermark;

using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

/// <summary>
///     Add a watermark to an element.
/// </summary>
/// <seealso cref="System.Windows.Documents.Adorner" />
internal class WatermarkAdorner : Adorner
{
    /// <summary>
    ///     The text of the watermark.
    /// </summary>
    private readonly string watermark;

    /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.Adorner" /> class.</summary>
    /// <param name="adornedElement">The element to bind the adorner to.</param>
    /// <param name="watermark">The text of the watermark.</param>
    /// <exception cref="T:System.ArgumentNullException">adornedElement is <see langword="null" />.</exception>
    public WatermarkAdorner(UIElement adornedElement, string watermark)
        : base(adornedElement)
    {
        this.IsHitTestVisible = false;
        this.watermark = watermark;
    }

    /// <summary>
    ///     When overridden in a derived class, participates in rendering operations that are directed by the layout
    ///     system. The rendering instructions for this element are not used directly when this method is invoked, and are
    ///     instead preserved for later asynchronous use by layout and drawing.
    /// </summary>
    /// <param name="drawingContext">
    ///     The drawing instructions for a specific element. This context is provided to the layout
    ///     system.
    /// </param>
    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);

        if (this.AdornedElement is not Control control)
        {
            return;
        }

        var formattedText = new FormattedText(
            this.watermark,
            CultureInfo.CurrentUICulture,
            FlowDirection.LeftToRight,
            new Typeface(control.FontFamily.Source),
            control.FontSize,
            new SolidColorBrush(Colors.Gray),
            new NumberSubstitution(),
            VisualTreeHelper.GetDpi(control).PixelsPerDip);

        formattedText.SetFontStyle(FontStyles.Italic);

        var origin = control is ComboBox comboBox
            ? WatermarkAdorner.CalculateOrigin(
                comboBox,
                formattedText)
            : WatermarkAdorner.CalculateOrigin(
                control,
                formattedText);

        drawingContext.DrawText(
            formattedText,
            origin);
    }

    /// <summary>
    ///     Calculate the origin of the watermark text <paramref name="formattedText" />.
    /// </summary>
    /// <param name="control">The control that adorner layer is used.</param>
    /// <param name="formattedText">The <see cref="FormattedText" /> of the watermark.</param>
    /// <returns>The calculated origin of the watermark.</returns>
    /// <remarks>
    ///     A <see cref="ComboBox" /> uses a control for displaying the selected element. The calculation does not include
    ///     the margin and padding of this inner control.
    /// </remarks>
    private static Point CalculateOrigin(ComboBox control, FormattedText formattedText)
    {
        var origin = WatermarkAdorner.CalculateOrigin(
            control as Control,
            formattedText);

        var x = origin.X;
        switch (control.HorizontalContentAlignment)
        {
            case HorizontalAlignment.Left:
            case HorizontalAlignment.Stretch:
                x += control.Padding.Left;
                break;
            case HorizontalAlignment.Center:
                break;
            case HorizontalAlignment.Right:
                x -= control.Padding.Right;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        // a combo box ignores the vertical content alignment
        var y = control.RenderSize.Height / 2 - formattedText.Height / 2;

        return new Point(
            x,
            y);
    }

    /// <summary>
    ///     Calculate the origin of the watermark text <paramref name="formattedText" />.
    /// </summary>
    /// <param name="control">The control that adorner layer is used.</param>
    /// <param name="formattedText">The <see cref="FormattedText" /> of the watermark.</param>
    /// <returns>The calculated origin of the watermark.</returns>
    private static Point CalculateOrigin(Control control, FormattedText formattedText)
    {
        var x = control.HorizontalContentAlignment switch
        {
            HorizontalAlignment.Left or HorizontalAlignment.Stretch => control.Padding.Left,
            HorizontalAlignment.Center => control.RenderSize.Width / 2 - formattedText.Width / 2,
            HorizontalAlignment.Right => control.RenderSize.Width - control.Padding.Right - formattedText.Width,
            _ => throw new ArgumentOutOfRangeException()
        };

        var y = control.VerticalContentAlignment switch
        {
            VerticalAlignment.Stretch or VerticalAlignment.Top => control.Padding.Top,
            VerticalAlignment.Center => control.RenderSize.Height / 2 - formattedText.Height / 2,
            VerticalAlignment.Bottom => control.RenderSize.Height - formattedText.Height - control.Padding.Bottom,
            _ => throw new ArgumentOutOfRangeException()
        };

        return new Point(
            x,
            y);
    }
}
