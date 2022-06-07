using System;
using System.Collections.Generic;
using System.IO;

namespace Common
{
    public class AppPath
    {
        //private const string rutaTemporalWindows ="~/temp";

        private static void CrearRutaFolder(string ruta)
        {
            if (!Directory.Exists(ruta)) Directory.CreateDirectory(ruta);
        }

        public static string ObtenerRutaInterna()
        {
            return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", string.Empty));
        }

        public static string RutaArchivoInterno(string nombreFolder, string nombreArchivo, string extension)
        {
            string rutaFolder = Path.Combine(ObtenerRutaInterna(), nombreFolder);
            CrearRutaFolder(rutaFolder);
            return Path.Combine(rutaFolder, $"{nombreArchivo}.{extension}");
        }

        public static string CrearArchivoTextoInterno(string texto, string nombreFolder, string nombreArchivo, string extension)
        {
            string rutaArchivo = RutaArchivoInterno(nombreFolder, nombreArchivo, extension);
            File.WriteAllText(rutaArchivo, texto);
            return rutaArchivo;
        }

        public static string RutaArchivo(string ruta, string nombreArchivo, string extension)
        {
            CrearRutaFolder(ruta);
            if (string.IsNullOrEmpty(extension))
                return Path.Combine(ruta, nombreArchivo);
            else
                return Path.Combine(ruta, $"{nombreArchivo}.{extension}");
        }

        public static string RutaArchivo(string ruta, string nombreFolder, string nombreArchivo, string extension)
        {
            string rutaFolder = Path.Combine(ruta, nombreFolder);
            CrearRutaFolder(rutaFolder);
            if (string.IsNullOrEmpty(extension))
                return Path.Combine(rutaFolder, nombreArchivo);
            else
                return Path.Combine(rutaFolder, $"{nombreArchivo}.{extension}");
        }

        public static string CrearArchivoTexto(string texto, string ruta, string nombreFolder, string nombreArchivo, string extension)
        {
            string rutaArchivo = RutaArchivo(ruta, nombreFolder, nombreArchivo, extension);
            File.WriteAllText(rutaArchivo, texto);
            return rutaArchivo;
        }

        public static void CrearArchivoDesdeBase64(string contenidoStringToByte, string rutaArchivo)
        {
            File.WriteAllBytes(rutaArchivo, Convert.FromBase64String(contenidoStringToByte));
        }

        public static void MoverArchivo(string rutaArchivo, string rutaDestino)
        {
            File.Move(rutaArchivo, Path.Combine(rutaDestino, Path.GetFileName(rutaArchivo)));
        }

        public static List<string> ListarArchivos(string ruta)
        {
            List<string> lsArchivos = new List<string>();
            if (!Directory.Exists(ruta)) return lsArchivos;

            string[] files = Directory.GetFiles(ruta);
            foreach (var item in files)
                lsArchivos.Add(Path.GetFileName(item));

            return lsArchivos;
        }
    }
}
