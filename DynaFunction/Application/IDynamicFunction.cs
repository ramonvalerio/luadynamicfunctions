using System.Collections.Generic;

namespace DynaFunction.Application
{
    public interface IDynaFunction
    {
        IList<double?> Execute(string formula);

        void AddScript(string script);

        void AddFileScript(string fileName);
    }
}