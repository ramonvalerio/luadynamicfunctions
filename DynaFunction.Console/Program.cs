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

            for (int i = 0; i < 10000; i++)
                data.AddData(DateTime.Now.AddDays(i), (double)i);

            DynaFunction.AddFunctor(new Functor("A", "Multiple50(Multiple100(Multiple10(x * 2)))", data));
            DynaFunction.AddFunctor(new Functor("B", "Multiple10(x + 2)", data));
            DynaFunction.AddFunctor(new Functor("C", "Multiple10(x + 3)", data));
            DynaFunction.AddFunctor(new Functor("Multiple10", "(x * 10)", data));
            DynaFunction.AddFunctor(new Functor("Multiple50", "(x * 50)", data));
            DynaFunction.AddFunctor(new Functor("Multiple100", "(x * 100)", data));

            using (var dynaFunction = new DynaFunction())
            {
                // Result 1
                var result = dynaFunction.Execute("A * (B + C) / 500");

                for (int i = 0; i < result.Y.Count; i++)
                    Console.WriteLine($"X:{result.X[i].ToString("dd/MM/yyyy")} Y:{result.Y[i]}");

                Console.WriteLine(JsonConvert.SerializeObject(result));
                Console.WriteLine($"Time Result 1: {dynaFunction.TimeResult.TotalSeconds}");

                // Result 2
                var result2 = dynaFunction.Execute("A * B");

                for (int i = 0; i < result2.Y.Count; i++)
                    Console.WriteLine($"X:{result2.X[i].ToString("dd/MM/yyyy")} Y:{result2.Y[i]}");

                Console.WriteLine(JsonConvert.SerializeObject(result2));
                Console.WriteLine($"Time Result 2: {dynaFunction.TimeResult.TotalSeconds}");

                Console.ReadKey();
            }
        }
    }
}