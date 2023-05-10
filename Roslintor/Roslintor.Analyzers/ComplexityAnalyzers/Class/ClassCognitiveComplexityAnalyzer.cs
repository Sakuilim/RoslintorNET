using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslintor.Analyzers.Helpers.ComplexityCalculationHelpers;

namespace Roslintor.Analyzers.ComplexityAnalyzers.Class
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ClassCognitiveComplexityAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "CAR003";
        private const string Title = "Reduce cognitive complexity of this class";
        private const string MessageFormat = "Class '{0}' cognitive complexity is {1}. Consider simplifying your class.";
        private const string Description = "Cognitive complexity of this class is too high. Simplify your class to not be complex.";
        private const string Category = "Performance";

        private const int CognitiveComplexityThreshold = 55;

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
                AnalyzeMethod, SyntaxKind.ClassDeclaration);
        }

        private static void AnalyzeMethod(SyntaxNodeAnalysisContext context)
        {
            var classDeclaration = (ClassDeclarationSyntax)context.Node;

            var cognitiveComplexity = CognitiveComplexityCalculator.CalculateCognitiveComplexity(classDeclaration);

            if (cognitiveComplexity > CognitiveComplexityThreshold)
            {
                var diagnostic = Diagnostic.Create(Rule, classDeclaration.Identifier.GetLocation(), classDeclaration.Identifier.Text, cognitiveComplexity);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
