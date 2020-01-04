using System.Diagnostics.CodeAnalysis;

namespace Inw.TestData
{
    [ExcludeFromCodeCoverage]
    public class TestClass3
    {
        private bool _active;

        public TestClass3(bool active)
        {
            _active = active;
        }
    }
}
