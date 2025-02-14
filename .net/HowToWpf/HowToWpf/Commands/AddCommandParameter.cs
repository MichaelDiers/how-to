namespace HowToWpf.Commands;

/// <summary>
///     Describes a command parameter that provides an integer value.
/// </summary>
/// <param name="value">The integer value of the command parameter.</param>
internal class AddCommandParameter(int value)
{
    /// <summary>
    ///     Gets the value of the command parameter.
    /// </summary>
    public int AddValue => value;
}
