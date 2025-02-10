namespace HowToWpf.Commands;

using System.Windows.Input;
using Md.Libs.Wpf.Base;
using Md.Libs.Wpf.Commands;

internal class CommandsViewModel : ViewModelBase
{
    /// <summary>
    ///     The command parameter for all commands.
    /// </summary>
    private string commandParameter = string.Empty;

    /// <summary>
    ///     The result of a command execution.
    /// </summary>
    private int commandResult;

    /// <summary>
    ///     Gets the <see cref="ICommand" /> implemented by <see cref="AsyncCommand{T,TT}" />.
    /// </summary>
    public ICommand AsyncCommandExampleCommand =>
        new AsyncCommand<string, int>(
            cmdParameter => !string.IsNullOrWhiteSpace(cmdParameter),
            async cmdParameter =>
            {
                await Task.Delay(3000);
                return cmdParameter?.Length ?? 0;
            },
            executeResult => this.CommandResult = executeResult);

    /// <summary>
    ///     Gets the <see cref="ICommand" /> implemented by <see cref="CommandBase" />.
    /// </summary>
    public ICommand CommandBaseExampleCommand =>
        new CommandBase(
            cmdParameter => !string.IsNullOrWhiteSpace(cmdParameter as string),
            cmdParameter =>
            {
                SpinWait.SpinUntil(
                    () => false,
                    new TimeSpan(
                        0,
                        0,
                        3));
                this.CommandResult = (cmdParameter as string)?.Length ?? 0;
            });

    /// <summary>
    ///     Gets or sets the command parameter for all commands..
    /// </summary>
    public string CommandParameter
    {
        get => this.commandParameter;
        set =>
            this.SetField(
                ref this.commandParameter,
                value);
    }

    /// <summary>
    ///     Gets or sets the command parameter for all commands.
    /// </summary>
    public int CommandResult
    {
        get => this.commandResult;
        set =>
            this.SetField(
                ref this.commandResult,
                value);
    }

    public ICommand GenericCommandBaseExampleCommand =>
        new GenericCommandBase<string>(
            cmdParameter => !string.IsNullOrWhiteSpace(cmdParameter),
            cmdParameter =>
            {
                SpinWait.SpinUntil(
                    () => false,
                    new TimeSpan(
                        0,
                        0,
                        3));
                this.CommandResult = cmdParameter?.Length ?? 0;
            });
}
