using Common;
using System.Data;
using System.Threading.Tasks;

namespace Repository
{
    public sealed class DbRepository
    {
        private const string PLUS_FE_SP_LISTAR_DOCUMENTOS_PENDIENTES = "PLUS_FE_SP_LISTAR_DOCUMENTOS_PENDIENTES";
        private const string PLUS_FE_SP_LISTAR_LINEAS = "PLUS_FE_SP_LISTAR_LINEAS";
        private const string PLUS_FE_SP_LISTAR_ANTICIPOS = "PLUS_FE_SP_LISTAR_ANTICIPOS";
        private const string PLUS_FE_SP_LISTAR_CUOTAS = "PLUS_FE_SP_LISTAR_CUOTAS";
        private const string PLUS_FE_SP_LISTAR_GUIAS_REFERENCIAS = "PLUS_FE_SP_LISTAR_GUIAS_REFERENCIAS";
        private const string PLUS_FE_SP_DATOS_EMISOR = "PLUS_FE_SP_DATOS_EMISOR";
        private const string PLUS_FE_SP_DATOS_GUIA_REMISION = "PLUS_FE_SP_DATOS_GUIA_REMISION";
        
        private const string PLUS_FE_SP_LISTAR_BAJAS_PENDIENTES = "PLUS_FE_SP_LISTAR_BAJAS_PENDIENTES";
        private const string PLUS_FE_SP_LISTAR_DOCUMENTOS_ENVIADOS = "PLUS_FE_SP_LISTAR_DOCUMENTOS_ENVIADOS";

        private readonly string PLUS_FE_UPDATE_ESTADO_INTEGRADOR = Properties.Resources.PLUS_FE_UPDATE_ESTADO_INTEGRADOR;
        private readonly string PLUS_FE_UPDATE_ESTADO_SUNAT = Properties.Resources.PLUS_FE_UPDATE_ESTADO_SUNAT;
        private readonly string PLUS_FE_UPDATE_ESTADO_PDF = Properties.Resources.PLUS_FE_UPDATE_ESTADO_PDF;

        private readonly DbHelper _db;

        public DbRepository()
        {
            _db = new DbHelper(AppConfig.MSSQLConnection);
        }

        public async Task<DataTable> ListarDocumentosPendientes()
        {
            return await _db.DoExecuteReader(PLUS_FE_SP_LISTAR_DOCUMENTOS_PENDIENTES);
        }

        public async Task<DataTable> ListarDetalleDocumento(string objType, string docEntry)
        {
            return await _db.DoExecuteReader(PLUS_FE_SP_LISTAR_LINEAS, objType, docEntry);
        }

        public async Task<DataTable> ListarAnticipos(string objType, string docEntry)
        {
            return await _db.DoExecuteReader(PLUS_FE_SP_LISTAR_ANTICIPOS, objType, docEntry);
        }

        public async Task<DataTable> ListarCuotas(string objType, string docEntry)
        {
            return await _db.DoExecuteReader(PLUS_FE_SP_LISTAR_CUOTAS, objType, docEntry);
        }

        public async Task<DataTable> ListarGuiasReferencia(string docEntry)
        {
            return await _db.DoExecuteReader(PLUS_FE_SP_LISTAR_GUIAS_REFERENCIAS, docEntry);
        }

        public async Task<DataTable> ListarComunicadosBajaPendientes()
        {
            return await _db.DoExecuteReader(PLUS_FE_SP_LISTAR_BAJAS_PENDIENTES);
        }

        public async Task<DataTable> DatosGuiaRemision(string objType, string docEntry)
        {
            return await _db.DoExecuteReader(PLUS_FE_SP_DATOS_GUIA_REMISION, objType, docEntry);
        }

        public async Task<DataTable> DatosEmisor()
        {
            return await _db.DoExecuteReader(PLUS_FE_SP_DATOS_EMISOR);
        }

        public async Task<DataTable> ListarDocumentosEnviados(EnumConsultaEstado enumConsultaEstado)
        {
            string estado = enumConsultaEstado == EnumConsultaEstado.SUNAT ? "ES" : "EP";
            return await _db.DoExecuteReader(PLUS_FE_SP_LISTAR_DOCUMENTOS_ENVIADOS, estado);
        }

        public async Task ActualizarEstadoIntegrador(EnumEstadoDoc enumEstadoDoc, string mensaje, string docEntry, string objType)
        {
            string statement = string.Format(PLUS_FE_UPDATE_ESTADO_INTEGRADOR, (short)enumEstadoDoc, mensaje, docEntry, objType);
            await _db.DoNonQuery(statement);
        }

        public async Task ActualizarEstadoSunat(string codigoEstado, string mensaje, string docEntry, string objType)
        {
            string statement = string.Format(PLUS_FE_UPDATE_ESTADO_SUNAT, codigoEstado, mensaje, docEntry, objType);
            await _db.DoNonQuery(statement);
        }

        public async Task ActualizarEstadoPdf(EnumEstadoPdf enumEstadoPdf, string mensaje, string docEntry, string objType)
        {
            string statement = string.Format(PLUS_FE_UPDATE_ESTADO_PDF, (short)enumEstadoPdf, mensaje, docEntry, objType);
            await _db.DoNonQuery(statement);
        }

    }
}
