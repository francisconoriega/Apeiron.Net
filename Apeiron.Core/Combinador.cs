using ApeironOnline.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApeironOnline.Core
{
    public class Combinador
    {
        public Combinador()
        {
            this.CombinacionesDeGrupos = new List<CombinacionGrupos>();
        }
        private List<CombinacionGrupos> CombinacionesDeGrupos { get; set; }
        private int maxHuecosPorDia = 0;
        private TimeSpan maxDuracionPorHueco;
        private int numMaterias = 0;
        private int numCiclos = 0;
        private List<CentrosUniversitarios> filtroCentros;
        private ILookup<string, string> filtroMaestros;
        private bool soloConCupo;
        private bool permiteHorariosIncompletos;

        public List<CombinacionGrupos> EncuentraTodos(List<Materia> materias, ILookup<string, string> filtroMaestros, TimeSpan maxDuracionPorHueco, List<CentrosUniversitarios> filtroCentros = null, int maxHuecosPorDia = int.MaxValue, bool soloConCupo = true, bool permiteHorariosIncompletos = true)
        {
            this.numCiclos = 0;
            this.permiteHorariosIncompletos = permiteHorariosIncompletos;
            this.soloConCupo = soloConCupo;
            this.filtroMaestros = filtroMaestros;
            this.filtroCentros = filtroCentros;
            this.maxDuracionPorHueco = maxDuracionPorHueco;
            this.maxHuecosPorDia = maxHuecosPorDia;
            this.CombinacionesDeGrupos = new List<CombinacionGrupos>();
            this.numMaterias = materias.Count();
            ExploraCombinaciones(new CombinacionGrupos(), materias);
            return this.CombinacionesDeGrupos;
        }

        /// <summary>
        /// Funcion Recursiva que arma toda las combinaciones posibles de los grupos, siempre y cuando
        /// no sea limitado por el numero de ciclos o por especificaciones como numero de huecos o 
        /// solo permitir horarios completos (que tienen todas las materias seleccionadas)
        /// </summary>
        /// <param name="combinacionActual"></param>
        /// <param name="materias"></param>
        public void ExploraCombinaciones(CombinacionGrupos combinacionActual, IEnumerable<Materia> materias)
        {
            if (numCiclos++ > 20000)
            {
                return;
            }

            //Si ya n hay materias, ya terminamos las combinaciones posibles.
            if (materias.Count() == 0)
            {
                if (combinacionActual.Grupos.Count == numMaterias)
                {
                    combinacionActual.Completo = true;
                }
                else
                {
                    combinacionActual.Completo = false;
                }

                this.CombinacionesDeGrupos.Add(combinacionActual.Clone());
                return;
            }
            else
            {
                //Dado que limitamos el número de permutaciones posibles, ordenar los grupos por cupo primero
                //aseguro que los primeros horarios que se encuentren tengan cupo.
                var gruposOrdenadosPorCupo = materias.First().Grupos.OrderByDescending(g => g.CupoDisponible);
                foreach (var grupo in gruposOrdenadosPorCupo)
                {
                    var grupoValido = false;
                    if (!combinacionActual.ColisionaCon(grupo))
                    {
                        if (filtroMaestros == null || filtroMaestros[grupo.Materia.Clave].FirstOrDefault() == "*" || filtroMaestros[grupo.Materia.Clave].Contains(grupo.Maestro))
                        {
                            if (filtroCentros == null || filtroCentros.Contains(grupo.CentroUniversitario))
                            {
                                var huecosPorDia = combinacionActual.HuecosSiAgregoGrupo(grupo);

                                if (huecosPorDia.Select(m => m.Value.Count()).Max() <= this.maxHuecosPorDia && //Selecciona cuantos huecos hay en cada día, y el máximo de esos huecos por día
                                    huecosPorDia.SelectMany(m => m.Value).All(h => h <= maxDuracionPorHueco)) //Checa que todos los huecos sean menor o igual que MaxDuracion
                                {
                                    grupoValido = true;
                                    combinacionActual.Grupos.Add(grupo);
                                    ExploraCombinaciones(combinacionActual, materias.Skip(1));
                                    combinacionActual.Grupos.Remove(grupo);
                                }
                            }
                        }
                    }

                    //si el grupo no es valido, porque colisiona o el maestro no esta en la lista permitida (filtro)
                    //pero el combinador permite horarios sin todos las materias, seguimos explorando la combinación
                    //actual, saltandonos esta materia.
                    if (!grupoValido && this.permiteHorariosIncompletos)
                    {
                        ExploraCombinaciones(combinacionActual, materias.Skip(1));
                    }
                }
            }
        }
    }
}