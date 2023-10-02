using System.Collections.Generic;
using System.Text;


namespace ValidationGenerator.Core.SourceCodeBuilder;

public class ClassValidationData 
{
    public string NameSpace { get; set; }
    public string ClassName { get; set; }
    public List<PropertyValidationData> PropertyValidationList { get; set; }
    public ClassValidationData()
    {
        PropertyValidationList ??= new List<PropertyValidationData>();
    }

    public string GetSourceCode()
    {
        string propertyNullCheckBlocks = IfCheckBuilderForProperties(PropertyValidationList);
        string methodDeclaration = MethodBuilder(propertyNullCheckBlocks);
        return ClassBuilder(methodDeclaration);
    }
    private string MethodBuilder(string nullCheckForProperties)
    {
        StringBuilder codeBuilder = new ();
        codeBuilder.AppendLine("        public void ThrowIfNotValid()");
        codeBuilder.AppendLine("        {");
        codeBuilder.AppendLine();
        codeBuilder.AppendLine(nullCheckForProperties);
        codeBuilder.AppendLine();
        codeBuilder.AppendLine("        }");
        return codeBuilder.ToString();
    }
    private string IfCheckBuilderForProperties(List<PropertyValidationData> properties)
    {
        StringBuilder codeBuilder = new ();
        foreach (var property in properties)
        {
            codeBuilder.AppendLine();
            codeBuilder.AppendLine("if (@@Prop@@ is null)".Replace("@@Prop@@", property.ProperyName));
            codeBuilder.AppendLine("{");
            codeBuilder.AppendLine("    throw new ArgumentNullException(nameof(@@Prop@@));".Replace("@@Prop@@", property.ProperyName));
            codeBuilder.AppendLine("}");
            codeBuilder.AppendLine();
        }
        return codeBuilder.ToString();
    }
    private string ClassBuilder(string methodDeclaration)
    {
        StringBuilder codeBuilder = new ();
        codeBuilder.AppendLine("namespace @@namespace@@".Replace("@@namespace@@", NameSpace));
        codeBuilder.AppendLine("{");
        codeBuilder.AppendLine("public partial class @@class@@".Replace("@@class@@", ClassName));
        codeBuilder.AppendLine("{");
        codeBuilder.AppendLine();
        codeBuilder.AppendLine(methodDeclaration);
        codeBuilder.AppendLine();
        codeBuilder.AppendLine("}");
        codeBuilder.AppendLine("}");
        return codeBuilder.ToString();
    }
}
