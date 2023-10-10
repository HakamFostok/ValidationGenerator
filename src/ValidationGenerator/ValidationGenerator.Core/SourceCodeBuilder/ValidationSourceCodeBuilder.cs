using System;
using System.Collections.Generic;
using System.Text;
using ValidationGenerator.Shared;

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
            foreach (var attributeValidation in property.AttributeValidationList)
            {
                string ifCondition = $"if ({property.ProperyName} @@Expression@@)";
                string exceptionBlock = "throw new @@Exception@@(@@errorMessage@@)";
                
                if (attributeValidation.AttributeName.Equals(nameof(NotNullGeneratorAttribute)))
                {
                    ifCondition = ifCondition.Replace("@@Expression@@", "is null");
                    exceptionBlock = exceptionBlock.Replace("@@Exception@@",nameof(ArgumentNullException));

                    foreach (var attributeArgument in attributeValidation.AttributeArguments)
                    {
                        if (attributeArgument?.Name?.Equals(nameof(NotNullGeneratorAttribute.ErrorMessage)) == true)
                        {
                            exceptionBlock = exceptionBlock.Replace("@@errorMessage@@", $"\"{attributeArgument.Expression}\"");
                        }
                    }
                }

                if (exceptionBlock.Contains("@@errorMessage@@"))
                {
                    exceptionBlock = exceptionBlock.Replace("@@errorMessage@@", string.Empty);
                }

                // other attribute types will be checked

                codeBuilder.AppendLine();
                codeBuilder.AppendLine($"            {ifCondition}");
                codeBuilder.AppendLine("            {");
                codeBuilder.AppendLine($"                {exceptionBlock};");
                codeBuilder.AppendLine("            }");
                codeBuilder.AppendLine();


            }
           
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
