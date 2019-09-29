using System;

namespace Inw.Logger
{
    public class ConsoleLogger : IDoLog
    {
        private readonly bool _verbose;
        private readonly bool _debug;
        private readonly bool _warning;
        private readonly bool _info;
        private readonly bool _error;

        public ConsoleLogger(bool verbose = false,
            bool debug = false,
            bool warning = true,
            bool info = true,
            bool error = true)
        {
            _verbose = verbose;
            _debug = debug;
            _warning = warning;
            _info = info;
            _error = error;
        }

        public void Debug(string logmessage)
        {
            if (_debug)
                Console.WriteLine("[DEBUG]\t" + logmessage);
        }

        public void Error(string logmessage)
        {
            if (_error)
                using (var colorScope = new ColorScope(ConsoleColor.Black, ConsoleColor.Red))
                    Console.WriteLine("[ERROR]\t" + logmessage);
        }

        public void Info(string logmessage)
        {
            if (_info)
                Console.WriteLine("[INFO]\t" + logmessage);
        }

        public void Warning(string logmessage)
        {
            if (_warning)
                using (var colorScope = new ColorScope(ConsoleColor.Black, ConsoleColor.Yellow))
                    Console.WriteLine("[WARN]\t" + logmessage);
        }

        public void Verbose(string logmessage)
        {
            if (_verbose)
                Console.WriteLine("[VERBOSE]\n\t" + logmessage);
        }

        private class ColorScope : IDisposable
        {
            private ConsoleColor _oldBackgrund;
            private ConsoleColor _oldForeground;

            public ColorScope(ConsoleColor background, ConsoleColor foreground)
            {
                _oldBackgrund = Console.BackgroundColor;
                _oldForeground = Console.ForegroundColor;

                Console.BackgroundColor = background;
                Console.ForegroundColor = foreground;
            }

            public void Dispose()
            {
                Console.ForegroundColor = _oldForeground;
                Console.BackgroundColor = _oldBackgrund;
            }
        }
    }
}
