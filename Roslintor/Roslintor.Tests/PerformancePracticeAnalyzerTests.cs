using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = Roslintor.Test.CSharpAnalyzerVerifier<
    Roslintor.Analyzers.PerformanceAnalyzers.PerformancePracticeAnalyzer>;

namespace Roslintor.Tests
{
    [TestClass]
    public class PerformancePracticeAnalyzerTests
    {
        [TestMethod]
        public async Task ListToHashSetAnalysis_Should_NotTrigger_ForHashSetUsage()
        {
            var test = @"
            using System;
            using System.Collections.Generic;
            using System.Linq;

            namespace ConsoleApplication1
            {
                public class TestClass
                {   
                    public void MethodName()
                    {
                        HashSet<int> numbers = new HashSet<int> { 1, 2, 3, 4, 5 };
                        bool containsThree = numbers.Contains(3);
                    }    
                }
            }";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }
        [TestMethod]
        public async Task ListToHashSetAnalysis_Should_Trigger_ForListUsage()
        {
            var test = @"
            using System;
            using System.Collections.Generic;
            using System.Linq;

            namespace ConsoleApplication1
            {
                public class TestClass
                {   
                    public void MethodName()
                    {
                        List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
                        bool containsThree = numbers.{|#0:Contains|}(3);
                    }    
                }
            }";

            var expected = VerifyCS.Diagnostic("PPA01").WithLocation(0).WithArguments("Contains");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }
    }
}
