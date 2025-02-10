namespace Md.Libs.Wpf.Commands;

/// <summary>
///     Describes the error event args.
/// </summary>
/// <param name="error">The error message.</param>
public class ErrorEventArgs(string error) : EventArgs
{
    /// <summary>
    ///     Gets the error message.
    /// </summary>
    public string Error => error;
}
