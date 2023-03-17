using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslintor.Helper;

namespace Roslintor.ComplexityAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CognitiveComplexityAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "CA02";
        private const string Title = "Reduce cognitive complexity of this method";
        private const string MessageFormat = "Method '{0}' cognitive complexity is too high. Consider simplifying your method.";
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
            var method = (MethodDeclarationSyntax)context.Node;

            var cognitiveComplexity = CalculateCognitiveComplexity(method);

            if (cognitiveComplexity > 10)
            {
                var diagnostic = Diagnostic.Create(Rule, method.Identifier.GetLocation(), method.Identifier.Text);
                context.ReportDiagnostic(diagnostic);
            }
        }

        private static int CalculateCognitiveComplexity(MethodDeclarationSyntax method)
        {
            var cognitiveComplexity = 0;

            var walker = new CognitiveComplexityWalker();
            walker.Visit(method.Body);

            cognitiveComplexity += walker.NestingDepth;
            cognitiveComplexity += walker.ControlFlowBranches;
            cognitiveComplexity += walker.LogicalBranches;
            cognitiveComplexity += walker.CaseStatements;

            return cognitiveComplexity;
        }
    }
}
