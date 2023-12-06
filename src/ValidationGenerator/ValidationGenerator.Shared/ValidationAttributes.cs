using System;

namespace ValidationGenerator.Shared;


public class BaseValidationAttribute : Attribute
{
    public string ErrorMessage { get; set; }
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class NotEmptyGeneratorAttribute : BaseValidationAttribute
{
    
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class NotNullGeneratorAttribute : BaseValidationAttribute
{
    
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class MinimumLengthGeneratorAttribute : BaseValidationAttribute
{
    public int MinimumLength { get; set; }
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class MaximumLengthGeneratorAttribute : BaseValidationAttribute
{
    public int MaximumLength { get; set; }
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class Base64GeneratorAttribute : BaseValidationAttribute
{
    
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class EmailGeneratorAttribute : BaseValidationAttribute
{

}

[AttributeUsage(AttributeTargets.Property)]
public sealed class AlphaNumericGeneratorAttribute : BaseValidationAttribute
{

}

[AttributeUsage(AttributeTargets.Property)]
public sealed class SpecialCharacterGeneratorAttribute : BaseValidationAttribute
{

}

[AttributeUsage(AttributeTargets.Property)]
public sealed class RegexMatchGeneratorAttribute : BaseValidationAttribute
{
    public string Regex { get; set; }
}


