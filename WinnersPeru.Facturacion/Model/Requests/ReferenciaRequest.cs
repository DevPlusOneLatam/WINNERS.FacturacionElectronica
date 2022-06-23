using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Requests
{
    public class ReferenciaRequest
    {
        public List<DocumentoReferenciaList> DocumentoReferenciaList { get; set; }
    }

    public class DocumentoReferenciaList
    {
        public string TipoDocumento { get; set; }
        public string Numero { get; set; }
        public string Serie { get; set; }
    }
}
