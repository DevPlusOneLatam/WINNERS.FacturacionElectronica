using Common;
using Model.Requests;
using Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Business
{
    public class DbService
    {
        public List<DataEstadoRequest<DocumentoRequest>> LsFacturaBoleta { get; private set; }
        public List<DataEstadoRequest<NotaRequest>> LsCreditoDebito { get; private set; }
        public List<DataEstadoRequest<GuiaRemisionRequest>> LsGuiaRemision { get; private set; }

        private readonly DbRepository _repository;

        public DbService()
        {
            _repository = new DbRepository();
        }

        public async Task ListarDocumentosPendientes()
        {
            DataTable dt = await _repository.ListarDocumentosPendientes();

            LsFacturaBoleta = new List<DataEstadoRequest<DocumentoRequest>>();
            LsCreditoDebito = new List<DataEstadoRequest<NotaRequest>>();
            LsGuiaRemision = new List<DataEstadoRequest<GuiaRemisionRequest>>();

            foreach (DataRow registro in dt.Rows)
            {
                string tipoDocumento = registro["DOC_TIPO"].ToString();
                switch (tipoDocumento)
                {
                    case "01":
                    case "03":
                        LsFacturaBoleta.Add(await EstructuraFacturaBoleta(registro, tipoDocumento));
                        break;
                    case "07":
                    case "08":
                        LsCreditoDebito.Add(await EstructuraCreditoDebito(registro, tipoDocumento));
                        break;
                    case "09":
                        LsGuiaRemision.Add(await EstructuraGuiaRemision(registro, tipoDocumento));
                        break;
                    default:
                        break;
                }
            }
        }

        public async Task<DataEstadoRequest<ResumenBaja>> ListarComunicadosBajaPendientes()
        {
            DataRow drEmisor = (await _repository.DatosEmisor()).Rows[0];
            ResumenBaja resumenBaja = new ResumenBaja
            {
                Emisor = new EmisorRequest()
                {
                    TipoDocumentoIdentidad = drEmisor["EMISOR_TIPO_DOCUMENTO_IDENTIDAD"].ToString(),
                    NumeroDocumentoIdentidad = drEmisor["EMISOR_DOCUMENTO_IDENTIDAD"].ToString(),
                    NombreLegal = drEmisor["EMISOR_RAZON_SOCIAL"].ToString(),
                    NombreComercial = drEmisor["EMISOR_NOMBRE_COMERCIAL"].ToString(),
                    Correo = drEmisor["EMISOR_CORREO_ELECTRONICO"].ToString()
                },

                ResumenBajaItemList = new List<ResumenBajaItemList>()
            };

            DataTable dt = await _repository.ListarComunicadosBajaPendientes();
            foreach (DataRow dr in dt.Rows)
            {
                ResumenBajaItemList bajaItem = new ResumenBajaItemList
                {
                    DocEntry = dr["FACTURACION_DOCENTRY"].ToString(),
                    ObjType = dr["FACTURACION_OBJTYPE"].ToString(),
                    TipoComprobante = dr["DOC_TIPO"].ToString(),
                    Serie = dr["DOC_SERIE"].ToString(),
                    Numero = dr["DOC_CORRELATIVO"].ToString(),
                    Motivo = dr["BAJA_MOTIVO_DESCRIPCION"].ToString()
                };
                resumenBaja.ResumenBajaItemList.Add(bajaItem);
            }

            return new DataEstadoRequest<ResumenBaja>()
            {
                Data = resumenBaja,
                Comprobante = "ComunicadoBaja",
            };
        }

        public async Task<List<DataEstadoRequest<ConsultaEstadoRequest>>> ListarDocumentosEnviados(EnumConsultaEstado enumConsultaEstado)
        {
            DataTable dt = await _repository.ListarDocumentosEnviados(enumConsultaEstado);
            List<DataEstadoRequest<ConsultaEstadoRequest>> lsEstado = new List<DataEstadoRequest<ConsultaEstadoRequest>>();

            foreach (DataRow dr in dt.Rows)
            {
                ConsultaEstadoRequest request = new ConsultaEstadoRequest
                {
                    Emisor = dr["EMISOR_DOCUMENTO_IDENTIDAD"].ToString(),
                    TipoComprobante = dr["DOC_TIPO"].ToString(),
                    Serie = dr["DOC_SERIE"].ToString(),
                    Numero = dr["DOC_CORRELATIVO"].ToString()
                };

                lsEstado.Add(new DataEstadoRequest<ConsultaEstadoRequest>
                {
                    Comprobante = dr["FACTURACION_ID"].ToString(),
                    DocEntry = dr["FACTURACION_DOCENTRY"].ToString(),
                    ObjType = dr["FACTURACION_OBJTYPE"].ToString(),
                    Data = request
                });
            }

            return lsEstado;
        }

        public async Task ActualizarEstadoIntegrador(EnumEstadoDoc enumEstadoDoc, string mensaje, string docEntry, string objType)
        {
            await _repository.ActualizarEstadoIntegrador(enumEstadoDoc, mensaje, docEntry, objType);
        }

        public async Task ActualizarEstadoSunat(string codigoEstado, string mensaje, string docEntry, string objType)
        {
            await _repository.ActualizarEstadoSunat(codigoEstado, mensaje, docEntry, objType);
        }

        public async Task ActualizarEstadoPdf(EnumEstadoPdf enumEstadoPdf, string mensaje, string docEntry, string objType)
        {
            await _repository.ActualizarEstadoPdf(enumEstadoPdf, mensaje, docEntry, objType);
        }

        private async Task<DataEstadoRequest<DocumentoRequest>> EstructuraFacturaBoleta(DataRow registro, string tipoDocumento)
        {
            string FacturacionID = registro["FACTURACION_ID"].ToString();
            string FacturacionDocEntry = registro["FACTURACION_DOCENTRY"].ToString();
            string FacturacionObjType = registro["FACTURACION_OBJTYPE"].ToString();

            bool EsPagoCredito = Convert.ToBoolean(registro["ES_PAGO_CREDITO"]);
            bool EsDescuentoGlobal = Convert.ToBoolean(registro["ES_DESCUENTO_GLOBAL"]);
            bool EsFacturaContingencia = Convert.ToBoolean(registro["ES_FACTURA_CONTINGENCIA"]);
            bool EsFacturaExportacion = Convert.ToBoolean(registro["ES_FACTURA_EXPORTACION"]);
            bool EsConsumeAnticipo = Convert.ToBoolean(registro["ES_CONSUME_ANTICIPO"]);
            bool EsConsumeGuiaRemision = Convert.ToBoolean(registro["ES_CONSUME_GUIA_REMISION"]);

            #region Estructura de documento

            DocumentoRequest request = new DocumentoRequest
            {
                Close2u = new Close2uRequest()
                {
                    TipoIntegracion = registro["INTEGRADOR_TIPO_INTEGRACION"].ToString(),
                    TipoRegistro = registro["INTEGRADOR_TIPO_REGISTRO"].ToString(),
                    TipoPlantilla = registro["INTEGRADOR_TIPO_PLANTILLA"].ToString()
                },

                DatosDocumento = new DatosDocumentoRequest()
                {
                    Serie = registro["DOC_SERIE"].ToString(),
                    Numero = registro["DOC_CORRELATIVO"].ToString(),
                    FechaEmision = registro["DOC_FECHA_EMISION"].ToString(),
                    FechaVencimiento = registro["DOC_FECHA_VENCIMIENTO"].ToString(),
                    HoraEmision = registro["DOC_HORA_EMISION"].ToString(),
                    Ordencompra = registro["DOC_ORDEN_COMPRA"].ToString(),
                    Moneda = registro["DOC_CODIGO_MONEDA"].ToString(),
                    CondicionPago = registro["DOC_CONDICION_PAGO"].ToString(),
                    Glosa = registro["DOC_COMENTARIOS"].ToString(),
                    FormaPago = registro["DOC_CODIGO_MEDPAGO"].ToString(),
                    PuntoEmisor = ""
                },

                Emisor = new EmisorRequest()
                {
                    TipoDocumentoIdentidad = registro["EMISOR_TIPO_DOCUMENTO_IDENTIDAD"].ToString(),
                    NumeroDocumentoIdentidad = registro["EMISOR_DOCUMENTO_IDENTIDAD"].ToString(),
                    NombreComercial = registro["EMISOR_NOMBRE_COMERCIAL"].ToString(),
                    NombreLegal = registro["EMISOR_RAZON_SOCIAL"].ToString(),
                    Correo = registro["EMISOR_CORREO_ELECTRONICO"].ToString(),
                    DomicilioFiscal = new DomicilioFiscalRequest()
                    {
                        Direccion = registro["EMISOR_DIRECCION"].ToString(),
                        Urbanizacion = registro["EMISOR_URBANIZACION"].ToString(),
                        Distrito = registro["EMISOR_DISTRITO"].ToString(),
                        Provincia = registro["EMISOR_PROVINCIA"].ToString(),
                        Departamento = registro["EMISOR_DEPARTAMENTO"].ToString(),
                        Pais = registro["EMISOR_PAIS"].ToString(),
                        Ubigeo = registro["EMISOR_UBIGEO"].ToString()
                    }
                },

                InformacionAdicional = new InformacionAdicionalRequest()
                {
                    TipoOperacion = registro["DOC_CODIGO_TIPO_OPERACION"].ToString()
                },

                Receptor = new ReceptorRequest()
                {
                    TipoDocumentoIdentidad = registro["RECEPTOR_TIPO_DOCUMENTO_IDENTIDAD"].ToString(),
                    NumeroDocumentoIdentidad = registro["RECEPTOR_DOCUMENTO_IDENTIDAD"].ToString(),
                    NombreLegal = registro["RECEPTOR_RAZON_SOCIAL"].ToString(),
                    NombreComercial = registro["RECEPTOR_NOMBRE_COMERCIAL"].ToString(),
                    Correo = registro["RECEPTOR_CORREO_ELECTRONICO"].ToString(),
                    CorreoCopia = "",
                    DomicilioFiscal = new DomicilioFiscalRequest()
                    {
                        Urbanizacion = registro["RECEPTOR_URBANIZACION"].ToString(),
                        Direccion = registro["RECEPTOR_DIRECCION"].ToString(),
                        Distrito = registro["RECEPTOR_DISTRITO"].ToString(),
                        Provincia = registro["RECEPTOR_PROVINCIA"].ToString(),
                        Departamento = registro["RECEPTOR_DEPARTAMENTO"].ToString(),
                        Pais = registro["RECEPTOR_PAIS"].ToString(),
                        Ubigeo = registro["RECEPTOR_UBIGEO"].ToString()
                    }
                },

                FactorCambio = Convert.ToDecimal(registro["DOC_TIPO_CAMBIO"]),
                DescuentoGlobal = EsDescuentoGlobal ? Convert.ToDecimal(registro["IMPORTE_DESCUENTO_PORCENTAJE"]) : (decimal?)null,
                Sector = EsDescuentoGlobal ? new SectorRequest() { TipoTotalDescuentos = "0" } : null, //0: Descuento por % | 1: Descuento por Monto
            };

            if (EsConsumeGuiaRemision)
            {
                DataTable dtGuiasReferencia = await _repository.ListarGuiasReferencia(FacturacionDocEntry);
                request.Referencias = new ReferenciaRequest
                {
                    DocumentoReferenciaList = new List<DocumentoReferenciaList>()
                };
                foreach (DataRow guia in dtGuiasReferencia.Rows)
                {
                    DocumentoReferenciaList referencia = new DocumentoReferenciaList()
                    {
                        TipoDocumento = guia["GUIA_TIPO"].ToString(),
                        Serie = guia["GUIA_SERIE"].ToString(),
                        Numero = guia["GUIA_CORRELATIVO"].ToString()
                    };
                    request.Referencias.DocumentoReferenciaList.Add(referencia);
                }
            }

            if (EsPagoCredito)
            {
                DataTable dtCuotas = await _repository.ListarCuotas(FacturacionObjType, FacturacionDocEntry);
                request.Cuotas = new List<CuotaRequest>();
                foreach (DataRow cuota in dtCuotas.Rows)
                {
                    CuotaRequest cuotaRequest = new CuotaRequest()
                    {
                        Numero = cuota["CUOTA_LINEA"].ToString(),
                        Monto = Convert.ToDecimal(cuota["CUOTA_MONTO"]),
                        Fecha = cuota["CUOTA_FECHA_PAGO"].ToString(),
                        Moneda = cuota["CUOTA_CODIGO_MONEDA"].ToString()
                    };
                    request.Cuotas.Add(cuotaRequest);
                }
            }

            if (EsConsumeAnticipo)
            {
                DataTable dtAnticipos = await _repository.ListarAnticipos(FacturacionObjType, FacturacionDocEntry);
                if (dtAnticipos.Rows.Count > 0)
                {
                    request.Anticipos = new List<AnticipoRequest>();
                    foreach (DataRow anticipo in dtAnticipos.Rows)
                    {
                        AnticipoRequest anticipoRequest = new AnticipoRequest()
                        {
                            Documento = new DocumentoAnticipoRequest()
                            {
                                TipoDocumento = anticipo["ANTICIPO_TIPO"].ToString(),
                                Serie = anticipo["ANTICIPO_SERIE"].ToString(),
                                Numero = anticipo["ANTICIPO_CORRELATIVO"].ToString()
                            },
                            TotalIgv = Convert.ToDecimal(anticipo["ANTICIPO_IMPUESTO"]),
                            TotalVentaExonerada = EsFacturaExportacion || Convert.ToDecimal(registro["IMPORTE_OPE_EXONERADA"]) == 0 ? 0 : Convert.ToDecimal(anticipo["ANTICIPO_MONTO"]),
                            TotalVentaGravada = EsFacturaExportacion || Convert.ToDecimal(registro["IMPORTE_OPE_GRAVADA"]) == 0 ? 0 : Convert.ToDecimal(anticipo["ANTICIPO_MONTO"]),
                            TotalVentaInafecta = EsFacturaExportacion || Convert.ToDecimal(registro["IMPORTE_OPE_INAFECTA"]) == 0 ? 0 : Convert.ToDecimal(anticipo["ANTICIPO_MONTO"]),
                            TotalVentaExportacion = EsFacturaExportacion ? Convert.ToDecimal(anticipo["ANTICIPO_MONTO"]) : 0
                        };
                        request.Anticipos.Add(anticipoRequest);
                    }
                }
            }

            request.DetalleDocumento = new List<DetalleDocumentoRequest>();
            DataTable dtDetalle = await _repository.ListarDetalleDocumento(FacturacionObjType, FacturacionDocEntry);
            foreach (DataRow linea in dtDetalle.Rows)
            {
                bool EsGratuidad = Convert.ToBoolean(linea["ES_GRATUIDAD"].ToString());
                bool EsDescuentoLinea = Convert.ToBoolean(linea["ES_DESCUENTO_LINEA"]);

                DetalleDocumentoRequest detRequest = new DetalleDocumentoRequest()
                {
                    CodigoProducto = linea["CODIGO_PRODUCTO"].ToString(),
                    CodigoProductoSunat = linea["CODIGO_SUNAT"].ToString(),
                    Descripcion = linea["DESCRIPCION"].ToString(),
                    TipoAfectacion = linea["TIPO_AFECTACION"].ToString(),
                    UnidadMedida = linea["UNIDAD_SUNAT"].ToString(),
                    Cantidad = Convert.ToInt32(linea["CANTIDAD"]),
                    ValorVentaUnitarioItem = !EsGratuidad ? Convert.ToDecimal(linea["VALOR_UNITARIO"]) : (decimal?)null,
                    ValorReferencialUnitarioItem = EsGratuidad ? Convert.ToDecimal(linea["VALOR_UNITARIO"]) : (decimal?)null,
                    Descuento = EsDescuentoLinea ? new DescuentoRequest() { Monto = Convert.ToDecimal(linea["DESCUENTO_PORCENTAJE"]) } : null,
                    EsPorcentaje = EsDescuentoLinea ? "true" : null
                };
                request.DetalleDocumento.Add(detRequest);
            }

            #endregion

            return new DataEstadoRequest<DocumentoRequest>
            {
                Comprobante = FacturacionID,
                DocEntry = FacturacionDocEntry,
                ObjType = FacturacionObjType,
                TipoDocumento = tipoDocumento,
                Data = request
            };
        }

        private async Task<DataEstadoRequest<NotaRequest>> EstructuraCreditoDebito(DataRow registro, string tipoDocumento)
        {
            string FacturacionID = registro["FACTURACION_ID"].ToString();
            string FacturacionDocEntry = registro["FACTURACION_DOCENTRY"].ToString();
            string FacturacionObjType = registro["FACTURACION_OBJTYPE"].ToString();

            #region Estructura de documento

            NotaRequest request = new NotaRequest
            {
                Close2u = new Close2uRequest()
                {
                    TipoIntegracion = registro["INTEGRADOR_TIPO_INTEGRACION"].ToString(),
                    TipoRegistro = registro["INTEGRADOR_TIPO_REGISTRO"].ToString(),
                    TipoPlantilla = registro["INTEGRADOR_TIPO_PLANTILLA"].ToString()
                },

                DatosDocumento = new DatosDocumentoRequest()
                {
                    Serie = registro["DOC_SERIE"].ToString(),
                    Numero = registro["DOC_CORRELATIVO"].ToString(),
                    FechaEmision = registro["DOC_FECHA_EMISION"].ToString(),
                    FechaVencimiento = registro["DOC_FECHA_VENCIMIENTO"].ToString(),
                    HoraEmision = registro["DOC_HORA_EMISION"].ToString(),
                    Ordencompra = registro["DOC_ORDEN_COMPRA"].ToString(),
                    Moneda = registro["DOC_CODIGO_MONEDA"].ToString(),
                    CondicionPago = registro["DOC_CONDICION_PAGO"].ToString(),
                    Glosa = registro["DOC_COMENTARIOS"].ToString(),
                    FormaPago = registro["DOC_CODIGO_MEDPAGO"].ToString(),
                    PuntoEmisor = ""//Pregunta que es
                },

                Emisor = new EmisorRequest()
                {
                    TipoDocumentoIdentidad = registro["EMISOR_TIPO_DOCUMENTO_IDENTIDAD"].ToString(),
                    NumeroDocumentoIdentidad = registro["EMISOR_DOCUMENTO_IDENTIDAD"].ToString(),
                    NombreLegal = registro["EMISOR_RAZON_SOCIAL"].ToString(),
                    NombreComercial = registro["EMISOR_NOMBRE_COMERCIAL"].ToString(),
                    Correo = registro["EMISOR_CORREO_ELECTRONICO"].ToString(),
                    DomicilioFiscal = new DomicilioFiscalRequest()
                    {
                        Direccion = registro["EMISOR_DIRECCION"].ToString(),
                        Urbanizacion = registro["EMISOR_URBANIZACION"].ToString(),
                        Distrito = registro["EMISOR_DISTRITO"].ToString(),
                        Provincia = registro["EMISOR_PROVINCIA"].ToString(),
                        Departamento = registro["EMISOR_DEPARTAMENTO"].ToString(),
                        Pais = registro["EMISOR_PAIS"].ToString(),
                        Ubigeo = registro["EMISOR_UBIGEO"].ToString()
                    }
                },

                InformacionAdicional = new InformacionAdicionalRequest()
                {
                    TipoOperacion = registro["DOC_CODIGO_TIPO_OPERACION"].ToString()
                },

                Receptor = new ReceptorRequest()
                {
                    TipoDocumentoIdentidad = registro["RECEPTOR_TIPO_DOCUMENTO_IDENTIDAD"].ToString(),
                    NumeroDocumentoIdentidad = registro["RECEPTOR_DOCUMENTO_IDENTIDAD"].ToString(),
                    NombreLegal = registro["RECEPTOR_RAZON_SOCIAL"].ToString(),
                    NombreComercial = registro["RECEPTOR_NOMBRE_COMERCIAL"].ToString(),
                    Correo = registro["RECEPTOR_CORREO_ELECTRONICO"].ToString(),
                    CorreoCopia = "",
                    DomicilioFiscal = new DomicilioFiscalRequest()
                    {
                        Direccion = registro["RECEPTOR_DIRECCION"].ToString(),
                        Urbanizacion = registro["RECEPTOR_URBANIZACION"].ToString(),
                        Distrito = registro["RECEPTOR_DISTRITO"].ToString(),
                        Provincia = registro["RECEPTOR_PROVINCIA"].ToString(),
                        Departamento = registro["RECEPTOR_DEPARTAMENTO"].ToString(),
                        Pais = registro["RECEPTOR_PAIS"].ToString(),
                        Ubigeo = registro["RECEPTOR_UBIGEO"].ToString()
                    }
                },

                ComprobanteAjustado = new ComprobanteAjustadoRequest()
                {
                    TipoDocumento = registro["REFERENCIA_TIPO"]?.ToString(),
                    Serie = registro["REFERENCIA_SERIE"]?.ToString(),
                    Numero = registro["REFERENCIA_CORRELATIVO"]?.ToString(),
                    FechaEmision = registro["REFERENCIA_FECHA_EMISION"].ToString()
                },

                Motivo = registro["REFERENCIA_CODIGO_MOTIVO"].ToString(),
                DescuentoGlobal = null,
                Sector = null //0: Descuento por % | 1: Descuento por Monto
            };

            request.DetalleDocumento = new List<DetalleDocumentoRequest>();
            DataTable dtDetalle = await _repository.ListarDetalleDocumento(FacturacionObjType, FacturacionDocEntry);
            foreach (DataRow linea in dtDetalle.Rows)
            {
                bool EsGratuidad = Convert.ToBoolean(linea["ES_GRATUIDAD"].ToString());

                DetalleDocumentoRequest detRequest = new DetalleDocumentoRequest()
                {
                    CodigoProducto = linea["CODIGO_PRODUCTO"].ToString(),
                    CodigoProductoSunat = linea["CODIGO_SUNAT"].ToString(),
                    Descripcion = linea["DESCRIPCION"].ToString(),
                    TipoAfectacion = linea["TIPO_AFECTACION"].ToString(),
                    UnidadMedida = linea["UNIDAD_SUNAT"].ToString(),
                    Cantidad = Convert.ToInt32(linea["CANTIDAD"]),
                    ValorVentaUnitarioItem = !EsGratuidad ? Convert.ToDecimal(linea["VALOR_UNITARIO"]) : (decimal?)null,
                    ValorReferencialUnitarioItem = EsGratuidad ? Convert.ToDecimal(linea["VALOR_UNITARIO"]) : (decimal?)null,
                    Descuento = null,
                    EsPorcentaje = null
                };
                request.DetalleDocumento.Add(detRequest);
            }

            #endregion

            return new DataEstadoRequest<NotaRequest>
            {
                Comprobante = FacturacionID,
                DocEntry = FacturacionDocEntry,
                ObjType = FacturacionObjType,
                TipoDocumento = tipoDocumento,
                Data = request
            };
        }

        private async Task<DataEstadoRequest<GuiaRemisionRequest>> EstructuraGuiaRemision(DataRow registro, string tipoDocumento)
        {
            string FacturacionID = registro["FACTURACION_ID"].ToString();
            string FacturacionDocEntry = registro["FACTURACION_DOCENTRY"].ToString();
            string FacturacionObjType = registro["FACTURACION_OBJTYPE"].ToString();

            #region Estructura de documento

            GuiaRemisionRequest request = new GuiaRemisionRequest
            {
                Close2u = new Close2uRequest()
                {
                    TipoIntegracion = registro["INTEGRADOR_TIPO_INTEGRACION"].ToString(),
                    TipoRegistro = registro["INTEGRADOR_TIPO_REGISTRO"].ToString(),
                    TipoPlantilla = registro["INTEGRADOR_TIPO_PLANTILLA"].ToString()
                },

                DatosDocumento = new DatosGuiaRequest()
                {
                    Serie = registro["DOC_SERIE"].ToString(),
                    Numero = registro["DOC_CORRELATIVO"].ToString(),
                    FechaEmision = registro["DOC_FECHA_EMISION"].ToString(),
                    Glosa = registro["DOC_COMENTARIOS"].ToString(),
                },

                Remitente = new RemitenteDestinatarioRequest()
                {
                    TipoDocumentoIdentidad = registro["EMISOR_TIPO_DOCUMENTO_IDENTIDAD"].ToString(),
                    NumeroDocumentoIdentidad = registro["EMISOR_DOCUMENTO_IDENTIDAD"].ToString(),
                    NombreLegal = registro["EMISOR_RAZON_SOCIAL"].ToString()
                },

                Destinatario = new RemitenteDestinatarioRequest()
                {
                    TipoDocumentoIdentidad = registro["RECEPTOR_TIPO_DOCUMENTO_IDENTIDAD"].ToString(),
                    NumeroDocumentoIdentidad = registro["RECEPTOR_DOCUMENTO_IDENTIDAD"].ToString(),
                    NombreLegal = registro["RECEPTOR_RAZON_SOCIAL"].ToString()
                }
            };

            DataRow drGuia = (await _repository.DatosGuiaRemision(FacturacionObjType, FacturacionDocEntry)).Rows[0];
            request.DatosEnvio = new DatoEnvioRequest()
            {
                FechaTraslado = drGuia["GUIA_FECHA_INICIO_TRASLADO"].ToString(),
                FechaEntrega = drGuia["GUIA_FECHA_INICIO_TRASLADO"].ToString(),
                NumeroPallet = Convert.ToDecimal(drGuia["GUIA_NUMERO_BULTO"] ?? 0),
                UnidadMedida = drGuia["GUIA_UNIDAD_MEDIDA"].ToString(),
                PesoBruto = Convert.ToDecimal(drGuia["GUIA_PESO"] ?? 0),
                //NumeroContenedor = Convert.ToDecimal(drGuia["FE_NUM_CONTENEDOR"])
                ModalidadTraslado = drGuia["GUIA_MODALIDAD_TRASLADO"].ToString(),
                TransbordoProgramado = drGuia["GUIA_INDICADOR_TRANSBORDO_PROGRAMADO"].ToString(),
                MotivoTraslado = drGuia["GUIA_CODIGO_MOTIVO"].ToString(),
                DescripcionTraslado = drGuia["GUIA_DESCRIPCION_MOTIVO"].ToString(),
                
                PuntoPartida = new PuntoLlegadaPartidaRequest()
                {
                    Direccion = drGuia["PUNTO_PARTIDA_DIRECCION"].ToString(),
                    Ubigeo = drGuia["PUNTO_PARTIDA_UBIGEO"].ToString()
                },
                PuntoLlegada = new PuntoLlegadaPartidaRequest()
                {
                    Direccion = drGuia["PUNTO_LLEGADA_DIRECCION"].ToString(),
                    Ubigeo = drGuia["PUNTO_LLEGADA_UBIGEO"].ToString()
                }
            };

            request.Transportista = new ConductorTransportistaRequest()
            {
                TipoDocumentoIdentidad = drGuia["TRANSPORTISTA_TIPO_DOCUMENTO_IDENTIDAD"].ToString(),
                NumeroDocumentoIdentidad = drGuia["TRANSPORTISTA_DOCUMENTO_IDENTIDAD"].ToString(),
                NombreLegal = drGuia["TRANSPORTISTA_RAZON_SOCIAL"].ToString()
            };

            request.Vehiculos = new List<VehiculoRequest>();
            request.Vehiculos.Add(new VehiculoRequest()
            {
                Placa = drGuia["VEHICULO_NUMERO_PLACA"].ToString(),
                Conductor = new ConductorTransportistaRequest()
                {
                    TipoDocumentoIdentidad = drGuia["CONDUCTOR_TIPO_DOCUMENTO_IDENTIDAD"].ToString(),
                    NumeroDocumentoIdentidad = drGuia["CONDUCTOR_DOCUMENTO_IDENTIDAD"].ToString(),
                    NombreLegal = drGuia["CONDUCTOR_RAZON_SOCIAL"].ToString()
                }
            });

            request.DetalleGuia = new List<DetalleGuiaRequest>();

            DataTable dtDetalle = await _repository.ListarDetalleDocumento(FacturacionObjType, FacturacionDocEntry);
            foreach (DataRow linea in dtDetalle.Rows)
            {
                DetalleGuiaRequest detalleGuiaRequest = new DetalleGuiaRequest()
                {
                    NumeroOrden = linea["NUMERO_LINEA"].ToString(),
                    CodigoProducto = linea["CODIGO_PRODUCTO"].ToString(),
                    Descripcion = linea["DESCRIPCION"].ToString(),
                    Cantidad = Convert.ToInt32(linea["CANTIDAD"]),
                    UnidadMedida = linea["UNIDAD_SUNAT"].ToString()
                };
                request.DetalleGuia.Add(detalleGuiaRequest);
            }

            #endregion

            return new DataEstadoRequest<GuiaRemisionRequest>
            {
                Comprobante = FacturacionID,
                DocEntry = FacturacionDocEntry,
                ObjType = FacturacionObjType,
                TipoDocumento = tipoDocumento,
                Data = request
            };
        }

    }
}
