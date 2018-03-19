using DynaFunction.Domain.Model;
using System.Collections.Generic;

namespace DynaFunction.Repository
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

        public static Functor getFunctorByName(string name)
        {
            return _functors[name];
        }
    }
}