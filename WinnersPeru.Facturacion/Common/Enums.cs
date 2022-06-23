namespace Common
{
    public enum EnumMethods : short { GET, POST, PUT, DELETE }
    public enum EnumRoute : short { Factura, Boleta, Nota_Credito, Nota_Debito, Guia_Remision, Retencion, ConsultaPDF, Resumen_Baja, ConsultarEstado }
    public enum EnumConsultaEstado : short { SUNAT, PDF }
    public enum EnumEstadoPdf : short { Descargado = 0, Error = 1, Pendiente = 2}
    public enum EnumEstadoDoc : short { DocEnSeguimiento = 0, DocConError = 1, BajaEnSeguimiento = 2, BajaConError = 3}
}
