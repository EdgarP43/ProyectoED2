using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProyectoED2.Modelos;
using ProyectoED2.ArbolBStar;
using System.IO;

namespace ProyectoED2.Data
{
    public class Archivo
    {
        public string d1 = @"D:\Escritorio\ProyecEDII\ProyectoED2\obj\producto.csv";
        public string d2 = @"D:\Escritorio\ProyecEDII\ProyectoED2\obj\sucursal-producto.csv";
        public string d3 = @"D:\Escritorio\ProyecEDII\ProyectoED2\obj\sucursal.csv";
        public ArbolProducto productos = new ArbolProducto(9);
        public ArbolSucursal sucursales = new ArbolSucursal(9);
        public ArbolSP inventario = new ArbolSP(9);
        public List<Sucursal> rSucursal = new List<Sucursal>();
        public List<Producto> rProductos = new List<Producto>();
        public List<SucursalPrecio> rInventario = new List<SucursalPrecio>();
        public void insertarSucursal(Sucursal nueva)
        {
            string escribir = nueva.id.ToString() + "," + nueva.nombre + "," + nueva.direccion;
            using (StreamWriter sw = new StreamWriter(d3))
            {
                sw.WriteLine(escribir);
            }
            sucursales.Insertar(nueva);
            rSucursal.Add(nueva);
        }
        public void insertarProducto(Producto nueva)
        {
            string escribir = nueva.id.ToString() + "," + nueva.nombre + "," + nueva.precio;
            using (StreamWriter sw = new StreamWriter(d1))
            {
                sw.WriteLine(escribir);
            }
            productos.Insertar(nueva);
            rProductos.Add(nueva);
        }
        public void insertarSyP(SucursalPrecio nueva)
        {
            string escribir = nueva.idSucursal.ToString() + "," + nueva.idProducto.ToString() + "," + nueva.cantidadInv;
            using (StreamWriter sw = new StreamWriter(d2))
            {
                sw.WriteLine(escribir);
            }
            inventario.Insertar(nueva);
        }
        public void leerSucursales()
        {
            using (StreamReader sr = new StreamReader(d3))
            {
                string linea = "";
                while ((linea = sr.ReadLine()) != null)
                {
                    string[] sucursal = linea.Split(',');
                    Sucursal creado = new Sucursal();
                    creado.id = Convert.ToInt32(sucursal[0]);
                    creado.nombre = sucursal[1];
                    creado.direccion = sucursal[2];
                    sucursales.Insertar(creado);
                }
            }
        }
        public void leerProductos()
        {
            using (StreamReader sr = new StreamReader(d1))
            {
                string linea = "";
                while ((linea = sr.ReadLine()) != null)
                {
                    string[] sucursal = linea.Split(',');
                    Producto creado = new Producto();
                    creado.id = Convert.ToInt32(sucursal[0]);
                    creado.nombre = sucursal[1];
                    creado.precio = Convert.ToDouble(sucursal[2]);
                    productos.Insertar(creado);

                }
            }
        }
        public void leerInventario()
        {
            using (StreamReader sr = new StreamReader(d2))
            {
                string linea = "";
                while ((linea = sr.ReadLine()) != null)
                {
                    string[] sucursal = linea.Split(',');

                    SucursalPrecio creado = new SucursalPrecio();
                    creado.idSucursal = Convert.ToInt32(sucursal[0]);
                    creado.idProducto = Convert.ToInt32(sucursal[1]);
                    creado.cantidadInv = Convert.ToInt32(sucursal[2]);
                    inventario.Insertar(creado);
                }
            }
        }
        public void actualizarSucursales(Sucursal actualizar)
        {
            File.Delete(d3);
            ArbolSucursal nuevo = new ArbolSucursal(9);
            using (StreamWriter sw1 = File.CreateText(d3))
            foreach (var item in rSucursal)
            {
                if (item.id == actualizar.id)
                {
                    item.nombre = actualizar.nombre;
                    item.direccion = actualizar.direccion;
                }
                using (StreamWriter sw = new StreamWriter(d3))
                {
                   sw.WriteLine(item.id.ToString() + "," + item.nombre + "," + item.direccion);
                }
                    nuevo.Insertar(item);
            }
            sucursales.raiz = nuevo.raiz;
        }
        public void actualizarProducto(Producto actualizar)
        {
            File.Delete(d1);
            ArbolProducto nuevo = new ArbolProducto(9);
            using (StreamWriter sw1 = File.CreateText(d1))
            foreach (var item in rProductos)
            {
                if (item.id == actualizar.id)
                {
                    item.nombre = actualizar.nombre;
                    item.precio = actualizar.precio;
                }
                using (StreamWriter sw = new StreamWriter(d1))
                {
                    sw.WriteLine(item.id.ToString() + "," + item.nombre + "," + item.precio.ToString());
                }
                    nuevo.Insertar(item);
            }
            productos.raiz = nuevo.raiz;

        }
        public void actualizarInventarioQty(SucursalPrecio actualizar)
        {
            File.Delete(d2);
            ArbolSP nuevo = new ArbolSP(9);
            using (StreamWriter sw1 = File.CreateText(d2))
            foreach (var item in rInventario)
            {
                if ((item.idSucursal == actualizar.idSucursal) && (item.idProducto == item.idProducto))
                {
                    item.cantidadInv += actualizar.cantidadInv;
                }
                using (StreamWriter sw = new StreamWriter(d2))
                {
                    sw.WriteLine(item.idSucursal.ToString() + "," + item.idProducto.ToString() + "," + item.cantidadInv.ToString());
                }
                nuevo.Insertar(item);
            }
            inventario.raiz = nuevo.raiz;

        }
    }
}
