using FluentAssertions;
using Inw.ArgumentExtraction.Extractors;
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
    public class ArgumentExtractorTest
    {
        private static NullLogger _logger;
        private SolutionLoader _solutionLoader;
        private Solution _solution;
        private SymbolExtractorWithCompilation _symbolExtractor;

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            _logger = new NullLogger();
            _solutionLoader = new SolutionLoader(new WorkspaceCreator(logger: _logger),  _logger);
            _solution = await _solutionLoader.LoadSolution(TestDataLocator.CalculateTestDataSolutionPath());
            _symbolExtractor = new SymbolExtractorWithCompilation(_logger);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _solutionLoader.Dispose();
        }

        [Test]
        public async Task ArgumentExtractor_SymbolFound_ReportsCorrectLocation()
        {
            var symbolToUse = await GetTestSymbolFor("LocationTest");
            var sut = new InvocationArgumentExtractor(_logger);

            var result = await sut.FindArguments(symbolToUse, _solution);

            result.Should().HaveCount(1);
            var firstResult = result.First();
            var location = firstResult.FilePosition;
            location.line.Should().Be(22);
            location.col.Should().Be(23);
        }

        [Test]
        [TestCase("FunctionInLambda", "5")]
        [TestCase("FunctionInConstructor", "new[] { true, false }")]
        public async Task ArgumentExtractor_NestedInvocations_ReportsCorrectArguments(string methodName,
            string expectedArgument)
        {
            var symbolToUse = await GetTestSymbolFor(methodName);
            var sut = new InvocationArgumentExtractor(_logger);

            var result = await sut.FindArguments(symbolToUse, _solution);

            result.Should().HaveCount(1);
            var arguments = result.First().Arguments;
            arguments.Should().HaveCount(1);
            arguments.First().ToString().Should().Be(expectedArgument);
        }

        private async Task<ISymbol> GetTestSymbolFor(string methodName)
        {
            var methodParameterMapping = new Dictionary<string, string[]>()
            {
                {"FunctionInLambda", new[] {"int"}},
                {"FunctionInConstructor", new[] { "params", "bool" }},
                {"LocationTest", new[] {"int"}}
            };

            return (await _symbolExtractor.FindSymbols(_solution,
                    "Inw.TestData.SymbolProvidingTestClass",
                    methodName,
                    methodParameterMapping[methodName]))
                .FirstOrDefault();
        }
    }
}
