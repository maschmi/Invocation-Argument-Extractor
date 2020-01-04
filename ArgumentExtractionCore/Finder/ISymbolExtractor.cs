using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inw.ArgumentExtraction.Finder
{
    public interface ISymbolExtractor
    {
        Task<IEnumerable<ISymbol>> FindSymbols(Solution solution, string fullTypeName, string methodName, string[] methodParameterTypes);
    }
}
