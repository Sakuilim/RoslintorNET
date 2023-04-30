using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = Roslintor.Test.CSharpAnalyzerVerifier<
    Roslintor.Analyzers.SecurityAnalyzers.SecureStringAnalyzer>;

namespace Roslintor.Tests
{
    [TestClass]
    public class SecureStringAnalyzerTests
    {
        [TestMethod]
        public async Task SecureStringAnalysis_Should_NotTrigger_ForSafeStrings()
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
                        string safeString1 = ""This is a safe string."";
                        string safeString2 = ""Another example of a safe string."";
                        string safeString3 = ""No sensitive words here."";
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
                        string {|#0:stringVarPassword|} = ""This string is not safe, it contains Password"";
                        string {|#1:stringVarSecret|} = ""This string is not safe, it contains a secret"";
                        string {|#2:stringVarPsw|} = ""This string is not safe, it contains psw"";
                        string safeString = ""This string is safe."";
                    }    
                }
            }";

            var expected1 = VerifyCS.Diagnostic("SSA001").WithLocation(0).WithArguments("stringVarPassword");
            var expected2 = VerifyCS.Diagnostic("SSA001").WithLocation(1).WithArguments("stringVarSecret");
            var expected3 = VerifyCS.Diagnostic("SSA001").WithLocation(2).WithArguments("stringVarPsw");

            await VerifyCS.VerifyAnalyzerAsync(test, expected1, expected2, expected3);
        }
    }
}
