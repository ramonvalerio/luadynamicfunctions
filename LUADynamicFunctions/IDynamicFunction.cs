using System.Collections.Generic;

namespace LUADynamicFunctions
{
    public interface IDynamicFunction
    {
        IList<double?> Execute(string formula);
    }
}