using Newtonsoft.Json;
using System.Collections.Generic;

namespace Model.Requests
{
    public class ResumenBaja
    {
        public EmisorRequest Emisor { get; set; }
        public List<ResumenBajaItemList> ResumenBajaItemList { get; set; }
    }

    public class ResumenBajaItemList
    {
        public string Id { get; set; }
        public string Motivo { get; set; }
        public string Numero { get; set; }
        public string Serie { get; set; }
        public string TipoComprobante { get; set; }

        [JsonIgnore]
        public string DocEntry { get; set; }
        [JsonIgnore]
        public string ObjType { get; set; }
    }
}
