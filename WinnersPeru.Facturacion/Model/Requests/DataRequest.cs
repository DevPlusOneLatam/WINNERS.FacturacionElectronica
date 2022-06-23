namespace Model.Requests
{
    public class DataEstadoRequest<T>
    {
        public string Comprobante { get; set; }
        public string TipoDocumento { get; set; }
        public string DocEntry { get; set; }
        public string ObjType { get; set; }
        public T Data { get; set; }
    }
}
