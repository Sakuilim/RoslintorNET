using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslintor.Helper;

namespace Roslintor.NamingAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class PerformancePracticeAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "PPA01";
        private const string Title = "Use a Hash Table for Searching";
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
            context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.InvocationExpression);
        }

        private void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            var invocation = context.Node as InvocationExpressionSyntax;

            if (invocation != null &&
                invocation.Expression is MemberAccessExpressionSyntax memberAccess &&
                memberAccess.Name.Identifier.ValueText.Equals("IndexOf"))
            {
                var symbolInfo = context.SemanticModel.GetSymbolInfo(memberAccess.Name);
                if (symbolInfo.Symbol != null &&
                    symbolInfo.Symbol.ContainingType.SpecialType == SpecialType.System_Collections_Generic_IList_T)
                {
                    var diagnostic = Diagnostic.Create(Rule, memberAccess.Name.GetLocation());
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }

        private void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is InvocationExpressionSyntax invocation &&
                invocation.Expression is MemberAccessExpressionSyntax memberAccess &&
                memberAccess.Name.Identifier.ValueText == "IndexOf" &&
                context.SemanticModel.GetSymbolInfo(memberAccess).Symbol is IMethodSymbol methodSymbol &&
                methodSymbol.ContainingType?.SpecialType == SpecialType.System_Collections_Generic_IList_T)
            {
                var diagnostic = Diagnostic.Create(Rule, memberAccess.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
