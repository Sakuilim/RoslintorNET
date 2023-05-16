using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

namespace Roslintor.Analyzers.SizeAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MethodSize80Analyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "MSA003";
        private const string Title = "Method is too big";
        private const string MessageFormat = "Method '{0}' is too big. Consider extracting some code to another method.";
        private const string Description = "Make your method smaller.";
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
                if (method != null && method.Body != null && method.Body.Statements.Count >= 80)
                {
                    var diagnostic = Diagnostic.Create(Rule, method.Identifier.GetLocation(), method.Identifier.Text);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
