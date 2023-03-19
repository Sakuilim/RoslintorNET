using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using VerifyCS = Roslintor.Test.CSharpCodeFixVerifier<
    Roslintor.ComplexityAnalyzers.NestingLevelAnalyzer,
    Roslintor.NamingCodeFix.RoslintorCodeFixProvider>;

namespace Roslintor.Tests
{
    [TestClass]
    public class NestingLevelAnalyzerTests
    {
        [TestMethod]
        public async Task NestingLevelAnalysis_Should_ReturnNestingLevelGood()
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
        public async Task NestingLevelAnalysis_Should_ReturnNestingLevelTooHigh()
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
                        var a = 10;
                        var b = 10;
                        var c = 10;
                        var d = 10;
                        var e = 10;
                        var ee = 10;
                        while(ee > 10)
                        {
                            if(a > 10)
                            {
                                if(b > 16)
                                {
                                    if(c > 161)
                                    {
                                        if(d > 99)
                                        {
                                            e = 51;
                                            ee++;
                                        }
                                    }
                                }
                            }
                        }
                    }    
                }
            }";

            var expected = VerifyCS.Diagnostic("FA01").WithLocation(0).WithArguments("MethodName", "3");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);

        }
    }
}
