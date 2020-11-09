using System;

namespace DocsPaVO.Rde
{
    [Serializable()]
    public class ConfigRde
    {
        private bool enforceSecurity = true;
        public string ArrivoLabel { get; set; }
        public string PartenzaLabel { get; set; }
        public string InternoLabel { get; set; }
        public string CodiceClassificaDefault { get; set; }
        public string CodiceAmministrazione { get; set; }
        public string CodiceRegistro { get; set; }
        public string CodiceStringaProtEmerg { get; set; }
        public string SecurityString { get; set; }

        public bool EnforceSecurity
        {
            get
            {
                return enforceSecurity;
            }
        }
    }
}
