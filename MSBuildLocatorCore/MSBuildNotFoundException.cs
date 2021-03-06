﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Inw.ArgumentExtractor.MSBuildLocator.Core
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class MsBuildNotFoundException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public MsBuildNotFoundException()
        {
        }

        public MsBuildNotFoundException(string message) : base(message)
        {
        }

        public MsBuildNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MsBuildNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
