using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Roslintor.Shared
{
    public static class Globals
    {
        public static SyntaxTree Tree { get; set; }
        public static CompilationUnitSyntax Root { get; set; }
        public static CSharpCompilation Compilation { get; set; }
        public static SemanticModel Model { get; set; }
        public static Dictionary<IMethodSymbol, string> Renames { get; set; } = new Dictionary<IMethodSymbol, string>();
    }
}