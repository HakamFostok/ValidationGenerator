﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace ValidationGenerator.Core.Extensions;

internal static class CodeFormatterExtension
{
    /// <summary>
    /// Format source generated code
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    /// <seealso href="https://stackoverflow.com/a/47152803"/>
    internal static string FormatCode(this string code)
    {
        if (string.IsNullOrEmpty(code))
            return string.Empty;

        SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
        SyntaxNode root = tree.GetRoot().NormalizeWhitespace();
        return root.ToFullString();
    }
}
