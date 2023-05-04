using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslintor.Analyzers.ModularityAnalyzers.CouplingAnalyzers
{
    [ExcludeFromCodeCoverage]
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CouplingAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "COA001";
        private const string Category = "Performance";
        private const string Title = "High coupling detected";
        private const string MessageFormat = "The type '{0}' has {1} dependencies on other types or members. Consider refactoring to reduce coupling.";
        private const string Description = "Methods with high coupling can be difficult to understand and maintain.";

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
                AnalyzeType, SyntaxKind.ClassDeclaration);
        }
        private static void AnalyzeType(SyntaxNodeAnalysisContext context)
        {
            // Get the class declaration syntax node
            var type = (ClassDeclarationSyntax)context.Node;

            // Count the number of dependencies on other types or members
            var dependencies = type.DescendantNodes()
                .OfType<IdentifierNameSyntax>()
                .Select(name => context.SemanticModel.GetSymbolInfo(name).Symbol)
                .Where(symbol => symbol != null && symbol.Kind != SymbolKind.Local)
                .GroupBy(symbol => symbol.ContainingAssembly)
                .Count();

            if (dependencies > 5) // You can set a threshold value for coupling
            {
                // Report a diagnostic for the type
                context.ReportDiagnostic(Diagnostic.Create(Rule, type.Identifier.GetLocation(), type.Identifier, dependencies));
            }
        }
    }
}
