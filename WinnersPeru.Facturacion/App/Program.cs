using Business;
using Common;
using Model.Requests;
using Model.Responses;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace App
{
    static class Program
    {
        static void Main(string[] args)
        {
            JsonResult.Inicializar();
            Iniciar();
        }

        //No modificar los tipos de métodos, de lo contrario las excepciones podrían no ser controladas.
        private static void Iniciar()
        {
            try
            {
                DbService dbService = new DbService();
                //Console.WriteLine("Preparando documentos por enviar...");
                DocumentoPorEnviar(dbService).Wait(); //Espera que la tarea retornada haya finalizado, mas no el ContinueWith interno.
                //Console.WriteLine("Preparando comunicado de baja...");
                ComunicadoBajaPorEnviar(dbService).Wait();
                //Console.WriteLine("Preparando descarga de pdf...");
                ConsultarEstadoPdf(dbService, AppConfig.ActivePDFPath ? AppConfig.PDFPath : string.Empty).Wait();
                //Console.WriteLine("Esperando unos segundos antes de llamar a consulta de estados...");
                System.Threading.Thread.Sleep(10000); //Espera 5 segundos antes de llamar al consultar estados para intentar recuperar el estado Sunat aprobado o rechazado.
                //Console.WriteLine("Preparando consulta de estados...");
                ConsultarEstadoSunat(dbService).Wait();
                //VerificarPdf(dbService).Wait();
                //ConsultarEstadoPdf(dbService, AppConfig.ActivePDFPath ? AppConfig.PDFPath : string.Empty).Wait();
            }
            catch (Exception ex)
            {
                TextLog.Generar(ex.ToString()).Wait();
            }
        }

        //Solo debe retornar la tarea, sin ser asíncrono o no lanzará las excepciones en el método RecuperarDocumentosPorEnviar
        //No usar ContinueTask directamente en la tarea o no controlará la excepción
        private static Task DocumentoPorEnviar(DbService dbService)
        {
            Task task = dbService.ListarDocumentosPendientes();
            task.ContinueWith(tsk =>
            {
                //Console.WriteLine("Realizando envio de facturas...");
                #region Facturas - Boletas
                dbService.LsFacturaBoleta?.ForEach(async data =>
                {
                    try
                    {
                        if (AppConfig.ActiveOnlyForJson)
                            await TextLog.GenerarJson(data.Comprobante, JsonResult.SerializarToCamelCase(data.Data));
                        else
                        {
                            OperacionResponse response = ApiService.EnviarDocumento(data.TipoDocumento, data.Data).Result;
                            if (response.IsSuccess)
                                dbService.ActualizarEstadoIntegrador(EnumEstadoDoc.DocEnSeguimiento, "Documento enviado", data.DocEntry, data.ObjType).Wait();
                            else
                                dbService.ActualizarEstadoIntegrador(EnumEstadoDoc.DocConError, response.Contenido.Length > 1000 ? response.Contenido.Substring(0, 1000) : response.Contenido, data.DocEntry, data.ObjType).Wait();

                            if (!response.IsSuccess || (response.IsSuccess && AppConfig.ActiveLogForSuccessful))
                                await TextLog.Generar(data.Comprobante, response.IsSuccess.ToString(), response.Contenido, response.JsonString);
                        }
                    }
                    catch (Exception ex)
                    {
                        await TextLog.Generar(ex.ToString());
                    }
                });
                #endregion

                //Console.WriteLine("Realizando envio de notas de crédito y débito...");
                #region Notas Credito - Debito
                dbService.LsCreditoDebito?.ForEach(async data =>
                {
                    try
                    {
                        if (AppConfig.ActiveOnlyForJson)
                            await TextLog.GenerarJson(data.Comprobante, JsonResult.SerializarToCamelCase(data.Data));
                        else
                        {
                            OperacionResponse response = ApiService.EnviarDocumento(data.TipoDocumento, data.Data).Result;
                            if (response.IsSuccess)
                                dbService.ActualizarEstadoIntegrador(EnumEstadoDoc.DocEnSeguimiento, "Documento enviado", data.DocEntry, data.ObjType).Wait();
                            else
                                dbService.ActualizarEstadoIntegrador(EnumEstadoDoc.DocConError, response.Contenido.Length > 1000 ? response.Contenido.Substring(0, 1000) : response.Contenido, data.DocEntry, data.ObjType).Wait();

                            if (!response.IsSuccess || (response.IsSuccess && AppConfig.ActiveLogForSuccessful))
                                await TextLog.Generar(data.Comprobante, response.IsSuccess.ToString(), response.Contenido, response.JsonString);
                        }
                    }
                    catch (Exception ex)
                    {
                        await TextLog.Generar(ex.ToString());
                    }
                });
                #endregion

                //Console.WriteLine("Realizando guia de remisión...");
                #region Guia Remision
                dbService.LsGuiaRemision?.ForEach(async data =>
                {
                    try
                    {
                        if (AppConfig.ActiveOnlyForJson)
                            await TextLog.GenerarJson(data.Comprobante, JsonResult.SerializarToCamelCase(data.Data));
                        else
                        {
                            OperacionResponse response = ApiService.EnviarDocumento(data.TipoDocumento, data.Data).Result;
                            if (response.IsSuccess)
                                dbService.ActualizarEstadoIntegrador(EnumEstadoDoc.DocEnSeguimiento, "Documento enviado", data.DocEntry, data.ObjType).Wait();
                            else
                                dbService.ActualizarEstadoIntegrador(EnumEstadoDoc.DocConError, response.Contenido.Length > 1000 ? response.Contenido.Substring(0, 1000) : response.Contenido, data.DocEntry, data.ObjType).Wait();

                            if (!response.IsSuccess || (response.IsSuccess && AppConfig.ActiveLogForSuccessful))
                                await TextLog.Generar(data.Comprobante, response.IsSuccess.ToString(), response.Contenido, response.JsonString);
                        }
                    }
                    catch (Exception ex)
                    {
                        await TextLog.Generar(ex.ToString());
                    }
                });
                #endregion
            }).Wait(); //Espera el método asíncrono en ContinueWith

            return task;
        }

        private static async Task ComunicadoBajaPorEnviar(DbService dbService)
        {
            var comunicadoBaja = dbService.ListarComunicadosBajaPendientes().Result;
            if (comunicadoBaja.Data.ResumenBajaItemList.Count > 0)
            {
                try
                {
                    if (AppConfig.ActiveOnlyForJson)
                        await TextLog.GenerarJson(comunicadoBaja.Comprobante, JsonResult.SerializarToCamelCase(comunicadoBaja.Data));
                    else
                    {
                        OperacionResponse response = ApiService.EnviarComunicadoBaja(comunicadoBaja.Data).Result;
                        if (response.IsSuccess)
                        {
                            ResumenBajaResponse resumenBajaResponse = JsonResult.Deserializar<ResumenBajaResponse>(response.Contenido);
                            foreach (ResumenBajaItemList item in comunicadoBaja.Data.ResumenBajaItemList)
                                dbService.ActualizarEstadoIntegrador(EnumEstadoDoc.BajaEnSeguimiento, resumenBajaResponse.Identificador, item.DocEntry, item.ObjType).Wait();
                        }
                        else
                        {
                            foreach (ResumenBajaItemList item in comunicadoBaja.Data.ResumenBajaItemList)
                                dbService.ActualizarEstadoIntegrador(EnumEstadoDoc.BajaConError, response.Contenido.Length > 1000 ? response.Contenido.Substring(0, 1000) : response.Contenido, item.DocEntry, item.ObjType).Wait();
                        }

                        if (/*!response.IsSuccess || */(response.IsSuccess && AppConfig.ActiveLogForSuccessful)) //Ya no genera logs cuando hay error, suficiente con guardar el mensaje en el campo de usuario.
                            await TextLog.Generar(comunicadoBaja.Comprobante, response.IsSuccess.ToString(), response.Contenido, response.JsonString);
                    }
                }
                catch (Exception ex)
                {
                    await TextLog.Generar(ex.ToString());
                }
            }
        }

        private static async Task ConsultarEstadoSunat(DbService dbService)
        {
            var documentoPorResponder = dbService.ListarDocumentosEnviados(EnumConsultaEstado.SUNAT).Result;
            foreach (DataEstadoRequest<ConsultaEstadoRequest> request in documentoPorResponder)
            {
                try
                {
                    if (AppConfig.ActiveOnlyForJson)
                        await TextLog.GenerarJson(request.Comprobante, JsonResult.SerializarToCamelCase(request.Data));
                    else
                    {
                        OperacionResponse response = ApiService.ConsultarEstado(request.Data).Result;
                        if (response.IsSuccess)
                        {
                            var estadoResponse = JsonResult.Deserializar<ConsultaEstadoResponse>(response.Contenido);
                            dbService.ActualizarEstadoSunat(estadoResponse.EstadoSunat.Codigo, $"{estadoResponse.EstadoSunat.Codigo} - {estadoResponse.EstadoSunat.Valor}", request.DocEntry, request.ObjType).Wait();
                        }

                        if (/*!response.IsSuccess || */(response.IsSuccess && AppConfig.ActiveLogForSuccessful)) //Ya no genera logs cuando hay error, suficiente con guardar el mensaje en el campo de usuario.
                            await TextLog.Generar(request.Comprobante, response.IsSuccess.ToString(), response.Contenido, response.JsonString);
                    }
                }
                catch (Exception ex)
                {
                    await TextLog.Generar(ex.ToString());
                }
            }
        }

        private static async Task ConsultarEstadoPdf(DbService dbService, string rutaCarpeta)
        {
            var documentoPorResponder = dbService.ListarDocumentosEnviados(EnumConsultaEstado.PDF).Result;
            foreach (DataEstadoRequest<ConsultaEstadoRequest> request in documentoPorResponder)
            {
                try
                {
                    if (AppConfig.ActiveOnlyForJson)
                        await TextLog.GenerarJson(request.Comprobante, JsonResult.SerializarToCamelCase(request.Data));
                    else
                    {
                        OperacionResponse response = ApiService.ConsultarPdf(request.Data).Result;
                        if (response.IsSuccess)
                        {
                            try
                            {
                                string idComprobante = $"{request.Data.TipoComprobante}{request.Data.Serie}-{request.Data.Numero}";
                                string nombreArchivo = $"{request.Data.Emisor}-{idComprobante}";
                                string rutaArchivo = "";
                                if (string.IsNullOrEmpty(rutaCarpeta))
                                    rutaArchivo = AppPath.RutaArchivoInterno("PDF", nombreArchivo, "pdf");
                                else
                                    rutaArchivo = AppPath.RutaArchivo(rutaCarpeta, nombreArchivo, "pdf");

                                AppPath.CrearArchivoDesdeBase64(response.Contenido, rutaArchivo);
                                dbService.ActualizarEstadoPdf(EnumEstadoPdf.Descargado, idComprobante, request.DocEntry, request.ObjType).Wait();
                                continue;
                            }
                            catch (Exception e) { response.Contenido += Environment.NewLine + e.ToString(); }
                        }

                        dbService.ActualizarEstadoPdf(EnumEstadoPdf.Error, response.Contenido, request.DocEntry, request.ObjType).Wait();

                        if (/*!response.IsSuccess || */(response.IsSuccess && AppConfig.ActiveLogForSuccessful)) //Ya no genera logs cuando hay error, suficiente con guardar el mensaje en el campo de usuario.
                            await TextLog.Generar(request.Comprobante, response.IsSuccess.ToString(), response.Contenido, response.JsonString);
                    }
                }
                catch (Exception e)
                {
                    await TextLog.Generar(e.ToString());
                }
            }
        }

        private static async Task VerificarPdf(DbService dbService)
        {
            var documentoPorResponder = dbService.ListarDocumentosEnviados(EnumConsultaEstado.PDF).Result;
            foreach (DataEstadoRequest<ConsultaEstadoRequest> request in documentoPorResponder)
            {
                try
                {
                    string idComprobante = $"{request.Data.TipoComprobante}{request.Data.Serie}-{request.Data.Numero}";
                    string nombreArchivo = $"{request.Data.Emisor}-{idComprobante}.pdf";
                    bool exists = AppPath.ListarArchivos(AppConfig.PDFPath).Exists(e => e.EndsWith(nombreArchivo));
                    if (!exists) //Actualiza el estado de descarga a Pendiente
                        dbService.ActualizarEstadoPdf(EnumEstadoPdf.Pendiente, idComprobante, request.DocEntry, request.ObjType).Wait();
                }
                catch (Exception e)
                {
                    await TextLog.Generar(e.ToString());
                }
            }
        }

    }

}
