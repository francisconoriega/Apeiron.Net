using System.Collections.Generic;

namespace Apeiron.Core.Models
{
    public class Maestro
    {
        public string Nombre { get; set; }
        public List<Grupo> Grupos { get; set; }
    }
}