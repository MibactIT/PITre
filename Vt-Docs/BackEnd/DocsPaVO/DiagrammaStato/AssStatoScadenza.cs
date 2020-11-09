using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocsPaVO.DiagrammaStato
{
    public class AssStatoScadenza
    {
        public String IdStato { get; set; }

        public String TerminiScadenza { get; set; }

        public String Tipo { get; set; }
    }

    public struct TipoStato
    {
        public const String SOSPENSIVO = "S";
        public const String INTERRUTTIVO = "I";
    }
}
