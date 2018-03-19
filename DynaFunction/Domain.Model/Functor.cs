﻿using System;

namespace DynaFunction.Domain.Model
{
    public class Functor
    {
        public string Name { get; set; }
        public string Expression { get; set; }
        public Data Data { get; set; }

        public Functor(string name, string expression)
        {
            Name = name;
            Expression = expression;
            Data = new Data();
        }

        public Functor(string name, string expression, Data data)
        {
            Name = name;
            Expression = expression;
            Data = data;
        }

        public string GetScriptFunction(params string[] parameters)
        {
            string parametersResult = string.Empty;

            for (int i = 0; i < parameters.Length; i++)
            {
                if (i == (parameters.Length - 1))
                    parametersResult += $"{parameters[i]}";
                else
                    parametersResult += $"{parameters[i]}, ";
            }

            var result = $@"function {this.Name}({parametersResult})
                                result = {this.Expression}
                                if result == nil then
                                    return 0
                                end
                                return result
                            end";

            return result;
        }
    }
}