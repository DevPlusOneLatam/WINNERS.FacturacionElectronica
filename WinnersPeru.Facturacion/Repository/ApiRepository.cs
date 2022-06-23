using Common;
using Model.Responses;
using System;
using System.Net.Http;
//using System.Text.Json;
using System.Threading.Tasks;

namespace Repository
{
    public class ApiRepository
    {
        private static EnumRoute ObtenerRutaApi(string tipoDocumento)
        {
            switch (tipoDocumento)
            {
                case "01":
                    return EnumRoute.Factura;
                case "03":
                    return EnumRoute.Boleta;
                case "07":
                    return EnumRoute.Nota_Credito;
                case "08":
                    return EnumRoute.Nota_Debito;
                case "09":
                default:
                    return EnumRoute.Guia_Remision;
            }
        }

        private static HttpClient ObtenerHttpClient()
        {
            string url = AppConfig.ActiveApiProd ? AppConfig.ApiProdUrl : AppConfig.ApiTestUrl;
            string token = AppConfig.ActiveApiProd ? AppConfig.ApiProdToken : AppConfig.ApiTestToken;

            HttpClient httpClient = new HttpClient();
            Uri baseUri = new Uri(url);
            httpClient.BaseAddress = baseUri;
            httpClient.Timeout = TimeSpan.FromSeconds(120);
            httpClient.DefaultRequestHeaders.Add("X-Auth-Token", token);

            //httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //httpClient.DefaultRequestHeaders.Clear();
            //httpClient.DefaultRequestHeaders.ConnectionClose = true;
            return httpClient;
        }

        public static async Task<OperacionResponse> EnviarDocumento<T>(string tipoDocumento, T documento)
        {
            EnumRoute enumRoute = ObtenerRutaApi(tipoDocumento);
            string route = enumRoute.ToString().ToLower().Replace("_", "-");

            HttpResponseMessage httpResponse;
            if (enumRoute == EnumRoute.Guia_Remision)
                httpResponse = await ObtenerHttpClient().PostAsync(route, documento, JsonResult.jsonFormatter);
            else
                httpResponse = await ObtenerHttpClient().PutAsync(route, documento, JsonResult.jsonFormatter);

            return new OperacionResponse
            {
                IsSuccess = httpResponse.IsSuccessStatusCode,
                Contenido = await httpResponse?.Content.ReadAsStringAsync(),
                JsonString = JsonResult.SerializarToCamelCase(documento)
            };
        }

        public static async Task<OperacionResponse> EnviarResumenBaja<T>(T documento)
        {
            HttpResponseMessage httpResponse
                = await ObtenerHttpClient().PutAsync("resumen-baja", documento, JsonResult.jsonFormatter);

            return new OperacionResponse
            {
                IsSuccess = httpResponse.IsSuccessStatusCode,
                Contenido = await httpResponse.Content.ReadAsStringAsync(),
                JsonString = JsonResult.SerializarToCamelCase(documento)
            };
        }

        public static async Task<OperacionResponse> ConsultarEstado<T>(T documento)
        {
            HttpResponseMessage httpResponse
                = await ObtenerHttpClient().PutAsync("consultarEstado", documento, JsonResult.jsonFormatter);

            return new OperacionResponse
            {
                IsSuccess = httpResponse.IsSuccessStatusCode,
                Contenido = await httpResponse.Content.ReadAsStringAsync(),
                JsonString = !httpResponse.IsSuccessStatusCode ? JsonResult.SerializarToCamelCase(documento) : null
            };
        }

        public static async Task<OperacionResponse> ConsultarPdf<T>(T documento)
        {
            HttpResponseMessage httpResponse = await ObtenerHttpClient().PutAsync("consultarPdf", documento, JsonResult.jsonFormatter);
            
            return new OperacionResponse
            {
                IsSuccess = httpResponse.IsSuccessStatusCode,
                Contenido = await httpResponse.Content.ReadAsStringAsync(),
                JsonString = !httpResponse.IsSuccessStatusCode ? JsonResult.SerializarToCamelCase(documento) : null
            };
        }

    }
}
