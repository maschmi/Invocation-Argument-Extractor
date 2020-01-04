using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Inw.ArgumentExtractor.MSBuildLocator.Core
{
    internal sealed class MsBuildLocator : IMsBuildLocator
    {
        public string LocateMsBuild(params string[] additionSearchPaths)
        {
            List<string> hintPaths = new List<string>();
            hintPaths = GuessHintPathsByOs().ToList();
            if(additionSearchPaths != null)
                hintPaths.AddRange(additionSearchPaths);

            string path = null;
            foreach (var  hintPath in hintPaths)
            {
                var results = SearchForMsBuild(hintPath);

                path = results
                    .Where(p => !p.Contains("nuget", StringComparison.InvariantCultureIgnoreCase))
                    .Where(p => !p.Contains("ref", StringComparison.InvariantCultureIgnoreCase))
                    .OrderByDescending(p => p).FirstOrDefault();

                if (path != null)
                    break;
            }
            
            if (string.IsNullOrWhiteSpace(path))
                throw new MsBuildNotFoundException($"MSBuild.dll/MSBuild.exe not found under {hintPaths}");

            return path;
        }

        private IEnumerable<string> SearchForMsBuild(string hintPath)
        {
            var results =
                Directory.EnumerateFiles(hintPath, "msbuild.dll", SearchOption.AllDirectories);
            if (!results.Any() && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                results = Directory.EnumerateFiles(hintPath, "msbuild.exe", SearchOption.AllDirectories);
            return results;
        }

        private IEnumerable<string> GuessHintPathsByOs()
        {
            var linux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            var windows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var osx = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

            if (linux)
                return new [] {"/usr/share/dotnet", "/etc", "/usr", "/opt"}; //from specific to unspecific
            if (windows)
                return new [] { @"C:\Program Files\dotnet" };
            if (osx)
                return new [] { "/usr/local/share/dotnet/dotnet" };
            
            throw new NotImplementedException("unknown os");
        }
    }
}    
