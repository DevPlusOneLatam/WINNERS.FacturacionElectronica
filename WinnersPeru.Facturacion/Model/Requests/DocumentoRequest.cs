using System.Collections.Generic;

namespace Model.Requests
{
    public class DocumentoRequest
    {
        public Close2uRequest Close2u { get; set; }
        public EmisorRequest Emisor { get; set; }
        public ReceptorRequest Receptor { get; set; }
        public SectorRequest Sector { get; set; } //Usado solo para especificar si se envía descuento global
        public ReferenciaRequest Referencias { get; set; }
        public List<CuotaRequest> Cuotas { get; set; }
        public List<AnticipoRequest> Anticipos { get; set; }
        public DatosDocumentoRequest DatosDocumento { get; set; }
        public List<DetalleDocumentoRequest> DetalleDocumento { get; set; }
        public InformacionAdicionalRequest InformacionAdicional { get; set; }

        public decimal? DescuentoGlobal { get; set; }//:null
        //public decimal Detraccion { get; set; }//:null,
        public decimal FactorCambio { get; set; }//:null,
        //public decimal OtrosCargos { get; set; }//:null,
        //public decimal Percepcion { get; set; }//:null,
    }

    public class SectorRequest
    {
        public string TipoTotalDescuentos { get; set; }
    }

    public class AnticipoRequest
    {
        public DocumentoAnticipoRequest Documento { get; set; }
        public decimal TotalIgv { get; set; }
        public decimal TotalVentaExonerada { get; set; }
        public decimal TotalVentaGravada { get; set; }
        public decimal TotalVentaInafecta { get; set; }
        public decimal TotalVentaExportacion { get; set; }
    }

    public class DocumentoAnticipoRequest
    {
        public string TipoDocumento { get; set; }
        public string Serie { get; set; }
        public string Numero { get; set; }
    }
}
