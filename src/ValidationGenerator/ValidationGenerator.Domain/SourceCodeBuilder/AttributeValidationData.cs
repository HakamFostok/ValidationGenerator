﻿namespace ValidationGenerator.Domain;

public class AttributeValidationData
{
    public string AttributeName { get; set; }
    public List<AttributeArgumentInfo> AttributeArguments { get; set; }
    public AttributeValidationData()
    {
        AttributeArguments ??= new List<AttributeArgumentInfo>();
    }
}
