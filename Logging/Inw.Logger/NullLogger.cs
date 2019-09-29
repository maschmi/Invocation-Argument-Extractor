namespace Inw.Logger
{
    public class NullLogger : IDoLog
    {
        public void Debug(string logmessage)
        {
        }

        public void Error(string logmessage)
        {
        }

        public void Info(string logmessage)
        {
        }

        public void Verbose(string logmessage)
        {
        }

        public void Warning(string logmessage)
        {
        }


    }
}
