﻿using Newtonsoft.Json;
using System;

namespace DynaFunction.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var dynaFunction = new Application.DynaFunction())
            {
                var result = dynaFunction.Execute("A * (B + C) / 500");

                //var jsonResult = JsonConvert.SerializeObject(result);

                //for (int i = 0; i < result.Y.Count; i++)
                //    Console.WriteLine($"Data {result.X[i].ToString("dd/MM/yyyy")} {result.Y[i]}");

                Console.WriteLine($"Time Result: {dynaFunction.TimeResult.TotalSeconds}");
                Console.ReadKey();
            }
        }
    }
}