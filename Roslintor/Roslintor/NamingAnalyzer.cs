using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslintor.Helper;

namespace Roslintor
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NamingAnalyzer : DiagnosticAnalyzer
    {
        private const string DiagnosticId = "Roslintor";
        private const string Title = "Rename your method";
        private const string MessageFormat = "Method '{0}' is not written in camelCase. Consider changing your method name to camelCase.";
        private const string Description = "Change your method name to camelCase.";

        private static readonly DiagnosticDescriptor Rule =
       new DiagnosticDescriptor(
           DiagnosticId,
           Title,
           MessageFormat,
           "Naming",
           DiagnosticSeverity.Warning,
           isEnabledByDefault: true,
           description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(
                AnalyzeType,
                SyntaxKind.ClassDeclaration,
                SyntaxKind.StructDeclaration,
                SyntaxKind.RecordStructDeclaration);
        }

        private static void CodeBlockAction(CodeBlockAnalysisContext codeBlockContext)
        {
            // We only care about method bodies.
            if (codeBlockContext.OwningSymbol.Kind != SymbolKind.Method)
            {
                return;
            }

            // Report diagnostic for void non-virtual methods with empty method bodies.
            IMethodSymbol method = (IMethodSymbol)codeBlockContext.OwningSymbol;
            BlockSyntax block = (BlockSyntax)codeBlockContext.CodeBlock.ChildNodes().FirstOrDefault(n => n.Kind() == SyntaxKind.Block);
            if (method.ReturnsVoid && !method.IsVirtual && block != null && block.Statements.Count == 0)
            {
                SyntaxTree tree = block.SyntaxTree;
                Location location = method.Locations.First(l => tree.Equals(l.SourceTree));
                Diagnostic diagnostic = Diagnostic.Create(Rule, location, method.Name);
                codeBlockContext.ReportDiagnostic(diagnostic);
            }
        }

        private static void AnalyzeType(SyntaxNodeAnalysisContext context)
        {
            var methodDeclaration = context.Node.DescendantNodes().OfType<MemberDeclarationSyntax>();

            foreach (var item in methodDeclaration)
            {
                var method = item as MethodDeclarationSyntax;
                if(method != null)
                {
                    if (!CamelCaseHelper.IsCamelCase(method.Identifier.Text))
                    {
                        var diagnostic = Diagnostic.Create(Rule, method.Identifier.GetLocation(), method.Identifier.Text);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }
    }
}
