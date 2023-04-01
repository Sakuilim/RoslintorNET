using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslintor.ComplexityAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CodeDuplicationAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "CD01";
        private const string Category = "Performance";
        private const string Title = "Code duplication detected";
        private const string MessageFormat = "The code in this method is very similar to the code in method '{0}'. Consider refactoring the code to avoid duplication.";
        private const string Description = "Methods with duplicate code can be difficult to understand and maintain.";

        private static readonly DiagnosticDescriptor Rule =
       new DiagnosticDescriptor(
           DiagnosticId,
           Title,
           MessageFormat,
           Category,
           DiagnosticSeverity.Warning,
           isEnabledByDefault: false,
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
            // Get the syntax tree for the method
            var method = (MethodDeclarationSyntax)context.Node;
            var methodTree = method.SyntaxTree;

            // Compare the method's syntax tree with all the other methods' syntax trees in the project
            var otherMethodTrees = context.SemanticModel.SyntaxTree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().Where(m => m != method).Select(m => m.SyntaxTree).Distinct();
            foreach (var otherTree in otherMethodTrees)
            {
                // Check if the trees are equivalent
                if (SyntaxFactory.AreEquivalent(methodTree.GetRoot(), otherTree.GetRoot()))
                {
                    // Report a diagnostic for the method
                    var otherMethod = (MethodDeclarationSyntax)otherTree.GetRoot().DescendantNodes().First(n => n is MethodDeclarationSyntax);
                    context.ReportDiagnostic(Diagnostic.Create(Rule, method.GetLocation(), otherMethod.Identifier));
                }
            }
        }
    }
}
