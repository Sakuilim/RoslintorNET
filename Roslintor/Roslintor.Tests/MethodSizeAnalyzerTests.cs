using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using VerifyCS = Roslintor.Test.CSharpAnalyzerVerifier<
    Roslintor.Analyzers.NamingAnalyzers.MethodSizeAnalyzer>;

namespace Roslintor.Tests
{
    [TestClass]
    public class MethodSizeAnalyzerTests
    {
        [TestMethod]
        public async Task MethodSize_Empty_ShouldShowWarning()
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
                        
                    }    
                }
            }";

            var expected = VerifyCS.Diagnostic("MSA002").WithLocation(0).WithArguments("MethodName");

            Console.WriteLine(expected);

            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task MethodSize_Empty_ShouldNotShowWarning()
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
    }
}
