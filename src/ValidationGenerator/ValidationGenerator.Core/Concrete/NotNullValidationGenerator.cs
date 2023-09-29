using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Linq;
using System.Text;

namespace ValidationGenerator.Core.Concrete
{


    [Generator]
    public class NotNullValidationGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var validationGeneratorClasses = context.Compilation.SyntaxTrees.Where(x => x.GetText().ToString().Contains("[Vali")).ToList();
            var propertiesWithNotNullAttribute = validationGeneratorClasses.Where(x => x.GetText().ToString().Contains("[NotNul")).ToList();

            SourceText classTextSource = validationGeneratorClasses.First().GetText();
            string classText = classTextSource.ToString();

            StringBuilder codeBuilder = new StringBuilder();

            // Append the code to the StringBuilder
            codeBuilder.AppendLine("namespace TestLab");
            codeBuilder.AppendLine("{");
            codeBuilder.AppendLine("    public partial class User");
            codeBuilder.AppendLine("    {");
            codeBuilder.AppendLine("        public void ThrowIfNotValid()");
            codeBuilder.AppendLine("        {");
            codeBuilder.AppendLine("            if (Id is null)");
            codeBuilder.AppendLine("            {");
            codeBuilder.AppendLine("                throw new ArgumentNullException(nameof(Id));");
            codeBuilder.AppendLine("            }");
            codeBuilder.AppendLine("        }");
            codeBuilder.AppendLine("    }");
            codeBuilder.AppendLine("}");

            // TODO : dynamiclly find attributes and add inside ThrowIfNotValid method

            context.AddSource("User_Validator", SourceText.From(codeBuilder.ToString(), Encoding.UTF8));
        }

        public void Initialize(GeneratorInitializationContext context)
        {

//#if DEBUG

//            if (!Debugger.IsAttached)
//            {
//                Debugger.Launch();
//            }

//#endif

        }
    }
}

