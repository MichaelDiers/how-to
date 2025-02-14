namespace Md.Libs.Wpf.Commands;

using System.Windows.Input;
using System.Windows.Threading;
using Md.Libs.Wpf.Base;

/// <summary>
///     A generic and asynchronous implementation of <see cref="ICommand" />. Executes <see cref="ICommand.Execute" /> in a
///     new background <see cref="Task" /> and does not block the UI.
/// </summary>
/// <typeparam name="TCommandParameter">
///     The type of the command parameter used in <see cref="ICommand.CanExecute" /> and
///     <see cref="ICommand.Execute" />.
/// </typeparam>
/// <typeparam name="TExecuteResult">The result type of the execute method.</typeparam>
/// <seealso cref="ViewModelBase" />
/// <seealso cref="ICancellableCommand" />
internal class AsyncCommand<TCommandParameter, TExecuteResult> : ViewModelBase, ICancellableCommand
{
    /// <summary>
    ///     A function to check whether a command can be executed. if <see cref="canExecute" /> is <c>null</c> the execution of
    ///     the command is not restricted.
    /// </summary>
    private readonly Func<TCommandParameter?, bool>? canExecute;

    /// <summary>
    ///     Provides UI services for a thread.
    /// </summary>
    private readonly Dispatcher dispatcher;

    /// <summary>
    ///     The function is executed in a new background <see cref="Task" /> and does not block the UI thread.
    /// </summary>
    private readonly Func<TCommandParameter?, CancellationToken, Task<TExecuteResult?>>? execute;

    /// <summary>
    ///     If <c>true</c>, a new command is initialized that supports cancellation. If <c>false</c>,
    ///     a new command is initialized whose sole purpose is to abort another command. A cancel command cannot be created
    ///     outside this class and is not cancelable.
    /// </summary>
    private readonly bool isCancelCommand;

    /// <summary>
    ///     The action is called with the result of <see cref="execute" />. If required the action
    ///     executes using a UI thread dispatcher.
    /// </summary>
    private readonly Action<Task<TExecuteResult?>>? postExecute;

    /// <summary>
    ///     The optional action is executed before <see cref="execute" />. The action executes in the
    ///     ui thread.
    /// </summary>
    private readonly Action? preExecute;

    /// <summary>
    ///     The command that stops the execution of <see cref="execute" /> by cancelling the
    ///     <see cref="cancellationTokenSource" />. The value is <c>null</c> if the command is a cancel command itself:
    ///     <see cref="isCancelCommand" /> is <c>true</c>.
    /// </summary>
    private IAsyncCommand? cancelCommand;

    /// <summary>
    ///     Provides the <see cref="CancellationToken" /> of the <see cref="execute" /> function. Is only set if
    ///     <see cref="Execute" /> is called.
    /// </summary>
    private CancellationTokenSource? cancellationTokenSource;

    /// <summary>
    ///     Indicates weather the command is executing: <c>true</c> the command is running; otherwise <c>false</c>.
    /// </summary>
    private bool isActive;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AsyncCommand{TCommandParameter,TExecuteResult}" /> class.
    /// </summary>
    /// <param name="canExecute">
    ///     A function to check whether a command can be executed. if <paramref name="canExecute" /> is
    ///     <c>null</c> the execution of the command is not restricted.
    /// </param>
    /// <param name="preExecute">
    ///     The optional action is executed before <paramref name="execute" />. The action executes in the
    ///     ui thread.
    /// </param>
    /// <param name="execute">
    ///     The function is executed in a new background <see cref="Task" /> and does not block the UI
    ///     thread.
    /// </param>
    /// <param name="postExecute">
    ///     The action is called with the result of <paramref name="execute" />. If required the action
    ///     executes using a UI thread dispatcher.
    /// </param>
    /// <param name="dispatcher">Provides UI services for a thread.</param>
    public AsyncCommand(
        Func<TCommandParameter?, bool>? canExecute,
        Action? preExecute,
        Func<TCommandParameter?, CancellationToken, Task<TExecuteResult?>>? execute,
        Action<Task<TExecuteResult?>>? postExecute,
        Dispatcher dispatcher
    )
        : this(
            canExecute,
            preExecute,
            execute,
            postExecute,
            dispatcher,
            false)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="AsyncCommand{TCommandParameter,TExecuteResult}" /> class.
    /// </summary>
    /// <param name="canExecute">
    ///     A function to check whether a command can be executed. if <paramref name="canExecute" /> is
    ///     <c>null</c> the execution of the command is not restricted.
    /// </param>
    /// <param name="preExecute">
    ///     The optional action is executed before <paramref name="execute" />. The action executes in the
    ///     ui thread.
    /// </param>
    /// <param name="execute">
    ///     The function is executed in a new background <see cref="Task" /> and does not block the UI
    ///     thread.
    /// </param>
    /// <param name="postExecute">
    ///     The action is called with the result of <paramref name="execute" />. If required the action
    ///     executes using a UI thread dispatcher.
    /// </param>
    /// <param name="isCancelCommand">
    ///     If <c>true</c>, a new command is initialized that supports cancellation. If <c>false</c>,
    ///     a new command is initialized whose sole purpose is to abort another command. A cancel command cannot be created
    ///     outside this class and is not cancelable.
    /// </param>
    /// <param name="dispatcher">Provides UI services for a thread.</param>
    private AsyncCommand(
        Func<TCommandParameter?, bool>? canExecute,
        Action? preExecute,
        Func<TCommandParameter?, CancellationToken, Task<TExecuteResult?>>? execute,
        Action<Task<TExecuteResult?>>? postExecute,
        Dispatcher dispatcher,
        bool isCancelCommand
    )
    {
        this.canExecute = canExecute;
        this.preExecute = preExecute;
        this.execute = execute;
        this.postExecute = postExecute;
        this.dispatcher = dispatcher;
        this.isCancelCommand = isCancelCommand;
    }

    /// <summary>
    ///     Gets or sets the command that stops the execution of <see cref="execute" /> by cancelling the
    ///     <see cref="cancellationTokenSource" />. The value is <c>null</c> if the command is a cancel command itself:
    ///     <see cref="isCancelCommand" /> is <c>true</c>.
    /// </summary>
    public IAsyncCommand? CancelCommand
    {
        get => this.cancelCommand;
        set =>
            this.SetField(
                ref this.cancelCommand,
                value);
    }

    /// <summary>
    ///     Gets or sets a value that indicates weather the command is executing: <c>true</c> the command is running; otherwise
    ///     <c>false</c>.
    /// </summary>
    public bool IsActive
    {
        get => this.isActive;
        set
        {
            this.SetField(
                ref this.isActive,
                value);
            CommandManager.InvalidateRequerySuggested();
        }
    }

    /// <summary>Determines whether the command can execute in its current state.</summary>
    /// <param name="parameter">
    ///     Data used by the command. If the command does not require data to be passed, this object can be
    ///     set to <see langword="null" />.
    /// </param>
    /// <returns>
    ///     <see langword="true" /> if this command can be executed; otherwise, <see langword="false" />.
    /// </returns>
    public virtual bool CanExecute(object? parameter)
    {
        return !this.IsActive && (this.canExecute is null || this.canExecute((TCommandParameter?) parameter));
    }

    /// <summary>
    ///     Occurs when changes take place that affect whether the command should execute.
    /// </summary>
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    /// <summary>Defines the method to be called when the command is invoked.</summary>
    /// <param name="parameter">
    ///     Data used by the command. If the command does not require data to be passed, this object can be
    ///     set to <see langword="null" />.
    /// </param>
    public void Execute(object? parameter)
    {
        this.Setup();

        this.PreExecute();

        if (!this.RunExecute(parameter))
        {
            if (!this.PostExecute())
            {
                this.CleanUp();
            }
        }
    }

    /// <summary>
    ///     Resets all values that are required during command execution only.
    /// </summary>
    private void CleanUp()
    {
        if (this.dispatcher.CheckAccess())
        {
            this.cancellationTokenSource = null;
            this.CancelCommand = null;
            this.IsActive = false;
        }
        else
        {
            this.dispatcher.Invoke(
                () =>
                {
                    this.cancellationTokenSource = null;
                    this.CancelCommand = null;
                    this.IsActive = false;
                });
        }
    }

    /// <summary>
    ///     Execute <see cref="postExecute" />.
    /// </summary>
    /// <returns>Returns <c>true</c> if <see cref="postExecute" /> is not null; otherwise <c>false</c>.</returns>
    private bool PostExecute()
    {
        if (this.postExecute is null)
        {
            return false;
        }

        try
        {
            this.dispatcher.Invoke(() => this.postExecute(Task.FromResult<TExecuteResult?>(default)));
        }
        finally
        {
            this.CleanUp();
        }

        return true;
    }

    /// <summary>
    ///     Executes the action <see cref="preExecute" />.
    /// </summary>
    private void PreExecute()
    {
        try
        {
            if (this.preExecute is not null)
            {
                if (this.dispatcher.CheckAccess())
                {
                    this.preExecute();
                }
                else
                {
                    this.dispatcher.Invoke(this.preExecute);
                }
            }
        }
        catch
        {
            this.CleanUp();
            throw;
        }
    }

    /// <summary>
    ///     Executes the <see cref="execute" /> function.
    /// </summary>
    /// <param name="parameter">The command parameter of <see cref="execute" />.</param>
    /// <returns><c>true</c> if <see cref="execute" /> is not <c>null</c>; otherwise <c>false</c>.</returns>
    private bool RunExecute(object? parameter)
    {
        if (this.execute is null)
        {
            return false;
        }

        Task.Run(
                () => this.execute(
                    (TCommandParameter?) parameter,
                    this.cancellationTokenSource?.Token ?? CancellationToken.None),
                this.cancellationTokenSource?.Token ?? CancellationToken.None)
            .ContinueWith(
                task =>
                {
                    if (this.postExecute is not null)
                    {
                        this.dispatcher.Invoke(() => this.postExecute(task));
                    }
                })
            .ContinueWith(_ => this.dispatcher.Invoke(this.CleanUp));
        return true;
    }

    /// <summary>
    ///     Setups the data required for executing the command.
    /// </summary>
    private void Setup()
    {
        this.IsActive = true;
        this.cancellationTokenSource = new CancellationTokenSource();

        if (!this.isCancelCommand)
        {
            this.CancelCommand = new AsyncCommand<object, object>(
                _ => this.IsActive,
                null,
                async (_, _) =>
                {
                    await (this.cancellationTokenSource?.CancelAsync() ?? Task.CompletedTask);
                    return Task.FromResult<object?>(null);
                },
                null,
                this.dispatcher,
                true);
        }
    }
}
