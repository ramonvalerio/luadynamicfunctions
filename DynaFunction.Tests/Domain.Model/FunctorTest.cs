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
            _data.AddData(DateTime.Now, (double)10);
            _data.AddData(DateTime.Now, (double)20);
            _data.AddData(DateTime.Now, (double)30);

            DynaFunction.AddFunctor(new Functor("A", "B + C", _data));
            DynaFunction.AddFunctor(new Functor("B", "x + 1", _data));
            DynaFunction.AddFunctor(new Functor("C", "x * 2", _data));    
        }

        [TestMethod]
        public void A()
        {
            var valorEsperado = _dynaFunction.Execute(DynaFunction.GetFunctorByName("A"));
            Assert.IsTrue(valorEsperado.Y?[0] == 31);
        }

        [TestMethod]
        public void B()
        {
            var valorEsperado = _dynaFunction.Execute(DynaFunction.GetFunctorByName("B"));
            Assert.IsTrue(valorEsperado.Y?[0] == 11);
        }

        [TestMethod]
        public void C()
        {
            var valorEsperado = _dynaFunction.Execute(DynaFunction.GetFunctorByName("C"));
            Assert.IsTrue(valorEsperado.Y?[0] == 20);
        }
    }
}
