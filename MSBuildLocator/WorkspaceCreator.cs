using System;
using System.Linq;
using ArgumentExtractionCore;
using Inw.Logger;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
    
namespace Inw.ArgumentExtractor.MSBuildLocator
{
    public class WorkspaceCreator : IWorkspaceCreator
    {
        private readonly ISelectionHandler _selectionHandler;
        private readonly IDoLog _logger;

        /// <summary>
        /// Uses Microsoft.Build.Locator.MSBuildLocator to locate VisualStudio instances and subsequently the MSBuild executable. Then creates a MSBuildWorkspace.
        /// </summary>
        /// <param name="logger">Logger implementing IDoLog</param>
        /// <param name="selectionHandler">Used when multiple VisualStudio instances are found. Defaults to TakeLastVsInstance.<br/>
        /// If you want to ask the user use SelectVsInstanceOnCli or provide your own implementation.</param>
        public WorkspaceCreator(IDoLog logger = null, ISelectionHandler selectionHandler = null)
        {
            var _ = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions); //hack needed to copy Microsoft.CodeAnalysis.CSharp.Workspaces 
            _selectionHandler = selectionHandler ?? new TakeLastVsInstance();
            _logger = logger ?? new NullLogger();
        }
        
        /// <summary>
        /// Creates a MSBuld workspace using the MSBuild found by Microsoft.Build.Locator.MSBuildLocator.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="VisualStudioNotFoundException">No installed VisualStudio or MSBuildTools were found.</exception>
        public MSBuildWorkspace CreateWorkspace()
        {
            var instance = GetVsInstance();
            
            _logger.Info($"Using MSBuild at '{instance.MSBuildPath}' to load projects.");

            // NOTE: Be sure to register an instance with the MSBuildLocator 
            //       before calling MSBuildWorkspace.Create()
            //       otherwise, MSBuildWorkspace won't MEF compose.
            if(!Microsoft.Build.Locator.MSBuildLocator.IsRegistered)
                Microsoft.Build.Locator.MSBuildLocator.RegisterInstance(instance);
            
            return MSBuildWorkspace.Create();
        }
        
        private VisualStudioInstance GetVsInstance()
        {
            var instances = Microsoft.Build.Locator.MSBuildLocator.QueryVisualStudioInstances().ToArray();
            
            if (instances.Length == 0)
                throw new VisualStudioNotFoundException();

            if (instances.Length == 1)
                return instances[0];

            return _selectionHandler.SelectVisualStudioInstance(instances);
        }

    }
}
