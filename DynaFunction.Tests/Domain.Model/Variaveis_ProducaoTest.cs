using DynaFunction.Core.Domain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DynaFunction.Tests
{
    [TestClass]
    public class Variaveis_ProducaoTest
    {
        private readonly DynaFunction _dynaFunction;
        private readonly Data _data;

        public Variaveis_ProducaoTest()
        {
            _dynaFunction = new DynaFunction();
            _data = new Data();

            for (int i = 0; i < 1; i++)
                _data.AddData(DateTime.Now.AddDays(i), (double)10);

            DynaFunction.AddFunctor(new Functor("Bp", "Qo + Qw", _data));
            DynaFunction.AddFunctor(new Functor("Qo", "x + 1", _data));
            DynaFunction.AddFunctor(new Functor("Qw", "x * 2", _data));    
        }

        [TestMethod]
        public void Bp()
        {
            var valorEsperado = _dynaFunction.Execute(DynaFunction.GetFunctorByName("Bp"));
            Assert.IsTrue(valorEsperado.Y?[0] == 31);
        }

        [TestMethod]
        public void Qo()
        {
            var valorEsperado = _dynaFunction.Execute(DynaFunction.GetFunctorByName("Qo"));
            Assert.IsTrue(valorEsperado.Y?[0] == 11);
        }

        [TestMethod]
        public void Qw()
        {
            var valorEsperado = _dynaFunction.Execute(DynaFunction.GetFunctorByName("Qw"));
            Assert.IsTrue(valorEsperado.Y?[0] == 20);
        }
    }
}
