using System;

namespace DynaFunction.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            var dynaFunction = new Application.DynaFunction();

            //1 - Set Functions
            //dynaFunction.AddFunction(new Functor { Name = "A", Expression = "x + 2" });
            //dynaFunction.AddFunction(new Functor { Name = "B", Expression = "x + 10" });
            //dynaFunction.AddFunction(new Functor { Name = "C", Expression = "x * 2" });

            //2 - Set Constants
            ///dynaFunction.AddConstants(new Constant { Name = "PI", Value = 3.1415926535897932 });

            //3 - Commit


            //4 - Execute
            //var result = dynaFunction.Execute("A * (B + C) / 500 + PI");
            var result = dynaFunction.Execute("A * (B + C) / 500");

            for (int i = 0; i < result.Count; i++)
                Console.WriteLine($"Linha {i}: {result[i]}");

            Console.WriteLine($"Time Result: {dynaFunction.TimeResult.TotalSeconds}");
            Console.ReadKey();
        }
    }
}