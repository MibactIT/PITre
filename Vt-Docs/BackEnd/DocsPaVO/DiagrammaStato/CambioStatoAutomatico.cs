using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocsPaVO.DiagrammaStato
{
    [Serializable]
    public class CambioStatoAutomatico
    {
        public String IdStatoIniziale { get; set; }

        public String IdStatoFinale { get; set; }

        public EventoCambioStato TipoEvento { get; set; }

        public ProfilazioneDinamica.Templates Tipologia { get; set; }

        public trasmissione.RagioneTrasmissione Ragione { get; set; }

        public String IdAmm { get; set; }

    }
}
