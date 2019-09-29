using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using System.Collections.Generic;

namespace Inw.ArgumentExtraction.DTO
{
    public sealed class ArgumentResults
    {
        public string FileName { get; }
        public string FilePath { get; }
        public (int line, int col) FilePosition { get; }

        public List<ArgumentSyntax> Arguments { get; }

        public ArgumentResults(ReferenceLocation location)
        {
            var startPosition = location.Location.GetLineSpan().StartLinePosition;
            FilePosition = (startPosition.Line + 1, startPosition.Character + 1);
            FilePath = location.Document.FilePath;
            FileName = location.Document.Name;

            Arguments = new List<ArgumentSyntax>();
        }
    }
}
