namespace Inw.ArgumentExtractor.MSBuildLocator.Core
{
    public interface IMsBuildLocator
    {
        /// <summary>
        /// Needs to return a path to a MSBuild.dll which can be used later on. 
        /// </summary>
        /// <param name="hintPath"></param>
        /// <returns></returns>
        string LocateMsBuild(string hintPath = "");
    }
}
