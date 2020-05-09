using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProyectoED2.Modelos;

namespace ProyectoED2.ArbolBStar
{
    public class NodoP
    {
        int gradoMaximo;
        public NodoP[] hijos { get; set; }
        public NodoP Padre { get; set; }
        public int indiceHijoDePadre { get; set; }
        public Producto[] Llaves { get; set; }
        public bool esHoja { get; set; }
        public bool estaLleno { get; set; }
        public int tamaño { get; set; }
        public bool esRaiz { get; set; }
        public string lineaNodo { get; set; }
        public int[] lineasHijos { get; set; }
        // public string[] lineasDatos { get; set; 
        public List<string> lineasDatos = new List<string>();
        public NodoP(int grado)
        {
            gradoMaximo = grado;
            Llaves = new Producto[grado - 1];
            hijos = new NodoP[grado];

        }
    }
}
