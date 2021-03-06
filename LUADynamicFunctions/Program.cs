﻿using System;
using System.Collections.Generic;

namespace LUADynamicFunctions
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = new List<double?>();
            data.Add(null);
            data.Add(null);

            for (int i = 0; i < 10; i++) //limite: 10000000
                data.Add(i);

            var formula = "(A * (B + C))";

            var dynamicFunction = new DynamicFunction();
            var result = dynamicFunction.Execute(formula, data);

            for (int i = 0; i < result.Count; i++)
                Console.WriteLine($"Linha {i}: {result[i]}");

            Console.WriteLine($"Time Result: {dynamicFunction.TimeResult.TotalSeconds}");
            Console.ReadKey();
        }
    }
}