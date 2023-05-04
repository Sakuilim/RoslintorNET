using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = Roslintor.Test.CSharpAnalyzerVerifier<
    Roslintor.Analyzers.MaintainabilityAnalyzers.Class.ClassMaintainabilityAnalyzer>;

namespace Roslintor.Tests.MaintainabilityAnalyzerTests.Class
{
    [TestClass]
    public class ClassMaintainabilityAnalyzerTests
    {
        [TestMethod]
        public async Task ClassMaintainabilityIndexAnalysis_Should_ReturnClassMaintainabilityIndexLevelGood()
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
                        var x = 1;
                    }    
                }
            }";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }
        [TestMethod]
        public async Task ClassMaintainabilityIndexAnalysis_Should_ReturnClassMaintainabilityIndexLevelTooLow()
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
                public class {|#0:TestClass|}
                {   
                    public void MethodName(string name)
                    {
                        var x = 10;
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
                        var c121 = 10;
                        var d121 = 10;
                        var e121 = 10;
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
                    public void MethodName2(string name)
                    {
                        var x = 0;
                        var k = 33;
                        if (x > k)
                        {
                            while(x > 0)
                            {
                                x = 63;
                            }
                        }
                        if(k > x)
                        {
                            for(int i=0;i<x; i++)
                            {
                                k = 4;
                            }
                        }
                        if(k == x)
                        {
                            while (k < 0)
                            {
                                x++;
                            }
                        }
                    }   
                    public void MethodName3(string name)
                    {
                        var x = 0;
                        var k = 223;
                        if (x > k)
                        {
                            while(x > 0)
                            {
                                x = 215;
                            }
                        }
                        if(k > x)
                        {
                            for(int i=0;i<x; i++)
                            {
                                k = 4;
                            }
                        }
                        if(k == x)
                        {
                            while (k < 0)
                            {
                                x++;
                            }
                        }
                    }    
                    public void MethodName4(string name)
                    {
                        var x = 0;
                        var k = 334;
                        if (x > k)
                        {
                            while(x > 0)
                            {
                                x = 632;
                            }
                        }
                        if(k > x)
                        {
                            for(int i=0;i<x; i++)
                            {
                                k = 4;
                            }
                        }
                        if(k == x)
                        {
                            while (k < 0)
                            {
                                x++;
                            }
                        }
                    } 
                }
            }";

            var expected = VerifyCS.Diagnostic("MA002").WithLocation(0).WithArguments("TestClass");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);

        }
    }
}
