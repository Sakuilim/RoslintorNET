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
    public class GoToExemptAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "BPA01";
        private const string Title = "Don't use 'goto' statement";
        private const string MessageFormat = "Statement '{0}' should not be used. Consider changing your statement to not use 'goto'.";
        private const string Description = "Update your statement usage to not include 'goto' statement";

        private static readonly DiagnosticDescriptor Rule =
       new DiagnosticDescriptor(
           DiagnosticId,
           Title,
           MessageFormat,
           "BestPractices",
           DiagnosticSeverity.Warning,
           isEnabledByDefault: true,
           description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            //context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            //context.EnableConcurrentExecution();
            //context.RegisterSyntaxNodeAction(
            //    AnalyzeType,
            //    SyntaxKind.ClassDeclaration,
            //    SyntaxKind.StructDeclaration,
            //    SyntaxKind.RecordStructDeclaration);
        }

        private static void AnalyzeType(SyntaxNodeAnalysisContext context)
        {
            //var methodDeclaration = context.Node.DescendantNodes().OfType<MemberDeclarationSyntax>();

            //foreach (var item in methodDeclaration)
            //{
            //    var method = item as MethodDeclarationSyntax;
            //    if (method != null)
            //    {
            //        if (!CamelCaseHelper.IsCamelCase(method.Identifier.Text))
            //        {
            //            var diagnostic = Diagnostic.Create(Rule, method.Identifier.GetLocation(), method.Identifier.Text);
            //            context.ReportDiagnostic(diagnostic);
            //        }
            //    }
            //}
        }
    }
}
