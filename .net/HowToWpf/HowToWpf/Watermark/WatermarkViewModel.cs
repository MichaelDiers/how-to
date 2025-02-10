namespace HowToWpf.Watermark;

using System.Windows.Input;
using Md.Libs.Wpf.Base;
using Md.Libs.Wpf.Commands;

internal class WatermarkViewModel : ViewModelBase
{
    /// <summary>
    ///     The selected combobox value.
    /// </summary>
    private string? selectedComboBoxValue;

    /// <summary>
    ///     The text of the textbox.
    /// </summary>
    private string textBoxText = string.Empty;

    public IEnumerable<string> ComboBoxValues => ["First", "Second", "Third"];

    /// <summary>
    ///     Gets the reset values command.
    /// </summary>
    public ICommand ResetValuesCommand =>
        new CommandBase(
            _ =>
            {
                this.SelectedComboBoxValue = null;
                this.TextBoxText = string.Empty;
            });

    /// <summary>
    ///     Gets or sets the selected combobox value.
    /// </summary>
    public string? SelectedComboBoxValue
    {
        get => this.selectedComboBoxValue;
        set =>
            this.SetField(
                ref this.selectedComboBoxValue,
                value);
    }

    /// <summary>
    ///     Gets the set values command.
    /// </summary>
    public ICommand SetValuesCommand =>
        new CommandBase(
            _ =>
            {
                this.SelectedComboBoxValue = this.ComboBoxValues.First();
                this.TextBoxText = nameof(WatermarkViewModel.TextBoxText);
            });

    /// <summary>
    ///     Gets or sets the text of the textbox.
    /// </summary>
    public string TextBoxText
    {
        get => this.textBoxText;
        set =>
            this.SetField(
                ref this.textBoxText,
                value);
    }
}
