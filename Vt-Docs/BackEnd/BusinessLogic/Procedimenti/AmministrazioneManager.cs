using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace BusinessLogic.Procedimenti
{
    public class AmministrazioneManager
    {
        private static ILog logger = LogManager.GetLogger(typeof(AmministrazioneManager));

        public static List<String> GetTipologieDocumento(String[] AOO)
        {
            DocsPaDB.Query_DocsPAWS.Procedimenti p = new DocsPaDB.Query_DocsPAWS.Procedimenti();
            return p.GetTipologieDocumento(AOO);
        }

        public static List<String> GetTipologieFascicolo(String[] AOO)
        {
            DocsPaDB.Query_DocsPAWS.Procedimenti p = new DocsPaDB.Query_DocsPAWS.Procedimenti();
            return p.GetTipologieFascicolo(AOO);
        }

        public static List<DocsPaVO.utente.Registro> GetAOO()
        {
            DocsPaDB.Query_DocsPAWS.Procedimenti p = new DocsPaDB.Query_DocsPAWS.Procedimenti();
            return p.GetAOO();
        }
    }
}
