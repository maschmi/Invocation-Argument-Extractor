using Microsoft.CodeAnalysis.MSBuild;

namespace ArgumentExtractionCore
{
    public interface IWorkspaceCreator
    {
        MSBuildWorkspace CreateWorkspace();
    }
}
