using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = Roslintor.Test.CSharpAnalyzerVerifier<
    Roslintor.Analyzers.MaintainabilityAnalyzers.Method.MethodMaintainabilityAnalyzer>;

namespace Roslintor.Tests.MaintainabilityAnalyzerTests
{
    [TestClass]
    public class MethodMaintainabilityAnalyzerTests
    {
        [TestMethod]
        public async Task MethodMaintainabilityIndexAnalysis_Should_ReturnMethodMaintainabilityIndexLevelGood()
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
        public async Task MethodMaintainabilityIndexAnalysis_Should_ReturnMethodMaintainabilityIndexLevelTooLow()
        {
            var test = @"
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
                            for(int i=0; i<x; i++)
                            {
                                k = 1;
                            }
                        }
                        if(k == x)
                        {
                            while (k < 0)
                            {
                                x++;
                            }
                        }

                        var a = 1;
                        var b = 2;
                        var c = 3;
                        var d = 4;

                        if (a > b)
                        {
                            if (b > c)
                            {
                                if (c > d)
                                {
                                    d = 1;
                                }
                            }
                        }

                        var e = 5;
                        var f = 6;
                        if (e > f)
                        {
                            if (f > e)
                            {
                                e = 1;
                            }
                        }

                        var g = 7;
                        var h = 8;
                        if (g > h)
                        {
                            if (h > g)
                            {
                                g = 1;
                            }
                        }

                        var m = 9;
                        var n = 10;
                        if (m > n)
                        {
                            if (n > m)
                            {
                                m = 1;
                            }
                        }

                        var p = 11;
                        var q = 12;
                        if (p > q)
                        {
                            if (q > p)
                            {
                                p = 1;
                            }
                        }
                    }    
                }
            }";

            var expected = VerifyCS.Diagnostic("MA01").WithLocation(0).WithArguments("MethodName");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);

        }
    }
}
