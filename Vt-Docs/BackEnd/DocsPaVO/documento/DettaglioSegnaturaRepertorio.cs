using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsPaVO.documento
{
    public class DettaglioSegnaturaRepertorio
    {
        public string DocNumber { get; set; }
        public string FormatoContatore { get; set; }
        public string Contatore { get; set; }
        public int? Anno { get; set; }
        public string CodiceUO { get; set; }
        public int? IdAooRf { get; set; }
        public DateTime? DataInserimento { get; set; }
        public DateTime? DataAnnullamento { get; set; }
        public string Tipologia { get; set; }
        public string Classifica { get; set; }
        public string Onnicomprensiva { get; set; }
        public string IsPermanente { get; set; }
    }
}
