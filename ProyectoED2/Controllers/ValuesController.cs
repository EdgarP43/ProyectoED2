using System;
using System.Collections.Generic;
using System.Linq;
using ProyectoED2.Modelos;
using System.IO;
using ProyectoED2.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProyectoED2.Cifrado;
using ProyectoED2.Huffman;

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
            public string clave { get; set; }
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
            //var arch = new FileStream(@"D:\Escritorio\ProyecEDII\ProyectoED2\obj\sucursal.csv", FileMode.OpenOrCreate);
            var existe = Metadata.sucursales.Busqueda(value.id);
            if (existe)
            {
                Metadata.actualizarSucursales(value);
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

        [Route("add/Productos")]
        [HttpPost]
        public void PostProductos([FromBody] object value)
        {
            var val = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            Data datos = Newtonsoft.Json.JsonConvert.DeserializeObject<Data>(val);
            var rutaOriginal1 = Path.GetFullPath(@"obj\" + datos.NombreArchivo1);
            var rutaOriginal2 = Path.GetFullPath(@"obj\" + datos.NombreArchivo2);
            var rutaOriginal3 = Path.GetFullPath(@"obj\" + datos.NombreArchivo3);
            using (var stream = new FileStream(@"D:\Escritorio\ProyectEDii\ProyectoED2\Keys\Llave.txt", FileMode.OpenOrCreate))
            {
                using (var reader = new StreamWriter(stream))
                {
                    reader.Write(datos.clave);
                }
            }
            SDES modeloSdes1 = new SDES();
            var OpsDic = modeloSdes1.LeerOperaciones(@"D:\Escritorio\ProyectEDii\ProyectoED2\Keys\Oper.txt");
            var binaryKey = string.Empty;
            modeloSdes1.VerificarLLave(datos.clave, ref binaryKey);
            var key1 = string.Empty;
            var key2 = string.Empty;
            var sbox0 = modeloSdes1.CrearSBox0();
            var sbox1 = modeloSdes1.CrearSBox1();
            modeloSdes1.GenerarLlaves(OpsDic, binaryKey, ref key1, ref key2);
            var bytesCifrados = modeloSdes1.CifrarTexto(rutaOriginal1, OpsDic, key1, key2, sbox0, sbox1);
            key1 = string.Empty;
            key2 = string.Empty;
            modeloSdes1.GenerarLlaves(OpsDic, binaryKey, ref key1, ref key2);
            var bytesDecifrados = modeloSdes1.DescifrarTexto(bytesCifrados, OpsDic, key1, key2, sbox0, sbox1);
            var vec1 = rutaOriginal1.Split(@"\");
            var vec2 = vec1[vec1.Length - 1].Split(".");
            var pathHuffman = Path.GetFullPath("Archivos Cifrados//");
            using (var stream = new FileStream(pathHuffman + vec1[vec1.Length - 1], FileMode.OpenOrCreate))
            {
                using (var reader = new BinaryWriter(stream))
                {
                    foreach (var item in bytesDecifrados)
                    {
                        reader.Write(item);
                    }
                }
            }
            var pathHuffman2 = Path.GetFullPath("Archivos Comprimidos//");
            Huffman.Huffman.Instancia.CompresiónHuffman(pathHuffman + vec1[vec1.Length - 1], vec2, pathHuffman2);
            //arch2
            SDES modeloSdes2 = new SDES();
            key1 = string.Empty;
            key2 = string.Empty;
            modeloSdes2.GenerarLlaves(OpsDic, binaryKey, ref key1, ref key2);
            var bytesCifrados2 = modeloSdes2.CifrarTexto(rutaOriginal2, OpsDic, key1, key2, sbox0, sbox1);
            key1 = string.Empty;
            key2 = string.Empty;
            modeloSdes2.GenerarLlaves(OpsDic, binaryKey, ref key1, ref key2);
            var bytesDecifrados2 = modeloSdes2.DescifrarTexto(bytesCifrados2, OpsDic, key1, key2, sbox0, sbox1);
            var vec12 = rutaOriginal2.Split(@"\");
            var vec22 = vec12[vec12.Length - 1].Split(".");
            var pathHuffman20 = Path.GetFullPath("Archivos Cifrados//");
            using (var stream = new FileStream(pathHuffman20 + vec12[vec12.Length - 1], FileMode.OpenOrCreate))
            {
                using (var reader = new BinaryWriter(stream))
                {
                    foreach (var item in bytesDecifrados2)
                    {
                        reader.Write(item);
                    }
                }
            }
            var pathHuffman22 = Path.GetFullPath("Archivos Comprimidos//");
            Huffman.Huffman.Instancia.CompresiónHuffman(pathHuffman20 + vec12[vec12.Length - 1], vec22, pathHuffman22);
            //arch3
            SDES modeloSdes3 = new SDES();
            key1 = string.Empty;
            key2 = string.Empty;
            modeloSdes3.GenerarLlaves(OpsDic, binaryKey, ref key1, ref key2);
            var bytesCifrados23 = modeloSdes3.CifrarTexto(rutaOriginal3, OpsDic, key1, key2, sbox0, sbox1);
            key1 = string.Empty;
            key2 = string.Empty;
            modeloSdes3.GenerarLlaves(OpsDic, binaryKey, ref key1, ref key2);
            var bytesDecifrados23 = modeloSdes3.DescifrarTexto(bytesCifrados23, OpsDic, key1, key2, sbox0, sbox1);
            var vec13 = rutaOriginal3.Split(@"\");
            var vec23 = vec13[vec13.Length - 1].Split(".");
            var pathHuffman3 = Path.GetFullPath("Archivos Cifrados//");
            using (var stream = new FileStream(pathHuffman3 + vec13[vec13.Length - 1], FileMode.OpenOrCreate))
            {
                using (var reader = new BinaryWriter(stream))
                {
                    foreach (var item in bytesDecifrados23)
                    {
                        reader.Write(item);
                    }
                }
            }
            var pathHuffman23 = Path.GetFullPath("Archivos Comprimidos//");
            Huffman.Huffman.Instancia.CompresiónHuffman(pathHuffman3 + vec13[vec13.Length - 1], vec23, pathHuffman23);
        }

        [Route("actualizar/Producto")]
        [HttpPost]
        public string PostProductoNuevo([FromBody] Producto value)
        {
            var existe = Metadata.productos.Busqueda(value.id);
            if (existe)
            {
                Metadata.actualizarProducto(value);
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
            var existe = Metadata.inventario.Busqueda(value.idSucursal, value.idProducto);
            if (existe)
            {
                Metadata.actualizarInventarioQty(value);
                return "Sucursal: " + value.idSucursal + " IDproducto: " + value.idProducto + " actualizada";
            }
            else
            {
                return "no se encontro la sucursal ingresada";
            }
        }

        [Route("mostrar/Productos")]
        [HttpPost]
        public string PostMostrarpro()
        {
            var txt = "--Lista productos--\n";
            var list = Metadata.productos.Todo();
            foreach (var item in list)
            {
                if (item!=null)
                {
                    txt += "ID: " + item.id.ToString() + "\n";
                    txt += "NombreProducto: " + item.nombre.ToString() + "\n";
                    txt += "Precio Q: " + item.precio.ToString() + "\n";
                }
            }
            return txt;
        }

        [Route("mostrar/Sucursales")]
        [HttpPost]
        public string PostMostrarSucursales()
        {
            var txt = "--Lista sucursales--\n";
            var list = Metadata.sucursales.Todo();
            foreach (var item in list)
            {
                if (item != null)
                {
                    txt += "ID: " + item.id.ToString() + "\n";
                    txt += "NombreSucursal: " + item.nombre.ToString() + "\n";
                    txt += "Direccion: " + item.direccion + "\n";
                }
            }
            return txt;
        }

        [Route("mostrar/SucurProduct")]
        [HttpPost]
        public string PostMostrarSucurProduct()
        {
            var txt = "--Lista Inventario--\n";
            var list = Metadata.inventario.Todo();
            foreach (var item in list)
            {
                if (item != null)
                {
                    txt += "IDSucursal: " + item.idSucursal.ToString() + "\n";
                    txt += "IDproducto: " + item.idProducto.ToString() + "\n";
                    txt += "Cant. inventario: " + item.cantidadInv.ToString() + "\n";
                }
            }
            return txt;
        }
    }
}

