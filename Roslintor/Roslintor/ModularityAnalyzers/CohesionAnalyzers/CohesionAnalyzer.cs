using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslintor.Analyzers.ModularityAnalyzers
{
    [ExcludeFromCodeCoverage]
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CohesionAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "COA002";
        private const string Category = "Performance";
        private const string Title = "Low cohesion detected";
        private const string MessageFormat = "The method '{0}' has an unused parameter. Consider refactoring the class to improve cohesion.";
        private const string Description = "Methods with high cohesion can be difficult to understand and maintain.";

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
            // Get the method declaration syntax node
            var method = (MethodDeclarationSyntax)context.Node;

            // Check if the method has any unused parameters
            var unusedParameters = method.ParameterList.Parameters.Where(p => !context.SemanticModel.GetSymbolInfo(p.Type).Symbol.DeclaringSyntaxReferences.Any());
            if (unusedParameters.Any())
            {
                // Report a diagnostic for the method
                context.ReportDiagnostic(Diagnostic.Create(Rule, method.Identifier.GetLocation(), method.ParameterList.Parameters));
            }
        }
    }
}
