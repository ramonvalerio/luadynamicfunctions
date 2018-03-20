using DynaFunction.Core.Domain.Model;
using System.Collections.Generic;

namespace DynaFunction.Core.Repository
{
    internal static class FunctorRepository
    {
        private static readonly Dictionary<string, Functor> _functors;

        static FunctorRepository()
        {
            _functors = new Dictionary<string, Functor>();
        }

        public static void AddFunctor(Functor functor)
        {
            if (_functors.ContainsKey(functor.Name))
                return;

            _functors.Add(functor.Name, functor);
        }

        public static Functor GetFunctorByName(string name)
        {
            return _functors[name];
        }
    }
}