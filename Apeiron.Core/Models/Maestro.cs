using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApeironOnline.Models
{
    public class Maestro
    {
        public string Nombre { get; set; }
        public List<Grupo> Grupos { get; set; }
    }
}