using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VerifyCS = Roslintor.Test.CSharpCodeFixVerifier<
    Roslintor.NamingAnalyzers.PerformancePracticeAnalyzer,
    Roslintor.NamingCodeFix.RoslintorCodeFixProvider>;

namespace Roslintor.Tests
{
    [TestClass]
    public class PerformancePracticeAnalyzerTests
    {
        [TestMethod]
        public async Task PerformancePractice_Wrong_ShouldNotShowWarning()
        {
            var test = @"
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Text;
            using System.Threading.Tasks;
            using System.Diagnostics;
            using System.Collections;

            namespace ConsoleApplication1
            {
                public class TestClass
                {   
                    public void {|#0:MethodName|}(string name)
                    {
                        Hashtable ht = new Hashtable();
                        
                        ht.Add(""001"", ""MeLOL"");

                    }    
                }
            }";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task PerformancePractice_Wrong_ShouldShowWarning()
        {
            var test = @"
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Text;
            using System.Threading.Tasks;
            using System.Diagnostics;
            using System.Collections;

            namespace ConsoleApplication1
            {
                public class TestClass
                {   
                    public void Method(string name)
                    {
                        var list = new List<string> { ""foo"", ""bar"", ""baz"" };
                        var searchTerm = ""baz"";
                        var index = list.IndexOf(searchTerm); // This will trigger a diagnostic because List.IndexOf is less efficient than using a HashSet or Dictionary.
                    }    
                }
            }";

            var expected = VerifyCS.Diagnostic("PPA01");

            Console.WriteLine(expected);

            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }
    }
}
