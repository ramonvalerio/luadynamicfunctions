using DynaFunction.Domain.Model;
using System;
using System.Collections.Generic;

namespace DynaFunction.Repository
{
    public class FunctionRepository
    {
        private readonly Dictionary<string, Functor> _functors;

        public FunctionRepository()
        {
            _functors = new Dictionary<string, Functor>();

            var data = new Data();
            var values = new List<double?>();

            for (int i = 0; i < 1000000; i++)
            {
                data.AddData(DateTime.Now.AddDays(i), (double)i);
            }

            var functor0 = new Functor("A", "Multiple50(Multiple100(Multiple10(x * 2)))");
            functor0.Data = data;

            var functor1 = new Functor("B", "Multiple10(x + 2)");
            functor1.Data = data;

            var functor2 = new Functor("C", "Multiple10(x + 3)");
            functor2.Data = data;

            var functor3 = new Functor("Multiple10", "(x * 10)");
            functor3.Data = data;

            var functor4 = new Functor("Multiple50", "(x * 50)");
            functor4.Data = data;

            var functor5 = new Functor("Multiple100", "(x * 100)");
            functor5.Data = data;

            _functors.Add(functor0.Name, functor0);
            _functors.Add(functor1.Name, functor1);
            _functors.Add(functor2.Name, functor2);
            _functors.Add(functor3.Name, functor3);
            _functors.Add(functor4.Name, functor4);
            _functors.Add(functor5.Name, functor5);
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