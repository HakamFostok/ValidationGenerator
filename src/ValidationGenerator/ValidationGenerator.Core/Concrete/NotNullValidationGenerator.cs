using Microsoft.CodeAnalysis;
using System.Diagnostics;

namespace ValidationGenerator.Core.Concrete;

[Generator]
public class NotNullValidationGenerator : ISourceGenerator
{
    public NotNullValidationGenerator()
    {

    }
    public void Execute(GeneratorExecutionContext context)
    {

    }

    public void Initialize(GeneratorInitializationContext context)
    {
        Debugger.Launch();
        context.RegisterForSyntaxNotifications(() =>
        new AttributeSyntaxReceiver<NotNullAttribute>());
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class NotNullAttribute : Attribute
{
}
