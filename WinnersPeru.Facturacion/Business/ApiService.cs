using Model.Responses;
using Repository;
using System.Threading.Tasks;

namespace Business
{
    public class ApiService
    {
        public static async Task<OperacionResponse> EnviarDocumento<T>(string tipoDocumento, T documento)
        {
            return await ApiRepository.EnviarDocumento(tipoDocumento, documento);
        }

        public static async Task<OperacionResponse> EnviarComunicadoBaja<T>(T documento)
        {
            return await ApiRepository.EnviarResumenBaja(documento);
        }

        public static async Task<OperacionResponse> ConsultarEstado<T>(T documento)
        {
            return await ApiRepository.ConsultarEstado(documento);
        }

        public static async Task<OperacionResponse> ConsultarPdf<T>(T documento)
        {
            return await ApiRepository.ConsultarPdf(documento);
        }
    }
}
