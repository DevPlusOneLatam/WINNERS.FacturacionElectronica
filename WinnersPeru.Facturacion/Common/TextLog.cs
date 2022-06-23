using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class TextLog
    {
        private const string NombreFolderLog = "Log";
        private const string FormatoFecha_NombreJson = "dd-MM-yyyy hh.mm.ss";
        private const string FormatoFecha_NombreLog = "dd-MM-yyyy hh.mm.ss.fff";
        private const string FormatoFecha_NombreLogException = "dd-MM-yyyy hh.mm.ss.fffffff";

        public static async Task Generar(string comprobante, string codigoEstado, string contenido, string json)
        {
            Task task = new Task(() =>
            {
                StringBuilder sb = new StringBuilder();
                string fechaHora = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                sb.Append($"=================== REGISTRO DE ENVIO - {fechaHora} ===================");
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append($"=================== Respuesta Close2u ===================");
                sb.Append(Environment.NewLine);
                sb.Append("Estado: " + codigoEstado + Environment.NewLine);
                sb.Append("Comprobante: " + comprobante + Environment.NewLine);
                sb.Append("Contenido: " + contenido + Environment.NewLine);
                sb.Append(Environment.NewLine);

                string nombreFolder = Path.Combine(NombreFolderLog, comprobante);
                string nombreArchivo = comprobante + " " + DateTime.Now.ToString(FormatoFecha_NombreLog);
                AppPath.CrearArchivoTextoInterno(sb.ToString(), nombreFolder, nombreArchivo, "txt");
                if (json != null) AppPath.CrearArchivoTextoInterno(json, nombreFolder, nombreArchivo, "json");

                sb.Clear();
            });

            task.Start();
            await task;
        }

        public static async Task Generar(string excepcion)
        {
            Task task = new Task(() =>
            {
                StringBuilder sb = new StringBuilder();
                string fechaHora = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                sb.Append($"=================== REGISTRO DE ERRORES - {fechaHora} ===================");
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append($"=================== Excepcion ===================");
                sb.Append(Environment.NewLine);
                sb.Append(excepcion);

                string nombreArchivo = "Log " + DateTime.Now.ToString(FormatoFecha_NombreLogException);
                AppPath.CrearArchivoTextoInterno(sb.ToString(), NombreFolderLog, nombreArchivo, "txt");
                sb.Clear();
            });

            task.Start();
            await task;
        }

        public static async Task GenerarJson(string comprobante, string json)
        {
            Task task = new Task(() =>
            {
                string nombreFolder = Path.Combine(NombreFolderLog, comprobante);
                string nombreArchivo = comprobante + " " + DateTime.Now.ToString(FormatoFecha_NombreJson);
                AppPath.CrearArchivoTextoInterno(json, nombreFolder, nombreArchivo, "json");
            });

            task.Start();
            await task;
        }
    }
}
