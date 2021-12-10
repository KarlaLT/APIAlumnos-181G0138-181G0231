using System;
using System.Collections.Generic;
using System.Text;

namespace AppAlumnos.Models
{
    public class Alumno
    {
        public int IdAlumno { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public float EspanolEv1 { get; set; }
        public float EspanolEv2 { get; set; }
        public float EspanolEv3 { get; set; }
        public float MatematicasEv1 { get; set; }
        public float MatematicasEv2 { get; set; }
        public float MatematicasEv3 { get; set; }
        public float Cnev1 { get; set; }
        public float Cnev2 { get; set; }
        public float Cnev3 { get; set; }
        public double PromEspanol { get; set; }
        public double PromCn { get; set; }
        public double PromMate { get; set; }
    }
}
