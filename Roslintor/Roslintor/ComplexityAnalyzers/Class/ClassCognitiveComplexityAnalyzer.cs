using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslintor.Helper;

namespace Roslintor.Analyzers.ComplexityAnalyzers.Class
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ClassCognitiveComplexityAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "CA03";
        private const string Title = "Reduce cognitive complexity of this class";
        private const string MessageFormat = "Class '{0}' cognitive complexity is too high. Consider simplifying your class.";
        private const string Description = "Simplify your class to not be complex.";

        private const int CognitiveComplexityThreshold = 55;

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
                AnalyzeMethod, SyntaxKind.ClassDeclaration);
        }

        private static void AnalyzeMethod(SyntaxNodeAnalysisContext context)
        {
            var classDeclaration = (ClassDeclarationSyntax)context.Node;

            var cognitiveComplexity = CalculateCognitiveComplexity(classDeclaration);

            if (cognitiveComplexity > CognitiveComplexityThreshold)
            {
                var diagnostic = Diagnostic.Create(Rule, classDeclaration.Identifier.GetLocation(), classDeclaration.Identifier.Text);
                context.ReportDiagnostic(diagnostic);
            }
        }

        private static int CalculateCognitiveComplexity(ClassDeclarationSyntax classDeclaration)
        {
            var cognitiveComplexity = 0;

            var walker = new CognitiveComplexityWalker();

            foreach(var method in classDeclaration.ChildNodes())
            {
                walker.Visit(method);
            }

            cognitiveComplexity += walker.NestingDepth;
            cognitiveComplexity += walker.ControlFlowBranches;
            cognitiveComplexity += walker.LogicalBranches;
            cognitiveComplexity += walker.CaseStatements;

            return cognitiveComplexity;
        }
    }
}
