using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using ValidationGenerator.Core.SourceCodeBuilder;

namespace ValidationGenerator.Core.Concrete
{


    [Generator]
    public class NotNullValidationGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
          
            List<SyntaxTree> classWithAttributes = context.Compilation.SyntaxTrees.Where(st => st.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>()
                    .Any(p => p.DescendantNodes().OfType<AttributeSyntax>().Any(x=>x.Name.GetText().ToString() == "ValidationGenerator"))).ToList();


            NotNullSourceCodeBuilder notNullSourceCodeBuilder;
            string soruceCode = string.Empty;

            classWithAttributes.ForEach(x =>
            {
                string namespaceValue = string.Empty;


                var normalNameSpace = x.GetRoot().DescendantNodes().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();

                if (normalNameSpace is null)
                {
                    namespaceValue  = x.GetRoot().DescendantNodes().OfType<FileScopedNamespaceDeclarationSyntax>().FirstOrDefault().Name.GetText().ToString();
                   
                }
                else
                {
                    namespaceValue = normalNameSpace.Name.GetText().ToString();
                }

               string className = x.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault().Identifier.ValueText;

                var items = x.GetRoot()
                 .DescendantNodes()
                 .OfType<AttributeSyntax>()
                 .Where(z => z.Name.GetText().ToString() == "NotNull")
                 .ToList();
                var props = items.FirstOrDefault().SyntaxTree.GetRoot().DescendantNodes().OfType<PropertyDeclarationSyntax>().ToList();
                List<string> propertyNames = props.ConvertAll(c => c.Identifier.ValueText);

                notNullSourceCodeBuilder = new NotNullSourceCodeBuilder(namespaceValue, className,propertyNames);
                string sourceCode = notNullSourceCodeBuilder.GetSourceCode();
                context.AddSource(className+"_Validator_G", SourceText.From(sourceCode, Encoding.UTF8));
            });

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

