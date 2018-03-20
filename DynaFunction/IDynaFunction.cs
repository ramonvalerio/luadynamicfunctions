using DynaFunction.Core.Domain.Model;

namespace DynaFunction
{
    public interface IDynaFunction
    {
        Data Execute(Functor functor);

        void AddScript(string script);

        void AddFileScript(string fileName);
    }
}