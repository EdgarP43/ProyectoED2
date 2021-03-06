﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProyectoED2.Modelos;

namespace ProyectoED2.ArbolBStar
{
    public class NodoS
    {
        int gradoMaximo;
        public NodoS[] hijos { get; set; }
        public NodoS Padre { get; set; }
        public int indiceHijoDePadre { get; set; }
        public Sucursal[] Llaves { get; set; }
        public bool esHoja { get; set; }
        public bool estaLleno { get; set; }
        public int tamaño { get; set; }
        public bool esRaiz { get; set; }
        public string lineaNodo { get; set; }
        public int[] lineasHijos { get; set; }
        // public string[] lineasDatos { get; set; 
        public List<string> lineasDatos = new List<string>();
        public NodoS(int grado)
        {
            gradoMaximo = grado;
            Llaves = new Sucursal[grado - 1];
            hijos = new NodoS[grado];

        }
    }
}
