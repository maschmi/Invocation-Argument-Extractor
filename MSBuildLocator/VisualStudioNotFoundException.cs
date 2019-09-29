using System;
using System.Runtime.Serialization;

namespace Inw.ArgumentExtractor.MSBuildLocator
{
    [Serializable]
    public class VisualStudioNotFoundException : Exception
    {
        
        public VisualStudioNotFoundException() { }
        public VisualStudioNotFoundException(string message) : base(message) { }
        public VisualStudioNotFoundException(string message, Exception inner) : base(message, inner) { }

        protected VisualStudioNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
    
}
