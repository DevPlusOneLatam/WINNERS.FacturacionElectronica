namespace Model.Requests
{
    public class DetalleDocumentoRequest
    {
        public string CodigoProducto { get; set; }
        public string CodigoProductoSunat { get; set; }
        public string Descripcion { get; set; }
        public string TipoAfectacion { get; set; }
        public string UnidadMedida { get; set; }
        public int Cantidad { get; set; }
        //public decimal? PrecioVentaUnitarioItem { get; set; }
        public decimal? ValorVentaUnitarioItem { get; set; }
        public decimal? ValorReferencialUnitarioItem { get; set; } //PARA VENTAS GRATUITAS
        public string EsPorcentaje { get; set; }
        public DescuentoRequest Descuento { get; set; }
    }

    public class DescuentoRequest
    {
        public decimal Monto { get; set; }
    }
}
