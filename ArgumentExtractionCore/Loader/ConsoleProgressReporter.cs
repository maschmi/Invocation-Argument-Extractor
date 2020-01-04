using Inw.Logger;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.IO;

namespace Inw.ArgumentExtraction.Loader
{
    internal class ConsoleProgressReporter : IProgress<ProjectLoadProgress>
    {
        private readonly IDoLog _logger;

        public ConsoleProgressReporter(IDoLog logger)
        {
            _logger = logger ?? new NullLogger();
        }

        public void Report(ProjectLoadProgress loadProgress)
        {
            var projectDisplay = Path.GetFileName(loadProgress.FilePath);
            if (loadProgress.TargetFramework != null)
            {
                projectDisplay += $" ({loadProgress.TargetFramework})";
            }

            _logger.Verbose($"{loadProgress.Operation,-15} {loadProgress.ElapsedTime,-15:m\\:ss\\.fffffff} {projectDisplay}");
        }
    }
}

