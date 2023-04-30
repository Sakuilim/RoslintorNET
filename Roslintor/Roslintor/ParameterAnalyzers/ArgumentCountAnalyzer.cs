using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslintor.Analyzers.ParameterAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ArgumentCountAnalyzer : DiagnosticAnalyzer
    {
        private const string DiagnosticId = "AC001";
        private const string Category = "Naming";
        private const string Title = "Reduce the amount of arguments in a method";
        private const string MessageFormat = "Method '{0}' has {1} arguments, which exceeds the maximum limit of {2}";
        private const string Description = "Methods with excessive amount of arguments can be difficult to understand and maintain.";

        private const int MaxArgumentCount = 4;

        private static readonly DiagnosticDescriptor Rule =
       new DiagnosticDescriptor(
           DiagnosticId,
           Title,
           MessageFormat,
           Category,
           DiagnosticSeverity.Warning,
           isEnabledByDefault: true,
           description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(
                AnalyzeMethod, SyntaxKind.MethodDeclaration);
        }
        private static void AnalyzeMethod(SyntaxNodeAnalysisContext context)
        {
            var method = (MethodDeclarationSyntax)context.Node;

            if (method.ParameterList.Parameters.Count > MaxArgumentCount)
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, method.Identifier.GetLocation(), method.Identifier.ValueText, method.ParameterList.Parameters.Count, MaxArgumentCount));
            }
        }
    }
}