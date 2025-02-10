namespace Md.Libs.Wpf.Commands;

using System.Windows.Input;
using System.Windows.Threading;

/// <summary>
///     A non-UI blocking <see cref="ICommand" /> implementation. Executes <see cref="ICommand.Execute" /> in a background
///     <see cref="Task" />.
/// </summary>
/// <typeparam name="TCommandParameter">The type of the command parameter.</typeparam>
/// <typeparam name="TExecuteResult">The type of the execution result.</typeparam>
/// <param name="dispatcher">Provides UI services for a thread.</param>
/// <param name="canExecute">The function is executed at <see cref="ICommand.CanExecute" />.</param>
/// <param name="taskFunc">
///     The function is executed at <see cref="ICommand.Execute" /> in a background
///     <see cref="Task{T}" />.
/// </param>
/// <param name="dispatcherAction">
///     The result of <paramref name="taskFunc" /> is executed by using a
///     <see cref="Dispatcher" />.
/// </param>
public class AsyncCommand<TCommandParameter, TExecuteResult>(
    Dispatcher dispatcher,
    Func<TCommandParameter?, bool>? canExecute,
    Func<TCommandParameter?, Task<TExecuteResult>> taskFunc,
    Action<TExecuteResult> dispatcherAction
) : ICommand
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="AsyncCommand{TCommandParameter, TExecuteResult}" /> class.
    /// </summary>
    /// <param name="canExecute">The function is executed at <see cref="ICommand.CanExecute" />.</param>
    /// <param name="taskFunc">
    ///     The function is executed at <see cref="ICommand.Execute" /> in a background
    ///     <see cref="Task{T}" />.
    /// </param>
    /// <param name="dispatcherAction">
    ///     The result of <paramref name="taskFunc" /> is executed by using a
    ///     <see cref="Dispatcher" />.
    /// </param>
    public AsyncCommand(
        Func<TCommandParameter?, bool>? canExecute,
        Func<TCommandParameter?, Task<TExecuteResult>> taskFunc,
        Action<TExecuteResult> dispatcherAction
    )
        : this(
            Dispatcher.CurrentDispatcher,
            canExecute,
            taskFunc,
            dispatcherAction)
    {
    }

    /// <summary>Determines whether the command can execute in its current state.</summary>
    /// <param name="parameter">
    ///     Data used by the command. If the command does not require data to be passed, this object can be
    ///     set to <see langword="null" />.
    /// </param>
    /// <returns>
    ///     <see langword="true" /> if this command can be executed; otherwise, <see langword="false" />.
    /// </returns>
    public bool CanExecute(object? parameter)
    {
        return !AsyncCommandTaskHandler.IsBackgroundTaskActive &&
               (canExecute is null || canExecute((TCommandParameter?) parameter));
    }

    /// <summary>Occurs when changes take place that affect whether the command should execute.</summary>
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
        this.Dispatch(AsyncCommandTaskHandler.Start);

        var commandParameter = (TCommandParameter?) parameter;
        Task.Run(async () => await taskFunc(commandParameter))
            .ContinueWith(
                task =>
                {
                    if (!task.IsCompletedSuccessfully)
                    {
                        // todo
                        return;
                    }

                    var executeResult = task.Result;
                    this.Dispatch(
                        () =>
                        {
                            dispatcherAction(executeResult);
                            AsyncCommandTaskHandler.Terminated();
                            CommandManager.InvalidateRequerySuggested();
                        });
                });
    }

    /// <summary>
    ///     Execute <paramref name="dispatcherCallback" /> by <see cref="Dispatcher.Invoke(System.Action)" />.
    /// </summary>
    /// <param name="dispatcherCallback">The action that is dispatched.</param>
    private void Dispatch(Action dispatcherCallback)
    {
        if (dispatcher.CheckAccess())
        {
            dispatcherCallback();
        }
        else
        {
            dispatcher.Invoke(dispatcherCallback);
        }
    }
}
