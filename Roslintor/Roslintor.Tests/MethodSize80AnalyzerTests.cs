using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using VerifyCS = Roslintor.Test.CSharpAnalyzerVerifier<
    Roslintor.Analyzers.SizeAnalyzers.MethodSize80Analyzer>;

namespace Roslintor.Tests
{
    [TestClass]
    public class MethodSize80AnalyzerTests
    {
        [TestMethod]
        public async Task MethodSize_TooBig_ShouldShowWarning()
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
                        var a = 10;
                        var b = 10;
                        var c = 10;
                        var d = 10;
                        var e = 10;
                        var f = 10;
                        var g = 10;
                        var h = 10;
                        var j = 10;
                        var k = 10;
                        var l = 10;
                        var aa = 10;
                        var bb = 10;
                        var cc = 10;
                        var dd = 10;
                        var ee = 10;
                        var ff = 10;
                        var gg = 10;
                        var hh = 10;
                        var jj = 10;
                        var kk = 10;
                        var ll = 10;
                        var mm = 10;
                        var nn = 10;
                        var oo = 10;
                        var qq = 10;
                        var rr = 10;
                        var zz = 10;
                        var tt = 10;
                        var aaa = 10;
                        var bbb = 10;
                        var ccc = 10;
                        var ddd = 10;
                        var eee = 10;
                        var fff = 10;
                        var ggg = 10;
                        var kk1 = 10;
                        var ll1 = 10;
                        var mm1 = 10;
                        var nn1 = 10;
                        var oo1 = 10;
                        var qq1 = 10;
                        var rr1 = 10;
                        var zz1 = 10;
                        var tt1 = 10;
                        var aaa2 = 10;
                        var bbb2 = 10;
                        var ccc2 = 10;
                        var ddd2 = 10;
                        var eee2 = 10;
                        var fff2 = 10;
                        var ggg2 = 10;
                        var a12 = 10;
                        var b12 = 10;
                        var c12 = 10;
                        var d12 = 10;
                        var e12 = 10;
                        var f12 = 10;
                        var g12 = 10;
                        var h12 = 10;
                        var j12 = 10;
                        var k12 = 10;
                        var l12 = 10;
                        var aa5 = 10;
                        var bb5 = 10;
                        var cc5 = 10;
                        var dd5 = 10;
                        var ee5 = 10;
                        var ff5 = 10;
                        var gg5 = 10;
                        var hh5 = 10;
                        var jj5 = 10;
                        var kk5 = 10;
                        var ll5 = 10;
                        var mm5 = 10;
                        var nn5 = 10;
                        var oo5 = 10;
                        var qq5 = 10;
                        var rr5 = 10;
                        var zz5 = 10;
                        var tt5 = 10;
                    }    
                }
            }";

            var expected = VerifyCS.Diagnostic("MSA003").WithLocation(0).WithArguments("MethodName");

            Console.WriteLine(expected);

            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task MethodSize_TooBig_ShouldNotShowWarning()
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

            await VerifyCS.VerifyAnalyzerAsync(test);
        }
    }
}
