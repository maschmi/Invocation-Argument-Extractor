using System;
using FluentAssertions;
using Inw.ArgumentExtraction.Finder;
using Inw.ArgumentExtraction.Loader;
using Inw.Logger;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Inw.ArgumentExtractor.MSBuildLocator.Core;

namespace Inw.ArgumentExtractionTests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal class SymbolExtractorTest
    {
        private static NullLogger _logger;
        private SolutionLoader _solutionLoader;
        private Solution _solution;

        private static IEnumerable<ISymbolExtractor> _extractors
        {
            get
            {
                yield return new SymbolExtractorWithCompilation(_logger);
                yield return new SymbolExtractorWithSymbolFinder(_logger);
            }
        }

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            _logger = new NullLogger();
            _solutionLoader = new SolutionLoader(new WorkspaceCreator(), _logger);
            _solution = await _solutionLoader.LoadSolution(TestDataLocator.CalculateTestDataSolutionPath());
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _solutionLoader.Dispose();
        }

        [TestCaseSource(nameof(_extractors))]
        public async Task Extractor_FindSystemConsoleWriteLineString_ReturnSymbol(ISymbolExtractor extractor)
        {
            var expectedSymbolString = "System.Console.WriteLine(string)";
            var result = await extractor.FindSymbols(_solution, "System.Console", "WriteLine", new[] { "string" });

            result.Should().HaveCount(1);
            var symbol = result.First();
            symbol.Kind.Should().Be(SymbolKind.Method);
            symbol.ToString().Should().Be(expectedSymbolString);
        }

        [TestCaseSource(nameof(_extractors))]
        public async Task Extractor_FindTestMethodWithParams_ReturnSymbol(ISymbolExtractor extractor)
        {
            var expectedSymbolString = "Inw.TestData.TestClass2.ParamsMethod(params int[])";
            var result = await extractor.FindSymbols(_solution, "Inw.TestData.TestClass2", "ParamsMethod", new[] { "params", "int" });

            result.Should().HaveCount(1);
            var symbol = result.First();
            symbol.Kind.Should().Be(SymbolKind.Method);
            symbol.ToString().Should().Be(expectedSymbolString);
        }
    }
}
