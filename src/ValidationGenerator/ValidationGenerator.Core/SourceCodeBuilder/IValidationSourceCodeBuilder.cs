using System;
using System.Collections.Generic;
using System.Text;

namespace ValidationGenerator.Core.SourceCodeBuilder
{
    public interface IValidationSourceCodeBuilder
    {
        string GetSourceCode();
        string MethodBuilder(string checkForProperties);
        string IfCheckBuilderForProperties(List<string> properties);
    }
}
