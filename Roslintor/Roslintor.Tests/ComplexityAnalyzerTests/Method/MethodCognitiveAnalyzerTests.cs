using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = Roslintor.Test.CSharpAnalyzerVerifier<
    Roslintor.Analyzers.ComplexityAnalyzers.Method.MethodCognitiveComplexityAnalyzer>;

namespace Roslintor.Tests.ComplexityAnalyzerTests
{
    [TestClass]
    public class MethodCognitiveAnalyzerTests
    {
        [TestMethod]
        public async Task CognitiveComplexityAnalysis_Should_ReturnCognitiveComplexityLevelGood()
        {
            var test = @"
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Text;
            using System.Threading.Tasks;
            using System.Diagnostics;

            namespace ConsoleApplication1
            {
                public class TestClass
                {   
                    public void {|#0:MethodName|}(string name)
                    {
                        var x = 1;
                    }    
                }
            }";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }
        [TestMethod]
        public async Task CognitiveComplexityAnalysis_Should_ReturnCognitiveComplexityLevelTooHigh()
        {
            var test = @"
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Text;
            using System.Threading.Tasks;
            using System.Diagnostics;

            namespace ConsoleApplication1
            {
                public class TestClass
                {   
                    public void {|#0:MethodName|}(string name)
                    {
                        var x = 0;
                        var k = 1;
                        if (x > k)
                        {
                            while(x > 0)
                            {
                                x = 2;
                            }
                        }
                        if(k > x)
                        {
                            for(int i=0;i<x; i++)
                            {
                                k = 1;
                            }
                        }
                        if(k == x)
                        {
                            do
                            {
                                x++;
                            } while (k < 0);
                        }

                        switch (x)
                        {
                            case 1:
                                k = 2;
                                break;
                            case 2:
                                k = 3;
                                break;
                            default:
                                k = 4;
                                break;
                        }

                        try
                        {
                            x = k / x;
                        }
                        catch (Exception)
                        {
                            x = -1;
                        }

                        Func<int, int> parenthesizedLambda = (y) => y * 2;
                        Action<int> anonymousMethod = delegate (int y) { x = y * 3; };

                        int LocalFunction(int y)
                        {
                            return y * 4;
                        }
                    }    
                }
            }";

            var expected = VerifyCS.Diagnostic("CA002").WithLocation(0).WithArguments("MethodName");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);

        }
    }
}
