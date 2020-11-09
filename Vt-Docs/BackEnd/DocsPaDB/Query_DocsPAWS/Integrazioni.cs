using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Collections;
using System.Data;

namespace DocsPaDB.Query_DocsPAWS
{
    public class Integrazioni : DBProvider
    {
        private static ILog logger = LogManager.GetLogger(typeof(Integrazioni));

        public ArrayList MIBACT_BACHECA_getDocsDaNotificare(string statoInvia, string statoAggiorna, string campoNCirc)
        {
            ArrayList retVal = new ArrayList();
            try{
                DocsPaUtils.Query q = DocsPaUtils.InitQuery.getInstance().getQuery("MIBACT_GET_C_NOTIFY");
                q.setParam("statinotifica", string.Format("'{0}','{1}'", statoInvia.ToUpper(), statoAggiorna.ToUpper()));
                q.setParam("statoinvia", statoInvia.ToUpper());
                q.setParam("descncircolare", campoNCirc.ToUpper());
                string queryString = q.getSQL();
            logger.Debug(queryString);
            DocsPaVO.ExternalServices.MIBACT_Bacheca_info infoBacheca;
            DataSet dataset = new DataSet();
            this.ExecuteQuery(out dataset, "BACHECADOCS", queryString);
            if (dataset.Tables["BACHECADOCS"] != null && dataset.Tables["BACHECADOCS"].Rows.Count > 0)
            {
                logger.Debug("Righe: " + dataset.Tables["BACHECADOCS"].Rows.Count);
                foreach (DataRow r in dataset.Tables["BACHECADOCS"].Rows)
                {
                    infoBacheca = new DocsPaVO.ExternalServices.MIBACT_Bacheca_info();
                    infoBacheca.doc_titolo = r["DOC_TITOLO"].ToString();
                    infoBacheca.doc_data_creazione = r["DOC_DATA_CREAZIONE"].ToString();
                    infoBacheca.doc_protocollo = r["DOC_PROTOCOLLO"].ToString();
                    infoBacheca.doc_n_circolare = r["DOC_N_CIRCOLARE"].ToString();
                    infoBacheca.doc_autore = r["DOC_AUTORE"].ToString();
                    infoBacheca.servizio_codice = r["SERVIZIO_CODICE"].ToString();
                    infoBacheca.servizio_descrizione = r["SERVIZIO_DESCRIZIONE"].ToString();
                    infoBacheca.ufficio_emittente_codice = r["UFFECOD_ESPIIDAOO"].ToString();
                    infoBacheca.espi_codice = r["ESPI_CODICE"].ToString();
                    infoBacheca.espi_id_aoo = r["UFFECOD_ESPIIDAOO"].ToString();
                    //infoBacheca.circolare_annullata = r["CODE_AMM"].ToString();
                    infoBacheca.statoCircolare = r["STATO"].ToString();
                    infoBacheca.annullamento = r["DOC_DATA_ANNULLA"].ToString();
                    //infoBacheca.nuova_circolare = r["CODE_AMM"].ToString();
                    infoBacheca.id_amministrazione = r["ID_AMMINISTRAZIONE"].ToString();
                    infoBacheca.id_utente = r["ID_UTENTE"].ToString();
                    infoBacheca.id_gruppo = r["ID_GRUPPO"].ToString();
                    retVal.Add(infoBacheca);                    
                }
            }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                retVal = null;
            }
            return retVal;
        }

        public ArrayList MIBACT_BACHECA_GetFileInfoDoc(string idDoc)
        {
            ArrayList retVal = new ArrayList();
            try
            {
                DocsPaUtils.Query q = DocsPaUtils.InitQuery.getInstance().getQuery("MIBACT_GET_INFOFILE_DOC");
                q.setParam("idDoc", idDoc);
                string queryString = q.getSQL();
                logger.Debug(queryString);
                DocsPaVO.ExternalServices.MIGR_File_Info file_info;
                DataSet dataset = new DataSet();
                this.ExecuteQuery(out dataset, "MGR_FS_FI", queryString);
                if (dataset.Tables["MGR_FS_FI"] != null && dataset.Tables["MGR_FS_FI"].Rows.Count > 0)
                {
                    logger.Debug("Righe: " + dataset.Tables["MGR_FS_FI"].Rows.Count);
                    foreach (DataRow r in dataset.Tables["MGR_FS_FI"].Rows)
                    {
                        file_info = new DocsPaVO.ExternalServices.MIGR_File_Info();
                        file_info.PathOld = r["PATH"].ToString();
                        file_info.VersionId = r["VERSION_ID"].ToString();
                        file_info.Docnumber = r["DOCNUMBER"].ToString();
                        file_info.Filesize = r["FILE_SIZE"].ToString();
                        file_info.ImprontaComp = r["VAR_IMPRONTA"].ToString();
                        file_info.Ext = r["EXT"].ToString();
                        file_info.NomeOriginale = r["VAR_NOMEORIGINALE"].ToString();
                        file_info.DataFileAcq = r["DATA_ACQ_FILE"].ToString();
                        file_info.DataVersCreazione = r["DATA_CREA_VERS"].ToString();
                        file_info.Version = r["VERSION"].ToString();
                        file_info.VersionLabel = r["VERSION_LABEL"].ToString();
                        file_info.IdPeopleAutore = r["ID_PEOPLE"].ToString();
                        file_info.IdCorrRuoloAutore = r["ID_CORR_RUOLO"].ToString();
                        file_info.MessaggioLog = r["OGGETTO"].ToString();
                        
                        retVal.Add(file_info);
                    }
                }
                else
                {
                    retVal = null;
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex);
                retVal = null;
            }
            return retVal;
        }
    }
}
