namespace ValidationGenerator.Domain;

public class PropertyValidationData
{
    public string PropertyName { get; set; }
    public string PropertyType { get; set; }
    public bool IsReferenceType { get; set; }

    public List<AttributeValidationData> AttributeValidationList { get; set; }
    public PropertyValidationData()
    {
        AttributeValidationList ??= new List<AttributeValidationData>();
    }
}
