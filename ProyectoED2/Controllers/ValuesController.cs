using System;
using System.Collections.Generic;
using System.Linq;
using ProyectoED2.Modelos;
using System.IO;
using ProyectoED2.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoED2.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        public static Archivo Metadata = new Archivo();
        
        public class Data
        {
            public string NombreArchivo1 { get; set; }
            public string NombreArchivo2 { get; set; }
            public string NombreArchivo3 { get; set; }
        }
        public class SucursalProducto
        {
            public int idSucursal { get; set; }
            public int idProducto { get; set; }
            public int cantidadInv { get; set; }
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            Metadata.leerInventario();
            Metadata.leerProductos();
            Metadata.leerSucursales();
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [Route("add/Sucursal")]
        [HttpPost]
        public void PostSucu([FromBody] object value)
        {
            var val = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            Sucursal datos = Newtonsoft.Json.JsonConvert.DeserializeObject<Sucursal>(val);
            Metadata.insertarSucursal(datos);
        }

        [Route("actualizar/Sucursal")]
        [HttpPost]
        public string PostSucursalNueva([FromBody] Sucursal value)
        {        
            var arch = new FileStream(@"D:\Escritorio\ProyecEDII\ProyectoED2\obj\sucursal.csv", FileMode.OpenOrCreate);
            var existe = Metadata.sucursales.Busqueda(value.id);
            if (existe)
            {
                //Metadata.actualizarSucursales(value, @"D:\Escritorio\ProyecEDII\ProyectoED2\obj\sucursal");
                return "Sucursal: " + value.nombre + " actualizada";
            }
            else
            {
                return "no se encontro la sucursal ingresada";
            }
        }

        [Route("add/Producto")]
        [HttpPost]
        public void PostProductos([FromBody] Producto value)
        {
            Metadata.insertarProducto(value);
        }

        //[Route("add/Productos")]
        //[HttpPost]
        //public void PostProductos([FromBody] object value)
        //{
        //    var val = Newtonsoft.Json.JsonConvert.SerializeObject(value);
        //    Data datos = Newtonsoft.Json.JsonConvert.DeserializeObject<Data>(val);
        //    var rutaOriginal = Path.GetFullPath(datos.NombreArchivo);
        //    var arch = new FileStream(rutaOriginal, FileMode.OpenOrCreate);
        //    var lector = new StreamReader(arch);
        //    while (!lector.EndOfStream)
        //    {
        //        lector.ReadLine();
        //        var vec = lector.ReadLine().Split(";");
        //        Producto NuevoProducto = new Producto();
        //        NuevoProducto.id = Convert.ToInt16(vec[0]);
        //        NuevoProducto.nombre = vec[1];
        //        NuevoProducto.precio = Convert.ToInt16(vec[2]);
        //        ArbolProductos.Insertar(NuevoProducto);
        //    }
        //}

        [Route("actualizar/Producto")]
        [HttpPost]
        public string PostProductoNuevo([FromBody] Producto value)
        {
            var arch = new FileStream(@"D:\Escritorio\ProyecEDII\ProyectoED2\obj\producto.csv", FileMode.OpenOrCreate);
            var existe = Metadata.productos.Busqueda(value.id);
            if (existe)
            {
                //Metadata.actualizarProducto(value, @"D:\Escritorio\ProyecEDII\ProyectoED2\obj\producto");
                return "Producto: " + value.nombre + " actualizado";
            }
            else
            {
                return "no se encontro el producto";
            }

        }

        [Route("Transferir/Units")]
        [HttpPost]
        public void PostTransfer([FromBody]object value)
        {
            var val = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            Data datos = Newtonsoft.Json.JsonConvert.DeserializeObject<Data>(val);
        }

        [Route("add/SucProdu")]
        [HttpPost]
        public void PostSucProdu([FromBody] object value)
        {
            var val = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            SucursalPrecio datos = Newtonsoft.Json.JsonConvert.DeserializeObject<SucursalPrecio>(val);
            var existeSucursal = Metadata.sucursales.Busqueda(datos.idSucursal);
            var existeProducto = Metadata.productos.Busqueda(datos.idProducto);
            if (existeProducto && existeSucursal)
            {
                Metadata.insertarSyP(datos);
            }
        }

        [Route("actualizar/SucuStore")]
        [HttpPost]
        public string PostSucPrecio([FromBody]SucursalPrecio value)
        {
            var arch = new FileStream(@"D:\Escritorio\ProyecEDII\ProyectoED2\obj\sucursal-producto.csv", FileMode.OpenOrCreate);
            var existe = Metadata.inventario.Busqueda(value.idSucursal, value.idProducto);
            if (existe)
            {
                //Metadata.actualizarInventarioQty(value, @"D:\Escritorio\ProyecEDII\ProyectoED2\obj\sucursal-producto");
                return "Sucursal: " + value.idSucursal+" IDproducto: "+value.idProducto + " actualizada";
            }
            else
            {
                return "no se encontro la sucursal ingresada";
            }
        }

    }
}
