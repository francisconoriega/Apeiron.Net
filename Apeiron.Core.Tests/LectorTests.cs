using System.Collections.Generic;
using System.Linq;
using Apeiron.Core.Models;
using Apeiron.Siiau;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Apeiron.Core.Tests
{
    [TestClass]
    public class LectorTests
    {
        LectorSiiau lector;

        public IEnumerable<string> ciclosEscolares { get; set; }

        public LectorTests()
        {
            lector = new LectorSiiau();
            ciclosEscolares = lector.GetCiclosEscolares().Result;
        }

        [TestMethod]
        public void GetCiclosEscolares()
        {
            Assert.IsNotNull(ciclosEscolares);
            Assert.IsTrue(ciclosEscolares.All(s => s.Length == 6));

        }

        [TestMethod]
        public void GetCentrosUniversitarios()
        {
            var c = lector.GetCentrosUniversitarios().Result;

            Assert.IsNotNull(c);
            Assert.IsTrue(c.Any(s => s.Value.Contains("CU")));
        }

        [TestMethod]
        public void GetMateriaPorCentro()
        {
            var a = lector.GetMateriaPorCentro("CC101", ciclosEscolares.Skip(1).First(), "D").Result;
            Assert.IsNotNull(a);
            Assert.AreEqual("CC101", a.Clave);
        }

        [TestMethod]
        public void GetMateriaPorCentroMultiples()
        {

            var a = lector.GetMateriaPorCentro("CC101", ciclosEscolares.Skip(1).First()).Result;
            Assert.IsNotNull(a);
            Assert.AreEqual("CC101", a.Clave);
            Assert.AreEqual(3, a.Grupos.Select(g => g.CentroUniversitario).Distinct().Count());
        }


        [TestMethod]
        public void GetMateriaPorCentroQueNoCorresponde()
        {
            var a = lector.GetMateriaPorCentro("A0296", ciclosEscolares.Skip(1).First(), "D").Result;
            Assert.IsNull(a);
        }

        [TestMethod]
        public void GetVariasMaterias()
        {
            IEnumerable<Materia> materias = lector.GetMateriasPorCentro(new string[] { "CC101","CC102","C103" }, ciclosEscolares.Skip(1).First(), "D").Result;
            Assert.AreEqual(3, materias.Count());
        }

    }
}
