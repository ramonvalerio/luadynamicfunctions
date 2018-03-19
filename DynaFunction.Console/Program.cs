using DynaFunction.Domain.Model;
using Newtonsoft.Json;
using System;

namespace DynaFunction.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = new Data();

            for (int i = 0; i < 100; i++)
                data.AddData(DateTime.Now.AddDays(i), (double)i);

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

            using (var dynaFunction = new DynaFunction())
            {
                dynaFunction.AddFunctor(functor0);
                dynaFunction.AddFunctor(functor1);
                dynaFunction.AddFunctor(functor2);
                dynaFunction.AddFunctor(functor3);
                dynaFunction.AddFunctor(functor4);
                dynaFunction.AddFunctor(functor5);

                var result = dynaFunction.Execute("A * (B + C) / 500");

                var jsonResult = JsonConvert.SerializeObject(result);

                for (int i = 0; i < result.Y.Count; i++)
                    Console.WriteLine($"X:{result.X[i].ToString("dd/MM/yyyy")} Y:{result.Y[i]}");

                Console.WriteLine(jsonResult);

                Console.WriteLine($"Time Result: {dynaFunction.TimeResult.TotalSeconds}");
                Console.ReadKey();
            }
        }
    }
}