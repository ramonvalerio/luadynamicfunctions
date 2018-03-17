using System;

namespace LUADynamicFunctions
{
    class Program
    {
        static void Main(string[] args)
        {
            var dynamicFunction = new DynamicFunction();
            var result = dynamicFunction.Execute("A + (B + C)");

            for (int i = 0; i < result.Count; i++)
                Console.WriteLine($"Linha {i}: {result[i]}");

            Console.WriteLine($"Time Result: {dynamicFunction.TimeResult.TotalSeconds}");
            Console.ReadKey();
        }
    }
}