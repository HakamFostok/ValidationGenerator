namespace ValidationGenerator.Domain;

public class AttributeArgumentInfo
{
    public string Name { get; set; }
    public string Expression { get; set; }
    public AttributeArgumentInfo(string name, string expression)
    {
        Name = name;
        Expression = expression;
    }
}
