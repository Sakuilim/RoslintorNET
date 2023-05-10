using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Linq;

namespace Roslintor.Helpers.Helpers
{
    public static class CyclomaticComplexityCalculator
    {
        public static int CalculateComplexity(MethodDeclarationSyntax method)
        {
            var decisionPoints = method.DescendantNodes().Count(node => IsDecisionPoint(node));
            return decisionPoints + 1;
        }
        private static bool IsDecisionPoint(SyntaxNode node)
        {
            return node.IsKind(SyntaxKind.IfStatement) || node.IsKind(SyntaxKind.ForStatement) ||
                node.IsKind(SyntaxKind.ForEachStatement) || node.IsKind(SyntaxKind.WhileStatement) ||
                node.IsKind(SyntaxKind.SwitchStatement) || node.IsKind(SyntaxKind.CaseSwitchLabel) ||
                node.IsKind(SyntaxKind.ConditionalExpression) || node.IsKind(SyntaxKind.LogicalAndExpression) ||
                node.IsKind(SyntaxKind.LogicalOrExpression);
        }

    }
}
