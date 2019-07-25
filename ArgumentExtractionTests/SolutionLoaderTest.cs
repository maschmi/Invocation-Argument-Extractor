using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using Inw.ArgumentExtraction.Loader;

namespace Inw.ArgumentExtractionTests
{
    [TestFixture]
    public class SolutionLoaderTest
    {
        private static string _pathToTestSolution = TestDataLocator.CalculateTestDataSolutionPath();

        [Test]
        public async Task SolutionLoader_FileIsFound_SolutionIsLoaded()
        {            
            using(var solLoader = new SolutionLoader(null, 1))
            {
                var solution = await solLoader.LoadSolution(_pathToTestSolution);
                solution.Projects.Select(p => p.Name).Should().BeEquivalentTo(new[] { "TestData" });
                
            }
        }
    }
}
