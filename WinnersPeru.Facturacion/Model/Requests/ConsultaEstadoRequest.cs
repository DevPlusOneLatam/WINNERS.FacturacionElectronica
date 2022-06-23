namespace Model.Requests
{
    public class ConsultaEstadoRequest
    {
        public string Emisor { get; set; }
        public string Numero { get; set; }
        public string Serie { get; set; }
        public string TipoComprobante { get; set; }
    }
}
