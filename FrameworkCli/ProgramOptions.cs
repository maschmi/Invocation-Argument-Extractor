using System;
using System.IO;

namespace Inw.ArgumentExtractionCli
{
    internal class ProgramOptions
    {
        public string PathToSolution { get; set; }
        public string FullTypeName { get; set; }
        public string MethodName { get; set; }
        public string[] MethodArguments { get; set; }
        public bool VerboseLogging { get; private set; }
        public bool DebugLogging { get; private set; }
        public bool HelpPrinted { get; private set; }

        private const string HELP_TEXT =
            "This little static code analysis helper searches for references of the defined method and the extracts the method arguments\n" +
            "Usage:\n" +
            "\t -s \t Path to the solution with the method to analyze \n" +
            "\t -t \t The full name of the type e.g. 'System.Console' \n" +
            "\t -m \t Methodname e.g. 'WriteLine' \n" +
            "\t -a \t Comma separated list of method arguments e.g. 'params,string' \n" +
            "\t -verbose \t Writes verbose log messages \n" +
            "\t -debug \t Writes debug log messages \n" +
            "\n" +
            "Known issues \n " +
            "- Still toying around";

        public ProgramOptions(string[] args)
        {
            FilterArgs(args);
            if (!HelpPrinted)
                Validate();
        }

        private void Validate()
        {
            if (!File.Exists(PathToSolution))
                throw new InvalidOperationException("Solution not found!");
            if (string.IsNullOrWhiteSpace(MethodName))
                throw new InvalidOperationException("No method to search for supplied. Consider using the -m switch.");
            if (string.IsNullOrWhiteSpace(FullTypeName))
                throw new InvalidOperationException("No type to search in supplied. Consider using the -t switch.");
        }

        private void FilterArgs(string[] args)
        {

            if (args.Length == 0)
                PrintHelp();

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-s":
                        PathToSolution = args[++i];
                        break;

                    case "-t":
                        FullTypeName = args[++i];
                        break;

                    case "-m":
                        MethodName = args[++i];
                        break;

                    case "-a":
                        MethodArguments = CalculateMethodArguments(args[++i]);
                        break;

                    case "-verbose":
                        VerboseLogging = true;
                        break;

                    case "-debug":
                        DebugLogging = true;
                        break;

                    default:
                        PrintHelp();
                        break;
                }

            }
        }

        private string[] CalculateMethodArguments(string args)
        {
            if (string.IsNullOrWhiteSpace(args))
                return new string[0];

            return args.Split(',');
        }

        private void PrintHelp()
        {
            Console.WriteLine(HELP_TEXT);
            HelpPrinted = true;
        }

    }
}
