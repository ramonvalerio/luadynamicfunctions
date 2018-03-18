using System.Collections.Generic;

namespace DynaFunction.Application
{
    public interface IDynaFunction
    {
        IList<double?> Execute(string formula);
    }
}