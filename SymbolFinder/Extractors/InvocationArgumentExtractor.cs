using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inw.Logger;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Inw.ArgumentExtraction.DTO;

namespace Inw.ArgumentExtraction.Extractors
{
    public sealed class InvocationArgumentExtractor
    {
        private readonly IDoLog _logger;

        public InvocationArgumentExtractor(IDoLog logger)
        {
            _logger = logger ?? new NullLogger();
        }

        public async Task<IEnumerable<ArgumentResults>> FindArguments(ISymbol symbol, Solution solution)
        {
            _logger.Debug($"Finding refernces for {symbol.ToString()} in {solution.FilePath}");
            var references = await SymbolFinder.FindReferencesAsync(symbol, solution);

            var results = new List<ArgumentResults>();
            foreach (var reference in references)                
                    results.AddRange(await ExtractArguments(reference));

            return results;
        }

        private async Task<IEnumerable<ArgumentResults>> ExtractArguments(ReferencedSymbol reference)
        {
            var result = new List<ArgumentResults>();
            foreach (var location in reference.Locations)
                result.Add(await CalculateArgumentResults(location));

            return result;
        }

        private async Task<ArgumentResults> CalculateArgumentResults(ReferenceLocation location)
        {
            var result = new ArgumentResults(location);

            result.Arguments.AddRange(await CalculateArguments(location));

            return result;
        }

        private async Task<IEnumerable<ArgumentSyntax>> CalculateArguments(ReferenceLocation location)
        {
            IEnumerable<SyntaxNode> nodes = await CalculateInvocationSyntaxNodes(location);
            if (!nodes.Any())
            {
                _logger.Warning("Did not find and invocation node.");
                return new ArgumentSyntax[0];
            }
            if (nodes.Count() > 1)
                _logger.Warning("Got multiple invocation nodes using only the first!");

            return CalculateArguments(nodes.First());
        }
        

        private async Task<IEnumerable<SyntaxNode>> CalculateInvocationSyntaxNodes(ReferenceLocation location)
        {
            var syntaxTree = location.Location.SourceTree;
            var sourceSpan = location.Location.SourceSpan;

            var syntaxRoot = await syntaxTree.GetRootAsync();

            return syntaxRoot
                .DescendantNodes(sourceSpan)
                .Where(n =>
                        sourceSpan.IntersectsWith(n.Span)
                        && n.IsKind(Microsoft.CodeAnalysis.CSharp.SyntaxKind.InvocationExpression));
                
        }

        private IEnumerable<ArgumentSyntax> CalculateArguments(SyntaxNode syntaxNode)
        {
            var invocationNode = syntaxNode as InvocationExpressionSyntax;
            if(invocationNode == null)
            {
                _logger.Warning("Syntax node is not an InvocationExpressionSyntax");
                return new ArgumentSyntax[0];
            }

            return invocationNode.ArgumentList.Arguments.ToArray();            
        }
    }
}
