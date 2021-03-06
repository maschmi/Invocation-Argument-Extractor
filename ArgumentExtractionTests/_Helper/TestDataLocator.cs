<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
=======
﻿using System.Diagnostics.CodeAnalysis;
>>>>>>> moving-to-netcore
using System.IO;
using System.Reflection;

namespace Inw.ArgumentExtractionTests
{
    [ExcludeFromCodeCoverage]
    internal static class TestDataLocator
    {
        public static string CalculateTestDataSolutionPath(string solutionName = "TestSolution.sln")
        {
            var assembly = Assembly.GetExecutingAssembly();
            var index = assembly.Location.IndexOf(CalculateAssemblyShortName(assembly));
            var suffix = @"TestSolution";
            var solutionRoot = assembly.Location.Substring(0, index);
            return Path.Combine(solutionRoot, suffix, solutionName);
        }

        private static string CalculateAssemblyShortName(Assembly assembly)
        {
            return assembly.GetName().Name;
        }
    }
}
