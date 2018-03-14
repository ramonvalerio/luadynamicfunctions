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
            var result = $@"function {functionName}(x)
                            {expression}
                        end";

            _functions.Add(functionName, result);
        }

        public IList<decimal> Execute(IEnumerable<decimal> collection)
        {
            var watch = Stopwatch.StartNew();

            var result = new List<decimal>();

            using (var state = new Lua())
            {
                foreach (var functionName in _functions.Keys)
                {
                    state.DoString(_functions[functionName]);
                    var function = state[functionName] as LuaFunction;

                    foreach (var item in collection)
                    {
                        result.Add(Convert.ToDecimal(function.Call(item).First()));
                    }
                }
            }

            watch.Stop();
            TimeResult = watch.Elapsed;

            return result;
        }
    }
}