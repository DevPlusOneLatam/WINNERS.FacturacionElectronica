namespace Model.Responses
{
    public class ConsultaEstadoResponse
    {
        public Estado EstadoComprobante { get; set; }
        public Estado EstadoEntrega { get; set; }
        public Estado EstadoSunat { get; set; }
        public string HashCode { get; set; }
        public string SerieNumero { get; set; }
        public string XmlFirma { get; set; }
    }

    public class Estado
    {
        public string Codigo { get; set; }
        public string Valor { get; set; }
    }

}
