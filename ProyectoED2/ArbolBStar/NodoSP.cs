using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProyectoED2.Modelos;
using ProyectoED2.ArbolBStar;

namespace ProyectoED2.ArbolBStar
{
    public class NodoSP
    {

        int gradoMaximo;
        public NodoSP[] hijos { get; set; }
        public NodoSP Padre { get; set; }
        public int indiceHijoDePadre { get; set; }
        public SucursalPrecio[] Llaves { get; set; }
        public bool esHoja { get; set; }
        public bool estaLleno { get; set; }
        public int tamaño { get; set; }
        public bool esRaiz { get; set; }
        public string lineaNodo { get; set; }
        public int[] lineasHijos { get; set; }
        // public string[] lineasDatos { get; set; 
        public List<string> lineasDatos = new List<string>();
        public NodoSP(int grado)
        {
            gradoMaximo = grado;
            Llaves = new SucursalPrecio[grado - 1];
            hijos = new NodoSP[grado];

        }
    }
}
