using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslintor.Helpers.Helpers;

namespace Roslintor.Analyzers.MaintainabilityAnalyzers.Class
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ClassMaintainabilityAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "MA002";
        private const string Category = "Maintainability";
        private const string Title = "Simplify this class to improve Maintainability Index";
        private const string MessageFormat = "Class '{0}' Maintainability Index is too low. Consider simplifying your class.";
        private const string Description = "Simplify your class to make it more maintainable.";

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

            var mi = MICalculator.CalculateMI(halsteadVolume, cyclomaticComplexity, linesOfCode);

            if (mi < 70)
            {
                var diagnostic = Diagnostic.Create(Rule, classDeclaration.Identifier.GetLocation(), classDeclaration.Identifier);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
