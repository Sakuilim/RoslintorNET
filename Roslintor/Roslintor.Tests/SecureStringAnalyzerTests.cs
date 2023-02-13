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
        public Task SecureStringAnalysis_Should_ReturnStringSecure()
        {
            // Method intentionally left empty.
            throw new NotImplementedException();
        }
        [TestMethod]
        public Task SecureStringAnalysis_Should_ReturnStringNotSecure()
        {
            // Method intentionally left empty.
            throw new NotImplementedException();
        }
    }
}
