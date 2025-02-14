namespace HowToWpf.Commands;

using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using Md.Libs.Wpf.Base;
using Md.Libs.Wpf.Commands;

/// <summary>
///     The view model of the <see cref="CommandsView" />.
/// </summary>
internal class CommandsViewModel : ViewModelBase
{
    /// <summary>
    ///     The add 3 command parameter.
    /// </summary>
    private AddCommandParameter add3CommandParameter;

    /// <summary>
    ///     The command to add by command parameter.
    /// </summary>
    private IAsyncCommand addCommandParameterAsyncCommand;

    /// <summary>
    ///     The add by command parameter sync command.
    /// </summary>
    private ICommand addCommandParameterSyncSyncCommand;

    /// <summary>
    ///     The command to add an integer to the result.
    /// </summary>
    private IAsyncCommand addIntAsyncCommand;

    /// <summary>
    ///     The command to add an int that is cancellable.
    /// </summary>
    private ICancellableCommand addIntCancellableCommand1;

    /// <summary>
    ///     The command to add an int that is cancellable.
    /// </summary>
    private ICancellableCommand addIntCancellableCommand2;

    /// <summary>
    ///     The sync command for adding an integer value.
    /// </summary>
    private ICommand addIntSyncCommand;

    /// <summary>
    ///     The current value async command.
    /// </summary>
    private int currentValueAsyncCommand;

    /// <summary>
    ///     The current value of the sync command operations.
    /// </summary>
    private int currentValueSyncCommand;

    private bool isActive;
    private bool isActiveCancellable;

    [SetsRequiredMembers]
    public CommandsViewModel()
    {
        this.CurrentValueSyncCommand = 0;
        this.AddIntSyncCommand = CommandFactory.CreateSyncCommand<int?>(
            value => value is not null,
            value =>
            {
                Thread.Sleep(2000);
                this.CurrentValueSyncCommand += (int) value!;
            });
        this.AddCommandParameterSyncCommand = CommandFactory.CreateSyncCommand<AddCommandParameter?>(
            value => value is not null,
            value =>
            {
                Thread.Sleep(2000);
                this.CurrentValueSyncCommand += value!.AddValue;
            });
        this.Add3CommandParameter = new AddCommandParameter(3);

        this.AddIntAsyncCommand = CommandFactory.CreateAsyncCommand<int?, int?>(
            value => !this.isActive && value is not null,
            () => this.isActive = true,
            async (value, _) =>
            {
                await Task.Delay(2000);
                return value;
            },
            value =>
            {
                this.CurrentValueAsyncCommand += value.Result!.Value;
                this.isActive = false;
            });
        this.AddCommandParameterAsyncCommand =
            CommandFactory.CreateAsyncCommand<AddCommandParameter?, AddCommandParameter?>(
                value => !this.isActive && value is not null,
                () => this.isActive = true,
                async (value, _) =>
                {
                    await Task.Delay(2000);
                    return value;
                },
                value =>
                {
                    this.CurrentValueAsyncCommand += value.Result!.AddValue;
                    this.isActive = false;
                });

        this.AddIntCancellableCommand1 = CommandFactory.CreateAsyncCommand<int?, int?>(
            value => !this.isActiveCancellable && value is not null,
            () => this.isActiveCancellable = true,
            async (value, cancellationToken) =>
            {
                await Task.Delay(
                    4000,
                    cancellationToken);
                return value;
            },
            value =>
            {
                try
                {
                    this.CurrentValueAsyncCommand += value.Result!.Value;
                }
                finally
                {
                    this.isActiveCancellable = false;
                }
            });
        this.AddIntCancellableCommand2 = CommandFactory.CreateAsyncCommand<int?, int?>(
            value => !this.isActiveCancellable && value is not null,
            () => this.isActiveCancellable = true,
            async (value, cancellationToken) =>
            {
                await Task.Delay(
                    4000,
                    cancellationToken);
                return value;
            },
            value =>
            {
                try
                {
                    this.CurrentValueAsyncCommand += value.Result!.Value;
                }
                finally
                {
                    this.isActiveCancellable = false;
                }
            });
    }

    /// <summary>
    ///     Gets or sets the add 3 command parameter.
    /// </summary>
    public AddCommandParameter Add3CommandParameter
    {
        get => this.add3CommandParameter;
        set =>
            this.SetField(
                ref this.add3CommandParameter,
                value);
    }

    /// <summary>
    ///     Gets or sets the command to add by command parameter.
    /// </summary>
    public IAsyncCommand AddCommandParameterAsyncCommand
    {
        get => this.addCommandParameterAsyncCommand;
        set =>
            this.SetField(
                ref this.addCommandParameterAsyncCommand,
                value);
    }

    /// <summary>
    ///     Gets or sets the add by command parameter sync command.
    /// </summary>
    public ICommand AddCommandParameterSyncCommand
    {
        get => this.addCommandParameterSyncSyncCommand;
        set =>
            this.SetField(
                ref this.addCommandParameterSyncSyncCommand,
                value);
    }

    /// <summary>
    ///     Gets or sets the command to add an integer to the result.
    /// </summary>
    public IAsyncCommand AddIntAsyncCommand
    {
        get => this.addIntAsyncCommand;
        set =>
            this.SetField(
                ref this.addIntAsyncCommand,
                value);
    }

    /// <summary>
    ///     Gets or sets the command to add an int that is cancellable.
    /// </summary>
    public ICancellableCommand AddIntCancellableCommand1
    {
        get => this.addIntCancellableCommand1;
        set =>
            this.SetField(
                ref this.addIntCancellableCommand1,
                value);
    }

    /// <summary>
    ///     Gets or sets the command to add an int that is cancellable.
    /// </summary>
    public ICancellableCommand AddIntCancellableCommand2
    {
        get => this.addIntCancellableCommand2;
        set =>
            this.SetField(
                ref this.addIntCancellableCommand2,
                value);
    }

    /// <summary>
    ///     Gets or sets the sync command to add an integer value.
    /// </summary>
    public ICommand AddIntSyncCommand
    {
        get => this.addIntSyncCommand;
        set =>
            this.SetField(
                ref this.addIntSyncCommand,
                value);
    }

    /// <summary>
    ///     Gets or sets the current value async command.
    /// </summary>
    public int CurrentValueAsyncCommand
    {
        get => this.currentValueAsyncCommand;
        set =>
            this.SetField(
                ref this.currentValueAsyncCommand,
                value);
    }

    /// <summary>
    ///     Gets or sets the current value of the sync command operations.
    /// </summary>
    public int CurrentValueSyncCommand
    {
        get => this.currentValueSyncCommand;
        set =>
            this.SetField(
                ref this.currentValueSyncCommand,
                value);
    }
}
