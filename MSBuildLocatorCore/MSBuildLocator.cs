using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Inw.ArgumentExtractor.MSBuildLocator.Core
{
    internal sealed class MsBuildLocator : IMsBuildLocator
    {
        private string _hintPath = string.Empty;

        public string LocateMsBuild(string hintPath = "")
        {
            if (string.IsNullOrWhiteSpace(_hintPath))
                _hintPath = GuessHintPathByOs();

            var results = SearchForMsBuild();

            var path = results
                .Where(p => !p.Contains("nuget", StringComparison.InvariantCultureIgnoreCase))
                .Where(p => !p.Contains("ref", StringComparison.InvariantCultureIgnoreCase))
                .OrderByDescending(p => p).FirstOrDefault();
            if (string.IsNullOrWhiteSpace(path))
                throw new MSBuildNotFoundException($"MSBuild.dll/MSBuild.exe not found under {_hintPath}");

            return path;
        }

        private IEnumerable<string> SearchForMsBuild()
        {
            var results =
                Directory.EnumerateFiles(_hintPath, "msbuild.dll", SearchOption.AllDirectories);
            if (!results.Any() && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                results = Directory.EnumerateFiles(_hintPath, "msbuild.exe", SearchOption.AllDirectories);
            return results;
        }

        private string GuessHintPathByOs()
        {
            var linux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            var windows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var osx = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

            if (linux)
                return "/etc";
            if (windows)
                return @"C:\Program Files\dotnet";
            if (osx)
                return "/usr/local/share/dotnet/dotnet";
            
            throw new NotImplementedException("unknown os");
        }
    }
}    
