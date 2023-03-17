using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace Roslintor.Helper
{
    public class CognitiveComplexityWalker : CSharpSyntaxWalker
    {
        public int NestingDepth { get; private set; }
        public int ControlFlowBranches { get; private set; }
        public int LogicalBranches { get; private set; }
        public int CaseStatements { get; private set; }

        public override void VisitIfStatement(IfStatementSyntax node)
        {
            ControlFlowBranches++;
            LogicalBranches++;
            base.VisitIfStatement(node);
        }

        public override void VisitWhileStatement(WhileStatementSyntax node)
        {
            ControlFlowBranches++;
            LogicalBranches++;
            base.VisitWhileStatement(node);
        }

        public override void VisitDoStatement(DoStatementSyntax node)
        {
            ControlFlowBranches++;
            LogicalBranches++;
            base.VisitDoStatement(node);
        }

        public override void VisitForStatement(ForStatementSyntax node)
        {
            ControlFlowBranches++;
            LogicalBranches++;
            base.VisitForStatement(node);
        }

        public override void VisitForEachStatement(ForEachStatementSyntax node)
        {
            ControlFlowBranches++;
            LogicalBranches++;
            base.VisitForEachStatement(node);
        }

        public override void VisitSwitchStatement(SwitchStatementSyntax node)
        {
            ControlFlowBranches++;
            CaseStatements += node.Sections.Sum(section => section.Labels.Count);
            base.VisitSwitchStatement(node);
        }

        public override void VisitCatchClause(CatchClauseSyntax node)
        {
            ControlFlowBranches++;
            base.VisitCatchClause(node);
        }

        public override void VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node)
        {
            NestingDepth++;
            base.VisitParenthesizedLambdaExpression(node);
            NestingDepth--;
        }

        public override void VisitAnonymousMethodExpression(AnonymousMethodExpressionSyntax node)
        {
            NestingDepth++;
            base.VisitAnonymousMethodExpression(node);
            NestingDepth--;
        }

        public override void VisitLocalFunctionStatement(LocalFunctionStatementSyntax node)
        {
            NestingDepth++;
            base.VisitLocalFunctionStatement(node);
            NestingDepth--;
        }
    }
}
