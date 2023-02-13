using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using VerifyCS = Roslintor.Test.CSharpCodeFixVerifier<
    Roslintor.NamingAnalyzers.ComplexityAnalyzer,
    Roslintor.NamingCodeFix.RoslintorCodeFixProvider>;

namespace Roslintor.Tests
{
    [TestClass]
    public class ComplexityAnalyzerTests
    {
        [TestMethod]
        public Task ComplexityAnalysis_Should_ReturnComplexityLevelGood()
        {
            // Method intentionally left empty.
            throw new NotImplementedException();
        }
        [TestMethod]
        public Task ComplexityAnalysis_Should_ReturnComplexityLevelTooHigh()
        {
            // Method intentionally left empty.
            throw new NotImplementedException();
        }
    }
}
