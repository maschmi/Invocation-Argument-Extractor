using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inw.TestData
{
    public class TestClass2
    {
        public TestClass2()
        {

        }
        
        //#####do not move, this is for positional tests (this is line 16)
        private void LocationMethod()
        {
            var testclass = new TestClass();
            testclass.LocationTest(19);
        }
        //##### end - do not move(this is line 22)
        
        public void TestMethod1()
        {
            Console.WriteLine("This is a test in class 2");
            var test = new Dictionary<string,string>();
            test.TryGetValue("test", out string result);
        }

        public void TestMethod2()
        {
            Console.WriteLine("This is another test in class 2");
            Console.WriteLine(2.5f);
        }

        public void TestInLambda()
        {
            var testClass = new TestClass();
            IEnumerable<int> myEnumerable = new List<int>();
            IEnumerable<int> something = myEnumerable.Select(e => e + testClass.FunctionInLambda(5));
        }
        
        public void ParamsMethod(params int[] vs)
        {
            //do nothing
        }
    }
}
