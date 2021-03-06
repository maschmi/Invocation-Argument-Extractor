<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
=======
﻿using FluentAssertions;
using Inw.ArgumentExtraction.Loader;
using NUnit.Framework;
>>>>>>> moving-to-netcore
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

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
            using (var solLoader = new SolutionLoader(null, 1))
            {
                var solution = await solLoader.LoadSolution(_pathToTestSolution);
                solution.Projects.Select(p => p.Name).Should().BeEquivalentTo(new[] { "TestDataCore" });
            }
        }
    }
}
