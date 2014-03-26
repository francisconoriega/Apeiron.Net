using Apeiron.Core.Models;
using Newtonsoft.Json;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Apeiron.Core
{
    public class CombinacionGrupos
    {
        private static readonly List<IsoDayOfWeek> SEMANA = new List<IsoDayOfWeek> { IsoDayOfWeek.Monday, IsoDayOfWeek.Tuesday, IsoDayOfWeek.Wednesday, IsoDayOfWeek.Thursday, IsoDayOfWeek.Friday, IsoDayOfWeek.Saturday };
        public CombinacionGrupos()
        {
            this.Grupos = new ObservableCollection<Grupo>();
            this.HuecosPorDia = new Dictionary<IsoDayOfWeek, List<TimeSpan>>();
            InitHuecosPorDia(this.HuecosPorDia);
            this.Grupos.CollectionChanged += Grupos_CollectionChanged;

        }

        void Grupos_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //TODO: No tener que calcular todos los huecos cada vez que se modifica la lista de grupos en esta combinación.
            //Solamente hacerlo para los días en que la nueva clase caiga
            this.HuecosPorDia = this.CalculaHuecos(this.Grupos);
        }

        private void InitHuecosPorDia(Dictionary<IsoDayOfWeek, List<TimeSpan>> huecos)
        {
            foreach (var dia in SEMANA)
            {
                huecos.Add(dia, new List<TimeSpan>());
            }
        }
        public ObservableCollection<Grupo> Grupos { get; set; }
        public Dictionary<IsoDayOfWeek, List<TimeSpan>> HuecosPorDia { get; set; }

        public bool ColisionaCon(Grupo otroGrupo)
        {
            //Esto se leería más fácil en un lenguaje funcional. Básicamente dice:
            //Si cualquiera de las horas de cualquiera de los grupos en this.Grupos (lugarHora) colisiona
            //con cualquiera de las horas del otroGrupo (otroGrupoLugarHora)

            if (this.Grupos.Any(g => g.LugaresHoras.Any(lugarHora => otroGrupo.LugaresHoras.Any(otroGrupoLugarHora => lugarHora.ColisionaCon(otroGrupoLugarHora)))))
            {
                return true;
            }
            return false;
        }

        internal CombinacionGrupos Clone()
        {
            CombinacionGrupos cg = new CombinacionGrupos();
            cg.Grupos = new ObservableCollection<Grupo>(this.Grupos);
            //Serializar y deserializar para hacer un clon del objeto.
            cg.HuecosPorDia = JsonConvert.DeserializeObject<Dictionary<IsoDayOfWeek, List<TimeSpan>>>(JsonConvert.SerializeObject(this.HuecosPorDia));
            cg.Completo = this.Completo;

            return cg;
        }

        public bool Completo { get; set; }

        private Dictionary<IsoDayOfWeek, List<TimeSpan>> CalculaHuecos(IEnumerable<Grupo> grupos)
        {
            var huecos = new Dictionary<IsoDayOfWeek, List<TimeSpan>>();
            InitHuecosPorDia(huecos);

            var horas = grupos.SelectMany(g => g.LugaresHoras);

            foreach (var dia in SEMANA)
            {
                var clasesEnDia = horas.Where(h => h.Dias.Contains(dia)).OrderBy(c => c.HoraInicio);
                for (int i = 0; i < clasesEnDia.Count() - 1; i++)
                {
                    //Clases terminan a las :55, hay que sumar 5 minutos para ver si hay hueco con el inicio de la siguiente clase o no.
                    if (clasesEnDia.ElementAt(i).HoraFin + Period.FromMinutes(5) != clasesEnDia.ElementAt(i + 1).HoraInicio)
                    {
                        //Desafortunadamente no hay Intervalo para LocalDateTime, así que tenemos que convertirlo en Instante.
                        var i1 = new Instant((clasesEnDia.ElementAt(i).HoraFin.LocalDateTime + Period.FromMinutes(5)).TickOfDay);
                        var i2 = new Instant(clasesEnDia.ElementAt(i + 1).HoraInicio.LocalDateTime.TickOfDay);
                        var inter = new Interval(i1, i2);
                        huecos[dia].Add(inter.Duration.ToTimeSpan());
                    }
                }
            }
            return huecos;
        }

        //Calcula los huecos si se agrega ese grupo, sin afectar los huecos actuales.
        internal Dictionary<IsoDayOfWeek, List<TimeSpan>> HuecosSiAgregoGrupo(Grupo grupo)
        {
            var tempGrupos = new List<Grupo>(this.Grupos);
            tempGrupos.Add(grupo);

            return this.CalculaHuecos(tempGrupos);
        }


        public TimeSpan DuracionHuecoMasGrande()
        {
            //busca en todos los días los huecos de cada día y regresa el mas grande de cada día o lo mas pequeño si no hay,
            //despues agarra el más grande de los esos valores.
            return this.HuecosPorDia.Select(m => m.Value.Count > 0 ? m.Value.Max() : TimeSpan.FromTicks(0)).Max();
        }

        public int MaxNumHuecosEnUnDia()
        {
            return this.HuecosPorDia.Select(m => m.Value.Count()).Max();
        }

        //Ayuda a debugear
        public override string ToString()
        {
            string str = string.Empty;
            str += "Hueco Grande: " + DuracionHuecoMasGrande();
            str += " | #Max huecos en un dia:" + MaxNumHuecosEnUnDia();
            str += " | Cupo: " + this.Grupos.Select(g => g.CupoDisponible).Sum() + "/" + this.Grupos.Select(g => g.Cupo).Sum();
            return str;
        }
    }
}
