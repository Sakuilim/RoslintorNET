using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslintor.Helper;

namespace Roslintor.NamingAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ComplexityAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "CA01";
        private const string Title = "Reduce complexity of this class";
        private const string MessageFormat = "Class '{0}' complexity is too high. Consider simplifying your class and methods.";
        private const string Description = "Simplify your class to not be complex.";

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
                AnalyzeType,
                SyntaxKind.ClassDeclaration,
                SyntaxKind.StructDeclaration,
                SyntaxKind.RecordStructDeclaration);
        }

        private static void AnalyzeType(SyntaxNodeAnalysisContext context)
        {
            var methodDeclaration = context.Node.DescendantNodes().OfType<MemberDeclarationSyntax>();

            foreach (var item in methodDeclaration)
            {
                var method = item as MethodDeclarationSyntax;
                if (method != null)
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
