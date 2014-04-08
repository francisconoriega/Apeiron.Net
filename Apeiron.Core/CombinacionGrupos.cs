using System;
using System.Collections.Generic;
using System.Linq;
using Apeiron.Core.Models;
using NodaTime;

namespace Apeiron.Core
{
    public class CombinacionGrupos
    {
        private static readonly HashSet<IsoDayOfWeek> SEMANA = new HashSet<IsoDayOfWeek> { IsoDayOfWeek.Monday, IsoDayOfWeek.Tuesday, IsoDayOfWeek.Wednesday, IsoDayOfWeek.Thursday, IsoDayOfWeek.Friday, IsoDayOfWeek.Saturday };

        public CombinacionGrupos(int numMaterias)
        {
            this.Grupos = new List<Grupo>(numMaterias);
            this.HuecosPorDia = new Dictionary<IsoDayOfWeek, List<Period>>();
        }
        public List<Grupo> Grupos { get; set; }
        public Dictionary<IsoDayOfWeek, List<Period>> HuecosPorDia { get; set; }

        public bool ColisionaCon(Grupo otroGrupo)
        {
            //Esto se leería más fácil en un lenguaje funcional. Básicamente dice:
            //Si cualquiera de las horas de cualquiera de los grupos en this.Grupos (lugarHora) colisiona
            //con cualquiera de las horas del otroGrupo (otroGrupoLugarHora)

            var horasDeLosGruposActuales = this.Grupos.SelectMany(g => g.LugaresHoras);
            var horasDelOtroGrupo = otroGrupo.LugaresHoras;

            if (horasDeLosGruposActuales.Any(h => horasDelOtroGrupo.Any(ho => h.ColisionaCon(ho))))
            {
                return true;
            }
            return false;
        }

        internal CombinacionGrupos Clone()
        {
            var cg = new CombinacionGrupos(this.Grupos.Count);
            cg.Grupos = new List<Grupo>(this.Grupos);

            var huecos = new Dictionary<IsoDayOfWeek, List<Period>>();

            foreach (var dia in this.HuecosPorDia)
            {
                huecos.Add(dia.Key, new List<Period>(dia.Value));
            }

            cg.HuecosPorDia = huecos;
            cg.Completo = this.Completo;

            return cg;
        }

        public bool Completo { get; set; }

        //TODO: no hacer la lista desde cero, solo agregar/quitar huecos
        private Dictionary<IsoDayOfWeek, List<Period>> CalculaHuecos(IEnumerable<Grupo> grupos)
        {
            var huecos = new Dictionary<IsoDayOfWeek, List<Period>>();

            if (grupos.Count() < 2)
            {
                return huecos;
            }

            var horas = grupos.SelectMany(g => g.LugaresHoras);

            foreach (var dia in SEMANA)
            {
                var clasesEnDia = horas.Where(h => h.Dias.Contains(dia)).OrderBy(c => c.HoraInicio).ToList();
                for (int i = 0; i < clasesEnDia.Count() - 1; i++)
                {
                    var period = Period.Between(clasesEnDia.ElementAt(i).HoraFin, clasesEnDia.ElementAt(i + 1).HoraInicio);

                    //Clases terminan a las :55, hay que tolerar al menos 5 minutos antes de considerarlo como hueco.
                    if (period.ToDuration().ToTimeSpan().TotalMinutes > 5)
                    {

                        if (!huecos.ContainsKey(dia))
                        {
                            huecos.Add(dia, new List<Period>());
                        }
                        huecos[dia].Add(period);
                    }

                    if (period.Minutes < 0)
                    {
                        throw new Exception();
                    }
                }
            }
            return huecos;
        }

        //Calcula los huecos si se agrega ese grupo, sin afectar los huecos actuales.
        internal Dictionary<IsoDayOfWeek, List<Period>> HuecosSiAgregoGrupo(Grupo grupo)
        {
            var tempGrupos = new List<Grupo>(this.Grupos);
            tempGrupos.Add(grupo);
            return this.CalculaHuecos(tempGrupos);
        }


        public TimeSpan DuracionHuecoMasGrande()
        {
            //busca en todos los días los huecos de cada día y regresa el mas grande de cada día o lo mas pequeño si no hay,
            //despues agarra el más grande de los esos valores.
            if (this.HuecosPorDia.Count == 0)
            {
                return TimeSpan.FromTicks(0);
            }
            return this.HuecosPorDia.SelectMany(h => h.Value).Max().ToDuration().ToTimeSpan();
        }

        public int MaxNumHuecosEnUnDia()
        {
            if (this.HuecosPorDia.Count == 0)
            {
                return 0;
            }
            return this.HuecosPorDia.Select(m => m.Value.Count()).Max();
        }

        //Ayuda a debugear
        public override string ToString()
        {
            string str = string.Join(",", this.Grupos.Select(g => g.NRC));
            str += "|Hueco Grande: " + DuracionHuecoMasGrande();
            str += " | #Max huecos en un dia:" + MaxNumHuecosEnUnDia();
            str += " | Cupo: " + this.Grupos.Select(g => g.CupoDisponible).Sum() + "/" + this.Grupos.Select(g => g.Cupo).Sum();
            return str;
        }

        public override int GetHashCode()
        {
            return string.Join(",", this.Grupos.Select(g => g.NRC)).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.Grupos.SequenceEqual((obj as CombinacionGrupos).Grupos);
        }

    }
}
