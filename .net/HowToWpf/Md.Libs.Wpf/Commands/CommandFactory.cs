namespace Md.Libs.Wpf.Commands;

using System.Windows.Input;
using System.Windows.Threading;

/// <summary>
///     A factory for <see cref="ICommand" /> and <see cref="ICancellableCommand" /> implementations.
/// </summary>
public static class CommandFactory
{
    /// <summary>
    ///     Provides UI services for a thread.
    /// </summary>
    private static readonly Dispatcher Dispatcher = Dispatcher.CurrentDispatcher;

    /// <summary>
    ///     Initializes a new instance of an <see cref="ICancellableCommand" /> implementing class. The command does not block
    ///     the ui thread during execution and can be cancelled.
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
    /// <returns>A new <see cref="ICancellableCommand" />.</returns>
    public static ICancellableCommand CreateAsyncCommand<TCommandParameter, TExecuteResult>(
        Func<TCommandParameter?, bool>? canExecute,
        Action? preExecute,
        Func<TCommandParameter?, CancellationToken, Task<TExecuteResult?>> execute,
        Action<Task<TExecuteResult?>> postExecute
    )
    {
        return new AsyncCommand<TCommandParameter, TExecuteResult>(
            canExecute,
            preExecute,
            execute,
            postExecute,
            CommandFactory.Dispatcher);
    }

    /// <summary>
    ///     Initializes a new instance of an <see cref="ICommand" /> implementing class. The command does block the ui thread
    ///     during execution and cannot be cancelled.
    /// </summary>
    /// <param name="canExecute">A function that determines whether the command can execute in its current state.</param>
    /// <param name="execute">Defines the method to be called when the command is invoked.</param>
    /// <returns>A new <see cref="ICommand" />.</returns>
    public static ICommand CreateSyncCommand<TCommandParameter>(
        Func<TCommandParameter?, bool>? canExecute,
        Action<TCommandParameter?> execute
    )
    {
        return new SyncCommand<TCommandParameter>(
            canExecute,
            execute);
    }

    /// <summary>
    ///     Initializes a new instance of an <see cref="ICommand" /> implementing class. The command does block the ui thread
    ///     during execution and cannot be cancelled.
    /// </summary>
    /// <param name="execute">Defines the method to be called when the command is invoked.</param>
    /// <returns>A new <see cref="ICommand" />.</returns>
    /// <remarks>
    ///     The command is executed with restrictions and therefore <see cref="ICommand.CanExecute" /> provides in any
    ///     case <c>true</c>.
    /// </remarks>
    public static ICommand CreateSyncCommand<TCommandParameter>(Action<TCommandParameter?> execute)
    {
        return new SyncCommand<TCommandParameter>(
            _ => true,
            execute);
    }
}
