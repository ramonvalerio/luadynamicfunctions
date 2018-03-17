using NCalc;
using NLua;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LUADynamicFunctions
{
    public class DynamicFunction : IDynamicFunction
    {
        public TimeSpan TimeResult { get; private set; }
        private Dictionary<string, Functor> _functors = new Dictionary<string, Functor>();
        private FunctionRepository _functionRepository = new FunctionRepository();
        private Expression _expression;
        private string _formula;

        public IList<double?> Execute(string formula)
        {
            var watch = Stopwatch.StartNew();

            _formula = $"({formula})";

            IdentifyFunctionsByFormula(formula);

            using (var lua = new Lua())
            {
                string[] parametersExecuteFunction = new string[_functors.Count];

                for (int i = 0; i < _functors.Count; i++)
                    parametersExecuteFunction[i] = $"x{i}";

                // Set All other Function
                foreach (var functionName in _functors.Keys)
                    lua.DoString(_functors[functionName].GetScriptFunction("x"));

                int maxLength = 49;
                var result = new List<double?>(maxLength);

                double?[] parameters = new double?[_functors.Count];

                for (int i = 0; i < maxLength; i++)
                {
                    var indexParameter = 0;

                    foreach (var functionName in _functors.Keys)
                    {
                        if (functionName == "Execute")
                            continue;

                        var param = _functors[functionName].Values[i]?.Value;
                        parameters[indexParameter] = param;
                        indexParameter++;
                    }

                    // Set Function Execute
                    var functorExecute = new Functor();
                    functorExecute.Name = "Execute";
                    functorExecute.Expression = _formula;
                    lua.DoString(functorExecute.GetScriptFunction(parametersExecuteFunction));
                    var mainFunction = lua["Execute"] as LuaFunction;

                    result.Add(execute(mainFunction, parameters));
                }

                watch.Stop();
                TimeResult = watch.Elapsed;

                return result;
            }
        }

        private double? execute(LuaFunction luaFunction, params double?[] parameters)
        {
            if (parameters.Length > 6)
                throw new Exception("Só é permitido no máximo 6 funções na fórmula.");

            object result = null;

            if (parameters.Length == 0)
                result = luaFunction.Call().FirstOrDefault();

            if (parameters.Length == 1)
                result = luaFunction.Call(parameters[0]).FirstOrDefault();

            if (parameters.Length == 2)
                result = luaFunction.Call(parameters[0], parameters[1]).FirstOrDefault();

            if (parameters.Length == 3)
                result = luaFunction.Call(parameters[0], parameters[1], parameters[2]).FirstOrDefault();

            if (parameters.Length == 4)
                result = luaFunction.Call(parameters[0], parameters[1], parameters[2], parameters[3]).FirstOrDefault();

            if (parameters.Length == 5)
                result = luaFunction.Call(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4]).FirstOrDefault();

            if (parameters.Length == 6)
            {
                result = luaFunction.Call(parameters[0], parameters[1], parameters[2],
                    parameters[3], parameters[4], parameters[5]).FirstOrDefault();
            }

            if (result == null)
                return null;

            return Convert.ToDouble(result);
        }

        private static T ConvertValue<T>(string value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        private void AddFunction(Functor functor)
        {
            _functors.Add(functor.Name, functor);
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
            _formula = _formula.Replace(name, $"{name}(x{_functors.Count})");

            var functor = _functionRepository.getFormulaByFunctionName(name);
            this.AddFunction(functor);
        }
    }
}