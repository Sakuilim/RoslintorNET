using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace Roslintor.Helpers.Helpers
{
    public static class HalsteadVolumeVisitor
    {
        public static double ComputeHalsteadVolume(MethodDeclarationSyntax method)
        {
            var operatorCounts = new Dictionary<string, int>();
            var operandCounts = new Dictionary<string, int>();
            var operatorTokens = new HashSet<SyntaxKind>
            {
                SyntaxKind.AmpersandAmpersandToken,
                SyntaxKind.BarBarToken,
                SyntaxKind.PlusToken,
                SyntaxKind.MinusToken,
                SyntaxKind.AsteriskToken,
                SyntaxKind.SlashToken,
                SyntaxKind.CaretToken,
                SyntaxKind.PercentToken,
                SyntaxKind.EqualsEqualsToken,
                SyntaxKind.ExclamationEqualsToken,
                SyntaxKind.LessThanToken,
                SyntaxKind.LessThanEqualsToken,
                SyntaxKind.GreaterThanToken,
                SyntaxKind.GreaterThanEqualsToken,
                SyntaxKind.QuestionQuestionToken,
                SyntaxKind.QuestionToken,
                SyntaxKind.ColonToken,
                SyntaxKind.DotToken
            };

            foreach (var token in method.DescendantTokens())
            {
                if (operatorTokens.Contains(token.Kind()))
                {
                    var operatorText = token.Text;
                    if (operatorCounts.ContainsKey(operatorText))
                    {
                        operatorCounts[operatorText]++;
                    }
                    else
                    {
                        operatorCounts[operatorText] = 1;
                    }
                }
                else if (token.IsKind(SyntaxKind.IdentifierToken) || token.IsKind(SyntaxKind.NumericLiteralToken) || token.IsKind(SyntaxKind.StringLiteralToken))
                {
                    var operandText = token.Text;
                    if (operandCounts.ContainsKey(operandText))
                    {
                        operandCounts[operandText]++;
                    }
                    else
                    {
                        operandCounts[operandText] = 1;
                    }
                }
            }

            var distinctOperators = operatorCounts.Keys.Count;
            var totalOperators = 0;
            foreach (var count in operatorCounts.Values)
            {
                totalOperators += count;
            }

            var distinctOperands = operandCounts.Keys.Count;
            var totalOperands = 0;
            foreach (var count in operandCounts.Values)
            {
                totalOperands += count;
            }

            var halsteadVolume = distinctOperators * Math.Log(totalOperators) + distinctOperands * Math.Log(totalOperands);
            return halsteadVolume;
        }
    }
}
