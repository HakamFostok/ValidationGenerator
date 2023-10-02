using Microsoft.CodeAnalysis;
using System.Collections.Generic;


namespace ValidationGenerator.Core.SourceCodeBuilder;

public class PropertyValidationData
{
    public string ProperyName { get; set; }
    public ITypeSymbol ProperyType { get; set; }
    public List<AttributeValidationData> AttributeValidationList { get; set; }
    public PropertyValidationData()
    {
        AttributeValidationList ??= new List<AttributeValidationData>();
    }
}
