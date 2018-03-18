using DynaFunction.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace DynaFunction.Repository
{
    public class FunctionRepository
    {
        private readonly Dictionary<string, Functor> _functors;

        public FunctionRepository()
        {
            _functors = new Dictionary<string, Functor>();

            var values = new List<double?>();

            for (int i = 1; i < 50; i++)
            {
                values.Add((double)i);
            }

            var functor0 = new Functor();
            functor0.Name = "A";
            functor0.Expression = "Multiple10(x * 2)";
            functor0.Data.Values = values;

            var functor1 = new Functor();
            functor1.Name = "B";
            functor1.Expression = "Multiple10(x + 2)";
            functor0.Data.Values = values;

            var functor2 = new Functor();
            functor2.Name = "C";
            functor2.Expression = "Multiple10(x + 3)";
            functor0.Data.Values = values;

            var functor3 = new Functor();
            functor3.Name = "Multiple10";
            functor3.Expression = "(x * 10)";
            functor0.Data.Values = values;

            var functor4 = new Functor();
            functor4.Name = "Multiple50";
            functor4.Expression = "(x * 50)";
            functor0.Data.Values = values;

            var functor5 = new Functor();
            functor5.Name = "Multiple100";
            functor5.Expression = "(x * 100)";
            functor0.Data.Values = values;

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