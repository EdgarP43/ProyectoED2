using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProyectoED2.Cifrado
{
    public class SDES
    {
        static int bufferLenght = 1000;
        public bool VerificarLLave(string llave, ref string binaryKey)
        {
            if (Convert.ToInt32(llave) >= 0 && Convert.ToInt32(llave) < 1024)
            {
                binaryKey = Convert.ToString(Convert.ToUInt32(llave), 2);
                binaryKey = binaryKey.PadLeft(10, '0');
                return true;
            }
            else
            {
                return false;
            }
        }

        public Dictionary<string, string> LeerOperaciones(string ubicacion)
        {
            var tmpDictionary = new Dictionary<string, string>();
            var line = string.Empty;
            StreamReader file = new StreamReader(ubicacion);
            while ((line = file.ReadLine()) != null)
            {
                var tmpArray = line.Split(':');
                tmpDictionary.Add(tmpArray[0], tmpArray[1]);
            }
            file.Close();
            return tmpDictionary;
        }

        public string LeftShift(string bloque, int shift)
        {
            return bloque.Substring(shift, bloque.Length - shift) + bloque.Substring(0, shift);
        }

        public string Permutando(string aPermutar, string array)
        {
            var tmpArray = string.Empty;
            for (int i = 0; i < array.Length; i++)
            {
                tmpArray += aPermutar[Convert.ToInt16(Convert.ToString(array[i]))];
            }
            return tmpArray;
        }

        public void GenerarLlaves(Dictionary<string, string> opsDictionary, string binaryKey, ref string Key1, ref string Key2)
        {
            var p10array = opsDictionary["P10"];
            var tmp10Array = Permutando(binaryKey, p10array);

            var bloque1 = tmp10Array.Substring(0, 5);
            var bloque2 = tmp10Array.Substring(5, 5);

            bloque1 = LeftShift(bloque1, 1);
            bloque2 = LeftShift(bloque2, 1);

            var bloqueTmp = bloque1 + bloque2;

            var p8array = opsDictionary["P8"];

            Key1 = Permutando(bloqueTmp, p8array);

            bloque1 = LeftShift(bloque1, 2);
            bloque2 = LeftShift(bloque2, 2);

            bloqueTmp = bloque1 + bloque2;

            Key2 = Permutando(bloqueTmp, p8array);
        }

        public string[,] CrearSBox0()
        {
            var sbox0 = new string[4, 4] { { "01", "00", "11", "10" }, { "11", "10", "01", "00" }, { "00", "10", "01", "11" }, { "11", "01", "11", "10" } };
            return sbox0;
        }

        public string[,] CrearSBox1()
        {
            var sbox1 = new string[4, 4] { { "00", "01", "10", "11" }, { "10", "00", "01", "11" }, { "11", "00", "01", "00" }, { "10", "01", "00", "11" } };
            return sbox1;
        }

        public List<byte> CifrarTexto(string ubicacionFile, Dictionary<string, string> opsDictionary, string key1, string key2, string[,] sbox0, string[,] sbox1)
        {
            var caracter = new byte();
            var binario = string.Empty;
            List<byte> listt = new List<byte>();
            using (var stream = new FileStream(ubicacionFile, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var byteBuffer = new byte[bufferLenght];
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        byteBuffer = reader.ReadBytes(bufferLenght);

                        for (int i = 0; i < byteBuffer.Length; i++)
                        {
                            caracter = byteBuffer[i];
                            binario = Convert.ToString(caracter, 2);
                            binario = binario.PadLeft(8, '0');

                            var IParray = opsDictionary["IP"];
                            var tmpIp = Permutando(binario, IParray);

                            var bloque1 = tmpIp.Substring(0, 4);
                            var bloque2 = tmpIp.Substring(4, 4);

                            var EParray = opsDictionary["EP"];
                            var tmpEP = Permutando(bloque2, EParray);

                            var XOR = string.Empty;
                            for (int j = 0; j < key1.Length; j++)
                            {
                                XOR += key1[j] ^ tmpEP[j];
                            }

                            var BloqueSbox0 = XOR.Substring(0, 4);
                            var FS0 = 2 * Convert.ToInt16(Convert.ToString(BloqueSbox0[0])) + 1 * Convert.ToInt16(Convert.ToString(BloqueSbox0[3]));
                            var CS0 = 2 * Convert.ToInt16(Convert.ToString(BloqueSbox0[1])) + 1 * Convert.ToInt16(Convert.ToString(BloqueSbox0[2]));
                            var ResultSB0 = sbox0[FS0, CS0];

                            var BloqueSbox1 = XOR.Substring(4, 4);
                            var FS1 = 2 * Convert.ToInt16(Convert.ToString(BloqueSbox1[0])) + 1 * Convert.ToInt16(Convert.ToString(BloqueSbox1[3]));
                            var CS1 = 2 * Convert.ToInt16(Convert.ToString(BloqueSbox1[1])) + 1 * Convert.ToInt16(Convert.ToString(BloqueSbox1[2]));
                            var ResultSB1 = sbox1[FS1, CS1];

                            var bloqueResultante = ResultSB0 + ResultSB1;

                            var P4array = opsDictionary["P4"];
                            var tmpP4 = Permutando(bloqueResultante, P4array);

                            var XORfinal = string.Empty;
                            for (int j = 0; j < tmpP4.Length; j++)
                            {
                                XORfinal += bloque1[j] ^ tmpP4[j];
                            }

                            var swap = bloque2 + XORfinal;

                            tmpEP = Permutando(XORfinal, EParray);

                            XOR = string.Empty;
                            for (int j = 0; j < key2.Length; j++)
                            {
                                XOR += tmpEP[j] ^ key2[j];
                            }

                            BloqueSbox0 = XOR.Substring(0, 4);
                            FS0 = 2 * Convert.ToInt16(Convert.ToString(BloqueSbox0[0])) + 1 * Convert.ToInt16(Convert.ToString(BloqueSbox0[3]));
                            CS0 = 2 * Convert.ToInt16(Convert.ToString(BloqueSbox0[1])) + 1 * Convert.ToInt16(Convert.ToString(BloqueSbox0[2]));
                            ResultSB0 = sbox0[FS0, CS0];

                            BloqueSbox1 = XOR.Substring(4, 4);
                            FS1 = 2 * Convert.ToInt16(Convert.ToString(BloqueSbox1[0])) + 1 * Convert.ToInt16(Convert.ToString(BloqueSbox1[3]));
                            CS1 = 2 * Convert.ToInt16(Convert.ToString(BloqueSbox1[1])) + 1 * Convert.ToInt16(Convert.ToString(BloqueSbox1[2]));
                            ResultSB1 = sbox1[FS1, CS1];

                            bloqueResultante = ResultSB0 + ResultSB1;

                            tmpP4 = string.Empty;
                            tmpP4 = Permutando(bloqueResultante, P4array);

                            XOR = string.Empty;
                            for (int j = 0; j < bloqueResultante.Length; j++)
                            {
                                XOR += bloque2[j] ^ tmpP4[j];
                            }

                            var union = XOR + XORfinal;

                            var IP1array = opsDictionary["IP1"];
                            var resultado = string.Empty;
                            for (int j = 0; j < IP1array.Length; j++)
                            {
                                resultado += union[Convert.ToInt16(Convert.ToString(IP1array[j]))];
                            }

                            listt.Add(Convert.ToByte(Convert.ToInt16(resultado, 2)));
                            //writer.Write(Convert.ToByte(Convert.ToInt32(resultado, 2)));
                        }

                    }
                }


            }
            return listt;
        }


        public List<byte> DescifrarTexto(List<byte> bytes, Dictionary<string, string> opsDictionary, string key1, string key2, string[,] sbox0, string[,] sbox1)
        {
            var caracter = new byte();
            var binario = string.Empty;
            List<byte> listt = new List<byte>();
            var byteBuffer = new byte[bufferLenght];
            for (int i = 0; i < bytes.Count; i++)
            {
                caracter = bytes[i];
                binario = Convert.ToString(caracter, 2);
                binario = binario.PadLeft(8, '0');

                var IParray = opsDictionary["IP"];
                var tmpIp = Permutando(binario, IParray);

                var bloque1 = tmpIp.Substring(0, 4);
                var bloque2 = tmpIp.Substring(4, 4);

                var EParray = opsDictionary["EP"];
                var tmpEP = Permutando(bloque2, EParray);

                var XOR = string.Empty;
                for (int j = 0; j < key1.Length; j++)
                {
                    XOR += key1[j] ^ tmpEP[j];
                }

                var BloqueSbox0 = XOR.Substring(0, 4);
                var FS0 = 2 * Convert.ToInt16(Convert.ToString(BloqueSbox0[0])) + 1 * Convert.ToInt16(Convert.ToString(BloqueSbox0[3]));
                var CS0 = 2 * Convert.ToInt16(Convert.ToString(BloqueSbox0[1])) + 1 * Convert.ToInt16(Convert.ToString(BloqueSbox0[2]));
                var ResultSB0 = sbox0[FS0, CS0];

                var BloqueSbox1 = XOR.Substring(4, 4);
                var FS1 = 2 * Convert.ToInt16(Convert.ToString(BloqueSbox1[0])) + 1 * Convert.ToInt16(Convert.ToString(BloqueSbox1[3]));
                var CS1 = 2 * Convert.ToInt16(Convert.ToString(BloqueSbox1[1])) + 1 * Convert.ToInt16(Convert.ToString(BloqueSbox1[2]));
                var ResultSB1 = sbox1[FS1, CS1];

                var bloqueResultante = ResultSB0 + ResultSB1;

                var P4array = opsDictionary["P4"];
                var tmpP4 = Permutando(bloqueResultante, P4array);

                var XORfinal = string.Empty;
                for (int j = 0; j < tmpP4.Length; j++)
                {
                    XORfinal += bloque1[j] ^ tmpP4[j];
                }

                var swap = bloque2 + XORfinal;

                tmpEP = Permutando(XORfinal, EParray);

                XOR = string.Empty;
                for (int j = 0; j < key2.Length; j++)
                {
                    XOR += tmpEP[j] ^ key2[j];
                }

                BloqueSbox0 = XOR.Substring(0, 4);
                FS0 = 2 * Convert.ToInt16(Convert.ToString(BloqueSbox0[0])) + 1 * Convert.ToInt16(Convert.ToString(BloqueSbox0[3]));
                CS0 = 2 * Convert.ToInt16(Convert.ToString(BloqueSbox0[1])) + 1 * Convert.ToInt16(Convert.ToString(BloqueSbox0[2]));
                ResultSB0 = sbox0[FS0, CS0];

                BloqueSbox1 = XOR.Substring(4, 4);
                FS1 = 2 * Convert.ToInt16(Convert.ToString(BloqueSbox1[0])) + 1 * Convert.ToInt16(Convert.ToString(BloqueSbox1[3]));
                CS1 = 2 * Convert.ToInt16(Convert.ToString(BloqueSbox1[1])) + 1 * Convert.ToInt16(Convert.ToString(BloqueSbox1[2]));
                ResultSB1 = sbox1[FS1, CS1];

                bloqueResultante = ResultSB0 + ResultSB1;

                tmpP4 = string.Empty;
                tmpP4 = Permutando(bloqueResultante, P4array);

                XOR = string.Empty;
                for (int j = 0; j < bloqueResultante.Length; j++)
                {
                    XOR += bloque2[j] ^ tmpP4[j];
                }

                var union = XOR + XORfinal;

                var IP1array = opsDictionary["IP1"];
                var resultado = string.Empty;
                for (int j = 0; j < IP1array.Length; j++)
                {
                    resultado += union[Convert.ToInt16(Convert.ToString(IP1array[j]))];
                }

                listt.Add(Convert.ToByte(Convert.ToInt16(resultado, 2)));
                //writer.Write(Convert.ToByte(Convert.ToInt32(resultado, 2)));
            }
            return listt;
        }

        //public string CrearArchivos(string rutaOriginal)
        //{
        //    var vec1 = rutaOriginal.Split("\\");
        //    var vec2 = vec1[vec1.Length - 1].Split(".");
        //    var destino = Path.GetFullPath("bin");
        //    using (var arch = new FileStream(rutaOriginal, FileMode.OpenOrCreate))
        //    {
        //        using (var reader = new StreamReader(arch))
        //        {
        //            using (var archNuevo = new FileStream(destino.Substring(0, destino.Length - 3) + vec1[vec1.Length - 1], FileMode.OpenOrCreate))
        //            {
        //                using (var writer = new StreamWriter(archNuevo))
        //                {
        //                    writer.Write(reader.ReadToEnd());
        //                }
        //            }
        //        }
        //    }
        //    return destino.Substring(0, destino.Length - 3) + vec1[vec1.Length - 1];
        //}
        //public void CargarArchivos(string ruta)
        //{
        //    FileInfo fi = new FileInfo(ruta);
        //    fi.Delete();
        //}

    }
}
