namespace Model.Requests
{
    public class DatosDocumentoRequest
    {
        public string FechaEmision { get; set; }
        public string FechaVencimiento { get; set; }
        public string FormaPago { get; set; }
        public string Glosa { get; set; }
        public string HoraEmision { get; set; }
        public string Moneda { get; set; }
        public string Numero { get; set; }
        public string Ordencompra { get; set; }
        public string PuntoEmisor { get; set; }
        public string Serie { get; set; }
        public string CondicionPago { get; set; }
    }
}
