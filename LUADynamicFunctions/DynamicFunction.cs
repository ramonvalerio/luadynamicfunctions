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
        private Dictionary<string, Functor> _functors = new Dictionary<string, Functor>();
        public TimeSpan TimeResult { get; private set; }
        private Expression _expression;
        private string _formula;

        public IList<double?> Execute(string formula, IEnumerable<double?> data)
        {
            var watch = Stopwatch.StartNew();
            var functionsLua = new List<LuaFunction>();
            var result = new List<double?>(data.Count());

            _formula = formula;

            IdentifyFunctionsByFormula(formula);

            using (var lua = new Lua())
            {
                var functorExecute = new Functor();
                functorExecute.Name = "Execute";
                functorExecute.Expression = _formula;

                _functors.Add("Execute", functorExecute);
                lua.DoString(_functors["Execute"].GetScriptFunction());
                functionsLua.Add(lua["Execute"] as LuaFunction);

                foreach (var functionName in _functors.Keys)
                    lua.DoString(_functors[functionName].GetScriptFunction());

                foreach (var x in data.AsParallel())
                {
                    double? resultAux = x;

                    foreach (var function in functionsLua)
                        resultAux = Convert.ToDouble(function.Call(resultAux,0,0).First());

                    result.Add(resultAux);
                }
            }

            watch.Stop();
            TimeResult = watch.Elapsed;

            return result;
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