using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;


namespace ValidationGenerator.Core.SourceCodeBuilder
{
    public class ValidationSourceCodeBuilder : IValidationSourceCodeBuilder
    {
        private readonly List<string> _properties;
        private readonly string _nameSpaceValue;
        private readonly string _className;

        public ValidationSourceCodeBuilder(string nameSpaceValue, string className, List<string> properties)
        {
            _nameSpaceValue = nameSpaceValue;
            _className = className;
            _properties = properties;
        }
        public string GetSourceCode()
        {
            string propertyNullCheckBlocks = IfCheckBuilderForProperties(_properties);
            string methodDeclaration = MethodBuilder(propertyNullCheckBlocks);
            string classDeclaration = ClassBuilder(methodDeclaration);
            return NameSpaceBuilder(classDeclaration);
        }
        private string MethodBuilder(string nullCheckForProperties)
        {
            StringBuilder codeBuilder = new StringBuilder();
            codeBuilder.AppendLine("        public void ThrowIfNotValid()");
            codeBuilder.AppendLine("        {");
            codeBuilder.AppendLine();
            codeBuilder.AppendLine(nullCheckForProperties);
            codeBuilder.AppendLine();
            codeBuilder.AppendLine("        }");
            return codeBuilder.ToString();
        }
        private string IfCheckBuilderForProperties(List<string> properties)
        {
            StringBuilder codeBuilder = new StringBuilder();
            foreach (string property in properties)
            {
                codeBuilder.AppendLine();
                codeBuilder.AppendLine("            if (@@Prop@@ is null)".Replace("@@Prop@@", property));
                codeBuilder.AppendLine("            {");
                codeBuilder.AppendLine("                throw new ArgumentNullException(nameof(@@Prop@@));".Replace("@@Prop@@", property));
                codeBuilder.AppendLine("            }");
                codeBuilder.AppendLine();
            }
            return codeBuilder.ToString();
        }
        private string ClassBuilder(string methodDeclaration)
        {
            StringBuilder codeBuilder = new StringBuilder();
            codeBuilder.AppendLine("public partial class @@class@@".Replace("@@class@@", _className));
            codeBuilder.AppendLine("{");
            codeBuilder.AppendLine();
            codeBuilder.AppendLine(methodDeclaration);
            codeBuilder.AppendLine();
            codeBuilder.AppendLine("}");
            return codeBuilder.ToString();
        }
        private string NameSpaceBuilder(string classDeclaration)
        {
            StringBuilder codeBuilder = new StringBuilder();
            codeBuilder.AppendLine("namespace @@namespace@@".Replace("@@namespace@@", _nameSpaceValue));
            codeBuilder.AppendLine("{");
            codeBuilder.AppendLine();
            codeBuilder.AppendLine(classDeclaration);
            codeBuilder.AppendLine();
            codeBuilder.AppendLine("}");
            return codeBuilder.ToString();
        }
    }

    public class ClassValidationData : IValidationSourceCodeBuilder
    {
        public string NameSpace { get; set; }
        public string ClassName { get; set; }
        public List<PropertyValidationData> PropertyValidationList { get; set; }
        public ClassValidationData()
        {
            if (PropertyValidationList is null)
            {
                PropertyValidationList = new List<PropertyValidationData>();
            }
        }

        public string GetSourceCode()
        {
            string propertyNullCheckBlocks = IfCheckBuilderForProperties(PropertyValidationList);
            string methodDeclaration = MethodBuilder(propertyNullCheckBlocks);
            return ClassBuilder(methodDeclaration);
        }
        private string MethodBuilder(string nullCheckForProperties)
        {
            StringBuilder codeBuilder = new StringBuilder();
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
            StringBuilder codeBuilder = new StringBuilder();
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
            StringBuilder codeBuilder = new StringBuilder();
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

    public class PropertyValidationData
    {
        public string ProperyName { get; set; }
        public ITypeSymbol ProperyType { get; set; }
        public List<AttributeValidationData> AttributeValidationList { get; set; }
        public PropertyValidationData()
        {
            if (AttributeValidationList is null)
            {
                AttributeValidationList = new List<AttributeValidationData>();
            }
        }
    }

    public class AttributeValidationData
    {
        public string AttributeName { get; set; }
        public List<AttributeArgumentInfo> AttributeArguments { get; set; }
        public AttributeValidationData()
        {
            if (AttributeArguments is null)
            {
                AttributeArguments = new List<AttributeArgumentInfo>();
            }
        }
    }
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
}
