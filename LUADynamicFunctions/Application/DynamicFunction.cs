using DynaFunction.Domain.Model;
using DynaFunction.Repository;
using NCalc;
using NLua;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace DynaFunction.Application
{
    public class DynaFunction : IDynaFunction
    {
        public TimeSpan TimeResult { get; private set; }
        private Dictionary<string, Functor> _functors = new Dictionary<string, Functor>();
        private Dictionary<string, Constant> _constants = new Dictionary<string, Constant>();
        private FunctionRepository _functionRepository = new FunctionRepository();
        private ConstantRepository _constantRepository = new ConstantRepository();
        private string _formula;
        private List<Data> _data;

        public DynaFunction()
        {
            _data = new List<Data>();
        }

        public IList<double?> Execute(string formula)
        {
            var watch = Stopwatch.StartNew();

            _formula = $"({formula})";

            IdentifyFunctionsByFormula(formula);

            using (var lua = new Lua())
            {
                string[] parametersExecuteFunction = new string[_functors.Count];

                // Create parameters
                for (int i = 0; i < _functors.Count; i++)
                    parametersExecuteFunction[i] = $"x{i}";

                // Declare constants
                foreach (var name in _constants.Keys)
                    _constants[name].CreateGlobalConstantValue(lua);

                // Declare functions
                foreach (var name in _functors.Keys)
                    lua.DoString(_functors[name].GetScriptFunction("x"));

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

                        var param = _functors[functionName].Data.Values[i].Value;
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

        private Data getData()
        {


            return null;
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
            if (!_functors.ContainsKey(functor.Name))
                _functors.Add(functor.Name, functor);
        }

        private void AddConstant(Constant constant)
        {
            if (!_constants.ContainsKey(constant.Name))
                _constants.Add(constant.Name, constant);
        }

        private void IdentifyFunctionsByFormula(string formula)
        {
            var expression = new Expression(formula);
            expression.EvaluateParameter += symbols_EvaluateParameter;
            expression.Evaluate();
        }

        private void IdentifyFunctionsByExpression(string formula)
        {
            var expression = new Expression(formula);
            expression.EvaluateFunction += expression_EvaluateFunction;
            expression.EvaluateParameter += expression_EvaluateParameter;
            expression.Evaluate();
        }

        private void symbols_EvaluateParameter(string name, ParameterArgs args)
        {
            args.Result = 0; // este valor é ignorado propositalmente
            _formula = _formula.Replace(name, $"{name}(x{_functors.Count})");

            var functor = _functionRepository.getFunctorByName(name);
            IdentifyFunctionsByExpression(functor.Expression);
            this.AddFunction(functor);
        }

        private void expression_EvaluateParameter(string name, ParameterArgs args)
        {
            args.Result = 0; // este valor é ignorado propositalmente
        }

        private void expression_EvaluateFunction(string name, FunctionArgs args)
        {
            args.Result = 0; // este valor é ignorado propositalmente
            var functor = _functionRepository.getFunctorByName(name);
            AddFunction(functor);
        }

        public DataTable GetData()
        {
            throw new NotImplementedException();
        }

        public void SetData(DataTable dataTable)
        {
            throw new NotImplementedException();
        }

        void IDynaFunction.AddFunction(Functor functor)
        {
            throw new NotImplementedException();
        }
    }
}