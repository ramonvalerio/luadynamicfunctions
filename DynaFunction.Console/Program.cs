using DynaFunction.Core.Domain.Model;
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

            DynaFunction.AddFunctor(new Functor("Qo", "Multiple50(Multiple100(Multiple10(x * 2)))", data));
            DynaFunction.AddFunctor(new Functor("Vo", "Multiple10(x + 2)", data));
            DynaFunction.AddFunctor(new Functor("Bp", "Multiple10(x + 3)", data));
            DynaFunction.AddFunctor(new Functor("Multiple10", "(x * 10)", data));
            DynaFunction.AddFunctor(new Functor("Multiple50", "(x * 50)", data));
            DynaFunction.AddFunctor(new Functor("Multiple100", "(x * 100)", data));

            var functorRamon = new Functor("Ramon", "Qo * (Vo + Bp) / 500", data);
            DynaFunction.AddFunctor(functorRamon);

            using (var dynaFunction = new DynaFunction())
            {
                // Result 1
                var result = dynaFunction.Execute(functorRamon);

                //for (int i = 0; i < result.Y.Count; i++)
                    //Console.WriteLine($"X:{result.X[i].ToString("dd/MM/yyyy")} Y:{result.Y[i]}");

                //Console.WriteLine(JsonConvert.SerializeObject(result));
                Console.WriteLine($"Time Result 1: {dynaFunction.TimeResult.TotalSeconds}");

                // Result 2
                //var result2 = dynaFunction.Execute("A * (B + C) / 500");

                //for (int i = 0; i < result2.Y.Count; i++)
                //    Console.WriteLine($"X:{result2.X[i].ToString("dd/MM/yyyy")} Y:{result2.Y[i]}");

                //Console.WriteLine(JsonConvert.SerializeObject(result2));
                //Console.WriteLine($"Time Result 2: {dynaFunction.TimeResult.TotalSeconds}");

                //var result3 = dynaFunction.Execute("A * (B + C) / 500");
                //Console.WriteLine($"Time Result 3: {dynaFunction.TimeResult.TotalSeconds}");

                //var result4 = dynaFunction.Execute("A * (B + C) / 500");
                //Console.WriteLine($"Time Result 4: {dynaFunction.TimeResult.TotalSeconds}");

                //var result5 = dynaFunction.Execute("A * (B + C) / 500");
                //Console.WriteLine($"Time Result 5: {dynaFunction.TimeResult.TotalSeconds}");

                Console.ReadKey();
            }
        }
    }
}