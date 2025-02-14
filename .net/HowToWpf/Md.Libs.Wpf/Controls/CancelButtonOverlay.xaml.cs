namespace Md.Libs.Wpf.Controls;

using System.Windows;
using System.Windows.Media;
using Md.Libs.Wpf.Commands;

/// <summary>
///     Interaction logic for CancelButtonOverlay.xaml
/// </summary>
public partial class CancelButtonOverlay
{
    /// <summary>
    ///     Extends the <see cref="CancelButtonOverlay" /> by a <see cref="DependencyProperty" /> wrapped by
    ///     <see cref="CancelCommand" />.
    /// </summary>
    public static readonly DependencyProperty CancelCommandProperty = DependencyProperty.Register(
        nameof(CancelButtonOverlay.CancelCommand),
        typeof(IAsyncCommand),
        typeof(CancelButtonOverlay),
        new PropertyMetadata(default(IAsyncCommand)));

    /// <summary>
    ///     Extends the <see cref="CancelButtonOverlay" /> by a <see cref="DependencyProperty" /> wrapped by
    ///     <see cref="CancellingCommandText" />.
    /// </summary>
    public static readonly DependencyProperty CancellingCommandTextProperty = DependencyProperty.Register(
        nameof(CancelButtonOverlay.CancellingCommandText),
        typeof(string),
        typeof(CancelButtonOverlay),
        new PropertyMetadata(default(string)));

    /// <summary>
    ///     Extends the <see cref="CancelButtonOverlay" /> by a <see cref="DependencyProperty" /> wrapped by
    ///     <see cref="CommandText" />.
    /// </summary>
    public static readonly DependencyProperty CommandTextProperty = DependencyProperty.Register(
        nameof(CancelButtonOverlay.CommandText),
        typeof(string),
        typeof(CancelButtonOverlay),
        new PropertyMetadata(default(string)));

    /// <summary>
    ///     Extends the <see cref="CancelButtonOverlay" /> by a <see cref="DependencyProperty" /> wrapped by
    ///     <see cref="Image" />.
    /// </summary>
    public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
        nameof(CancelButtonOverlay.Image),
        typeof(ImageSource),
        typeof(CancelButtonOverlay),
        new PropertyMetadata(default(ImageSource)));

    /// <summary>
    ///     Initializes a new instance of the <see cref="CancelButtonOverlay" /> class.
    /// </summary>
    public CancelButtonOverlay()
    {
        this.InitializeComponent();
    }

    /// <summary>
    ///     Gets or sets the value of <see cref="CancelCommandProperty" /> <see cref="DependencyProperty" />.
    /// </summary>
    public IAsyncCommand CancelCommand
    {
        get => (IAsyncCommand) this.GetValue(CancelButtonOverlay.CancelCommandProperty);
        set =>
            this.SetValue(
                CancelButtonOverlay.CancelCommandProperty,
                value);
    }

    /// <summary>
    ///     Gets or sets the value of <see cref="CancellingCommandTextProperty" /> <see cref="DependencyProperty" />.
    /// </summary>
    public string CancellingCommandText
    {
        get => (string) this.GetValue(CancelButtonOverlay.CancellingCommandTextProperty);
        set =>
            this.SetValue(
                CancelButtonOverlay.CancellingCommandTextProperty,
                value);
    }

    /// <summary>
    ///     Gets or sets the value of <see cref="CommandTextProperty" /> <see cref="DependencyProperty" />.
    /// </summary>
    public string CommandText
    {
        get => (string) this.GetValue(CancelButtonOverlay.CommandTextProperty);
        set =>
            this.SetValue(
                CancelButtonOverlay.CommandTextProperty,
                value);
    }

    /// <summary>
    ///     Gets or sets the value of <see cref="ImageProperty" /> <see cref="DependencyProperty" />.
    /// </summary>
    public ImageSource Image
    {
        get => (ImageSource) this.GetValue(CancelButtonOverlay.ImageProperty);
        set =>
            this.SetValue(
                CancelButtonOverlay.ImageProperty,
                value);
    }
}
