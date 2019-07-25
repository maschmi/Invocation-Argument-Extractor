using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inw.Logger;
using Microsoft.CodeAnalysis;

namespace Inw.ArgumentExtraction.Finder
{
    internal class SymbolExtractorWithCompilation : ISymbolExtractor
    {
        private readonly IDoLog _logger;

        public SymbolExtractorWithCompilation(IDoLog logger)
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

        private async Task<IEnumerable<ISymbol>> AnalyzeSolution(Solution solution, string fullName, string methodName, string[] methodParameterTypes)
        {
            var projects = solution.Projects;
            _logger.Verbose("Starting hunt for symbol");

            var symbols = new List<ISymbol>();
            foreach (var project in projects)
            {
                var result = await SearchSymbols(project, fullName, methodName, methodParameterTypes);
                if (result != null)
                {
                    _logger.Verbose("Found something");
                    symbols.AddRange(result);
                }
                else
                    _logger.Verbose("Found nothing");
            }

            return symbols;
        }

        private async Task<IEnumerable<ISymbol>> SearchSymbols(Project project, string fullName, string methodName, string[] methodParameterTypes)
        {
            _logger.Verbose("Hunting in " + project.Name);
            var compilation = await project.GetCompilationAsync();
            var typeSymbol = compilation.GetTypeByMetadataName(fullName);
            if (typeSymbol != null)
                return FindMethod(typeSymbol, methodName, methodParameterTypes);
            else
                _logger.Verbose("Did not find anything");
            return new ISymbol[0];
        }

        private IEnumerable<ISymbol> FindMethod(INamedTypeSymbol typeSymbol, string methodName, string[] methodParameterTypes)
        {
            var members = typeSymbol.GetMembers(methodName);
            List<ISymbol> result = new List<ISymbol>();
            foreach (var member in members)
            {
                var parts = member
                    .ToDisplayParts()
                    .Where(p => p.Kind == SymbolDisplayPartKind.Keyword);

                if (parts.Count() != methodParameterTypes.Count())
                    continue;

                if (parts.Select(p => p.ToString()).SequenceEqual(methodParameterTypes))
                    result.Add(member);
            }

            return result;
        }
    }
}
