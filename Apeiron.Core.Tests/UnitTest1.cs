using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Apeiron.Siiau;
using System.Linq;

namespace Apeiron.Core.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetCiclosEscolares()
        {
            var l = new LectorSiiau();
            var x = l.GetCiclosEscolares().Result;
            Assert.IsNotNull(x);
            Assert.IsTrue(x.All(s => s.Length == 6));

        }

        [TestMethod]
        public void GetCentrosUniversitarios()
        {
            var l = new LectorSiiau();
            var c = l.GetCentrosUniversitarios().Result;

            Assert.IsNotNull(c);
            Assert.IsTrue(c.Any(s => s.Value.Contains("CU")));

        }

        [TestMethod]
        public void GetMateriaPorCentroTodos()
        {
            var l = new LectorSiiau();
            var x = l.GetCiclosEscolares().Result;
             var z = l.GetMateriaPorCentro("A0296", x.Skip(1).First()).Result;
             Assert.IsNotNull(z);
             Assert.AreEqual<string>("A0296", z.Clave);
        }

        [TestMethod]
        public void GetMateriaPorCentro()
        {
            var l = new LectorSiiau();
            var x = l.GetCiclosEscolares().Result;
            var a = l.GetMateriaPorCentro("CC101", x.Skip(1).First(), "D").Result;
            Assert.IsNotNull(a);
            Assert.AreEqual<string>("CC101", a.Clave);
        }

        [TestMethod]
        public void GetMateriaPorCentroMultiples()
        {
            var l = new LectorSiiau();
            var x = l.GetCiclosEscolares().Result;
            var a = l.GetMateriaPorCentro("CC101", x.Skip(1).First()).Result;
            Assert.IsNotNull(a);
            Assert.AreEqual<string>("CC101", a.Clave);
            Assert.AreEqual<int>(3, a.Grupos.Select(g => g.CentroUniversitario).Distinct().Count());
        }


        [TestMethod]
        public void GetMateriaPorCentroQueNoCorresponde()
        {
            var l = new LectorSiiau();
            var x = l.GetCiclosEscolares().Result;
            var a = l.GetMateriaPorCentro("A0296", x.Skip(1).First(), "D").Result;
            Assert.IsNull(a);

        }

    }
}
