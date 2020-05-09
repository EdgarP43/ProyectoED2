using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProyectoED2.Modelos;

namespace ProyectoED2.ArbolBStar
{
    public class ArbolProducto
    {
        public NodoP raiz;
        private int tamanioMax;
        public List<Producto> contenidoArbol = new List<Producto>();
        public ArbolProducto(int tamanioMax)
        {
            if (tamanioMax < 8)
            {
                throw new Exception("El grado del arbol debe ser de almenos 5");
            }
            this.tamanioMax = tamanioMax;
            raiz = new NodoP(tamanioMax + 1);
            raiz.esHoja = true;
        }
        public void Insertar(Producto dato)
        {
            InsertarNodoP(raiz, dato);
        }
        private void InsertarNodoP(NodoP padre, Producto dato)
        {
            int index = padre.tamaño - 1;
            if (padre.tamaño > 0)
                while (index >= 0 && dato.id < padre.Llaves[index].id)
                {
                    index--;
                }
            index++;
            if (raiz.estaLleno)
            {
                partirRaiz(padre);
                InsertarNodoP(raiz, dato);
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
                InsertarNodoP(padre.hijos[index], dato);
            }
        }
        private void InsertarEnHoja(NodoP padre, int index, Producto dato)
        {
            if (padre.hijos[index].estaLleno) //NodoP lleno en el cual quiero insertar
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
        private void InsertarNoLleno(NodoP actual, Producto dato)
        {
            var i = actual.tamaño - 1;
            var m = actual.tamaño;

            //Cuando inserto en rama y si tiene hijos
            if (!actual.esHoja)
                while (i >= 0 && dato.id < actual.Llaves[i].id)
                {
                    actual.Llaves[i + 1] = actual.Llaves[i];
                    actual.hijos[i + 2] = actual.hijos[i + 1];
                    i--;
                }
            else
                while (i >= 0 && dato.id < actual.Llaves[i].id)
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
                actual.hijos[x] = new NodoP(tamanioMax);

                int j = 0;
                for (; actual.hijos[c].Llaves[j].id < dato.id; j++)
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
        private void RotarDerecha(NodoP padre, int indexhijo, Producto dato)
        {
            int llaveDelIndexPadre;
            int tamañoNodoP = padre.hijos[indexhijo].tamaño;
            if (indexhijo == 0)
                llaveDelIndexPadre = 0;
            else if (indexhijo == tamañoNodoP)
                llaveDelIndexPadre = tamañoNodoP - 1;
            else
                llaveDelIndexPadre = indexhijo;

            int tamañoNodoPHermano = padre.hijos[indexhijo + 1].tamaño;

            //Mover elementos a la hermano derech, entonces hacemos el index 0
            for (int j = tamañoNodoPHermano - 1; j >= 0; j--)
            {
                padre.hijos[indexhijo + 1].Llaves[j + 1] = padre.hijos[indexhijo + 1].Llaves[j];
            }

            //Si el no es hoja, tengo que moverlo a los hijos
            if (!padre.hijos[indexhijo].esHoja)
            {
                for (int j = tamañoNodoPHermano; j >= 0; j--)
                {
                    padre.hijos[indexhijo + 1].hijos[0] = padre.hijos[indexhijo + 1].hijos[j];

                    //Ulitmo hijo del nodo esta lleno y enviamos el primero al nodo derecho
                    padre.hijos[indexhijo + 1].hijos[0] = padre.hijos[indexhijo].hijos[tamañoNodoPHermano];
                    padre.hijos[indexhijo].hijos[tamañoNodoPHermano] = null;
                }
            }
            padre.hijos[indexhijo + 1].Llaves[0] = padre.Llaves[llaveDelIndexPadre];
            padre.hijos[indexhijo + 1].tamaño++;
            if (padre.hijos[indexhijo].esHoja && dato.id < padre.Llaves[llaveDelIndexPadre].id
                && dato.id > padre.hijos[indexhijo].Llaves[tamañoNodoP - 1].id)
            {
                padre.Llaves[llaveDelIndexPadre] = dato;
            }
            else
            {
                padre.Llaves[llaveDelIndexPadre] = padre.hijos[indexhijo].Llaves[tamañoNodoP - 1];

                padre.hijos[indexhijo].Llaves[tamañoNodoP - 1] = null;
                padre.hijos[indexhijo].tamaño--;
                InsertarNoLleno(padre.hijos[indexhijo], dato);
            }

            if (padre.hijos[indexhijo + 1].tamaño == tamañoNodoP)
                padre.hijos[indexhijo + 1].estaLleno = true;

        }
        private void RotarIzquierda(NodoP padre, int indexhijo, Producto dato)
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
               dato.id > padre.Llaves[llaveIndexPadre].id && dato.id < padre.hijos[indexhijo].Llaves[0].id)
            {
                //Meter dato si padre es mayor que dato y dato es mayor que el hermano derecho
                padre.Llaves[llaveIndexPadre] = dato;
            }
            else
            {
                //De estar el nodo lleno, a hacerlo mas pequeño 
                padre.Llaves[llaveIndexPadre] = padre.hijos[indexhijo].Llaves[0];

                //Mover los elementos del nodo lleno al izquierdo
                int tamañoNodoP = padre.hijos[indexhijo].tamaño;
                for (int i = 0; i < tamañoNodoP - 1; i++)
                {
                    padre.hijos[indexhijo].Llaves[i] = padre.hijos[indexhijo].Llaves[i + 1];
                }

                //Si no es hoja, tengo que mover al hijo
                //mover hijo a la izquierdo en campo 1
                if (!padre.hijos[indexhijo].esHoja)
                {
                    padre.hijos[indexhijo].hijos[tamañoNodoP] = padre.hijos[indexhijo].hijos[0];
                    for (int i = 0; i < tamañoNodoP - 1; i++)
                    {
                        padre.hijos[indexhijo].hijos[i] = padre.hijos[indexhijo].hijos[i + 1];
                    }
                }

                //Eliminar ultimo elemento del nodo lleno
                padre.hijos[indexhijo].Llaves[tamañoNodoP - 1] = null;

                //Cambiar tamaño
                padre.hijos[indexhijo].tamaño--;
                padre.hijos[indexhijo].estaLleno = false;

                InsertarNoLleno(padre.hijos[indexhijo], dato);
            }
        }
        private void PartirEn23(NodoP padre, int indexHijoLleno, Producto dato, bool hermanoIzquierdo)
        {
            //Si el tiene un padre, el no es raiz
            int indexLadoIzquierdo = hermanoIzquierdo ? (indexHijoLleno - 1) : indexHijoLleno;
            int indexLadoDerecho = hermanoIzquierdo ? (indexHijoLleno) : indexHijoLleno + 1;
            int dosTercios = (tamanioMax * 2) / 3;

            int indexLlavePadre = hermanoIzquierdo ? (indexHijoLleno - 1) : (indexHijoLleno);
            int tamañoMaxNodoP = tamanioMax - 1;
            bool esHoja = padre.hijos[indexHijoLleno].esHoja;

            int j = 0;
            var arr = new Producto[tamanioMax * 2];
            var hijos = new NodoP[tamanioMax * 2];

            for (int i = 0; i < tamañoMaxNodoP; i++)
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

            while (k < tamañoMaxNodoP)
            {
                padre.hijos[indexLadoIzquierdo].Llaves[k++] = null;
                padre.hijos[indexLadoIzquierdo].tamaño--;
            }
            padre.hijos[indexLadoIzquierdo].estaLleno = false;

            Producto padre1 = arr[j++];

            padre.Llaves[indexLlavePadre] = padre1;

            for (k = 0; k < dosTercios; k++)
            {
                padre.hijos[indexLadoDerecho].Llaves[k] = arr[j++];
            }
            while (k < tamañoMaxNodoP)
            {
                padre.hijos[indexLadoDerecho].Llaves[k++] = null;
                padre.hijos[indexLadoDerecho].tamaño--;
            }
            padre.hijos[indexLadoDerecho].estaLleno = false;

            Producto padre2 = arr[j++];

            var nuevoNodoP = new NodoP(tamanioMax);

            for (k = 0; j < tamanioMax * 2; k++)
            {
                nuevoNodoP.Llaves[k] = arr[j++];
                nuevoNodoP.tamaño++;
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
                nodoDerecho.Padre.hijos[m + 2] = nuevoNodoP;
                nodoDerecho.Padre.tamaño++;
                nuevoNodoP.Padre = nodoDerecho.Padre;
                nuevoNodoP.esHoja = esHoja;
            }
            if (nodoDerecho.Padre.tamaño == tamanioMax - 1)
                nodoDerecho.Padre.estaLleno = true;

        }
        private void partirRaiz(NodoP actual)
        {
            int indexMedio = actual.tamaño / 2;
            var nuevaRaiz = new NodoP(tamanioMax);
            var nuevoNodoP = new NodoP(tamanioMax);

            int j = 0;
            int i = indexMedio + 1;
            if (!actual.esHoja)
            {
                while (i < tamanioMax - 1)
                {
                    nuevoNodoP.Llaves[j] = actual.Llaves[i];
                    nuevoNodoP.hijos[j] = actual.hijos[i];
                    nuevoNodoP.hijos[j].Padre = nuevoNodoP;

                    actual.Llaves[i] = null;
                    actual.hijos[i] = null;
                    actual.tamaño--;
                    j++;
                    i++;
                }
                nuevoNodoP.hijos[j] = actual.hijos[i];
                actual.hijos[i] = null;
            }
            else
            {
                while (i < tamanioMax - 1)
                {
                    nuevoNodoP.Llaves[j] = actual.Llaves[i];
                    actual.Llaves[i] = null;
                    actual.tamaño--;
                    nuevoNodoP.tamaño++;
                    i++;
                    j++;
                }
                nuevoNodoP.esHoja = true;
                actual.esHoja = true;
            }

            actual.Padre = nuevaRaiz;
            actual.indiceHijoDePadre = 0;

            nuevoNodoP.Padre = nuevaRaiz;
            nuevoNodoP.indiceHijoDePadre = 1;
            nuevaRaiz.Padre = null;
            nuevaRaiz.Llaves[0] = actual.Llaves[indexMedio];
            nuevaRaiz.hijos[0] = actual;
            nuevaRaiz.hijos[1] = nuevoNodoP;
            nuevaRaiz.tamaño++;
            actual.Llaves[indexMedio] = null;
            actual.tamaño--;
            actual.estaLleno = false;
            this.raiz = nuevaRaiz;
        }
        public bool Busqueda(int id)
        {
            Producto resultado = BuscarCompa(raiz, id);
            if (resultado != null)
            {
               return true;
            }
            return false;
        }
        private Producto BuscarCompa(NodoP actual, int idB)
        {
            NodoP temporal = actual;

            int i = 0;
            while (i <= actual.tamaño - 1 && idB > actual.Llaves[i].id)
            {
                i++;
            }
            if (i <= actual.tamaño - 1 && idB == actual.Llaves[i].id)
            {
                return actual.Llaves[i];
            }
            else if (actual.esHoja)
            {
                return null;
            }
            else
            {
                return BuscarCompa(actual.hijos[i], idB);
            }
        }
        public List<Producto> Todo()
        {
            var listaTodo = Mostrar(raiz);
            return listaTodo;
        }
        private List<Producto> Mostrar(NodoP nodo)
        {
            var Lista = new List<Producto>();
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
