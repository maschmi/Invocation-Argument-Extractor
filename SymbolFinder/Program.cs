using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Inw.Logger;
using Microsoft.CodeAnalysis;
using Inw.ArgumentExtraction.DTO;
using Inw.ArgumentExtraction.Extractors;
using Inw.ArgumentExtraction.Finder;
using Inw.ArgumentExtraction.Loader;

namespace Inw.ArgumentExtraction
{
    public class Program
    {
        private static string _pathToTestSolution = @"C:\Users\martin\source\repos\SymbolFinder\TestSolution\TestSolution.sln";
        private static ConsoleLogger _logger;

        static async Task Main(string[] args)
        {
            _logger = new ConsoleLogger(verbose: true);
            

            using (var solLoader = new SolutionLoader(_logger, 1))
            {
                var solution = await solLoader.LoadSolution(_pathToTestSolution);

                _logger.Verbose("Running with Compilation");
            
                await Run(solution, new SymbolExtractorWithCompilation(_logger));          
            }
        }

        private async static Task Run(Solution solution, ISymbolExtractor symbolExtractor)
        {
            var argumentExtractor = new InvocationArgumentExtractor(_logger);

            var declaredSymbol = await symbolExtractor.FindSymbols(solution, "Inw.TestData.TestClass2", "ParamsMethod", new[] { "params", "int" });

            foreach (var symbol in declaredSymbol)
            {
                var argumentResults = await argumentExtractor.FindArguments(symbol, solution);
                _logger.Info("Reporting location and arguments for calls to " + symbol.ToDisplayString());
                foreach (var result in argumentResults.OrderBy(r => r.Location.Document.FilePath))
                {
                    PrintResult(result);
                }
            }

        }


        private static void PrintResult(ArgumentResults result)
        {
            var file = result.Location.Document.FilePath;
            var line = result.Location.Location.GetLineSpan().StartLinePosition;

            var arguments = result.Arguments.Select(a => a.ToString());
            _logger.Info($"Invocation in {file} at {line} using arguments:");
            _logger.Info(string.Join(" ; ", arguments));
        }
    }
}
