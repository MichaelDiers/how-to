namespace Md.Libs.Wpf.Commands;

using System.Windows.Threading;

/// <summary>
///     A monitor for the execution of <see cref="AsyncCommand{TCommandParameter,TExecuteResult}" />s.
/// </summary>
public static class AsyncCommandTaskHandler
{
    /// <summary>
    ///     The number of running background tasks.
    /// </summary>
    private static int backgroundTaskCount;

    /// <summary>
    ///     Provides UI services for a thread.
    /// </summary>
    private static readonly Dispatcher Dispatcher = Dispatcher.CurrentDispatcher;

    /// <summary>
    ///     A synchronization lock.
    /// </summary>
    private static readonly Lock Lock = new();

    /// <summary>
    ///     Indicates if a background task is active.
    /// </summary>
    public static bool IsBackgroundTaskActive => AsyncCommandTaskHandler.BackgroundTaskCount != 0;

    /// <summary>
    ///     Gets or sets the number of running background tasks.
    /// </summary>
    private static int BackgroundTaskCount
    {
        get => AsyncCommandTaskHandler.backgroundTaskCount;
        set
        {
            if (AsyncCommandTaskHandler.Dispatcher.CheckAccess())
            {
                AsyncCommandTaskHandler.backgroundTaskCount = value;
            }
            else
            {
                AsyncCommandTaskHandler.Dispatcher.Invoke(() => AsyncCommandTaskHandler.backgroundTaskCount = value);
            }
        }
    }

    /// <summary>
    ///     Raised if an error during the execution of a command occured.
    /// </summary>
    public static event EventHandler<ErrorEventArgs>? Error;

    /// <summary>
    ///     Raised if <see cref="BackgroundTaskCount" /> changed from 1 to 0 or from 0 to 1.
    /// </summary>
    public static event EventHandler<IsBackgroundTaskActiveChangedEventArgs>? IsBackgroundTaskActiveChanged;

    /// <summary>
    ///     Indicates the start of a command execution.
    /// </summary>
    public static void Start()
    {
        lock (AsyncCommandTaskHandler.Lock)
        {
            AsyncCommandTaskHandler.BackgroundTaskCount++;
            if (AsyncCommandTaskHandler.BackgroundTaskCount == 1)
            {
                AsyncCommandTaskHandler.IsBackgroundTaskActiveChanged?.Invoke(
                    null,
                    new IsBackgroundTaskActiveChangedEventArgs(
                        false,
                        true));
            }
        }
    }

    /// <summary>
    ///     Indicates the termination of a command execution.
    /// </summary>
    /// <param name="error">An error message that is raised by <see cref="Error" /> event.</param>
    public static void Terminated(string error)
    {
        AsyncCommandTaskHandler.Terminated();
        AsyncCommandTaskHandler.Error?.Invoke(
            null,
            new ErrorEventArgs(error));
    }

    /// <summary>
    ///     Indicates the termination of a command execution.
    /// </summary>
    public static void Terminated()
    {
        lock (AsyncCommandTaskHandler.Lock)
        {
            AsyncCommandTaskHandler.BackgroundTaskCount--;
            if (AsyncCommandTaskHandler.BackgroundTaskCount == 0)
            {
                AsyncCommandTaskHandler.IsBackgroundTaskActiveChanged?.Invoke(
                    null,
                    new IsBackgroundTaskActiveChangedEventArgs(
                        true,
                        false));
            }
        }
    }
}
