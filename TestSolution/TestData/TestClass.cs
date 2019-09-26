using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inw.TestData
{
    public class TestClass
    {
        public TestClass()
        {

        }
        
        public void LocationTest(int parameter)
        {}
            
        public void TestMethod1()
        {
            Console.WriteLine("This is a test");
            var test = new Dictionary<string,string>();
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
    }
}
