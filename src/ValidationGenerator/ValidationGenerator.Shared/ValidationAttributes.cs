namespace ValidationGenerator.Shared;

public class BaseValidationAttribute : Attribute
{
    public string ErrorMessage { get; set; }
}

public class CustomValidationFunctionAttribute : BaseValidationAttribute
{
    public string ValidationFunctionName { get; set;}

    public bool IsAsync { get; set; }
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class MustNotEmptyGeneratorAttribute : BaseValidationAttribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class MustNotNullGeneratorAttribute : BaseValidationAttribute
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
public sealed class MustValidBase64GeneratorAttribute : BaseValidationAttribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class MustValidEmailGeneratorAttribute : BaseValidationAttribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class MustValidAlphaNumericGeneratorAttribute : BaseValidationAttribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class MustContainSpecialCharacterGeneratorAttribute : BaseValidationAttribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class RegexMatchGeneratorAttribute : BaseValidationAttribute
{
    public string Regex { get; set; }
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class MustNotBeZeroGeneratorAttribute : BaseValidationAttribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class MustBeGreaterThanZeroGeneratorAttribute : BaseValidationAttribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class MustBeLowerThanZeroGeneratorAttribute : BaseValidationAttribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class MustBePositiveIntegerGeneratorAttribute : BaseValidationAttribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class MustBeNegativeIntegerGeneratorAttribute : BaseValidationAttribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class MustBeInRangeIntegerGeneratorAttribute : BaseValidationAttribute
{
    public int Minimum { get; set; }
    public int Maximum { get; set; }
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class CustomValidationIntegerAttribute : CustomValidationFunctionAttribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class MustBeTrueGeneratorAttribute : BaseValidationAttribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class MustBeFalseGeneratorAttribute : BaseValidationAttribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class CustomValidationBooleanGeneratorAttribute : CustomValidationFunctionAttribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class MustNotBeDefaultDateTimeGeneratorAttribute : CustomValidationFunctionAttribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class MustBePastDateTimeNowGeneratorAttribute : CustomValidationFunctionAttribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class MustBeFutureDateTimeNowGeneratorAttribute : CustomValidationFunctionAttribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class MustBePastDateTimeUTCNowGeneratorAttribute : CustomValidationFunctionAttribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class MustBeFutureDateTimeUTCNowGeneratorAttribute : CustomValidationFunctionAttribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class CustomValidationDateTimeGeneratorAttribute : CustomValidationFunctionAttribute
{
}