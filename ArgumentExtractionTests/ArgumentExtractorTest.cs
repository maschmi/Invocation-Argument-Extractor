using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Xml.XPath;
using FluentAssertions;
using Inw.ArgumentExtraction.DTO;
using Inw.ArgumentExtraction.Extractors;
using Inw.ArgumentExtraction.Finder;
using Inw.ArgumentExtraction.Loader;
using Inw.Logger;
using Inw.TestData;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using NUnit.Framework.Internal;

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
            _solutionLoader = new SolutionLoader(_logger, 0);
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
        [TestCase(nameof(SymbolProvidingTestClass.FunctionInLambda), "5")]
        [TestCase(nameof(SymbolProvidingTestClass.FunctionInConstructor), "new [] {true, false}")]
        public async Task ArgumentExtractor_NestedInvoocations_ReportsCorrectArguments(string methodName,
            string expectedaArgument)
        {
            var symbolToUse = await GetTestSymbolFor(methodName);
            var sut = new InvocationArgumentExtractor(_logger);

            var result = await sut.FindArguments(symbolToUse, _solution);

            result.Should().HaveCount(1);
            var arguments = result.First().Arguments;
            arguments.Should().HaveCount(1);
            arguments.First().ToString().Should().Be(expectedaArgument);
        }

        private async Task<ISymbol> GetTestSymbolFor(string methodName)
        {
            var methodParameterMapping = new Dictionary<string, string[]>()
            {
                {nameof(SymbolProvidingTestClass.FunctionInLambda), new[] {"int"}},
                {nameof(SymbolProvidingTestClass.FunctionInConstructor), new[] {"params", "bool"}},
                {nameof(SymbolProvidingTestClass.LocationTest), new[] {"int"}}
            };

            return (await _symbolExtractor.FindSymbols(_solution,
                    typeof(SymbolProvidingTestClass).FullName,
                    methodName,
                    methodParameterMapping[methodName]))
                .FirstOrDefault();
        }
    }
}