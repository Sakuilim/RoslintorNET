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
    public class ComplexityAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "CA01";
        private const string Title = "Reduce complexity of this method";
        private const string MessageFormat = "Method '{0}' complexity is too high. Consider simplifying your method.";
        private const string Description = "Simplify your method to not be complex.";

        private static readonly DiagnosticDescriptor Rule =
       new DiagnosticDescriptor(
           DiagnosticId,
           Title,
           MessageFormat,
           "Performance",
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

            var complexity = CalculateComplexity(methodDeclaration);

            if (complexity >= 6) // Threshold for high complexity
            {
                // Report diagnostic
                var diagnostic = Diagnostic.Create(Rule, methodDeclaration.Identifier.GetLocation(), methodDeclaration.Identifier.Text);
                context.ReportDiagnostic(diagnostic);
            }
        }
        private static int CalculateComplexity(MethodDeclarationSyntax method)
        {
            var decisionPoints = method.DescendantNodes().Count(node => IsDecisionPoint(node));
            return decisionPoints + 1;
        }
        private static bool IsDecisionPoint(SyntaxNode node)
        {
            return node.IsKind(SyntaxKind.IfStatement) || node.IsKind(SyntaxKind.ForStatement) ||
                node.IsKind(SyntaxKind.ForEachStatement) || node.IsKind(SyntaxKind.WhileStatement) ||
                node.IsKind(SyntaxKind.SwitchStatement) || node.IsKind(SyntaxKind.CaseSwitchLabel);
        }
    }
}
