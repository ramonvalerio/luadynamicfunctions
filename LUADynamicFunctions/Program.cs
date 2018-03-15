using System;
using System.Collections.Generic;

namespace LUADynamicFunctions
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = new List<double>();

            for (int i = 0; i < 50; i++)
                data.Add(i);

            var functionA = "A";
            var expressionA = @"x * 2";

            var functionB = "B";
            var expressionB = @"x + 10";

            var dynamicFunction = new DynamicFunction();
            dynamicFunction.AddFunction(functionA, expressionA);
            dynamicFunction.AddFunction(functionB, expressionB);

            var result = dynamicFunction.Execute("A + B", data);

            for (int i = 0; i < result.Count; i++)
                Console.WriteLine($"Linha {i}: {result[i]}");

            Console.WriteLine($"Time Result: {dynamicFunction.TimeResult.TotalSeconds}");
            Console.ReadKey();
        }
    }
}