using DocsPaVO.amministrazione;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Configuration;

namespace BusinessLogic.RDE
{
    public class RDEManager
    {
        private static ILog logger = LogManager.GetLogger(typeof(RDEManager));

        public static bool DownloadRDE(InfoAmministrazione infoAmm, out byte[] stream)
        {
            bool retVal = false;
            stream = null;

            DocsPaDB.Query_DocsPAWS.Amministrazione ammDb = new DocsPaDB.Query_DocsPAWS.Amministrazione();
            DocsPaVO.utente.UnitaOrganizzativa uOrg = new DocsPaVO.utente.UnitaOrganizzativa();

            try
            {
                uOrg.systemId = infoAmm.IDAmm;

                DocsPaVO.utente.Registro reg = null;

                ArrayList listaRegistri = new ArrayList();                
                listaRegistri = ammDb.getListRegByIdAmm(uOrg);

                if (listaRegistri.Count > 0)
                {
                    reg = (DocsPaVO.utente.Registro)listaRegistri[0];
                    string codiceClassDefault = ConfigurationManager.AppSettings["CODICE_CLASSIFICAZIONE_DEFAULT"];
                    DownloadRDE drde = new DownloadRDE();

                    drde.CodiceClassificaDefault = codiceClassDefault;
                    drde.ArrivoLabel = "Arrivo";
                    drde.PartenzaLabel = "Partenza";
                    drde.InternoLabel = "Interno";
                    drde.CodiceAmministrazione = infoAmm.Codice;
                    drde.CodiceRegistro = reg.codRegistro;
                    drde.CodiceStringaProtEmerg = "MiBAC|{0}|{1}|{2}-{3}";
                    drde.SecurityString = drde.calkHash(drde);

                    drde.addConfigXMLFile_toSetupZipped(drde, out stream);

                    retVal = true;
                }
            }
            catch (Exception ex)
            {
                logger.Debug(ex.Message);                
                logger.Debug("Errore durante il download del pacchetto RDE (DownloadRDE)");
            }
            return retVal;
        }
    }
}
