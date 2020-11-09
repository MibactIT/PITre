using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocsPaVO.documento
{
    [Serializable()]
    public class DettaglioSegnatura
    {
        public string SystemId { get; set; }
        public string ProfileID { get; set; }
        public string VersionId { get; set; }

        public string SegnaturaProtocollo { get; set; }
        public string SegnaturaRepertorio { get; set; }
        public string SegnaturaNP { get; set; }

        public string IsPermanenteProtocollo { get; set; }
        public string IsPermanenteRepertorio { get; set; }
        public string IsPermanenteNP { get; set; }

        public string Segnato { get; set; }


        


        // proprieta' di supporto
        public DettaglioSegnaturaPosition DettaglioSegnaturaPosition { get; set; }
        public DettaglioSegnaturaRepertorio DettaglioSegnaturaRepertorio { get; set; }


        public DettaglioSegnatura()
        {
            this.IsPermanenteProtocollo = String.Empty;
            this.IsPermanenteNP = String.Empty;
            this.IsPermanenteRepertorio = String.Empty;
            this.Segnato = String.Empty;
            this.SegnaturaProtocollo = String.Empty;
            this.SegnaturaRepertorio = String.Empty;
            this.SegnaturaNP = String.Empty;
        }


        public override string ToString()
        {
            List<string> _tempTesti = new List<string>();
            if (!String.IsNullOrWhiteSpace(this.SegnaturaProtocollo) && this.IsPermanenteProtocollo.Equals("1"))
            {
                _tempTesti.Add(this.SegnaturaProtocollo);
            }
            if (!String.IsNullOrEmpty(this.SegnaturaRepertorio) && "1".Equals(this.IsPermanenteRepertorio))
            {
                if (_tempTesti.Count == 0 || "1".Equals(this.DettaglioSegnaturaRepertorio?.Onnicomprensiva))
                {
                    _tempTesti.Add(this.SegnaturaRepertorio);
                }
            }

            return _tempTesti.Aggregate((i, j) => i + " " + j);
        }

    }
}
