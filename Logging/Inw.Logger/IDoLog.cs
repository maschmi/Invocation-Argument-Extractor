namespace Inw.Logger
{
    public interface IDoLog
    {
        void Debug(string logmessage);
        void Error(string logmessage);
        void Info(string logmessage);
        void Warning(string logmessage);
        void Verbose(string logmessage);
    }
}
