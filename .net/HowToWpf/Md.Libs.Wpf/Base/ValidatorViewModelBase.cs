namespace Md.Libs.Wpf.Base;

using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

/// <summary>
///     A base implementation for view models using <see cref="INotifyDataErrorInfo" />.
/// </summary>
/// <seealso cref="Md.Libs.Wpf.Base.ViewModelBase" />
/// <seealso cref="System.ComponentModel.INotifyDataErrorInfo" />
public class ValidatorViewModelBase : ViewModelBase, INotifyDataErrorInfo
{
    /// <summary>
    ///     A dictionary that contains the errors of the entire entity.
    /// </summary>
    private readonly IDictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

    /// <summary>Gets a value that indicates whether the entity has validation errors.</summary>
    /// <returns>
    ///     <see langword="true" /> if the entity currently has validation errors; otherwise, <see langword="false" />.
    /// </returns>
    public bool HasErrors => this.errors.Count > 0;

    /// <summary>
    ///     Occurs when the validation errors have changed for a property or for the entire entity.
    /// </summary>
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    /// <summary>Gets the validation errors for a specified property or for the entire entity.</summary>
    /// <param name="propertyName">
    ///     The name of the property to retrieve validation errors for; or <see langword="null" /> or
    ///     <see cref="F:System.String.Empty" />, to retrieve entity-level errors.
    /// </param>
    /// <returns>The validation errors for the property or entity.</returns>
    public IEnumerable GetErrors(string? propertyName)
    {
        var properties = !string.IsNullOrEmpty(propertyName) ? [propertyName] : this.errors.Keys;
        foreach (var property in properties)
        {
            if (!this.errors.TryGetValue(
                    property,
                    out var propertyErrors))
            {
                continue;
            }

            foreach (var propertyError in propertyErrors)
            {
                yield return propertyError;
            }
        }
    }

    /// <summary>
    ///     Resets the errors of the given <paramref name="propertyName" />.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    protected void ResetErrors([CallerMemberName] string propertyName = "")
    {
        this.errors.Remove(propertyName);
        this.ErrorsChanged?.Invoke(
            this,
            new DataErrorsChangedEventArgs(propertyName));
    }

    /// <summary>
    ///     Sets the error of the given <paramref name="propertyName" />.
    /// </summary>
    /// <param name="propertyError">The error of the given property.</param>
    /// <param name="propertyName">Name of the property.</param>
    protected void SetError(string propertyError, [CallerMemberName] string propertyName = "")
    {
        this.SetErrors(
            [propertyError],
            propertyName);
    }

    /// <summary>
    ///     Sets the errors of the given <paramref name="propertyName" />.
    /// </summary>
    /// <param name="propertyErrors">The errors of the given property.</param>
    /// <param name="propertyName">Name of the property.</param>
    protected void SetErrors(IEnumerable<string> propertyErrors, [CallerMemberName] string propertyName = "")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);

        var errorList = new List<string>(propertyErrors);
        if (!errorList.Any())
        {
            throw new ArgumentException(
                "Error list is empty.",
                nameof(propertyErrors));
        }

        this.errors[propertyName] = errorList;
        this.ErrorsChanged?.Invoke(
            this,
            new DataErrorsChangedEventArgs(propertyName));
    }
}
