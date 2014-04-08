using System;
using System.Collections.Generic;
using System.Linq;
using Apeiron.Core.Models;
using NodaTime;

namespace Apeiron.Core
{
    public class Combinador
    {
        public Combinador()
        {
            CombinacionesDeGrupos = new HashSet<CombinacionGrupos>();
        }
        private HashSet<CombinacionGrupos> CombinacionesDeGrupos { get; set; }

        private int maxHuecosPorDia = 0;
        private TimeSpan maxDuracionPorHueco;
        private int numMaterias = 0;
        private int numCiclos = 0;
        private HashSet<string> filtroCentros;
        private ILookup<string, string> filtroMaestros;
        private bool soloConCupo;
        private bool permiteHorariosIncompletos;

        public HashSet<CombinacionGrupos> EncuentraTodos(List<Materia> materias, TimeSpan maxDuracionPorHueco, ILookup<string, string> filtroMaestros = null, HashSet<string> filtroCentros = null, int maxHuecosPorDia = int.MaxValue, bool soloConCupo = true, bool permiteHorariosIncompletos = true)
        {
            numCiclos = 0;
            this.permiteHorariosIncompletos = permiteHorariosIncompletos;
            this.soloConCupo = soloConCupo;
            this.filtroMaestros = filtroMaestros;
            this.filtroCentros = filtroCentros;
            this.maxDuracionPorHueco = maxDuracionPorHueco;
            this.maxHuecosPorDia = maxHuecosPorDia;
            this.CombinacionesDeGrupos = new HashSet<CombinacionGrupos>();
            this.numMaterias = materias.Count();

            if (filtroCentros != null)
            {
                foreach (var materia in materias)
                {
                    materia.Grupos = materia.Grupos.Where(g => filtroCentros.Contains(g.CentroUniversitario));
                }
            }

            if (filtroMaestros != null)
            {
                foreach (var materia in materias)
                {
                    var filtro = filtroMaestros[materia.Clave];
                    if (filtro.Contains("*"))
                    {
                        continue;
                    }
                    materia.Grupos = materia.Grupos.Where(g => filtro.Contains(g.Maestro));
                }
            }

            ExploraCombinaciones(new CombinacionGrupos(materias.Count), materias.ToArray(), 0);

            return CombinacionesDeGrupos;
        }

        HashSet<string> MateriasConGruposOrdenados = new HashSet<string>();

        /// <summary>
        /// Funcion Recursiva que arma toda las combinaciones posibles de los grupos, siempre y cuando
        /// no sea limitado por el numero de ciclos o por especificaciones como numero de huecos o 
        /// solo permitir horarios completos (que tienen todas las materias seleccionadas)
        /// </summary>
        private void ExploraCombinaciones(CombinacionGrupos combinacionActual, Materia[] materias, int i)
        {
            if (numCiclos++ > 20000)
            {
                return;
            }

            //Si ya n hay materias, ya terminamos las combinaciones posibles.
            if (i >= materias.Length)
            {
                if (combinacionActual.Grupos.Count == 0)
                {
                    return;
                }

                if (combinacionActual.Grupos.Count == numMaterias)
                {
                    combinacionActual.Completo = true;
                }
                else
                {
                    combinacionActual.Completo = false;
                }

                // if (!CombinacionesDeGrupos.Contains(combinacionActual))

                CombinacionesDeGrupos.Add(combinacionActual.Clone());


                return;
            }
            else
            {
                //Dado que limitamos el número de permutaciones posibles, ordenar los grupos por cupo primero
                //asegura que los primeros horarios que se encuentren tengan cupo.
                if (!MateriasConGruposOrdenados.Contains(materias[i].Clave))
                {
                    materias[i].Grupos = materias[i].Grupos.OrderByDescending(g => g.CupoDisponible);
                    MateriasConGruposOrdenados.Add(materias[i].Clave);
                }
                foreach (var grupo in materias[i].Grupos)
                {
                    var grupoValido = false;
                    if (!soloConCupo || (soloConCupo && grupo.CupoDisponible > 0))
                    {
                        if (!combinacionActual.ColisionaCon(grupo))
                        {
                            var huecosPorDia = combinacionActual.HuecosSiAgregoGrupo(grupo);

                            if (HuecosAceptables(huecosPorDia.Select(e => e.Value))) //Checa que todos los huecos sean menor o igual que MaxDuracion
                            {
                                combinacionActual.HuecosPorDia = huecosPorDia;
                                grupoValido = true;
                                combinacionActual.Grupos.Add(grupo);
                                ExploraCombinaciones(combinacionActual, materias, i + 1);
                                combinacionActual.Grupos.Remove(grupo);
                            }
                        }
                    }

                    //si el grupo no es valido, porque colisiona o el maestro no esta en la lista permitida (filtro)
                    //pero el combinador permite horarios sin todos las materias, seguimos explorando la combinación
                    //actual, saltandonos esta materia.
                    if (!grupoValido && permiteHorariosIncompletos)
                    {
                        ExploraCombinaciones(combinacionActual, materias, i + 1);
                    }
                }
            }
        }

        private bool HuecosAceptables(IEnumerable<List<Period>> huecosPorDia)
        {
            if (huecosPorDia.Count() == 0)
            {
                return true;
            }

            if (huecosPorDia.Select(l => l.Count).Max() >= maxHuecosPorDia)
            {
                return false;
            }

            if (huecosPorDia.SelectMany(periodo => periodo).Any(periodo => periodo.Minutes > maxDuracionPorHueco.Minutes))
            {
                return false;
            }

            return true;
        }
    }
}