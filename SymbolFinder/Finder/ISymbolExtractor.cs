using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace Inw.ArgumentExtraction.Finder
{
    internal interface ISymbolExtractor
    {
        Task<IEnumerable<ISymbol>> FindSymbols(Solution solution, string fullTypeName, string methodName, string[] methodParameterTypes);
    }
}
