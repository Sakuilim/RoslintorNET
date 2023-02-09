using Microsoft.CodeAnalysis.CSharp.Testing.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using VerifyCS = Roslintor.Test.CSharpCodeFixVerifier<
    Roslintor.NamingAnalyzers.MethodSizeAnalyzer,
    Roslintor.NamingCodeFix.RoslintorCodeFixProvider>;

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

            var expected = VerifyCS.Diagnostic("MSA02").WithLocation(0).WithArguments("MethodName");

            Console.WriteLine(expected);

            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }
    }
}
