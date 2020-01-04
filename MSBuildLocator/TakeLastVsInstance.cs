using System.Linq;
using Microsoft.Build.Locator;

namespace Inw.ArgumentExtractor.MSBuildLocator
{
    public class TakeLastVsInstance : ISelectionHandler
    {
        public VisualStudioInstance SelectVisualStudioInstance(VisualStudioInstance[] visualStudioInstances)
        {
            return visualStudioInstances.Last();
        }
    }
}
