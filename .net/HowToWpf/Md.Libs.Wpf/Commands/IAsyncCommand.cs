namespace Md.Libs.Wpf.Commands;

using System.ComponentModel;
using System.Windows.Input;

/// <summary>
///     Extends <see cref="ICommand" /> and supports an asynchronous execution of <see cref="ICommand.Execute" />.
/// </summary>
/// <seealso cref="ICommand" />
/// <seealso cref="INotifyPropertyChanged" />
public interface IAsyncCommand : ICommand, INotifyPropertyChanged
{
    /// <summary>
    ///     Gets a value that indicates weather the command is executing: <c>true</c> the command is running; otherwise
    ///     <c>false</c>.
    /// </summary>
    bool IsActive { get; }
}
