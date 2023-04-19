using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = Roslintor.Test.CSharpCodeFixVerifier<
    Roslintor.Analyzers.ModularityAnalyzers.CouplingAnalyzer,
    Roslintor.SecureStringCodeFix.SecureStringFixProvider>;

namespace Roslintor.Tests.ModularityAnalyzerTests
{
    [TestClass]
    public class CouplingAnalyzerTests
    {
        [TestMethod]
        public async Task CodeCouplingAnalysis_Should_ReturnCodeCouplingLevelGood()
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
        public async Task CodeCouplingAnalysis_Should_ReturnCodeCouplingLevelTooHigh()
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
                    }      
                }
                public class TestClass1
                {   
                    public void MethodName(string name)
                    {
                        var m1 = new TestClass();
                        var x = 0;
                        var k = 1;
                    }      
                }
            }";

            var expected = VerifyCS.Diagnostic("MA01").WithLocation(0).WithArguments("MethodName",1);

            await VerifyCS.VerifyAnalyzerAsync(test, expected);

        }
    }
}
