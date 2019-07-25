using System;
using System.Runtime.Serialization;

namespace Inw.ArgumentExtractionCli
{
    [Serializable]
    internal class DontKnowWhatToDoException : Exception
    {
        public DontKnowWhatToDoException()
        {
        }

        public DontKnowWhatToDoException(string message) : base(message)
        {
        }

        public DontKnowWhatToDoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DontKnowWhatToDoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}