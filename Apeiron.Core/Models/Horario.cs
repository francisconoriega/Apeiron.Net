using System.Collections.Generic;
using System.Linq;
using NodaTime;

namespace Apeiron.Core.Models
{
    public class CuandoYDonde
    {
        public LocalTime HoraInicio { get; set; }
        public LocalTime HoraFin { get; set; }
        public HashSet<IsoDayOfWeek> Dias { get; set; }
        public string Edificio { get; set; }
        public string Aula { get; set; }

        public bool ColisionaCon(CuandoYDonde otro)
        {
            //si ninguno los días de otro es igual a mis días, no colisiona.
            if (!this.Dias.Any(diaA => otro.Dias.Any(diaOtro => diaOtro == diaA)))
            {
                return false;
            }

            //Si si tienen mismos días, checar la hora
            //El otro empieza cuando yo estoy en progreso.
            if (otro.HoraInicio >= this.HoraInicio && otro.HoraInicio <= this.HoraFin)
            {
                return true;
            }

            //Otro no se acaba antes de que empiece yo.
            if (otro.HoraFin >= this.HoraInicio && otro.HoraFin <= this.HoraFin)
            {
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return string.Join(",", Dias) + ": " + HoraInicio + " - " + HoraFin;
        }
    }
}