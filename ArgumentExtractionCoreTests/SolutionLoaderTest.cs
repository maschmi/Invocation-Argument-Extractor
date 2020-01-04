using FluentAssertions;
using Inw.ArgumentExtraction.Loader;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Inw.ArgumentExtractor.MSBuildLocator.Core;

namespace Inw.ArgumentExtractionTests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class SolutionLoaderTest
    {
        private static string _pathToTestSolution = TestDataLocator.CalculateTestDataSolutionPath();

        [Test]
        public async Task SolutionLoader_FileIsFound_SolutionIsLoaded()
        {
            using (var solLoader = new SolutionLoader(new WorkspaceCreator(),null))
            {
                var solution = await solLoader.LoadSolution(_pathToTestSolution);
                solution.Projects.Select(p => p.Name).Should().BeEquivalentTo(new[] { "TestDataCore" });
            }
        }
    }
}
