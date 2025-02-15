namespace HowToWpf.Watermark;

using System.Windows.Input;
using Libs.Wpf.Commands;
using Libs.Wpf.DependencyInjection;
using Libs.Wpf.ViewModels;
using Microsoft.Extensions.DependencyInjection;

internal class WatermarkViewModel : ViewModelBase
{
    /// <summary>
    ///     The reset values command.
    /// </summary>
    private ICommand resetValuesCommand = null!;

    /// <summary>
    ///     The selected combobox value.
    /// </summary>
    private string? selectedComboBoxValue;

    /// <summary>
    ///     The set values command.
    /// </summary>
    private ICommand setValuesCommand = null!;

    /// <summary>
    ///     The text of the textbox.
    /// </summary>
    private string textBoxText = string.Empty;

    /// <summary>
    ///     Initializes a new instance of the <see cref="WatermarkViewModel" /> class.
    /// </summary>
    public WatermarkViewModel()
    {
        var serviceProvider = CustomServiceProviderBuilder.Build(ServiceCollectionExtensions.TryAddCommandFactory);
        var commandFactory = serviceProvider.GetRequiredService<ICommandFactory>();
        this.ResetValuesCommand = commandFactory.CreateSyncCommand<object>(
            _ =>
            {
                this.SelectedComboBoxValue = null;
                this.TextBoxText = string.Empty;
            });
        this.SetValuesCommand = commandFactory.CreateSyncCommand<object>(
            _ =>
            {
                this.SelectedComboBoxValue = this.ComboBoxValues.First();
                this.TextBoxText = nameof(WatermarkViewModel.TextBoxText);
            });
    }

    public IEnumerable<string> ComboBoxValues => ["First", "Second", "Third"];

    /// <summary>
    ///     Gets or sets the reset values command.
    /// </summary>
    public ICommand ResetValuesCommand
    {
        get => this.resetValuesCommand;
        set =>
            this.SetField(
                ref this.resetValuesCommand,
                value);
    }

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
    ///     Gets or sets the set values command.
    /// </summary>
    public ICommand SetValuesCommand
    {
        get => this.setValuesCommand;
        set =>
            this.SetField(
                ref this.setValuesCommand,
                value);
    }

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
