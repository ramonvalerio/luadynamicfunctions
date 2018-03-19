using DynaFunction.Domain.Model;
using System.Collections.Generic;

namespace DynaFunction.Repository
{
    internal class FunctorRepository
    {
        private readonly Dictionary<string, Functor> _functors;

        public FunctorRepository()
        {
            _functors = new Dictionary<string, Functor>();
        }

        public void AddFunctor(Functor functor)
        {
            if (_functors.ContainsKey(functor.Name))
                return;

            _functors.Add(functor.Name, functor);
        }

        public Functor getFunctorByName(string name)
        {
            return _functors[name];
        }
    }
}