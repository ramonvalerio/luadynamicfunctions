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
        private Dictionary<string, Functor> _functors = new Dictionary<string, Functor>();
        public TimeSpan TimeResult { get; private set; }
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

                // Set Function Execute
                var functorExecute = new Functor();
                functorExecute.Name = "Execute";
                functorExecute.Expression = _formula;
                lua.DoString(functorExecute.GetScriptFunction(parametersExecuteFunction));
                var execute = lua["Execute"] as LuaFunction;

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

                    //var resultFunction = execute.Call(parameters).FirstOrDefault();
                    var resultFunction = execute.Call(parameters[0], parameters[1], parameters[2]).FirstOrDefault();

                    if (resultFunction == null)
                    {
                        result.Add(null);
                    }
                    else
                    {
                        result.Add(Convert.ToDouble(resultFunction));
                    }
                }

                watch.Stop();
                TimeResult = watch.Elapsed;

                return result;
            }
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

            var functionRepository = new FunctionRepository();
            var functor = functionRepository.getFormulaByFunctionName(name);
            this.AddFunction(functor);
        }

        private string ReplaceParameterValue(string formula, double? parameterValue)
        {
            if (parameterValue == null)
                return formula.Replace("x", "0");
            else
                return formula.Replace("x", parameterValue.ToString());
        }
    }
}