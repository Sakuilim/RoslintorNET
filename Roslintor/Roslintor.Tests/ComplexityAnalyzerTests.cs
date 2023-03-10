using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using VerifyCS = Roslintor.Test.CSharpCodeFixVerifier<
    Roslintor.NamingAnalyzers.ComplexityAnalyzer,
    Roslintor.NamingCodeFix.RoslintorCodeFixProvider>;

namespace Roslintor.Tests
{
    [TestClass]
    public class ComplexityAnalyzerTests
    {
        [TestMethod]
        public async Task ComplexityAnalysis_Should_ReturnComplexityLevelGood()
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
        public async Task ComplexityAnalysis_Should_ReturnComplexityLevelTooHigh()
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
                            while (k < 0)
                            {
                                x++;
                            }
                        }
                    }    
                }
            }";

            var expected = VerifyCS.Diagnostic("CA01").WithLocation(0).WithArguments("MethodName");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);

        }
    }
}
