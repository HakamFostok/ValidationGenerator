using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;

namespace ValidationGenerator.Core.Concrete;

public class AttributeSyntaxReceiver<TAttribute> : ISyntaxReceiver
   where TAttribute : Attribute
{
    public IList<PropertyDeclarationSyntax> Properties { get; } = new List<PropertyDeclarationSyntax>();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is PropertyDeclarationSyntax classDeclarationSyntax &&
            classDeclarationSyntax.AttributeLists.Count > 0 &&
            classDeclarationSyntax.AttributeLists
                .Any(al => al.Attributes
                    .Any(a => a.Name.ToString().EnsureEndsWith("Attribute").Equals(typeof(TAttribute).Name))))
        {
            Properties.Add(classDeclarationSyntax);
        }
    }
}

public static class StringExtensions
{
    public static string EnsureEndsWith(
        this string source,
        string suffix)
    {
        if (source.EndsWith(suffix))
        {
            return source;
        }
        return source + suffix;
    }
}