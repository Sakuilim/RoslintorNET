using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

namespace Roslintor.Analyzers.SizeAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MethodSizeAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "MSA002";
        private const string Title = "Empty method";
        private const string MessageFormat = "Method '{0}' is empty. Consider adding code to your method.";
        private const string Description = "Add code to your method.";
        private const string Category = "Style";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(
                AnalyzeMethod,
                SyntaxKind.ClassDeclaration,
                SyntaxKind.StructDeclaration,
                SyntaxKind.RecordStructDeclaration);
        }

        private void AnalyzeMethod(SyntaxNodeAnalysisContext context)
        {
            var methodDeclaration = context.Node.DescendantNodes().OfType<MemberDeclarationSyntax>();

            foreach (var item in methodDeclaration)
            {
                var method = item as MethodDeclarationSyntax;
                if (method != null && (method.Body == null || method.Body.Statements.Count == 0))
                {
                    var diagnostic = Diagnostic.Create(Rule, method.Identifier.GetLocation(), method.Identifier.Text);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
