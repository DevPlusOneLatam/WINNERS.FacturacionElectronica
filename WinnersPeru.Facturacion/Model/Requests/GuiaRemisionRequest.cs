using System.Collections.Generic;

namespace Model.Requests
{
    public class GuiaRemisionRequest
    {
        public Close2uRequest Close2u { get; set; }
        public DatosGuiaRequest DatosDocumento { get; set; }
        ///DocumentosRelacionados - No se envían
        //public List<DocumentoRelacionadoRequest> DocumentosRelacionados { get; set; } 

        public RemitenteDestinatarioRequest Remitente { get; set; }
        public RemitenteDestinatarioRequest Destinatario { get; set; }

        public DatoEnvioRequest DatosEnvio { get; set; }
        public ConductorTransportistaRequest Transportista { get; set; }
        public List<VehiculoRequest> Vehiculos { get; set; }

        public List<DetalleGuiaRequest> DetalleGuia { get; set; }
    }

    public class RemitenteDestinatarioRequest
    {
        public string NombreLegal { get; set; }
        public string NumeroDocumentoIdentidad { get; set; }
        public string TipoDocumentoIdentidad { get; set; }
    }

    public class DatosGuiaRequest
    {
        public string FechaEmision { get; set; }

        public string Glosa { get; set; }

        public string Numero { get; set; }

        public string Serie { get; set; }
    }

    public class DocumentoRelacionadoRequest
    {
        public string NumeroDocumento { get; set; }
        public string CodigoTipoDocumento { get; set; }
    }

    public class DatoEnvioRequest
    {
        public string MotivoTraslado { get; set; }
        public string DescripcionTraslado { get; set; }
        public string TransbordoProgramado { get; set; }
        public decimal PesoBruto { get; set; }
        public string UnidadMedida { get; set; }
        public decimal NumeroPallet { get; set; }
        public string ModalidadTraslado { get; set; }
        public string FechaTraslado { get; set; }
        public string FechaEntrega { get; set; }
        public decimal NumeroContenedor { get; set; }
        public PuntoLlegadaPartidaRequest PuntoLlegada { get; set; }
        public PuntoLlegadaPartidaRequest PuntoPartida { get; set; }
    }

    public class PuntoLlegadaPartidaRequest
    {
        public string Departamento { get; set; }
        public string Direccion { get; set; }
        public string Distrito { get; set; }
        public string Pais { get; set; }
        public string Provincia { get; set; }
        public string Ubigeo { get; set; }
        public string Urbanizacion { get; set; }
    }

    public class ConductorTransportistaRequest
    {
        public string NombreLegal { get; set; }
        public string NumeroDocumentoIdentidad { get; set; }
        public string TipoDocumentoIdentidad { get; set; }
    }

    public class VehiculoRequest
    {
        public string Placa { get; set; }
        public ConductorTransportistaRequest Conductor { get; set; }
    }

    public class DetalleGuiaRequest
    {
        public string CodigoProducto { get; set; }
        public string Descripcion { get; set; }
        public string UnidadMedida { get; set; }
        public int Cantidad { get; set; }
        public string NumeroOrden { get; set; }
    }

}
