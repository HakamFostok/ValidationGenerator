﻿namespace ValidationGenerator.Core.CodeTemplates;

internal static class Templates
{
    public static string ThrowIfNotValidMethodTemplate(string methodBody)
    {
        return $$"""
        public void ThrowIfNotValid()");
        {
            {{methodBody}}
        }
        """;
    }

    public static string GenerateClassTemplate(
        string throwIfNotValidMethodDeclaration,
        string validationResultFunctionDeclaration,
        string isValidPropertyDeclaration,
        string nameSpace,
        string className,
        string version)
    {
        return $$"""
         // <auto-generated/>
         
         #nullable enable
         
         namespace {{nameSpace}}
         {
             [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Validation.Generator", "{{version}}")]
             public partial class {{className}}
             {
                 {{validationResultFunctionDeclaration}}
         
                 {{throwIfNotValidMethodDeclaration}}
             
                 {{isValidPropertyDeclaration}}
             }
         }
         """;
    }

    public static class ValidationResultMethodTemplates
    {
        public static string GetValidationResultAndInsertIntoListTemplate(string propertyName)
        {
            return $$"""
            var validationResult_{{propertyName}} = Validate_{{propertyName}}();
            if (validationResult_{{propertyName}} is not null)
            {
                 result.ValidationResults.Add(validationResult_{{propertyName}});
            }
            """;
        }

        public static string GetPrivateValidationResultMethodTemplate(string propertyName,string ifCheckForPropertySourceCode)
        {
            return
            $$"""
            private ValidationGenerator.Shared.PropertyValidationResult? Validate_{{propertyName}}()
            {
                ValidationGenerator.Shared.PropertyValidationResult result = new ValidationGenerator.Shared.PropertyValidationResult()
                {
                    PropertyName = "{{propertyName}}",
                    Value = {{propertyName}},
                    ErrorMessages = new()
                };

                {{ifCheckForPropertySourceCode}}
                return result.ErrorMessages.Count > 0 ? result : null;
            }
            """;
        }

        public static string CheckConditionAndInsertIntoErrorMessagesTemplate(string condition,string validationMessage)
        {
            return $$"""
            if ({{condition}})
            {
                result.ErrorMessages.Add("{{validationMessage}}");
            }
            """;
        }

        public static string GetValidationResultMethodTemplate(string validationResultSetSourceCode,string privateMethodSourceCode)
        {
            return
            $$"""
            public ValidationGenerator.Shared.ValidationResult GetValidationResult()
            {
                ValidationGenerator.Shared.ValidationResult result = new();
                result.ValidationResults = new List<ValidationGenerator.Shared.PropertyValidationResult>();
            
                {{validationResultSetSourceCode}}
            
                return result;
            }
            
            {{privateMethodSourceCode}}
            """;
        }
    }
}