using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslintor.Helpers.Helpers;

namespace Roslintor.Analyzers.Helpers.ComplexityCalculationHelpers
{
    public static class CognitiveComplexityCalculator
    {
        public static int CalculateCognitiveComplexity(MethodDeclarationSyntax method)
        {
            var cognitiveComplexity = 0;

            var walker = new CognitiveComplexityWalker();
            walker.Visit(method.Body);

            cognitiveComplexity += walker.NestingDepth;
            cognitiveComplexity += walker.ControlFlowBranches;
            cognitiveComplexity += walker.LogicalBranches;
            cognitiveComplexity += walker.CaseStatements;

            return cognitiveComplexity;
        }
        public static int CalculateCognitiveComplexity(ClassDeclarationSyntax classDeclaration)
        {
            var cognitiveComplexity = 0;

            var walker = new CognitiveComplexityWalker();

            foreach (var method in classDeclaration.ChildNodes())
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
