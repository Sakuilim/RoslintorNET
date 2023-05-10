using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslintor.Analyzers.Helpers.ComplexityCalculationHelpers;

namespace Roslintor.Analyzers.ComplexityAnalyzers.Method
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MethodCognitiveComplexityAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "CAR002";
        private const string Title = "Reduce cognitive complexity of this method";
        private const string MessageFormat = "Method '{0}' cognitive complexity is {1}. Consider simplifying your method.";
        private const string Description = "Cognitive complexity of this method is too high. Simplify your class to not be complex.";
        private const string Category = "Performance";

        private const int CognitiveComplexityThreshold = 15;

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

            var cognitiveComplexity = CognitiveComplexityCalculator.CalculateCognitiveComplexity(method);

            if (cognitiveComplexity > CognitiveComplexityThreshold)
            {
                var diagnostic = Diagnostic.Create(Rule, method.Identifier.GetLocation(), method.Identifier.Text, cognitiveComplexity);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
