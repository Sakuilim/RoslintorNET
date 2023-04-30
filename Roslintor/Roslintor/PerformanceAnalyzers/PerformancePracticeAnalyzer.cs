using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslintor.Analyzers.PerformanceAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class PerformancePracticeAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "PPA001";
        private const string Title = "Use a HashTable for Searching";
        private const string MessageFormat = "Consider using a hash table for searching instead of a list.";
        private const string Category = "Performance";
        private const string Description = "Using a hash table can improve search performance.";

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
            context.RegisterSyntaxNodeAction(AnalyzeListUsage, SyntaxKind.InvocationExpression);
        }
        private static void AnalyzeListUsage(SyntaxNodeAnalysisContext context)
        {
            var invocation = (InvocationExpressionSyntax)context.Node;

            // Check if the method invoked is List<T>.Contains
            if (invocation.Expression is MemberAccessExpressionSyntax memberAccess
                && memberAccess.Name.Identifier.Text == nameof(List<object>.Contains)
                && context.SemanticModel.GetSymbolInfo(memberAccess).Symbol is IMethodSymbol methodSymbol
                && methodSymbol.ContainingType.Name.StartsWith("List"))
            {
                // Get the name of the List variable
                string listName = memberAccess.Expression.ToString();

                // Report a diagnostic suggesting to use HashSet instead of List
                context.ReportDiagnostic(Diagnostic.Create(Rule, memberAccess.Name.GetLocation(), listName));
            }
        }
    }
}
