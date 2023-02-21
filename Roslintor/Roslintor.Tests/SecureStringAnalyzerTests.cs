using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System;
using VerifyCS = Roslintor.Test.CSharpCodeFixVerifier<
    Roslintor.NamingAnalyzers.SecureStringAnalyzer,
    Roslintor.NamingCodeFix.RoslintorCodeFixProvider>;
namespace Roslintor.Tests
{
    [TestClass]
    public class SecureStringAnalyzerTests
    {
        [TestMethod]
        public async Task SecureStringAnalysis_Should_ReturnStringSecure()
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
                    public void MethodName(string name)
                    {
                        string x = ""This string is very safe"";
                    }    
                }
            }";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }
        [TestMethod]
        public async Task SecureStringAnalysis_Should_ReturnStringNotSecure()
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
                    public void MethodName(string name)
                    {
                        string {|#0:stringVar|} = ""This string is not safe, it contains Password"";
                    }    
                }
            }";

            var expected = VerifyCS.Diagnostic("SSA01").WithLocation(0).WithArguments("stringVar");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }
    }
}
