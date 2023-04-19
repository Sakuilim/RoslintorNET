using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = Roslintor.Test.CSharpCodeFixVerifier<
    Roslintor.Analyzers.ModularityAnalyzers.CohesionAnalyzer,
    Roslintor.SecureStringCodeFix.SecureStringFixProvider>;

namespace Roslintor.Tests.ModularityAnalyzerTests
{
    [TestClass]
    public class CohesionAnalyzerTests
    {
        [TestMethod]
        public async Task CodeCohesionAnalysis_Should_ReturnCodeCohesionLevelGood()
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
                        var x = name;
                    }
                    public void TestMethod()
                    {
                        MethodName(""lol"");
                    }  
                }
            }";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }
        [TestMethod]
        public async Task CodeCohesionAnalysis_Should_ReturnCodeCohesionLevelTooHigh()
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
            }";

            var expected = VerifyCS.Diagnostic("MA02").WithLocation(0).WithArguments("MethodName");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);

        }
    }
}
