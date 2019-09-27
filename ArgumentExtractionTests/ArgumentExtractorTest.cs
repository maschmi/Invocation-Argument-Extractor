using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
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
            var symbolToUse = (await GetTestSymbolForLocation()).FirstOrDefault();
            var sut = new InvocationArgumentExtractor(_logger);
            
            var result = await sut.FindArguments(symbolToUse, _solution);
            
            result.Should().HaveCount(1);
            var firstResult = result.First();
            var location = firstResult.FilePosition;
            location.line.Should().Be(22);
            location.col.Should().Be(23);
        }

        private async Task<IEnumerable<ISymbol>> GetTestSymbolForLocation()
        {
            return await _symbolExtractor.FindSymbols(_solution,
                typeof(TestClass).FullName,
                nameof(TestClass.LocationTest),
                new[] {"int"});
        }
        
        [Test]
        public async Task ArgumentExtractor_SymbolFoundInLambda_ReportsCorrectArguments()
        {
            var symbolToUse = (await GetTestSymbolLambdaInSelect()).FirstOrDefault();
            var sut = new InvocationArgumentExtractor(_logger);
            
            var result = await sut.FindArguments(symbolToUse, _solution);

            result.Should().HaveCount(1);
            result.First().Arguments.FirstOrDefault()?.ToString().Should().Be("5");
        }
        
        private async Task<IEnumerable<ISymbol>> GetTestSymbolLambdaInSelect()
        {
            return await _symbolExtractor.FindSymbols(_solution,
                typeof(TestClass).FullName,
                nameof(TestClass.FunctionInLambda),
                new[] {"int"});
        }
    }
}
