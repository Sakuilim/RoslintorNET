using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = Roslintor.Test.CSharpAnalyzerVerifier<
    Roslintor.Analyzers.CodeDuplicationAnalyzers.CodeDuplicationAnalyzer>;

namespace Roslintor.Tests.CodeDuplicationAnalyzerTests
{
    [TestClass]
    public class CodeDuplicationAnalyzerTests
    {
        [TestMethod]
        public async Task CodeDuplicationAnalysis_Should_ReturnCodeDuplicationLevelGood()
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
        public async Task CodeDuplicationAnalysis_Should_ReturnCodeDuplicationLevelTooHigh()
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
                    public void {|#1:MethodName|}(string name)
                    {
                        var x = 0;
                        var k = 1;
                    }      
                }
            }";

            var expected = VerifyCS.Diagnostic("CD001").WithLocation(0).WithArguments("MethodName");
            var expected2 = VerifyCS.Diagnostic("CD001").WithLocation(1).WithArguments("MethodName");

            await VerifyCS.VerifyAnalyzerAsync(test, expected, expected2);

        }
    }
}
