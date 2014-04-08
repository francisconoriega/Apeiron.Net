using System.Collections.Generic;

namespace Apeiron.Core.Models
{
    public class Grupo
    {
        public Grupo()
        {
            this.LugaresHoras = new List<CuandoYDonde>();
        }
        public string CentroUniversitario { get; set; }
        public string Maestro { get; set; }
        public string Seccion { get; set; }
        public string NRC { get; set; }
        public int Cupo { get; set; }
        public int CupoDisponible { get; set; }
        public string Periodo { get; set; }

        public IList<CuandoYDonde> LugaresHoras { get; set; }
        public Materia Materia { get; set; }

        public override string ToString()
        {
            string str = this.NRC + "|" + this.Materia.Clave + ": ";

            foreach (var lh in LugaresHoras)
            {
                foreach (var dia in lh.Dias)
                {
                    str += dia.ToString() + " ";
                }
                str += lh.HoraInicio.ToString() + " " + lh.HoraFin.ToString();
            }

            str += "| cupo: " + this.CupoDisponible + "/" + this.Cupo;
            return str;
        }

        public override bool Equals(object obj)
        {
            return this.NRC == (obj as Grupo).NRC;
        }

        public override int GetHashCode()
        {
            return this.NRC.GetHashCode();
        }
    }
}