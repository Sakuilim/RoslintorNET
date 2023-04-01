using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslintor.Helper;

namespace Roslintor.Analyzers.MaintainabilityAnalyzers.Method
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MethodMaintainabilityAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "MA01";
        private const string Category = "Maintainability";
        private const string Title = "Reduce complexity of this method";
        private const string MessageFormat = "Method '{0}' complexity is too high. Consider simplifying your method.";
        private const string Description = "Simplify your method to not be complex.";

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
            var mi = CalculateMI(halsteadVolume, cyclomaticComplexity, linesOfCode);

            if (mi < 55)
            {
                var diagnostic = Diagnostic.Create(Rule, method.Identifier.GetLocation(), method.Identifier);
                context.ReportDiagnostic(diagnostic);
            }
        }
        private static double CalculateMI(double halsteadVolume, int cyclomaticComplexity, int linesOfCode)
        {
            return 171 - 5.2 * System.Math.Log(halsteadVolume) - 0.23 * cyclomaticComplexity - 16.2 * System.Math.Log(linesOfCode);
        }
    }
}
