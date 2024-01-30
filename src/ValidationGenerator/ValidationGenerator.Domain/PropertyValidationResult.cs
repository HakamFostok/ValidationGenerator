using System.Diagnostics;

namespace ValidationGenerator.Shared;

/// <summary>
/// Represents the results of the validation for a property.
/// </summary>
[DebuggerDisplay($"{nameof(PropertyName)} = {{{nameof(PropertyName)}}}, {nameof(Value)} = {{{nameof(Value)}}}, {nameof(ErrorMessages.Count)} = {{ErrorMessages.Count}}")]
public class PropertyValidationResult
{
    public string PropertyName { get; }

    // TODO: discuss the idea of making this generic
    public object Value { get; }

    private List<string>? _errorMessages;
    public IReadOnlyList<string>? ErrorMessages => _errorMessages?.AsReadOnly();

    public PropertyValidationResult(string propertyName, object value)
    {
        PropertyName = propertyName;
        Value = value;
        _errorMessages = [];
    }

    public void AddError(string message)
    {
        _errorMessages.Add(message);
    }
}
