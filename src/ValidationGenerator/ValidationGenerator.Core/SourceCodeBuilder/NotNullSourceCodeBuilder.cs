using System;
using System.Collections.Generic;
using System.Text;

namespace ValidationGenerator.Core.SourceCodeBuilder
{
    public class NotNullSourceCodeBuilder
    {
        private readonly List<string> _properties;
        public NotNullSourceCodeBuilder(List<string> properties)
        {
            _properties = properties;
        }

        public string GetSourceCode()
        {
            string propertyChecks = IfCheckBuilderForProperties(_properties);
            return MethodBuilder(propertyChecks);
        }

        private string MethodBuilder(string nullCheckForProperties)
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
        private string IfCheckBuilderForProperties(List<string> properties)
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

        
    }
}
