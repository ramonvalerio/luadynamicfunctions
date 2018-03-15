using NLua;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LUADynamicFunctions
{
    public class DynamicFunction
    {
        private Dictionary<string, string> _functions;
        public TimeSpan TimeResult { get; set; }

        public DynamicFunction()
        {
            _functions = new Dictionary<string, string>();
        }

        public void AddFunction(string functionName, string expression)
        {
            var result = $@"function {functionName}(x) return {expression} end";
            _functions.Add(functionName, result);
        }

        public IList<double> Execute(string formula, IEnumerable<double> collection)
        {
            var watch = Stopwatch.StartNew();
            var result = new List<double>(collection.Count());

            using (var lua = new Lua())
            {
                var functionsLua = new List<LuaFunction>();

                foreach (var functionName in _functions.Keys)
                {
                    lua.DoString(_functions[functionName]);
                    functionsLua.Add(lua[functionName] as LuaFunction);
                }

                foreach (var x in collection)
                {
                    double resultAux = x;

                    foreach (var function in functionsLua)
                        resultAux = Convert.ToDouble(function.Call(resultAux).First());

                    result.Add(resultAux);
                }
            }

            watch.Stop();
            TimeResult = watch.Elapsed;

            return result;
        }
    }
}