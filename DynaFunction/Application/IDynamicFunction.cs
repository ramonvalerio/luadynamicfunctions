using DynaFunction.Domain.Model;

namespace DynaFunction.Application
{
    public interface IDynaFunction
    {
        Data Execute(string formula);

        void AddScript(string script);

        void AddFileScript(string fileName);
    }
}