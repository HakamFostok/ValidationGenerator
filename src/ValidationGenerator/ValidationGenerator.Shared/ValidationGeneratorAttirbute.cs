using System;

namespace ValidationGenerator.Shared;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class ValidationGeneratorAttribute : Attribute
{
    public bool GenerateIsValidProperty { get; set; }
    public bool GenerateThrowIfNotValid { get; set; }
    public bool GenerateValidationResult { get; set; }
}
