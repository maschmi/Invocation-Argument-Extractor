using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Inw.Logger;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.FindSymbols;

namespace Inw.ArgumentExtraction.Finder
{
    internal sealed class SymbolExtractorWithSymbolFinder : ISymbolExtractor
    {
        private readonly IDoLog _logger;

        public SymbolExtractorWithSymbolFinder(IDoLog logger)
        {
            _logger = logger ?? new NullLogger();
        }                

        public async Task<IEnumerable<ISymbol>> FindSymbols(Solution solution, string fullTypeName, string methodName, string[] methodParameterTypes)
        {
            if (solution == null)
                throw new ArgumentNullException(nameof(solution));

            if (string.IsNullOrWhiteSpace(fullTypeName))
                throw new InvalidOperationException(nameof(fullTypeName));

            return await AnalyzeSolution(solution, fullTypeName, methodName, methodParameterTypes);
        }

        private async Task<IEnumerable<ISymbol>> AnalyzeSolution(Solution solution, string fullTypeName, string methodName, string[] methodParameterTypes)
        {
            var projects = solution.Projects;
            _logger.Verbose("Starting hunt for symbol");

            var symbols = new List<ISymbol>();
            foreach (var project in projects)
            {
                var result = await SearchSymbols(project, fullTypeName, methodName, methodParameterTypes);
                if (result.Any())
                {
                    _logger.Verbose("Found something");
                    symbols.AddRange(result);
                }
                else
                    _logger.Verbose("Found nothing");
            }

            return symbols;
        }

        private async Task<IEnumerable<ISymbol>> SearchSymbols(Project project, string fullTypeName, string methodName, string[] methodParameterTypes)
        {
            _logger.Verbose("Hunting in " + project.Name);

            IEnumerable<ISymbol> typeSymbols = await SymbolFinder.FindDeclarationsAsync(project, methodName, false);
            if (typeSymbols?.Any() ?? false)
                return FindFirstMethod(typeSymbols, fullTypeName, methodParameterTypes);
            else
                _logger.Verbose("Did not find anything");
            return new ISymbol[0];
        }

        private IEnumerable<ISymbol> FindFirstMethod(IEnumerable<ISymbol> typeSymbols, string fullTypeName, string[] methodParameterTypes)
        {            
            List<ISymbol> result = new List<ISymbol>();
            foreach (var symbol in typeSymbols)
            {                
                if (!symbol.ContainingType.ToDisplayString().Equals(fullTypeName))
                    continue;

                var parts = symbol
                    .ToDisplayParts()
                    .Where(p => p.Kind == SymbolDisplayPartKind.Keyword);

                if (parts.Count() != methodParameterTypes.Count())
                    continue;

                if (parts.Select(p => p.Symbol.ToString()).SequenceEqual(methodParameterTypes))
                    result.Add(symbol);
            }

            return result;
        }
    }
}
