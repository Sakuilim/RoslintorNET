using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslintor.Helpers.Helpers;

namespace Roslintor.Analyzers.MaintainabilityAnalyzers.Method
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MethodMaintainabilityAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "MA001";
        private const string Category = "Maintainability";
        private const string Title = "Simplify this method to improve Maintainability Index";
        private const string MessageFormat = "Method '{0}' Maintainability Index is too low. Consider simplifying your method.";
        private const string Description = "Simplify your method to make it more maintainable.";

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
            var halsteadVolume = HalsteadVolumeVisitor.ComputeHalsteadVolume(method);
            var cyclomaticComplexity = CyclomaticComplexityHelper.CalculateComplexity(method);
            var linesOfCode = method.GetText().Lines.Count;
            var mi = MICalculator.CalculateMI(halsteadVolume, cyclomaticComplexity, linesOfCode);

            if (mi < 70)
            {
                var diagnostic = Diagnostic.Create(Rule, method.Identifier.GetLocation(), method.Identifier);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
