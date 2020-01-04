using System;
using Inw.Logger;
using Microsoft.Build.Locator;

namespace Inw.ArgumentExtractor.MSBuildLocator
{
    public class SelectVsInstanceOnCli : ISelectionHandler
    {
        private readonly IDoLog _logger;

        public SelectVsInstanceOnCli(IDoLog logger)
        {
            _logger = logger ?? new NullLogger();
        }
        
        /// <summary>
        /// When multiple VisualStudio instances are found it uses the supplied logger to print information about found instances. Then waits for console input to select one.
        /// </summary>
        /// <param name="visualStudioInstances"></param>
        /// <returns></returns>
        public VisualStudioInstance SelectVisualStudioInstance(VisualStudioInstance[] visualStudioInstances)
        {
            if (visualStudioInstances.Length == 1)
                return visualStudioInstances[0];
            
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
    }
}
