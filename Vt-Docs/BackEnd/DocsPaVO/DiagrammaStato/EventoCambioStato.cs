using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocsPaVO.DiagrammaStato
{
    [Serializable]
    public class EventoCambioStato
    {
        public String Id { get; set; }

        public String Codice { get; set; }

        public String Descrizione { get; set; }
    }
}
