using DynaFunction.Domain.Model;
using System.Collections.Generic;
using System.Data;

namespace DynaFunction.Application
{
    public interface IDynaFunction
    {
        DataTable GetData();

        IList<double?> Execute(string formula);

        void SetData(DataTable dataTable);

        void AddFunction(Functor functor);
    }
}