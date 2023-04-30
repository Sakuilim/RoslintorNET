using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslintor.Analyzers.CodeDuplicationAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CodeDuplicationAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "CD001";
        private const string Category = "Performance";
        private const string Title = "Code duplication";
        private const string MessageFormat = "The code in this method is very similar to the code in method '{0}'. Consider refactoring the code to avoid duplication.";
        private const string Description = "Methods with duplicate code can be difficult to understand and maintain.";

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
                AnalyzeCodeDuplication, SyntaxKind.MethodDeclaration);
        }
        private static void AnalyzeCodeDuplication(SyntaxNodeAnalysisContext context)
        {
            // Get the syntax tree for the method
            var method = (MethodDeclarationSyntax)context.Node;

            // Check if the method has a body
            if (method.Body == null)
            {
                return;
            }

            // Retrieve other method declarations from the same syntax tree
            var otherMethods = context.SemanticModel.SyntaxTree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().Where(m => m != method).ToList();

            // Compare the current method with all other methods
            foreach (var otherMethod in otherMethods)
            {
                // Check if the trees are equivalent
                if (SyntaxFactory.AreEquivalent(method, otherMethod))
                {
                    // Report a diagnostic for the method
                    context.ReportDiagnostic(Diagnostic.Create(Rule, method.Identifier.GetLocation(), otherMethod.Identifier));
                }
            }
        }
    }
}
