using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = Roslintor.Test.CSharpAnalyzerVerifier<
    Roslintor.Analyzers.ComplexityAnalyzers.Class.ClassCyclomaticComplexityAnalyzer>;

namespace Roslintor.Tests.ComplexityAnalyzerTests
{
    [TestClass]
    public class ClassComplexityAnalyzerTests
    {
        [TestMethod]
        public async Task ClassCyclomaticComplexityAnalysis_Should_ReturnClassCyclomaticComplexityLevelGood()
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
        public async Task ClassCyclomaticComplexityAnalysis_Should_ReturnClassCyclomaticComplexityLevelTooHigh()
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
                    }    
                    public void MethodName1(string name)
                    {
                        var x = 0;
                        var k = 2;
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

            var expected = VerifyCS.Diagnostic("CA04").WithLocation(0).WithArguments("TestClass");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);

        }
    }
}
