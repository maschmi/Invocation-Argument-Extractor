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
    }
}
