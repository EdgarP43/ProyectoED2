using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProyectoED2.Modelos;
using ProyectoED2.ArbolBStar;

namespace ProyectoED2.ArbolBStar
{
    public class ArbolSP
    {
        private NodoSP raiz;
        private int tamanioMax;
        public List<SucursalPrecio> contenidSucursaloArbol = new List<SucursalPrecio>();
        public ArbolSP(int tamanioMax)
        {
            if (tamanioMax < 8)
            {
                throw new Exception("El grado del arbol debe ser de almenos 5");
            }
            this.tamanioMax = tamanioMax;
            raiz = new NodoSP(tamanioMax + 1);
            raiz.esHoja = true;
        }
        public void Insertar(SucursalPrecio dato)
        {
            InsertarNodoSP(raiz, dato);
        }
        private void InsertarNodoSP(NodoSP padre, SucursalPrecio dato)
        {
            int index = padre.tamaño - 1;
            if (padre.tamaño > 0)
                while (index >= 0 && dato.idSucursal < padre.Llaves[index].idSucursal)
                {
                    index--;
                }
            index++;
            if (raiz.estaLleno)
            {
                partirRaiz(padre);
                InsertarNodoSP(raiz, dato);
            }
            else if (raiz.esHoja)
            {
                InsertarNoLleno(padre, dato);
            }
            else if (padre.hijos[index] != null && padre.hijos[index].esHoja)
            {
                InsertarEnHoja(padre, index, dato);
            }
            else
            {
                InsertarNodoSP(padre.hijos[index], dato);
            }
        }
        private void InsertarEnHoja(NodoSP padre, int index, SucursalPrecio dato)
        {
            if (padre.hijos[index].estaLleno) //NodoSP lleno en el cual quiero insertar
            {
                if (padre.hijos[index + 1] != null)// ¿Es el hermano correcto?
                {
                    if (padre.hijos[index + 1] != null)//Lleno, entonces partimos el nodo derecho
                    {
                        PartirEn23(padre, index, dato, false);

                    }
                    else //No esta lleno, entonces hago rotacion a la derecha
                    {
                        RotarDerecha(padre, index, dato);
                    }
                }
                else if (padre.hijos[index - 1] != null) //no hay hermano derecho, ¿Es el hijo izquierdo?
                {
                    if (padre.hijos[index - 1].estaLleno) //Lleno, entonces partimos el nodo izquierdo
                    {
                        PartirEn23(padre, index, dato, false);
                    }
                    else//No esta lleno, entonces hago rotacion a la derecha
                    {
                        RotarIzquierda(padre, index, dato);
                    }
                }
            }
            else
            {
                InsertarNoLleno(padre.hijos[index], dato);
            }
        }
        private void InsertarNoLleno(NodoSP actual, SucursalPrecio dato)
        {
            var i = actual.tamaño - 1;
            var m = actual.tamaño;

            //Cuando inserto en rama y si tiene hijos
            if (!actual.esHoja)
                while (i >= 0 && dato.idSucursal < actual.Llaves[i].idSucursal)
                {
                    actual.Llaves[i + 1] = actual.Llaves[i];
                    actual.hijos[i + 2] = actual.hijos[i + 1];
                    i--;
                }
            else
                while (i >= 0 && dato.idSucursal < actual.Llaves[i].idSucursal)
                {
                    actual.Llaves[i + 1] = actual.Llaves[i];
                    i--;
                }
            actual.Llaves[i + 1] = dato;
            actual.tamaño++;

            //Mover hijo si no es hoja
            if (!actual.esHoja)
            {
                var x = i + 1;
                var c = i + 2;
                actual.hijos[c] = actual.hijos[x];
                actual.hijos[x] = new NodoSP(tamanioMax);

                int j = 0;
                for (; actual.hijos[c].Llaves[j].idSucursal < dato.idSucursal; j++)
                {
                    actual.hijos[x].Llaves[j] = actual.hijos[c].Llaves[j];
                    if (!actual.hijos[c].esHoja)
                    {
                        actual.hijos[x].hijos[j] = actual.hijos[c].hijos[j];
                    }

                    actual.hijos[c].tamaño--;
                    actual.hijos[x].tamaño++;
                }
                int k;
                for (k = j; k < m; k++)
                {
                    actual.hijos[c].Llaves[k - j] = actual.hijos[c].Llaves[k];
                    actual.hijos[c].Llaves[k] = null;
                    if (!actual.hijos[c].esHoja)
                    {
                        actual.hijos[c].hijos[k - j] = actual.hijos[c].hijos[k];
                        actual.hijos[c].hijos[k] = null;
                    }
                }
                if (!actual.hijos[c].esHoja)
                {
                    actual.hijos[c].hijos[actual.hijos[c].tamaño] = actual.hijos[c].hijos[k - j];
                }
                if (actual.tamaño == tamanioMax - 1)
                    actual.estaLleno = true;
            }
        }
        private void RotarDerecha(NodoSP padre, int indexhijo, SucursalPrecio dato)
        {
            int llaveDelIndexPadre;
            int tamañoNodoSP = padre.hijos[indexhijo].tamaño;
            if (indexhijo == 0)
                llaveDelIndexPadre = 0;
            else if (indexhijo == tamañoNodoSP)
                llaveDelIndexPadre = tamañoNodoSP - 1;
            else
                llaveDelIndexPadre = indexhijo;

            int tamañoNodoSPHermano = padre.hijos[indexhijo + 1].tamaño;

            //Mover elementos a la hermano derech, entonces hacemos el index 0
            for (int j = tamañoNodoSPHermano - 1; j >= 0; j--)
            {
                padre.hijos[indexhijo + 1].Llaves[j + 1] = padre.hijos[indexhijo + 1].Llaves[j];
            }

            //Si el no es hoja, tengo que moverlo a los hijos
            if (!padre.hijos[indexhijo].esHoja)
            {
                for (int j = tamañoNodoSPHermano; j >= 0; j--)
                {
                    padre.hijos[indexhijo + 1].hijos[0] = padre.hijos[indexhijo + 1].hijos[j];

                    //Ulitmo hijo del nodo esta lleno y enviamos el primero al nodo derecho
                    padre.hijos[indexhijo + 1].hijos[0] = padre.hijos[indexhijo].hijos[tamañoNodoSPHermano];
                    padre.hijos[indexhijo].hijos[tamañoNodoSPHermano] = null;
                }
            }
            padre.hijos[indexhijo + 1].Llaves[0] = padre.Llaves[llaveDelIndexPadre];
            padre.hijos[indexhijo + 1].tamaño++;
            if (padre.hijos[indexhijo].esHoja && dato.idSucursal < padre.Llaves[llaveDelIndexPadre].idSucursal
                && dato.idSucursal > padre.hijos[indexhijo].Llaves[tamañoNodoSP - 1].idSucursal)
            {
                padre.Llaves[llaveDelIndexPadre] = dato;
            }
            else
            {
                padre.Llaves[llaveDelIndexPadre] = padre.hijos[indexhijo].Llaves[tamañoNodoSP - 1];

                padre.hijos[indexhijo].Llaves[tamañoNodoSP - 1] = null;
                padre.hijos[indexhijo].tamaño--;
                InsertarNoLleno(padre.hijos[indexhijo], dato);
            }

            if (padre.hijos[indexhijo + 1].tamaño == tamañoNodoSP)
                padre.hijos[indexhijo + 1].estaLleno = true;

        }
        private void RotarIzquierda(NodoSP padre, int indexhijo, SucursalPrecio dato)
        {
            int llaveIndexPadre;

            if (indexhijo == 0)
                llaveIndexPadre = 0;
            else if (indexhijo == tamanioMax)
                llaveIndexPadre = tamanioMax - 1;
            else
                llaveIndexPadre = indexhijo - 1;

            //Elemento de padre para hijo izquierdo
            InsertarNoLleno(padre.hijos[indexhijo - 1], padre.Llaves[llaveIndexPadre]);
            if (padre.hijos[indexhijo].esHoja &&
               dato.idSucursal > padre.Llaves[llaveIndexPadre].idSucursal && dato.idSucursal < padre.hijos[indexhijo].Llaves[0].idSucursal)
            {
                //Meter dato si padre es mayor que dato y dato es mayor que el hermano derecho
                padre.Llaves[llaveIndexPadre] = dato;
            }
            else
            {
                //De estar el nodo lleno, a hacerlo mas pequeño 
                padre.Llaves[llaveIndexPadre] = padre.hijos[indexhijo].Llaves[0];

                //Mover los elementos del nodo lleno al izquierdo
                int tamañoNodoSP = padre.hijos[indexhijo].tamaño;
                for (int i = 0; i < tamañoNodoSP - 1; i++)
                {
                    padre.hijos[indexhijo].Llaves[i] = padre.hijos[indexhijo].Llaves[i + 1];
                }

                //Si no es hoja, tengo que mover al hijo
                //mover hijo a la izquierdo en campo 1
                if (!padre.hijos[indexhijo].esHoja)
                {
                    padre.hijos[indexhijo].hijos[tamañoNodoSP] = padre.hijos[indexhijo].hijos[0];
                    for (int i = 0; i < tamañoNodoSP - 1; i++)
                    {
                        padre.hijos[indexhijo].hijos[i] = padre.hijos[indexhijo].hijos[i + 1];
                    }
                }

                //Eliminar ultimo elemento del nodo lleno
                padre.hijos[indexhijo].Llaves[tamañoNodoSP - 1] = null;

                //Cambiar tamaño
                padre.hijos[indexhijo].tamaño--;
                padre.hijos[indexhijo].estaLleno = false;

                InsertarNoLleno(padre.hijos[indexhijo], dato);
            }
        }
        private void PartirEn23(NodoSP padre, int indexHijoLleno, SucursalPrecio dato, bool hermanoIzquierdo)
        {
            //Si el tiene un padre, el no es raiz
            int indexLadoIzquierdo = hermanoIzquierdo ? (indexHijoLleno - 1) : indexHijoLleno;
            int indexLadoDerecho = hermanoIzquierdo ? (indexHijoLleno) : indexHijoLleno + 1;
            int dosTercios = (tamanioMax * 2) / 3;

            int indexLlavePadre = hermanoIzquierdo ? (indexHijoLleno - 1) : (indexHijoLleno);
            int tamañoMaxNodoSP = tamanioMax - 1;
            bool esHoja = padre.hijos[indexHijoLleno].esHoja;

            int j = 0;
            var arr = new SucursalPrecio[tamanioMax * 2];
            var hijos = new NodoSP[tamanioMax * 2];

            for (int i = 0; i < tamañoMaxNodoSP; i++)
            {
                arr[j++] = padre.hijos[indexLadoIzquierdo].Llaves[i];
                arr[j++] = padre.hijos[indexLadoDerecho].Llaves[i];
            }
            arr[j++] = dato;
            arr[j++] = padre.Llaves[indexLlavePadre];

            if (!esHoja)
            {
                int x = 0;
                for (int i = 0; i < tamanioMax; i++)
                {
                    hijos[x++] = padre.hijos[indexLadoIzquierdo].hijos[i];
                }
                for (int i = 0; i < tamanioMax; i++)
                {
                    hijos[x++] = padre.hijos[indexLadoDerecho].hijos[i];
                }
            }
            j = 0;
            int k;

            for (k = 0; k < dosTercios; k++)
            {
                padre.hijos[indexLadoIzquierdo].Llaves[k] = arr[j++];
            }

            while (k < tamañoMaxNodoSP)
            {
                padre.hijos[indexLadoIzquierdo].Llaves[k++] = null;
                padre.hijos[indexLadoIzquierdo].tamaño--;
            }
            padre.hijos[indexLadoIzquierdo].estaLleno = false;

            SucursalPrecio padre1 = arr[j++];

            padre.Llaves[indexLlavePadre] = padre1;

            for (k = 0; k < dosTercios; k++)
            {
                padre.hijos[indexLadoDerecho].Llaves[k] = arr[j++];
            }
            while (k < tamañoMaxNodoSP)
            {
                padre.hijos[indexLadoDerecho].Llaves[k++] = null;
                padre.hijos[indexLadoDerecho].tamaño--;
            }
            padre.hijos[indexLadoDerecho].estaLleno = false;

            SucursalPrecio padre2 = arr[j++];

            var nuevoNodoSP = new NodoSP(tamanioMax);

            for (k = 0; j < tamanioMax * 2; k++)
            {
                nuevoNodoSP.Llaves[k] = arr[j++];
                nuevoNodoSP.tamaño++;
            }

            var nodoDerecho = padre.hijos[indexLadoDerecho];

            if (padre.estaLleno)
            {
                if (padre.Padre == null)
                {
                    partirRaiz(padre);
                }
                else
                    InsertarEnHoja(padre.Padre, padre.indiceHijoDePadre, padre2);
            }
            else
            {
                int m = nodoDerecho.Padre.tamaño - 1;
                for (; m > indexLlavePadre; m--)
                {
                    nodoDerecho.Padre.hijos[m + 1] = nodoDerecho.Padre.hijos[m + 1];
                }
                nodoDerecho.Padre.Llaves[m + 1] = padre2;
                nodoDerecho.Padre.hijos[m + 2] = nuevoNodoSP;
                nodoDerecho.Padre.tamaño++;
                nuevoNodoSP.Padre = nodoDerecho.Padre;
                nuevoNodoSP.esHoja = esHoja;
            }
            if (nodoDerecho.Padre.tamaño == tamanioMax - 1)
                nodoDerecho.Padre.estaLleno = true;

        }
        private void partirRaiz(NodoSP actual)
        {
            int indexMedio = actual.tamaño / 2;
            var nuevaRaiz = new NodoSP(tamanioMax);
            var nuevoNodoSP = new NodoSP(tamanioMax);

            int j = 0;
            int i = indexMedio + 1;
            if (!actual.esHoja)
            {
                while (i < tamanioMax - 1)
                {
                    nuevoNodoSP.Llaves[j] = actual.Llaves[i];
                    nuevoNodoSP.hijos[j] = actual.hijos[i];
                    nuevoNodoSP.hijos[j].Padre = nuevoNodoSP;

                    actual.Llaves[i] = null;
                    actual.hijos[i] = null;
                    actual.tamaño--;
                    j++;
                    i++;
                }
                nuevoNodoSP.hijos[j] = actual.hijos[i];
                actual.hijos[i] = null;
            }
            else
            {
                while (i < tamanioMax - 1)
                {
                    nuevoNodoSP.Llaves[j] = actual.Llaves[i];
                    actual.Llaves[i] = null;
                    actual.tamaño--;
                    nuevoNodoSP.tamaño++;
                    i++;
                    j++;
                }
                nuevoNodoSP.esHoja = true;
                actual.esHoja = true;
            }

            actual.Padre = nuevaRaiz;
            actual.indiceHijoDePadre = 0;

            nuevoNodoSP.Padre = nuevaRaiz;
            nuevoNodoSP.indiceHijoDePadre = 1;
            nuevaRaiz.Padre = null;
            nuevaRaiz.Llaves[0] = actual.Llaves[indexMedio];
            nuevaRaiz.hijos[0] = actual;
            nuevaRaiz.hijos[1] = nuevoNodoSP;
            nuevaRaiz.tamaño++;
            actual.Llaves[indexMedio] = null;
            actual.tamaño--;
            actual.estaLleno = false;
            this.raiz = nuevaRaiz;
        }
        public SucursalPrecio Busqueda(int idSucursal)
        {
            SucursalPrecio resultado = BuscarCompa(raiz, idSucursal);
            if (resultado != null)
            {
                return null;
            }
            return resultado;

        }
        private SucursalPrecio BuscarCompa(NodoSP actual, int idSucursalB)
        {
            NodoSP temporal = actual;

            int i = 0;
            while (i <= actual.tamaño - 1 && idSucursalB > actual.Llaves[i].idSucursal)
            {
                i++;
            }
            if (i <= actual.tamaño - 1 && idSucursalB == actual.Llaves[i].idSucursal)
            {
                return actual.Llaves[i];
            }
            else if (actual.esHoja)
            {
                return null;
            }
            else
            {
                return BuscarCompa(actual.hijos[i], idSucursalB);
            }
        }
        public List<SucursalPrecio> Todo()
        {
            var listaTodo = Mostrar(raiz);
            return listaTodo;
        }
        private List<SucursalPrecio> Mostrar(NodoSP nodo)
        {
            var Lista = new List<SucursalPrecio>();
            if (!nodo.esHoja)
            {
                for (int i = 0; nodo.hijos[i] != null; i++)
                {
                    Mostrar(nodo.hijos[i]);
                }
            }
            for (int j = 0; j < nodo.Llaves.Length; j++)
            {
                Lista.Add(nodo.Llaves[j]);
            }
            return Lista;
        }
    }
}
