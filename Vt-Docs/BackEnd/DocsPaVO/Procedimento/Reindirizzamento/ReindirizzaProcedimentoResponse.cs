using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DocsPaVO.Procedimento.Reindirizzamento
{
    [Serializable]
    public class ReindirizzaProcedimentoResponse
    {
        public Boolean Success { get; set; }

        public String ErrorMessage { get; set; }
    }
}
