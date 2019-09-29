using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Inw.TestData
{
    [ExcludeFromCodeCoverage]
    public class SymbolProvidingTestClass
    {
        public SymbolProvidingTestClass()
        {

        }

        public void LocationTest(int parameter)
        { }

        public void TestMethod1()
        {
            Console.WriteLine("This is a test");
            var test = new Dictionary<string, string>();
            test.TryGetValue("test", out string result);
        }

        public void TestMethod2()
        {
            Console.WriteLine("This is another test");
            Console.WriteLine(2.5f);
        }

        public int FunctionInLambda(int i)
        {
            //do nothing
            return default;
        }

        public bool FunctionInConstructor(params bool[] parameters)
        {
            //do nothing
            return default;
        }
    }
}
