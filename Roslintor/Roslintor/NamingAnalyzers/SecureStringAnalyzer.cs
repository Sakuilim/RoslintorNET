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
        public const string DiagnosticId = "SSA01";
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
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeStringLiteral, SyntaxKind.StringLiteralExpression);
        }

        private static void AnalyzeStringLiteral(SyntaxNodeAnalysisContext context)
        {
            var literalExpression = (LiteralExpressionSyntax)context.Node;

            // check if the string contains potentially insecure content
            if (literalExpression.Token.ValueText.Contains("password")
                || literalExpression.Token.ValueText.Contains("secret")
                    || literalExpression.Token.ValueText.Contains("psw")
                        ||literalExpression.Token.ValueText.Contains("Password"))
            {

                var variableDeclaration = literalExpression.AncestorsAndSelf().OfType<VariableDeclaratorSyntax>().FirstOrDefault();
                if (variableDeclaration != null)
                {
                    var variableName = variableDeclaration.Identifier.ValueText;
                    var diagnostic = Diagnostic.Create(Rule, variableDeclaration.Identifier.GetLocation(), variableName);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
