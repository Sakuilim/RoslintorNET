using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslintor.Analyzers.FormatAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NestingLevelAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "FA001";
        private const string Category = "Format";
        private const string Title = "Method nesting level exceeded";
        private const string MessageFormat = "Method '{0}' has exceeded the maximum nesting level of {1}";
        private const string Description = "Methods with excessive nesting levels can be difficult to understand and maintain.";

        private const int NestingLevelThreshold = 8;

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

            int nestingLevel = method.DescendantNodes()
                .Count(node => IsNestingStatement(node));

            if (nestingLevel >= NestingLevelThreshold)
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, method.Identifier.GetLocation(), method.Identifier.ValueText, NestingLevelThreshold));
            }
        }
        private static bool IsNestingStatement(SyntaxNode node)
        {
            return node is IfStatementSyntax
                || node is ForStatementSyntax
                || node is WhileStatementSyntax
                || node is SwitchStatementSyntax
                || node is DoStatementSyntax
                || node is UsingStatementSyntax
                || node is BlockSyntax;
        }
    }
}
