using DynaFunction.Domain.Model;

namespace DynaFunction
{
    public interface IDynaFunction
    {
        Data Execute(string formula);

        void AddScript(string script);

        void AddFileScript(string fileName);

        //void AddFunctor(Functor functor);
    }
}