using NCalc;
using NLua;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LUADynamicFunctions
{
    public class DynamicFunction
    {
        private Dictionary<string, string> _functions = new Dictionary<string, string>();
        public TimeSpan TimeResult { get; private set; }
        private Expression _expression;
        private string _formula;

        private void AddFunction(string functionName, string expression)
        {
            var result = $@"function {functionName}(x)
                                if x == nil then x = 0 end
                            return {expression}
                        end";
            _functions.Add(functionName, result);
        }

        private void IdentifyFunctionsByFormula(string formula)
        {
            _expression = new Expression(formula);
            _expression.EvaluateParameter += _expression_EvaluateParameter;
            _expression.Evaluate();
        }

        private void _expression_EvaluateParameter(string name, ParameterArgs args)
        {
            args.Result = 0; // este valor é ignorado propositalmente
            _formula = _formula.Replace(name, $"{name}(x)");
            this.AddFunction(name, FunctionRepository.getFormulaByFunctionName(name));
        }

        private string ReplaceParameterValue(string formula, double? parameterValue)
        {
            if (parameterValue == null)
                return formula.Replace("x", "0");
            else
                return formula.Replace("x", parameterValue.ToString());
        }

        public IList<double?> Execute(string formula, IEnumerable<double?> data)
        {
            var watch = Stopwatch.StartNew();
            var functionsLua = new List<LuaFunction>();
            var result = new List<double?>(data.Count());

            _formula = formula;

            IdentifyFunctionsByFormula(formula);

            using (var lua = new Lua())
            {
                var scriptExecuteFunction = $@"function Execute(x)
                                                return {_formula}
                                            end";

                _functions.Add("Execute", scriptExecuteFunction);

                lua.DoString(_functions["Execute"]);
                functionsLua.Add(lua["Execute"] as LuaFunction);

                foreach (var functionName in _functions.Keys)
                    lua.DoString(_functions[functionName]);

                foreach (var x in data)
                {
                    double? resultAux = x;

                    foreach (var function in functionsLua)
                        resultAux = Convert.ToDouble(function.Call(resultAux).FirstOrDefault());

                    result.Add(resultAux);
                }
            }

            watch.Stop();
            TimeResult = watch.Elapsed;

            return result;
        }
    }
}