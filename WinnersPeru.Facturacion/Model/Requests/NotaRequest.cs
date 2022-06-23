using System.Collections.Generic;

namespace Model.Requests
{
    public class NotaRequest
    {
        public Close2uRequest Close2u { get; set; }
        public EmisorRequest Emisor { get; set; }
        public ReceptorRequest Receptor { get; set; }
        public SectorRequest Sector { get; set; }
        public DatosDocumentoRequest DatosDocumento { get; set; }
        public ComprobanteAjustadoRequest ComprobanteAjustado { get; set; }
        public List<DetalleDocumentoRequest> DetalleDocumento { get; set; }
        public InformacionAdicionalRequest InformacionAdicional { get; set; }

        public decimal? DescuentoGlobal { get; set; }//:null
        //public decimal Detraccion { get; set; }//:null,
        public decimal FactorCambio { get; set; }//:null,
        //public decimal OtrosCargos { get; set; }//:null,
        //public decimal Percepcion { get; set; }//:null,
        public string Motivo { get; set; }
    }
}
