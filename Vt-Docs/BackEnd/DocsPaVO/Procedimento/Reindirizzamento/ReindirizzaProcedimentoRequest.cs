using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DocsPaVO.Procedimento.Reindirizzamento
{
    [Serializable]
    public class ReindirizzaProcedimentoRequest
    {
        public String IdProject { get; set; }

        public String IdAOO { get; set; }

        public String Note { get; set; }

        public DocsPaVO.utente.InfoUtente Utente { get; set; }
    }
}
