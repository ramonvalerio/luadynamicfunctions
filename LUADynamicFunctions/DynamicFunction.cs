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
            var functionsLua = new List<LuaFunction>();

            _formula = $"({formula})";

            IdentifyFunctionsByFormula(formula);

            using (var lua = new Lua())
            {
                string[] parametersExecuteFunction = new string[_functors.Count];

                for (int i = 0; i < _functors.Count; i++)
                    parametersExecuteFunction[i] = $"x{i}";

                var functorExecute = new Functor();
                functorExecute.Name = "Execute";
                functorExecute.Expression = _formula;
                _functors.Add("Execute", functorExecute);

                lua.DoString(_functors["Execute"].GetScriptFunction(parametersExecuteFunction)); // parametros
                functionsLua.Add(lua["Execute"] as LuaFunction);

                foreach (var functionName in _functors.Keys)
                {
                    lua.DoString(_functors[functionName].GetScriptFunction());
                }

                int maxLength = 50;
                var result = new List<double?>(maxLength);

                double?[] parameters = new double?[_functors.Count - 1];

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

                    //var resultFunction = Convert.ToDouble(functionsLua[0].Call(parameters).First());
                    var resultFunction = Convert.ToDouble(functionsLua[0].Call(parameters[0], parameters[1], parameters[2]).FirstOrDefault());

                    result.Add(resultFunction);
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