using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Inw.TestData
{
    [ExcludeFromCodeCoverage]
    public class TestClass2
    {
        public TestClass2()
        {

        }

        
        
        //#####do not move, this is for positional tests (this is line 18)
        private void LocationMethod()
        {
            var testclass = new SymbolProvidingTestClass();
            testclass.LocationTest(19);
        }
        //##### end - do not move(this is line 24)

        public void TestMethod1()
        {
            Console.WriteLine("This is a test in class 2");
            var test = new Dictionary<string, string>();
            test.TryGetValue("test", out string result);
        }

        public void TestMethod2()
        {
            Console.WriteLine("This is another test in class 2");
            Console.WriteLine(2.5f);
        }

        public void TestInLambda()
        {
            var testClass = new SymbolProvidingTestClass();
            IEnumerable<int> myEnumerable = new List<int>();
            IEnumerable<int> something = myEnumerable.Select(e => e + testClass.FunctionInLambda(5));
        }

        public void TestInCtor()
        {
            var testClass = new SymbolProvidingTestClass();

            _ = new TestClass3(testClass.FunctionInConstructor(new[] { true, false }));
        }


        public void ParamsMethod(params int[] vs)
        {
            //do nothing
        }
    }
}
