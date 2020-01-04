using Microsoft.Build.Locator;

namespace Inw.ArgumentExtractor.MSBuildLocator
{
    public interface ISelectionHandler
    {
        VisualStudioInstance SelectVisualStudioInstance(VisualStudioInstance[] visualStudioInstances);
    }
}
