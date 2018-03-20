using DynaFunction.Core.Domain.Model;
using DynaFunction.Core.Repository;
using NCalc;
using NLua;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DynaFunction
{
    public class DynaFunction : IDynaFunction, IDisposable
    {
        public TimeSpan TimeResult { get; private set; }
        private Dictionary<string, Functor> _functors;
        private Dictionary<string, Constant> _constants;
        private string _formula;
        private Data _data;
        private Lua _lua;

        public DynaFunction()
        {
            _lua = new Lua();

            _functors = new Dictionary<string, Functor>();
            _constants = new Dictionary<string, Constant>();
            _data = new Data();
        }

        public Data Execute(Functor functor)
        {
            _functors.Clear();
            _constants.Clear();
            _data = new Data();

            var watch = Stopwatch.StartNew();

            _formula = $"({functor.Expression})";

            identifyFunctionsByFormula(functor.Expression);

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

            dynamic[] parameters = new dynamic[_functors.Count];

            for (int i = 0; i < 1; i++)
            {
                var indexParameter = 0;

                foreach (var functionName in _functors.Keys) // Temporário
                {
                    _data.X.Add(_functors[functionName].Data.X[i]);
                    break;
                }

                foreach (var functionName in _functors.Keys)
                {
                    var value = _functors[functionName].Data.Y[i];

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

        private dynamic execute(LuaFunction luaFunction, params dynamic[] parameters)
        {
            if (parameters.Length > 6)
                throw new Exception("Só é permitido no máximo 6 funções na fórmula.");

            object result = null;

            if (parameters.Length == 0)
                result = luaFunction.Call()[0];

            if (parameters.Length == 1)
                result = luaFunction.Call(parameters[0])[0];

            if (parameters.Length == 2)
                result = luaFunction.Call(parameters[0], parameters[1])[0];

            if (parameters.Length == 3)
                result = luaFunction.Call(parameters[0], parameters[1], parameters[2])[0];

            if (parameters.Length == 4)
                result = luaFunction.Call(parameters[0], parameters[1], parameters[2], parameters[3])[0];

            if (parameters.Length == 5)
                result = luaFunction.Call(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4])[0];

            if (parameters.Length == 6)
            {
                result = luaFunction.Call(parameters[0], parameters[1], parameters[2],
                    parameters[3], parameters[4], parameters[5])[0];
            }

            if (result == null)
                return null;

            return result;
        }

        private void addFunction(Functor functor)
        {
            if (!_functors.ContainsKey(functor.Name))
            {
                _functors.Add(functor.Name, functor);
            }
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

            if (name.ToUpper() == "X")
                return;

            _formula = _formula.Replace(name, $"{name}(x{_functors.Count})");

            var functor = FunctorRepository.GetFunctorByName(name);
            this.addFunction(functor);

            identifyFunctionsByExpression(functor.Expression);
        }

        private void expression_EvaluateFunction(string name, FunctionArgs args)
        {
            args.Result = 0; // este valor é ignorado propositalmente
            var functor = FunctorRepository.GetFunctorByName(name);
            _lua.DoString(functor.GetScriptFunction("x"));

            identifyFunctionsByExpression(args.Parameters[0].ParsedExpression.ToString());
        }

        private void expression_EvaluateParameter(string name, ParameterArgs args)
        {
            args.Result = 0; // este valor é ignorado propositalmente
        }

        public static Functor GetFunctorByName(string name)
        {
            return FunctorRepository.GetFunctorByName(name);
        }

        public static void AddFunctor(Functor functor)
        {
            FunctorRepository.AddFunctor(functor);
        }

        public void AddScript(string script)
        {
            _lua.DoString(script);
        }

        public void AddFileScript(string fileName)
        {
            _lua.DoFile(fileName);
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