using System;
using System.Collections.Generic;
using System.Linq;

namespace LUADynamicFunctions
{
    class Program
    {
        static void Main(string[] args)
        {
            var functionA = "FuncaoA";
            var expressionA = @"if x mod 2 then
                                    return x * 2
                                else
                                    return x * 100
                                end";

            var data = new List<decimal> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var dynamicFunction = new DynamicFunction();
            dynamicFunction.AddFunction(functionA, expressionA);
            var result = dynamicFunction.Execute(data);

            for (int i = 0; i < result.Count; i++)
                Console.WriteLine($"Linha {i}: {result[i]}");

            Console.WriteLine($"Time Result: {dynamicFunction.TimeResult.TotalSeconds}");
            Console.ReadKey();
        }
    }
}