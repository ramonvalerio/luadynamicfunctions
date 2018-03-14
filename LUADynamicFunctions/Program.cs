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
            var expressionA = @"if x % 2 then
                                    return x * 2
                                else
                                    return x * 100
                                end";

            var data = new List<decimal>();

            for (int i = 0; i < 1000000; i++)
                data.Add(i);

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