using Inw.Logger;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.IO;
using System.Threading.Tasks;
using ArgumentExtractionCore;

namespace Inw.ArgumentExtraction.Loader
{
    public sealed class SolutionLoader : IDisposable
    {
        private readonly IDoLog _logger;
        private readonly IWorkspaceCreator _workspaceCreator;
        private MSBuildWorkspace _workspace;
        
        public SolutionLoader(IWorkspaceCreator workspaceCreator, IDoLog logger = null)
        {
            var _ = typeof(Microsoft.CodeAnalysis.CSharp.SymbolDisplay);
            _workspaceCreator = workspaceCreator;

            if (logger == null)
                logger = new NullLogger();
            _logger = logger;
        }

        public async Task<Solution> LoadSolution(string pathToSolution)
        {
            _workspace = _workspaceCreator.CreateWorkspace();
            return await LoadSolutionImpl(pathToSolution);
        }

        private async Task<Solution> LoadSolutionImpl(string pathToSolution)
        {
            // Print message for WorkspaceFailed event to help diagnosing project load failures.
            _workspace.WorkspaceFailed += (o, e) => Console.WriteLine(e.Diagnostic.Message);

            var solutionPath = pathToSolution;
            _logger.Info($"Loading solution '{solutionPath}'");

            // Attach progress reporter so we print projects as they are loaded.
            var solution = await _workspace.OpenSolutionAsync(solutionPath, new ConsoleProgressReporter(_logger));
            _logger.Info($"Finished loading solution '{solutionPath}'");

            return solution;
        }
        

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls
        

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _workspace?.Dispose();
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
