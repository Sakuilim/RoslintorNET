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
    public class SecureStringAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "SA01";
        private const string Title = "Use a secure string";
        private const string MessageFormat = "String '{0}' is not a secure variable. Consider changing your method name to be more secure.";
        private const string Description = "Change your variable to be more secure.";

        private static readonly DiagnosticDescriptor Rule =
       new DiagnosticDescriptor(
           DiagnosticId,
           Title,
           MessageFormat,
           "Security",
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
            //    var methoddeclaration = context.node.descendantnodes().oftype<memberdeclarationsyntax>();

            //    foreach (var item in methoddeclaration)
            //    {
            //        var method = item as methoddeclarationsyntax;
            //        if (method != null)
            //        {
            //            if (!camelcasehelper.iscamelcase(method.identifier.text))
            //            {
            //                var diagnostic = diagnostic.create(rule, method.identifier.getlocation(), method.identifier.text);
            //                context.reportdiagnostic(diagnostic);
            //            }
            //        }
            //    }
            //}
        }
    }
}
