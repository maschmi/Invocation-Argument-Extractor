using System;
using System.IO;
using ArgumentExtractionCore;
using Inw.Logger;
using Microsoft.CodeAnalysis.MSBuild;

namespace Inw.ArgumentExtractor.MSBuildLocator.Core
{
    public class WorkspaceCreator : IWorkspaceCreator
    {
        private readonly IDoLog _logger;
        private readonly IMsBuildLocator _locator;
        private readonly string _hintPath;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hintPath">Overwrites using of default SDK paths to search for dotNETCore SDKs.</param>
        /// <param name="logger">Implementation of a logger according to the IDoLog interface, defaults to NullLogger.</param>
        /// <param name="msBuildLocator">Supply your own search logic to locate MSBuild.dll by implementing the IMsBuildLocator interface.</param>
        public WorkspaceCreator(string hintPath = "", IDoLog logger = null, IMsBuildLocator msBuildLocator = null)
        {
            _logger = logger ?? new NullLogger();
            _locator = msBuildLocator ?? new MsBuildLocator();
            _hintPath = hintPath;
        }
        
        /// <summary>
        /// Searches for the latest dotNetCore SDK and extracts the MSBuild.dll path.<br/>
        /// After registering it a environment variable "MSBUILD_EXE_PATH" a MSBuildWorkspace is created.<br/>
        /// If no hintPath was supplied at construction the default directories will be searched.
        /// </summary>
        /// <returns></returns>
        public MSBuildWorkspace CreateWorkspace()
        {
            var msBuildPath = _locator.LocateMsBuild(_hintPath);
            
            if (!(msBuildPath.EndsWith("MSBuild.dll", StringComparison.InvariantCultureIgnoreCase)))
                msBuildPath = Path.Combine(msBuildPath, "MSBuild.dll");

            _logger?.Info("Using MSBuild at " + msBuildPath);
            Environment.SetEnvironmentVariable("MSBUILD_EXE_PATH", msBuildPath);
            
            return MSBuildWorkspace.Create();
        }

        
    }
}
