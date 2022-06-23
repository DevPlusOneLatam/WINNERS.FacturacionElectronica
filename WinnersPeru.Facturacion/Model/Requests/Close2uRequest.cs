namespace Model.Requests
{
    public class Close2uRequest
    {
        public string TipoIntegracion { get; set; } //OFFLINE: Responsable del correlativo es la EMPRESA / El valor será CONTIGENCIA cuando se envíen ese tipo de documentos
        public string TipoRegistro { get; set; }
        public string TipoPlantilla { get; set; }
    }
}
