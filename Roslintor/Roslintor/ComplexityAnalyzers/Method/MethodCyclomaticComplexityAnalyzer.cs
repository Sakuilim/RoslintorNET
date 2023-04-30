using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslintor.Helpers.Helpers;

namespace Roslintor.Analyzers.ComplexityAnalyzers.Method
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MethodCyclomaticComplexityAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "CA001";
        private const string Category = "Performance";
        private const string Title = "Reduce cyclomatic complexity of this method";
        private const string MessageFormat = "Method '{0}' cyclomatic complexity is too high. Consider simplifying your method.";
        private const string Description = "Simplify your method to not be complex.";

        private const int CyclomaticComplexityThreshold = 6;

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
            var methodDeclaration = (MethodDeclarationSyntax)context.Node;

            var complexity = CyclomaticComplexityHelper.CalculateComplexity(methodDeclaration);

            if (complexity >= CyclomaticComplexityThreshold) // Threshold for high complexity
            {
                // Report diagnostic
                var diagnostic = Diagnostic.Create(Rule, methodDeclaration.Identifier.GetLocation(), methodDeclaration.Identifier.Text);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
