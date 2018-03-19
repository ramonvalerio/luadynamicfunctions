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
                data.AddData(DateTime.Now, (double)i);
            }

            var functor0 = new Functor();
            functor0.Name = "A";
            functor0.Expression = "Multiple50(Multiple100(Multiple10(x * 2)))";
            functor0.Data = data;

            var functor1 = new Functor();
            functor1.Name = "B";
            functor1.Expression = "Multiple10(x + 2)";
            functor1.Data = data;

            var functor2 = new Functor();
            functor2.Name = "C";
            functor2.Expression = "Multiple10(x + 3)";
            functor2.Data = data;

            var functor3 = new Functor();
            functor3.Name = "Multiple10";
            functor3.Expression = "(x * 10)";
            functor3.Data = data;

            var functor4 = new Functor();
            functor4.Name = "Multiple50";
            functor4.Expression = "(x * 50)";
            functor4.Data = data;

            var functor5 = new Functor();
            functor5.Name = "Multiple100";
            functor5.Expression = "(x * 100)";
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