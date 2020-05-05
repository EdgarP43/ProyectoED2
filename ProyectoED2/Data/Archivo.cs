using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoED2.Data
{
    public class Archivo
    {
        public string NombreArchivo { get; set; }
        public double Razon { get; set; }
        public double Factor { get; set; }
        public double Porcentaje { get; set; }
        public string NuevoNombre { get; set; }
        public string RutaNuevoNombre { get; set; }

        public Archivo()
        {
            NombreArchivo = "";
            RutaNuevoNombre = "";
            NuevoNombre = "";
            Razon = 0;
            Factor = 0;
            Porcentaje = 0;
        }
    }
}
