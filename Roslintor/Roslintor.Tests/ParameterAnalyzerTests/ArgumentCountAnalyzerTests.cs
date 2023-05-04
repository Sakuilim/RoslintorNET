using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = Roslintor.Test.CSharpAnalyzerVerifier<
    Roslintor.Analyzers.ParameterAnalyzers.ArgumentCountAnalyzer>;

namespace Roslintor.Tests.ParameterAnalyzerTests
{
    [TestClass]
    public class ArgumentCountAnalyzerTests
    {
        [TestMethod]
        public async Task ArgumentCountAnalysis_Should_ReturnArgumentCountGood()
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
        public async Task ArgumentCountAnalysis_Should_ReturnArgumentCountTooHigh()
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
                    public void {|#0:MethodName|}(string name, int number, double amount, string firstName, string lastName, string someName)
                    {
                        
                    }    
                }
            }";

            var expected = VerifyCS.Diagnostic("AC001").WithLocation(0).WithArguments("MethodName", "6", "5");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);

        }
    }
}
