using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslintor.Helper;

namespace Roslintor.Analyzers.MaintainabilityAnalyzers.Class
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ClassMaintainabilityAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "MA02";
        private const string Category = "Maintainability";
        private const string Title = "Reduce complexity of this class";
        private const string MessageFormat = "Class '{0}' complexity is too high. Consider simplifying your class.";
        private const string Description = "Simplify your class to not be complex.";

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

            var halsteadVolume = 0.00;
            var cyclomaticComplexity = 0;
            var linesOfCode = 0;

            foreach (var method in classDeclaration.ChildNodes())
            {
                halsteadVolume += HalsteadVolumeVisitor.ComputeHalsteadVolume(method as MethodDeclarationSyntax);
                cyclomaticComplexity += CyclomaticComplexityHelper.CalculateComplexity(method as MethodDeclarationSyntax);
                linesOfCode += classDeclaration.GetText().Lines.Count;
            }

            var mi = CalculateMI(halsteadVolume, cyclomaticComplexity, linesOfCode);

            if (mi < 55)
            {
                var diagnostic = Diagnostic.Create(Rule, classDeclaration.Identifier.GetLocation(), classDeclaration.Identifier);
                context.ReportDiagnostic(diagnostic);
            }
        }
        private static double CalculateMI(double halsteadVolume, int cyclomaticComplexity, int linesOfCode)
        {
            return 171 - 5.2 * System.Math.Log(halsteadVolume) - 0.23 * cyclomaticComplexity - 16.2 * System.Math.Log(linesOfCode);
        }
    }
}
