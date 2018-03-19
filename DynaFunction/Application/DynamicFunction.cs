using DynaFunction.Domain.Model;
using DynaFunction.Repository;
using NCalc;
using NLua;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DynaFunction.Application
{
    public class DynaFunction : IDynaFunction, IDisposable
    {
        public TimeSpan TimeResult { get; private set; }
        private Dictionary<string, Functor> _functors = new Dictionary<string, Functor>();
        private Dictionary<string, Constant> _constants = new Dictionary<string, Constant>();
        private FunctionRepository _functionRepository = new FunctionRepository();
        private ConstantRepository _constantRepository = new ConstantRepository();
        private string _formula;
        private Data _data;
        private Lua _lua;

        public DynaFunction()
        {
            _lua = new Lua();
            _data = new Data();
        }

        public Data Execute(string formula)
        {
            var watch = Stopwatch.StartNew();

            _formula = $"({formula})";

            identifyFunctionsByFormula(formula);

            string[] parametersExecuteFunction = new string[_functors.Count];

            // Create parameters
            for (int i = 0; i < _functors.Count; i++)
                parametersExecuteFunction[i] = $"x{i}";

            // Declare constants
            foreach (var name in _constants.Keys)
                _constants[name].CreateGlobalConstantValue(_lua);

            // Declare functions
            foreach (var name in _functors.Keys)
                _lua.DoString(_functors[name].GetScriptFunction("x"));

            double?[] parameters = new double?[_functors.Count];

            for (int i = 0; i < 100; i++)
            {
                var indexParameter = 0;

                foreach (var functionName in _functors.Keys) // Temporário
                {
                    _data.X.Add(_functors[functionName].Data.X[i].Date);
                    break;
                }

                foreach (var functionName in _functors.Keys)
                {
                    var value = _functors[functionName].Data.Y[i].Value;

                    parameters[indexParameter] = value;
                    indexParameter++;
                }

                var functorExecute = new Functor("Execute", _formula);
                _lua.DoString(functorExecute.GetScriptFunction(parametersExecuteFunction));

                var mainFunction = _lua["Execute"] as LuaFunction;
                _data.Y.Add(execute(mainFunction, parameters));
            }

            watch.Stop();
            TimeResult = watch.Elapsed;

            return _data;
        }

        public void AddScript(string script)
        {
            _lua.DoString(script);
        }

        public void AddFileScript(string fileName)
        {
            _lua.DoFile(fileName);
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

        private void addFunction(Functor functor)
        {
            if (!_functors.ContainsKey(functor.Name))
                _functors.Add(functor.Name, functor);
        }

        private void addConstant(Constant constant)
        {
            if (!_constants.ContainsKey(constant.Name))
                _constants.Add(constant.Name, constant);
        }

        private void identifyFunctionsByFormula(string formula)
        {
            var expression = new Expression(formula);
            expression.EvaluateParameter += symbols_EvaluateParameter;
            expression.Evaluate();
        }

        private void identifyFunctionsByExpression(string formula)
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
            this.addFunction(functor);

            identifyFunctionsByExpression(functor.Expression);
        }

        private void expression_EvaluateFunction(string name, FunctionArgs args)
        {
            args.Result = 0; // este valor é ignorado propositalmente
            var functor = _functionRepository.getFunctorByName(name);
            _lua.DoString(functor.GetScriptFunction("x"));

            identifyFunctionsByExpression(args.Parameters[0].ParsedExpression.ToString());
        }

        private void expression_EvaluateParameter(string name, ParameterArgs args)
        {
            args.Result = 0; // este valor é ignorado propositalmente
        }

        public void Dispose()
        {
            if (_lua != null)
                _lua.Dispose();

            TimeResult = TimeSpan.MinValue;
            _functors.Clear();
            _constants.Clear();
            _formula = string.Empty;
            _data = new Data();
        }
    }
}