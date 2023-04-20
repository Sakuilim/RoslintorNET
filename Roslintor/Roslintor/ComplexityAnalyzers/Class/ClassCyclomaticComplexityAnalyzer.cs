using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslintor.Helpers.Helpers;

namespace Roslintor.Analyzers.ComplexityAnalyzers.Class
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ClassCyclomaticComplexityAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "CA04";
        private const string Title = "Reduce cyclomatic complexity of this class";
        private const string MessageFormat = "Class '{0}' cyclomatic complexity is too high. Consider simplifying your class.";
        private const string Category = "Performance";
        private const string Description = "Simplify your class to not be complex.";

        private const int CyclomaticComplexityThreshold = 25;

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

            var complexity = 0;

            foreach(var method in classDeclaration.ChildNodes())
            {
                complexity += CyclomaticComplexityHelper.CalculateComplexity(method as MethodDeclarationSyntax);
            }

            if (complexity >= CyclomaticComplexityThreshold) // Threshold for high complexity
            {
                // Report diagnostic
                var diagnostic = Diagnostic.Create(Rule, classDeclaration.Identifier.GetLocation(), classDeclaration.Identifier.Text);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
