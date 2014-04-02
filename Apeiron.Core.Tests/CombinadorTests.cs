﻿using System;
using System.Collections.Generic;
using Apeiron.Siiau;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Diagnostics;

namespace Apeiron.Core.Tests
{
    [TestClass]
    public class CombinadorTests
    {
        [TestMethod]
        public void EncuentraHorarioCC()
        {
            Combinador c = new Combinador();

            List<string> claves = new List<string> { "cc100","cc101", "cc102", "cc103" };

            var l = new LectorSiiau();
            var materias = l.GetMateriasPorCentro(claves, "201410").Result;
            Stopwatch s = Stopwatch.StartNew();
            var horarios = c.EncuentraTodos(materias, maxDuracionPorHueco: TimeSpan.MaxValue, soloConCupo:false);
            s.Stop();
            Assert.AreEqual(s.ElapsedMilliseconds, 1);

            Assert.IsTrue(horarios.Count>9000);
            Assert.IsTrue(horarios.Distinct().Count() == horarios.Count);
        }
    }
}