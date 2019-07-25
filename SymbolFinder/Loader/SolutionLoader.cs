using System;
using System.Linq;
using System.Threading.Tasks;
using Inw.Logger;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace Inw.ArgumentExtraction.Loader
{
    public class SolutionLoader : IDisposable
    {
        private readonly MSBuildWorkspace _workspace;
        private readonly IDoLog _logger;

        public SolutionLoader(IDoLog logger, int? defaultVSInstance = null)
        {
            _logger = logger ?? new NullLogger();
            
            // Attempt to set the version of MSBuild.
            var visualStudioInstances = MSBuildLocator.QueryVisualStudioInstances().ToArray();
            var instance = GetInstance(visualStudioInstances, defaultVSInstance);
            
            _logger.Info($"Using MSBuild at '{instance.MSBuildPath}' to load projects.");

            // NOTE: Be sure to register an instance with the MSBuildLocator 
            //       before calling MSBuildWorkspace.Create()
            //       otherwise, MSBuildWorkspace won't MEF compose.
            if(!MSBuildLocator.IsRegistered)
                MSBuildLocator.RegisterInstance(instance);
            _workspace = MSBuildWorkspace.Create();
        }

        private VisualStudioInstance GetInstance(VisualStudioInstance[] visualStudioInstances, int? defaultVSInstance)
        {
            if (visualStudioInstances.Length == 1)
                // If there is only one instance of MSBuild on this machine, set that as the one to use.
                return visualStudioInstances[0];

            else if (defaultVSInstance != null)
                return visualStudioInstances[defaultVSInstance.Value];
            
                // Handle selecting the version of MSBuild you want to use.
            return SelectVisualStudioInstance(visualStudioInstances);
        }

        public async Task<Solution> LoadSolution(string pathToSln)
        {
            // Print message for WorkspaceFailed event to help diagnosing project load failures.
            _workspace.WorkspaceFailed += (o, e) => Console.WriteLine(e.Diagnostic.Message);

            var solutionPath = pathToSln;
            _logger.Info($"Loading solution '{solutionPath}'");

            // Attach progress reporter so we print projects as they are loaded.
            var solution = await _workspace.OpenSolutionAsync(solutionPath, new ConsoleProgressReporter(_logger));
            _logger.Info($"Finished loading solution '{solutionPath}'");

            return solution;
        }

        private VisualStudioInstance SelectVisualStudioInstance(VisualStudioInstance[] visualStudioInstances)
        {
            _logger.Warning("Multiple installs of MSBuild detected please select one:");
            for (int i = 0; i < visualStudioInstances.Length; i++)
            {
                _logger.Warning($"Instance {i + 1}");
                _logger.Warning($"    Name: {visualStudioInstances[i].Name}");
                _logger.Warning($"    Version: {visualStudioInstances[i].Version}");
                _logger.Warning($"    MSBuild Path: {visualStudioInstances[i].MSBuildPath}");
            }
            
            while (true)
            {
                var userResponse = Console.ReadLine();
                if (int.TryParse(userResponse, out int instanceNumber) &&
                    instanceNumber > 0 &&
                    instanceNumber <= visualStudioInstances.Length)
                {
                    return visualStudioInstances[instanceNumber - 1];
                }
                _logger.Info("Input not accepted, try again.");
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _workspace.Dispose();                    
                    MSBuildLocator.Unregister();
                }

                
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
