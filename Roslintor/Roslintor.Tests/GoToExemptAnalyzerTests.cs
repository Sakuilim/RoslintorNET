using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System;
using VerifyCS = Roslintor.Test.CSharpCodeFixVerifier<
    Roslintor.NamingAnalyzers.GoToExemptAnalyzer,
    Roslintor.NamingCodeFix.RoslintorCodeFixProvider>;

namespace Roslintor.Tests
{
    [TestClass]
    public class GoToExemptAnalyzerTests
    {
        [TestMethod]
        public Task GoToExemptAnalysis_Should_ReturnGoToIsExempt()
        {
            // Method intentionally left empty.
            throw new NotImplementedException();
        }
        [TestMethod]
        public Task ComplexityAnalysis_Should_ReturnGoToIsNotExempt()
        {
            // Method intentionally left empty.
            throw new NotImplementedException();
        }
    }
}
