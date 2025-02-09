namespace Md.Libs.Wpf.Base;

using System.ComponentModel;
using System.Runtime.CompilerServices;

/// <summary>
///     Base class for view models.
/// </summary>
public class ViewModelBase : INotifyPropertyChanged
{
    /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged" />
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    ///     Called when <see cref="PropertyChanged" /> occurs.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        this.PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    ///     Sets the value of <paramref name="field" /> to <paramref name="value" />. If the <paramref name="value" /> does not
    ///     equal the current value of <paramref name="field" /> <see cref="PropertyChanged" /> is raised for
    ///     <paramref name="propertyName" />.
    /// </summary>
    /// <typeparam name="T">The type of the field to set.</typeparam>
    /// <param name="field">The field whose value is set.</param>
    /// <param name="value">The new value of <paramref name="field" />.</param>
    /// <param name="propertyName">Name of the property that is set.</param>
    /// <returns><c>True</c> if the value of the property changed; otherwise <c>false</c>.</returns>
    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(
                field,
                value))
        {
            return false;
        }

        field = value;
        this.OnPropertyChanged(propertyName);
        return true;
    }
}
