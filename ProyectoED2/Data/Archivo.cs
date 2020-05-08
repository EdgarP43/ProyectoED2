using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProyectoED2.Modelos;
using System.IO;

namespace ProyectoED2.Data
{
    public class Archivo
    {
        public string d1 = "";
        public string d2 = "";
        public string d3 = "";
        public List<Sucursal> sucursales = new List<Sucursal>();
        public List<string> idSucursal = new List<string>();
        public List<Producto> productos = new List<Producto>();
        public List<string> idProductos = new List<string>();
        public List<SucursalPrecio> inventario = new List<SucursalPrecio>();
        public void insertarSucursal(Sucursal nueva)
        {
            string escribir = nueva.id.ToString() + "," + nueva.nombre + "," + nueva.direccion;
            using (StreamWriter sw = new StreamWriter(d3))
            {
                sw.WriteLine(escribir);
            }
            sucursales.Add(nueva);
            idSucursal.Add(nueva.id.ToString());
        }
        public void insertarProducto(Producto nueva)
        {
            string escribir = nueva.id.ToString() + "," + nueva.nombre + "," + nueva.precio;
            using (StreamWriter sw = new StreamWriter(d1))
            {
                sw.WriteLine(escribir);
            }
            productos.Add(nueva);
            idProductos.Add(nueva.id.ToString());
        }
        public void insertarSyP(SucursalPrecio nueva)
        {
            string escribir = nueva.idSucursal.ToString() + "," + nueva.idProducto.ToString() + "," + nueva.cantidadInv;
            using (StreamWriter sw = new StreamWriter(d2))
            {
                sw.WriteLine(escribir);
            }
            inventario.Add(nueva);
        }
        public void leerSucursales(string direccion)
        {
            using (StreamReader sr = new StreamReader(direccion))
            {
                string linea = null;
                while ((linea = sr.ReadLine()) == null)
                {
                    string[] sucursal = linea.Split(',');
                    Sucursal creado = new Sucursal();
                    creado.id = Convert.ToInt32(sucursal[0]);
                    creado.nombre = sucursal[1];
                    creado.direccion = sucursal[2];
                    sucursales.Add(creado);
                }
            }
        }
        public void leerProductos(string direccion)
        {
            using (StreamReader sr = new StreamReader(direccion))
            {
                string linea = null;
                while ((linea = sr.ReadLine()) == null)
                {
                    string[] sucursal = linea.Split(',');
                    Producto creado = new Producto();
                    creado.id = Convert.ToInt32(sucursal[0]);
                    creado.nombre = sucursal[1];
                    creado.precio = Convert.ToDouble(sucursal[2]);
                    productos.Add(creado);

                }
            }
        }
        public void leerInventario(string direccion)
        {
            using (StreamReader sr = new StreamReader(direccion))
            {
                string linea = null;
                while ((linea = sr.ReadLine()) == null)
                {
                    string[] sucursal = linea.Split(',');

                    SucursalPrecio creado = new SucursalPrecio();
                    creado.idSucursal = Convert.ToInt32(sucursal[0]);
                    creado.idProducto = Convert.ToInt32(sucursal[1]);
                    creado.cantidadInv = Convert.ToInt32(sucursal[2]);
                    inventario.Add(creado);
                }
            }
        }
        public void actualizarSucursales(Sucursal actualizar, string direccion)
        {
            File.Delete(direccion);
            using (StreamWriter sw1 = File.CreateText(direccion))
                foreach (var item in sucursales)
                {
                    if (item.id == actualizar.id)
                    {
                        item.nombre = actualizar.nombre;
                        item.direccion = actualizar.direccion;
                    }
                    using (StreamWriter sw = new StreamWriter(direccion))
                    {
                        sw.WriteLine(item.id.ToString() + "," + item.nombre + "," + item.direccion);
                    }
                }

        }
        public void actualizarProducto(Producto actualizar, string direccion)
        {
            File.Delete(direccion);
            using (StreamWriter sw1 = File.CreateText(direccion))
                foreach (var item in productos)
                {
                    if (item.id == actualizar.id)
                    {
                        item.nombre = actualizar.nombre;
                        item.precio = actualizar.precio;
                    }
                    using (StreamWriter sw = new StreamWriter(direccion))
                    {
                        sw.WriteLine(item.id.ToString() + "," + item.nombre + "," + item.precio.ToString());
                    }
                }

        }
        public void actualizarInventarioQty(SucursalPrecio actualizar, string direccion)
        {
            File.Delete(direccion);
            using (StreamWriter sw1 = File.CreateText(direccion))
                foreach (var item in inventario)
                {
                    if ((item.idSucursal == actualizar.idSucursal) && (item.idProducto == item.idProducto))
                    {
                        item.cantidadInv += actualizar.cantidadInv;
                    }
                    using (StreamWriter sw = new StreamWriter(direccion))
                    {
                        sw.WriteLine(item.idSucursal.ToString() + "," + item.idProducto.ToString() + "," + item.cantidadInv.ToString());
                    }
                }

        }
    }
}
