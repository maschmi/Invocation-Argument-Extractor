using System;
using System.Runtime.Serialization;

namespace Inw.ArgumentExtractor.MSBuildLocator.Core
{
    [Serializable]
    public class MSBuildNotFoundException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public MSBuildNotFoundException()
        {
        }

        public MSBuildNotFoundException(string message) : base(message)
        {
        }

        public MSBuildNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MSBuildNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
