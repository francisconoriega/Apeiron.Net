
using ApeironOnline.Models;
using NodaTime;
using System;
using System.Collections.Generic;
namespace ApeironOnline.Siiau
{
    public class RegistroSiiau  
    {
        public CentrosUniversitarios CentroUniversitario { get; set; }
        public string NRC { get; set; }
        public string Clave { get; set; }
        public string Materia { get; set; }
        public string SeccionId { get; set; }
        public int Creditos { get; set; }
        public int Cupo { get; set; }
        public int CupoDisponible { get; set; }
        public string Ses { get; set; }
        public string Horario { get; set; }
        public List<IsoDayOfWeek> Dias { get; set; }
        public string Edificio { get; set; }
        public string Aula { get; set; }
        public string Periodo { get; set; }
        public string Ses2 { get; set; }
        public string Profesor { get; set; }
    }
}