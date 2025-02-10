namespace Md.Libs.Wpf.Commands;

/// <summary>
///     The event args of the <see cref="AsyncCommandTaskHandler.Error" /> event.
/// </summary>
/// <param name="oldValue">The value before the change.</param>
/// <param name="newValue">The value after the change.</param>
public class IsBackgroundTaskActiveChangedEventArgs(bool oldValue, bool newValue) : EventArgs
{
    /// <summary>
    ///     Gets the previous value.
    /// </summary>
    public bool NewValue => newValue;

    /// <summary>
    ///     Gets the value after the change.
    /// </summary>
    public bool OldValue => oldValue;
}
