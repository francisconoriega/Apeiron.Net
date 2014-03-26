using Newtonsoft.Json;
using System.Collections.Generic;

namespace Apeiron.Core.Models
{
    public class Materia
    {
        public Materia()
        {
            this.Grupos = new List<Grupo>();
        }
        public string Clave { get; set; }
        public string Calendario { get; set; }
        public string Nombre { get; set; }

        public int Creditos { get; set; }


        [JsonIgnore]
        public List<Grupo> Grupos { get; set; }

        public override string ToString()
        {
            return Clave;
        }


    }
}