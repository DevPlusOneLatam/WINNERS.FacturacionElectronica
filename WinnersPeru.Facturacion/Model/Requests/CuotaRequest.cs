namespace Model.Requests
{
    public class CuotaRequest
    {
        public string Numero { get; set; }
        public decimal Monto { get; set; }
        public string Fecha { get; set; }
        public string Moneda { get; set; }
    }
}
