using System;
using System.Collections.Generic;
using System.Text;

namespace ValidationGenerator.Core.SourceCodeBuilder
{
    public class NotNullSourceCodeBuilder : IValidationSourceCodeBuilder
    {
        private readonly List<string> _properties;
        private readonly string _nameSpaceValue;
        private readonly string _className;
        public NotNullSourceCodeBuilder(string nameSpaceValue,string className,List<string> properties)
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
        public string MethodBuilder(string nullCheckForProperties)
        {
            StringBuilder codeBuilder = new StringBuilder();
            codeBuilder.AppendLine("        public void ThrowIfNull()");
            codeBuilder.AppendLine("        {");
            codeBuilder.AppendLine();
            codeBuilder.AppendLine(nullCheckForProperties);
            codeBuilder.AppendLine();
            codeBuilder.AppendLine("        }");
            return codeBuilder.ToString();
        }
        public string IfCheckBuilderForProperties(List<string> properties)
        {
            StringBuilder codeBuilder = new StringBuilder();
            foreach (string property in properties)
            {
                codeBuilder.AppendLine();
                codeBuilder.AppendLine("            if (@@Prop@@ is null)".Replace("@@Prop@@",property));
                codeBuilder.AppendLine("            {");
                codeBuilder.AppendLine("                throw new ArgumentNullException(nameof(@@Prop@@));".Replace("@@Prop@@", property));
                codeBuilder.AppendLine("            }");
                codeBuilder.AppendLine();
            }
            return codeBuilder.ToString();
        }
        public string ClassBuilder(string methodDeclaration)
        {
            StringBuilder codeBuilder = new StringBuilder();
            codeBuilder.AppendLine("public partial class @@class@@".Replace("@@class@@",_className));
            codeBuilder.AppendLine("{");
            codeBuilder.AppendLine();
            codeBuilder.AppendLine(methodDeclaration);
            codeBuilder.AppendLine();
            codeBuilder.AppendLine("}");
            return codeBuilder.ToString();
        }
        public string NameSpaceBuilder(string classDeclaration)
        {
            StringBuilder codeBuilder = new StringBuilder();
            codeBuilder.AppendLine("namespace @@namespace@@".Replace("@@namespace@@",_nameSpaceValue));
            codeBuilder.AppendLine("{");
            codeBuilder.AppendLine();
            codeBuilder.AppendLine(classDeclaration);
            codeBuilder.AppendLine();
            codeBuilder.AppendLine("}");
            return codeBuilder.ToString();
        }




    }
}
