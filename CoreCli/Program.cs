using Inw.ArgumentExtraction.DTO;
using Inw.ArgumentExtraction.Extractors;
using Inw.ArgumentExtraction.Finder;
using Inw.ArgumentExtraction.Loader;
using Inw.Logger;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inw.ArgumentExtractor.MSBuildLocator.Core;

namespace Inw.ArgumentExtractionCli
{
    public class Program
    {
        private static IDoLog _logger;

        static async Task Main(string[] args)
        {
            try
            {
                var options = new ProgramOptions(args);
                if (options.HelpPrinted)
                    return;

                await RunAnalysis(options);
            }
            catch (InvalidOperationException ex)
            {
                var logger = new ConsoleLogger();
                if (ex.Message != null)
                    logger.Error(ex.Message);
            }

        }

        private static async Task RunAnalysis(ProgramOptions options)
        {
            _logger = new ConsoleLogger(verbose: options.VerboseLogging, debug: options.DebugLogging);

            using var solLoader = new SolutionLoader(new WorkspaceCreator(logger: _logger), _logger);
            
            var solution = await solLoader.LoadSolution(options.PathToSolution);

            var extractor = new SymbolExtractorWithCompilation(_logger);
            var declaredSymbols = await extractor.FindSymbols(solution, options.FullTypeName, options.MethodName, options.MethodArguments);
            if(declaredSymbols.Count() == 0)
                _logger.Info($"Did not find anything for {options.FullTypeName}.{options.MethodName}({string.Join(", ", options.MethodArguments)})");
            await ArgumentExtraction(declaredSymbols, solution, options);
        }

        private static async Task ArgumentExtraction(IEnumerable<ISymbol> declaredSymbols, Solution solution, ProgramOptions options)
        {
            var argumentExtractor = new InvocationArgumentExtractor(_logger);

            foreach (var symbol in declaredSymbols)
            {
                var argumentResults = await argumentExtractor.FindArguments(symbol, solution);
                _logger.Info("Reporting location and arguments for calls to " + symbol.ToDisplayString());
                foreach (var result in argumentResults.OrderBy(r => r.FilePath))
                {
                    PrintResult(result);
                }
            }
        }

        private static void PrintResult(ArgumentResults result)
        {
            var file = result.FilePath;
            var line = result.FilePosition.line;

            var arguments = result.Arguments.Select(a => a.ToString());
            _logger.Info($"Invocation in {file} at {line} using arguments:");
            _logger.Info(string.Join(" ; ", arguments));
        }
    }
}
