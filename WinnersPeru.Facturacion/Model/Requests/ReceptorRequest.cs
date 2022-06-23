namespace Model.Requests
{
    public class ReceptorRequest
    {
        public string Correo { get; set; }
        public string CorreoCopia { get; set; }
        public string NombreComercial { get; set; }
        public string NombreLegal { get; set; }
        public string NumeroDocumentoIdentidad { get; set; }
        public string TipoDocumentoIdentidad { get; set; }
        public DomicilioFiscalRequest DomicilioFiscal { get; set; }
    }
}
