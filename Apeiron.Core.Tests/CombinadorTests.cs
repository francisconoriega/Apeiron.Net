using System;
using System.Collections.Generic;
using System.Linq;
using Apeiron.Siiau;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Apeiron.Core.Tests
{
    [TestClass]
    public class CombinadorTests
    {
        [TestMethod]
        public void EncuentraHorarioCC()
        {
            Combinador c = new Combinador();

            List<string> claves = new List<string> { "cc101", "cc100", "cc103" };

            var l = new LectorSiiau();          
            var materias = l.GetMateriasPorCentro(claves, "201410").Result;
            var horarios = c.EncuentraTodos(materias, maxDuracionPorHueco: TimeSpan.MaxValue,soloConCupo:false);

            Assert.IsTrue(horarios.Count > 9000);
            Assert.IsTrue(horarios.Distinct().Count() == horarios.Count);
        }
    }
}
