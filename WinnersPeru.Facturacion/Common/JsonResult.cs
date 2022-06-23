using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Formatting;

namespace Common
{
    public class JsonResult
    {
        public static JsonMediaTypeFormatter jsonFormatter { get; private set; }

        public static void Inicializar()
        {
            jsonFormatter = new JsonMediaTypeFormatter();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        public static string Serializar<T>(T documento)
        {
            return JsonConvert.SerializeObject(documento);
        }

        public static string SerializarToCamelCase<T>(T documento)
        {
            //var jsonFormatter = new JsonMediaTypeFormatter();
            //jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            return JsonConvert.SerializeObject(documento, jsonFormatter.SerializerSettings);
        }

        public static T Deserializar<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
