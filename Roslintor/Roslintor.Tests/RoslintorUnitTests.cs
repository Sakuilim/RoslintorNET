using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using VerifyCS = Roslintor.Test.CSharpCodeFixVerifier<
    Roslintor.NamingAnalyzers.NamingAnalyzer,
    Roslintor.NamingCodeFix.RoslintorCodeFixProvider>;

namespace Roslintor.Test
{
    [TestClass]
    public class RoslintorUnitTest
    {
        [TestMethod]
        public async Task CamelCase_Analyzer_ShouldDetect()
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

            var expected = VerifyCS.Diagnostic("NA01").WithLocation(0).WithArguments("MethodName");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task CamelCase_CodeFix_ShouldFix()
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
                public class ClassName
                {   
                    public void {|#0:MethodName|}(string name)
                    {

                    }   
                }
            }";

            var fixtest = @"
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Text;
            using System.Threading.Tasks;
            using System.Diagnostics;

            namespace ConsoleApplication1
            {
                public class ClassName
                {   
                    public void methodName(string name)
                    {

                    }   
                }
            }";

            var expected = VerifyCS.Diagnostic("NA01").WithLocation(0).WithArguments("MethodName");

            await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
        }
    }
}
