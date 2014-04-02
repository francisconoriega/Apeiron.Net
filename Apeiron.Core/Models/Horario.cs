using System.Collections.Generic;
using System.Linq;
using NodaTime;

namespace Apeiron.Core.Models
{
    public class LugarHora
    {
        public LocalTime HoraInicio { get; set; }
        public LocalTime HoraFin { get; set; }
        public HashSet<IsoDayOfWeek> Dias { get; set; }
        public string Edificio { get; set; }
        public string Aula { get; set; }

        public bool ColisionaCon(LugarHora lugarHora)
        {
            if (!this.Dias.Any(d => lugarHora.Dias.Any(lgd => lgd == d)))
            {
                return false;
            }

            if (lugarHora.HoraInicio >= this.HoraInicio && lugarHora.HoraInicio <= this.HoraFin)
            {
                return true;
            }
            if (lugarHora.HoraFin >= this.HoraInicio && lugarHora.HoraFin <= this.HoraFin)
            {
                return true;
            }

            return false;
        }
    }
}