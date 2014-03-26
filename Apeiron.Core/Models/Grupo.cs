
using System.Collections.Generic;
namespace ApeironOnline.Models
{
    public class Grupo
    {
        public Grupo()
        {
            this.LugaresHoras = new List<LugarHora>();
        }
        public CentrosUniversitarios CentroUniversitario { get; set; }
        public string Maestro { get; set; }
        public string Seccion { get; set; }
        public string NRC { get; set; }
        public int Cupo { get; set; }
        public int CupoDisponible { get; set; }
        public string Periodo { get; set; }

        public List<LugarHora> LugaresHoras { get; set; }
        public Materia Materia { get; set; }

        public override string ToString()
        {
            string str = this.Materia.Clave + ": ";

            foreach (var lh in LugaresHoras)
            {
                foreach (var dia in lh.Dias)
                {
                    str += dia.ToString() + " ";
                }
                str += lh.HoraInicio.ToString() + " " + lh.HoraFin.ToString() + "| cupo: "+this.CupoDisponible + "/"+this.Cupo;
            }
            return str;
        }
    }
}