namespace HowToWpf.NotifyDataErrorInfo;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Libs.Wpf.Commands;
using Libs.Wpf.DependencyInjection;
using Libs.Wpf.ViewModels;
using Microsoft.Extensions.DependencyInjection;

internal class NotifyDataErrorInfoViewModel : ValidatorViewModelBase
{
    /// <summary>
    ///     The password tag.
    /// </summary>
    private string passwordTag = string.Empty;

    /// <summary>
    ///     The selected combo box item.
    /// </summary>
    private string selectedComboBoxItem = string.Empty;

    /// <summary>
    ///     The submit command.
    /// </summary>
    private ICommand submitCommand = null!;

    /// <summary>
    ///     The text.
    /// </summary>
    private string text = string.Empty;

    /// <summary>
    ///     Initializes a new instance of the <see cref="NotifyDataErrorInfoViewModel" /> class.
    /// </summary>
    public NotifyDataErrorInfoViewModel()
    {
        var serviceProvider = CustomServiceProviderBuilder.Build(ServiceCollectionExtensions.TryAddCommandFactory);
        var commandFactory = serviceProvider.GetRequiredService<ICommandFactory>();

        this.SubmitCommand = commandFactory.CreateSyncCommand<PasswordBox>(
            passwordBox =>
            {
                if (passwordBox is null)
                {
                    this.SetError(
                        "Password is required.",
                        nameof(NotifyDataErrorInfoViewModel.PasswordTag));
                    return false;
                }

                if (passwordBox.SecurePassword.Length < 8)
                {
                    this.SetError(
                        "Password too short.",
                        nameof(NotifyDataErrorInfoViewModel.PasswordTag));
                    return false;
                }

                this.ResetErrors(nameof(NotifyDataErrorInfoViewModel.PasswordTag));
                return true;
            },
            _ => MessageBox.Show(
                "Executed",
                "Executed",
                MessageBoxButton.OK,
                MessageBoxImage.Information));
    }

    public IEnumerable<string> ComboBoxItems => ["First", "Second", "Third"];

    /// <summary>
    ///     Gets or sets the password tag. Used for password validation errors only.
    /// </summary>
    public string PasswordTag
    {
        get => this.passwordTag;
        set =>
            this.SetField(
                ref this.passwordTag,
                value);
    }

    /// <summary>
    ///     Gets or sets the selected combo box item.
    /// </summary>
    public string SelectedComboBoxItem
    {
        get => this.selectedComboBoxItem;
        set
        {
            this.SetField(
                ref this.selectedComboBoxItem,
                value);
            if (value == this.ComboBoxItems.Skip(1).First())
            {
                this.SetError("Do not select second item");
            }
            else
            {
                this.ResetErrors();
            }
        }
    }

    /// <summary>
    ///     Gets or sets the submit command.
    /// </summary>
    public ICommand SubmitCommand
    {
        get => this.submitCommand;
        set =>
            this.SetField(
                ref this.submitCommand,
                value);
    }

    /// <summary>
    ///     Gets or sets the text.
    /// </summary>
    public string Text
    {
        get => this.text;
        set
        {
            this.SetField(
                ref this.text,
                value);
            if (string.IsNullOrWhiteSpace(this.text))
            {
                this.SetError("The value is required.");
            }
            else
            {
                this.ResetErrors();
            }
        }
    }
}
