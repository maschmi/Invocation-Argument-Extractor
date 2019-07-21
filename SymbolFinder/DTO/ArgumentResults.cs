using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;

namespace Inw.ArgumentExtraction.DTO
{
    public sealed class ArgumentResults
    {
        public ReferenceLocation Location { get; }
        public List<ArgumentSyntax> Arguments { get; }

        public ArgumentResults(ReferenceLocation location)
        {            
            Location = location;
            Arguments = new List<ArgumentSyntax>();
        }
    }
}
