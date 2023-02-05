using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;


namespace Roslintor
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NamingAnalyzer : DiagnosticAnalyzer
    {
        private const string DiagnosticId = "Roslintor";
        private const string Title = "Rename your method";
        private const string MessageFormat = "Method '{0}' is not written in camelCase. Consider changing your method name to camelCase";
        private const string Description = "Change your method name to camelCase";

        private static readonly DiagnosticDescriptor Rule =
       new DiagnosticDescriptor(
           DiagnosticId,
           Title,
           MessageFormat,
           "Naming",
           DiagnosticSeverity.Warning,
           isEnabledByDefault: true,
           description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeSymbol, SymbolKind.Method);
            //context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            //context.EnableConcurrentExecution();
            //context.RegisterCodeBlockAction(CodeBlockAction);
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

        private static void AnalyzeSymbol(SyntaxNodeAnalysisContext context)
        {
            var methodDeclaration = context.Node as MethodDeclarationSyntax;

            if(methodDeclaration != null)
            {
                var methodName = methodDeclaration.Identifier.ValueText;
                if (!IsCamelCase(methodName))
                {
                    var diagnostic = Diagnostic.Create(Rule, methodDeclaration.GetLocation(), methodName);
                    context.ReportDiagnostic(diagnostic);
                }
            }

        }


        private static bool IsCamelCase(string name)
        {
            if (string.IsNullOrEmpty(name) || char.IsUpper(name[0]))
            {
                return false;
            }

            if (char.IsUpper(name[0]))
            {
                return false;
            }

            return true;
        }
    }
}
