using System;
using System.Collections.Generic;

namespace LUADynamicFunctions
{
    public class Functor
    {
        public string Name { get; set; }
        public string Expression { get; set; }
        public List<Data> Values { get; private set; }

        public Functor()
        {
            Values = new List<Data>();

            for (int i = 1; i < 50; i++)
                Values.Add(new Data(DateTime.Now, (double)i));
        }

        public string GetScriptFunction(params string[] parameters)
        {
            string parametersResult = string.Empty;

            for (int i = 0; i < parameters.Length; i++)
            {
                if (i == (parameters.Length - 1))
                {
                    parametersResult += $"{parameters[i]}";
                }
                else
                {
                    parametersResult += $"{parameters[i]}, ";
                }
            }

            var result = $@"function {this.Name}({parametersResult})
                                result =  {this.Expression}
                                if result == nil then
                                    return 0
                                end
                                return result
                            end";

            return result;
        }

        public void AddData(DateTime date, double? value)
        {
            Values.Add(new Data(date, value));
        }
    }

    public class Data
    {
        public DateTime Date { get; private set; }
        public double? Value { get; private set; }

        public Data(DateTime date, double? value)
        {
            Date = date;
            Value = value;
        }
    }
}
