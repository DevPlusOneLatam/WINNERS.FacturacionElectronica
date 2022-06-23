namespace Model.Requests
{
    public class DomicilioFiscalRequest
    {
        public string Direccion { get; set; }
        public string Urbanizacion { get; set; }
        public string Distrito { get; set; }
        public string Provincia { get; set; }
        public string Departamento { get; set; }
        public string Pais { get; set; }
        public string Ubigeo { get; set; }
    }
}
