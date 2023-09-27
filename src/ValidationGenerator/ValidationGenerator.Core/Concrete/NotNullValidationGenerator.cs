using Microsoft.CodeAnalysis;
using System.Diagnostics;
using ValidationGenerator.Shared;

namespace ValidationGenerator.Core.Concrete
{
    [Generator]
    public class NotNullValidationGenerator : ISourceGenerator
    {
        public NotNullValidationGenerator()
        {
#if DEBUG
            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }
#endif 
        }
        public void Execute(GeneratorExecutionContext context)
        {

#if DEBUG
            Debug.WriteLine("Execute code generator");

            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }
#endif 

        }

        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }
#endif 
            try
            {
                context.RegisterForSyntaxNotifications(() =>
            new AttributeSyntaxReceiver<NotNullAttribute>());
            }
            catch (System.Exception)
            {

                throw;
            }
            
        }
    }


}

