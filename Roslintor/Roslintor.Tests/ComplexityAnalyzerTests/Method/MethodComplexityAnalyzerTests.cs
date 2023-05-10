using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = Roslintor.Test.CSharpAnalyzerVerifier<
    Roslintor.Analyzers.ComplexityAnalyzers.Method.MethodCyclomaticComplexityAnalyzer>;

namespace Roslintor.Tests.ComplexityAnalyzerTests.Method
{
    [TestClass]
    public class MethodComplexityAnalyzerTests
    {
        [TestMethod]
        public async Task CyclomaticComplexityAnalysis_Should_ReturnCyclomaticComplexityLevelGood()
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
        public async Task CyclomaticComplexityAnalysis_Should_ReturnCyclomaticComplexityLevelTooHigh()
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
                        for (int o = 0; o < 15 + 1; o++)
                        {
                            for (int p = 0; p < 15 + 1; p++)
                            {
                                if (o == 5)
                                {
                                    p++;
                                }
                                else
                                {
                                    o++;
                                }
                            }
                        }
                        var x = 0;
                        var k = 1;
                        if (x > k || x == k)
                        {
                          x = 2;
                        }
                        if(k > x && k != x)
                        {
                            k = 1;
                        }
                        if(k == x)
                        {
                            x++;
                        }
                    }    
                }
            }";

            var expected = VerifyCS.Diagnostic("CAR001").WithLocation(0).WithArguments("MethodName","9");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);

        }
    }
}
