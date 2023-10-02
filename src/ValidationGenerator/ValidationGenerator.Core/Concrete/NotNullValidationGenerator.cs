using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using ValidationGenerator.Core.SourceCodeBuilder;
using ValidationGenerator.Core.Constants;
using System;

namespace ValidationGenerator.Core.Concrete
{


    [Generator]
    public class NotNullValidationGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {

            List<SyntaxTree> classWithAttributes = context.Compilation.SyntaxTrees.Where(st => st.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>()
                    .Any(p => p.DescendantNodes().OfType<AttributeSyntax>().Any(x => x.Name.GetText().ToString() == "ValidationGenerator"))).ToList();


            List<ClassValidationData> classValidationDatas = new List<ClassValidationData>();

            ValidationSourceCodeBuilder notNullSourceCodeBuilder;
            string soruceCode = string.Empty;

            classWithAttributes.ForEach(classDeclareation =>
            {
                ClassValidationData classValidationData = new ClassValidationData();
                classValidationData.NameSpace = GetNameSpace(classDeclareation);
                classValidationData.ClassName = GetClassName(classDeclareation);

                classValidationData.PropertyValidationList = new List<PropertyValidationData>();

                var propertyDeclarations = classDeclareation.GetRoot()
               .DescendantNodes()
               .OfType<PropertyDeclarationSyntax>();

                foreach (var propertyDeclaration in propertyDeclarations ) 
                {
                    PropertyValidationData propertyValidationData = new PropertyValidationData();

                    propertyValidationData.ProperyName = propertyDeclaration.Identifier.ValueText;
                    propertyValidationData.ProperyType = Type.GetType(propertyDeclaration.Type.ToString());


                    var asd = propertyDeclaration.SyntaxTree.GetRoot().DescendantNodes().OfType<TypeDeclarationSyntax>().FirstOrDefault();
                    
                }



                //var propertyValidationList = classDeclareation.GetRoot()
                // .DescendantNodes()
                // .OfType<AttributeSyntax>()
                // .Where(z => z.Name.GetText().ToString() != ValidationGeneratorKeys.ClassAttribute)
                // .ToList().ConvertAll(attribute =>
                // {

                //     var properties = attributes.FirstOrDefault().SyntaxTree.GetRoot().DescendantNodes().OfType<PropertyDeclarationSyntax>().ToList();

                //     List<AttributeArgumentInfo> argumentInfoList = new List<AttributeArgumentInfo>();
                //     if (attribute.ArgumentList != null)
                //     {
                //         argumentInfoList = attribute.ArgumentList.Arguments.ToList().ConvertAll(argument =>
                //         {
                //             string argumentName = argument.NameEquals.Name.Identifier.ValueText;
                //             string argumentExpression = argument.Expression.ToString();
                //             return new AttributeArgumentInfo(argumentName, argumentExpression);
                //         });
                //     }

                //     PropertyValidationData propertyValidationData = new PropertyValidationData()
                //     {
                //         ClassName = className,
                //         AttributeName = attribute.Name.GetText().ToString(),
                //         ProperyName = attribute.SyntaxTree.GetRoot().DescendantNodes().OfType<PropertyDeclarationSyntax>().FirstOrDefault().Identifier.ValueText,
                //         AttributeArguments = argumentInfoList
                //     };
                //     return propertyValidationData;
                // });


                //var props = attributes.FirstOrDefault().SyntaxTree.GetRoot().DescendantNodes().OfType<PropertyDeclarationSyntax>().ToList();
                //List<string> propertyNames = props.ConvertAll(c => c.Identifier.ValueText);

                //notNullSourceCodeBuilder = new ValidationSourceCodeBuilder(namespaceValue, className, propertyNames);
                //string sourceCode = notNullSourceCodeBuilder.GetSourceCode();
                //context.AddSource(className + "_Validator_G", SourceText.From(sourceCode, Encoding.UTF8));
            });

        }

        private static string GetClassName(SyntaxTree classDeclareation)
        {
            return classDeclareation.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault().Identifier.ValueText;
        }

        private static string GetNameSpace(SyntaxTree x)
        {
            string namespaceValue;
            var normalNameSpace = x.GetRoot().DescendantNodes().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();

            if (normalNameSpace is null)
            {
                namespaceValue = x.GetRoot().DescendantNodes().OfType<FileScopedNamespaceDeclarationSyntax>().FirstOrDefault().Name.GetText().ToString();
            }
            else
            {
                namespaceValue = normalNameSpace.Name.GetText().ToString();
            }

            return namespaceValue;
        }

        public void Initialize(GeneratorInitializationContext context)
        {

#if DEBUG

            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }

#endif

        }
    }
}

