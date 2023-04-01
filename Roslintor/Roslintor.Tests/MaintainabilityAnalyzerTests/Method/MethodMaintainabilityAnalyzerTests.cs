using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = Roslintor.Test.CSharpCodeFixVerifier<
    Roslintor.Analyzers.MaintainabilityAnalyzers.Method.MethodMaintainabilityAnalyzer,
    Roslintor.NamingCodeFix.RoslintorCodeFixProvider>;

namespace Roslintor.Tests.MaintainabilityAnalyzerTests
{
    [TestClass]
    public class MethodMaintainabilityAnalyzerTests
    {
        [TestMethod]
        public async Task MethodMaintainabilityIndexAnalysis_Should_ReturnMethodMaintainabilityIndexLevelGood()
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
                        var x = 1;
                    }    
                }
            }";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }
        [TestMethod]
        public async Task MethodMaintainabilityIndexAnalysis_Should_ReturnMethodMaintainabilityIndexLevelTooLow()
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
                        var x = 0;
                        var k = 1;
                        if (x > k)
                        {
                            while(x > 0)
                            {
                                x = 2;
                            }
                        }
                        if(k > x)
                        {
                            for(int i=0;i<x; i++)
                            {
                                k = 1;
                            }
                        }
                        if(k == x)
                        {
                            while (k < 0)
                            {
                                x++;
                            }
                        }
                        var a = 10;
                        var b = 10;
                        var c = 10;
                        var d = 10;
                        var e = 10;
                        var f = 10;
                        var g = 10;
                        var h = 10;
                        var j = 10;
                        var l = 10;
                        var aa = 10;
                        var bb = 10;
                        var cc = 10;
                        var dd = 10;
                        var ee = 10;
                        var ff = 10;
                        var gg = 10;
                        while(ee > 10)
                        {
                            if(a > 10)
                            {
                                if(b > 16)
                                {
                                    if(c > 161)
                                    {
                                        if(d > 99)
                                        {
                                            e = 51;
                                            ee++;
                                        }
                                    }
                                }
                            }
                        }
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
                        var a121 = 10;
                        var b121 = 10;
                        var c121 = 10;
                        var d121 = 10;
                        var e121 = 10;
                        var f121 = 10;
                        var g121 = 10;
                        var h121 = 10;
                        var j121 = 10;
                        var k121 = 10;
                        var l121 = 10;
                        var aa51 = 10;
                        var bb51 = 10;
                        var cc51 = 10;
                        var dd51 = 10;
                        var ee51 = 10;
                        var ff51 = 10;
                        var gg51 = 10;
                        var hh51 = 10;
                        var jj51 = 10;
                        var kk51 = 10;
                        var ll51 = 10;
                        var mm51 = 10;
                        var nn51 = 10;
                        var oo51 = 10;
                        var qq51 = 10;
                        var rr51 = 10;
                        var zz51 = 10;
                        var tt51 = 10;
                        if(qq > 10)
                        {
                            if(x > 16)
                            {
                                if(qq51 > 161)
                                {
                                    if(zz51 > 99)
                                    {
                                        tt = 51;
                                    }
                                }
                            }
                        }
                    }    
                }
            }";

            var expected = VerifyCS.Diagnostic("MA01").WithLocation(0).WithArguments("MethodName");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);

        }
    }
}
