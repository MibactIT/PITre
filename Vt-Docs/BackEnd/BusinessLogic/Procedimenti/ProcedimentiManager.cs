using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using DocsPaVO.Procedimento;
using log4net;

namespace BusinessLogic.Procedimenti
{
    public class ProcedimentiManager
    {

        private static ILog logger = LogManager.GetLogger(typeof(ProcedimentiManager));

        public static bool InsertDoc(string idProject, string idProfile, string idCorrGlobali, bool isDocPrincipale, string idProcedimento, bool visualizzato)
        {
            DocsPaDB.Query_DocsPAWS.Procedimenti proc = new DocsPaDB.Query_DocsPAWS.Procedimenti();
            return proc.InsertDoc(idProject, idProfile, idCorrGlobali, isDocPrincipale, idProcedimento, visualizzato);
        }

        public static bool InsertFaseProcedimento(string idProject, string idStato)
        {
            DocsPaDB.Query_DocsPAWS.Procedimenti proc = new DocsPaDB.Query_DocsPAWS.Procedimenti();
            return proc.InsertFaseProcedimento(idProject, idStato);
        }

        public static bool UpdateStato(string idProfile, string idProject)
        {
            DocsPaDB.Query_DocsPAWS.Procedimenti proc = new DocsPaDB.Query_DocsPAWS.Procedimenti();
            return proc.UpdateStato(idProfile, idProject);
        }

        public static bool IsDocVisualizzato(string idProject, string idProfile)
        {
            DocsPaDB.Query_DocsPAWS.Procedimenti proc = new DocsPaDB.Query_DocsPAWS.Procedimenti();
            return proc.IsDocVisualizzato(idProject, idProfile);
        }

        public static bool IsProcedimento(string idFascicolo)
        {
            bool result = false;

            Procedimento proc = GetProcedimentoByIdFascicolo(idFascicolo);
            if (proc != null && !string.IsNullOrEmpty(proc.Id) && proc.Id.Equals(idFascicolo))
                result = true;

            return result;
        }

        public static bool IsFolderInProceeding(string idFolder)
        {
            DocsPaDB.Query_DocsPAWS.Procedimenti proc = new DocsPaDB.Query_DocsPAWS.Procedimenti();
            return proc.IsFolderInProceeding(idFolder);
        }

        public static List<Procedimento> GetProcedimentiNonVisualizzati(string idCorrGlobali)
        {
            DocsPaDB.Query_DocsPAWS.Procedimenti proc = new DocsPaDB.Query_DocsPAWS.Procedimenti();
            return proc.GetProcedimentiNonVisualizzati(idCorrGlobali);
        }

        public static Procedimento GetProcedimentoByIdFascicolo(string idFascicolo)
        {
            DocsPaDB.Query_DocsPAWS.Procedimenti proc = new DocsPaDB.Query_DocsPAWS.Procedimenti();
            return proc.GetProcedimentoByIdFascicolo(idFascicolo);
        }

        public static Procedimento GetProcedimentoByIdFolder(string idFolder)
        {
            DocsPaDB.Query_DocsPAWS.Procedimenti proc = new DocsPaDB.Query_DocsPAWS.Procedimenti();
            return proc.GetProcedimentoByIdFolder(idFolder);
        }

        public static Procedimento GetProcedimentoByIdEsterno(string idProcedimento)
        {
            DocsPaDB.Query_DocsPAWS.Procedimenti proc = new DocsPaDB.Query_DocsPAWS.Procedimenti();
            return proc.GetProcedimentoByIdEsterno(idProcedimento);
        }

        public static Procedimento GetProcedimentoByIdDoc(string idProfile)
        {
            DocsPaDB.Query_DocsPAWS.Procedimenti proc = new DocsPaDB.Query_DocsPAWS.Procedimenti();
            return proc.GetProcedimentoByIdDoc(idProfile);
        }

        public static string GetIdIstanzaProcedimento(string idProject)
        {
            DocsPaDB.Query_DocsPAWS.Procedimenti proc = new DocsPaDB.Query_DocsPAWS.Procedimenti();
            return proc.GetIdIstanzaProcedimento(idProject);
        }

        public static EsitoProcedimento GetEsitoProcedimento(string idFascicolo)
        {
            DocsPaDB.Query_DocsPAWS.Procedimenti proc = new DocsPaDB.Query_DocsPAWS.Procedimenti();
            return proc.GetEsitoProcedimento(idFascicolo);
        }

        public static string[] GetTipiProcedimentoAmministrazione(string idAmm)
        {
            DocsPaDB.Query_DocsPAWS.Procedimenti proc = new DocsPaDB.Query_DocsPAWS.Procedimenti();
            return proc.GetTipiProcedimentoAmministrazione(idAmm);
        }

        public static string GetIdTemplateDocByDescProcedimento(string descrizione, string idAmm)
        {
            DocsPaDB.Query_DocsPAWS.Procedimenti proc = new DocsPaDB.Query_DocsPAWS.Procedimenti();
            return proc.GetIdTemplateDocByDescProcedimento(descrizione, idAmm);
        }

        public static string GetIdTemplateFascByDescProcedimento(string descrizione, string idAmm)
        {
            DocsPaDB.Query_DocsPAWS.Procedimenti proc = new DocsPaDB.Query_DocsPAWS.Procedimenti();
            return proc.GetIdTemplateFascByDescProcedimento(descrizione, idAmm);
        }

        public static void CambioStatoProcedimento(string idFascicolo, string tipoEvento, string idOggetto, DocsPaVO.utente.InfoUtente utente)
        {
            logger.Debug("BEGIN");
            DocsPaDB.Query_DocsPAWS.Procedimenti proc = new DocsPaDB.Query_DocsPAWS.Procedimenti();
            string idStato = proc.GetIdPerCambioStato(tipoEvento, idOggetto);

            if (!string.IsNullOrEmpty(idStato))
            {
                logger.Debug("Stato: " + idStato);
                DocsPaVO.ProfilazioneDinamica.Templates template = BusinessLogic.ProfilazioneDinamica.ProfilazioneFascicoli.getTemplateFascDettagli(idFascicolo);
                if (template != null)
                {
                    int idDiagram = BusinessLogic.DiagrammiStato.DiagrammiStato.getDiagrammaAssociatoFasc(template.ID_TIPO_FASC);
                    if (idDiagram != 0)
                    {
                        DocsPaVO.DiagrammaStato.DiagrammaStato stateDiagram = BusinessLogic.DiagrammiStato.DiagrammiStato.getDiagrammaById(idDiagram.ToString());
                        if (stateDiagram != null)
                        {
                            logger.DebugFormat("Trovato evento per cambio stato - stato={0} fascicolo={1} tipoevento={2}", idStato, idFascicolo, tipoEvento);
                            BusinessLogic.DiagrammiStato.DiagrammiStato.salvaModificaStatoFasc(idFascicolo, idStato, stateDiagram, utente.idPeople, utente, string.Empty);

                            // CABLATURA PER DEMO 21/11
                            if (tipoEvento.ToUpper() == "ACCETTAZIONE")
                            {
                                DocsPaVO.trasmissione.RagioneTrasmissione ragTrasm = BusinessLogic.Trasmissioni.RagioniManager.getRagione(idOggetto);
                                if (ragTrasm != null)
                                {
                                    if (template.ELENCO_OGGETTI != null && template.ELENCO_OGGETTI.Count > 0)
                                    {
                                        bool toUpdate = false;
                                        foreach (DocsPaVO.ProfilazioneDinamica.OggettoCustom ogg in template.ELENCO_OGGETTI)
                                        {
                                            if (ogg.DESCRIZIONE.ToUpper() == "RUOLO ASSEGNATARIO")
                                            {
                                                logger.Debug("Ruolo assegnatario - ID=" + utente.idCorrGlobali);
                                                ogg.VALORE_DATABASE = utente.idCorrGlobali;
                                                toUpdate = true;
                                            }
                                            if (ogg.DESCRIZIONE.ToUpper() == "UTENTE ASSEGNATARIO")
                                            {
                                                DocsPaVO.utente.Corrispondente corr = BusinessLogic.Utenti.UserManager.getCorrispondenteByIdPeople(utente.idPeople, DocsPaVO.addressbook.TipoUtente.INTERNO, utente);
                                                if (corr != null)
                                                {
                                                    logger.Debug("Utente assegnatario - idPeople=" + utente.idPeople + " - idCorrGlobali=" + corr.systemId);
                                                    ogg.VALORE_DATABASE = corr.systemId;
                                                    toUpdate = true;
                                                }
                                            }
                                        }
                                        if (toUpdate)
                                        {
                                            BusinessLogic.ProfilazioneDinamica.ProfilazioneFascicoli.salvaInserimentoUtenteProfDimFasc(template, idFascicolo);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            logger.Debug("END");
        }

        public static void CambioStatoAutomatico(string idFascicolo, string tipoEvento, DocsPaVO.utente.InfoUtente utente, string idTipoDoc, string idRagione)
        {
            logger.Debug("BEGIN");

            DocsPaVO.DiagrammaStato.CambioStatoAutomatico itemCambio = DiagrammiStato.DiagrammiStato.GetCambioAutomaticoStatoFasc(idFascicolo);
            if(itemCambio != null && !string.IsNullOrEmpty(itemCambio.IdStatoFinale) && itemCambio.TipoEvento != null && itemCambio.TipoEvento.Codice == tipoEvento)
            {
                logger.DebugFormat("Individuato automatismo. Stato di arrivo={0}, tipo evento {1}", itemCambio.IdStatoFinale, itemCambio.TipoEvento.Codice);

                if(itemCambio.TipoEvento.Codice == "ACCETTAZIONE" || itemCambio.TipoEvento.Codice == "RIFIUTO")
                {
                    if(itemCambio.Ragione != null && !string.IsNullOrEmpty(itemCambio.Ragione.systemId) && itemCambio.Ragione.systemId != idRagione)
                    {
                        logger.Debug("Ragione di trasmissione non coincidente. Automatismo non previsto");
                        return;
                    }
                    else
                    {
                        bool result = CambiaStatoFascicolo(idFascicolo, itemCambio.IdStatoFinale, utente);
                        if (!result)
                            logger.Debug("Cambio stato non effettuato!");
                    }
                }
                else if(itemCambio.TipoEvento.Codice == "SPEDIZIONE" || itemCambio.TipoEvento.Codice == "RICEZIONE")
                {
                    if(itemCambio.Tipologia != null && !string.IsNullOrEmpty(itemCambio.Tipologia.ID_TIPO_ATTO) && itemCambio.Tipologia.ID_TIPO_ATTO != idTipoDoc)
                    {
                        logger.Debug("Tipologia documentale differente. Automatismo non previsto");
                        return;
                    }
                    else
                    {
                        bool result = CambiaStatoFascicolo(idFascicolo, itemCambio.IdStatoFinale, utente);
                        if (!result)
                            logger.Debug("Cambio stato non effettuato!");
                    }
                }
            }
            
            logger.Debug("END");
        }

        public static string GetValoreFromProcedimento(DocsPaVO.ProfilazioneDinamica.Templates template, string key)
        {
            string result = string.Empty;
            string field = DocsPaUtils.Configuration.InitConfigurationKeys.GetValue("0", key);
            if(!string.IsNullOrEmpty(field) && template.ELENCO_OGGETTI != null && template.ELENCO_OGGETTI.Count > 0)
            {
                foreach(DocsPaVO.ProfilazioneDinamica.OggettoCustom oggetto in template.ELENCO_OGGETTI)
                {
                    if(oggetto.DESCRIZIONE.ToUpper() == field.ToUpper())
                    {
                        result = oggetto.VALORE_DATABASE;
                        break;
                    }
                }
            }
            return result;
        }

        public static string GetIdCorrDefault(DocsPaVO.ProfilazioneDinamica.Templates template, string key)
        {
            string result = string.Empty;
            string field = DocsPaUtils.Configuration.InitConfigurationKeys.GetValue("0", key);
            if (!string.IsNullOrEmpty(field) && template.ELENCO_OGGETTI != null && template.ELENCO_OGGETTI.Count > 0)
            {
                foreach (DocsPaVO.ProfilazioneDinamica.OggettoCustom oggetto in template.ELENCO_OGGETTI)
                {
                    if (oggetto.DESCRIZIONE.ToUpper() == field.ToUpper())
                    {
                        result = oggetto.ID_RUOLO_DEFAULT;
                        break;
                    }
                }
            }
            return result;
        }

        public static bool CheckStatoChisuraProcedimento(string idStato)
        {
            DocsPaDB.Query_DocsPAWS.Procedimenti proc = new DocsPaDB.Query_DocsPAWS.Procedimenti();
            return proc.CheckStatoChiusuraProcedimento(idStato);
        }

        public static DocsPaVO.Procedimento.Report.ReportProcedimentoResponse GetProcedimentiReport(DocsPaVO.Procedimento.Report.ReportProcedimentoRequest request)
        {
            DocsPaVO.Procedimento.Report.ReportProcedimentoResponse response = new DocsPaVO.Procedimento.Report.ReportProcedimentoResponse();

            try
            {
                DocsPaVO.documento.FileDocumento report = new DocsPaVO.documento.FileDocumento();

                DocsPaDB.Query_DocsPAWS.Procedimenti proc = new DocsPaDB.Query_DocsPAWS.Procedimenti();

                List<DocsPaVO.filtri.FiltroRicerca> filters = new List<DocsPaVO.filtri.FiltroRicerca>();

                filters.Add(new DocsPaVO.filtri.FiltroRicerca() { argomento = "ID_TIPO_FASC", valore = request.IdProcedimento });
                filters.Add(new DocsPaVO.filtri.FiltroRicerca() { argomento = "ANNO", valore = request.Anno });

                List<DettaglioProcedimento> items = proc.GetProcedimentiReport(filters);

                if (items.Count > 0)
                {
                    BusinessLogic.Modelli.AsposeModelProcessor.PdfModelProcessor processor = new Modelli.AsposeModelProcessor.PdfModelProcessor();

                    report = processor.CreaReportProcedimentoSingolo(request, items);

                    response.Doc = report;
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                logger.Error("Errore in GetProcedimentiReport: ", ex);
                response.Doc = null;
                response.Success = false;
            }

            return response;
        }

        public static DocsPaVO.documento.DocumentConsolidationStateInfo ConsolidateNoSecurity(DocsPaVO.utente.InfoUtente userInfo, string idDocument, DocsPaVO.documento.DocumentConsolidationStateEnum toState, bool bypassFinalStateCheck)
        {
            return BusinessLogic.Documenti.DocumentConsolidation.ConsolidateNoSecurity(userInfo, idDocument, toState, bypassFinalStateCheck);
        }

        public static void CheckEventiFirma(string idProfile, string idPeople, string idGruppo, string idAmm)
        {
            logger.Debug("BEGIN");
            try
            {
                DocsPaVO.ProfilazioneDinamica.Templates template = ProfilazioneDinamica.ProfilazioneDocumenti.getTemplate(idProfile);

                if (template != null)
                {
                    DocsPaVO.utente.InfoUtente infoUt = new DocsPaVO.utente.InfoUtente();
                    DocsPaVO.utente.Utente u = Utenti.UserManager.getUtenteById(idPeople);
                    DocsPaVO.utente.Ruolo r = Utenti.UserManager.getRuoloByIdGruppo(idGruppo);
                    infoUt = Utenti.UserManager.GetInfoUtente(u, r);

                    ArrayList listaFascicoli = BusinessLogic.Fascicoli.FascicoloManager.getFascicoliDaDocNoSecurity(infoUt, idProfile);
                    logger.Debug("Analisi fascicoli");
                    foreach (DocsPaVO.fascicolazione.Fascicolo f in listaFascicoli)
                    {
                        DocsPaVO.ProfilazioneDinamica.Templates templateFasc = ProfilazioneDinamica.ProfilazioneFascicoli.getTemplateFasc(f.systemID);
                        if (templateFasc != null && templateFasc.SYSTEM_ID != 0)
                        {
                            logger.Debug("Fascicolo ID=" + f.systemID + " tipizzato - " + templateFasc.DESCRIZIONE);
                            CambioStatoProcedimento(f.systemID, "FIRMA", template.SYSTEM_ID.ToString(), infoUt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Debug("Errore in CheckEventiFirma", ex);
            }

            logger.Debug("END");
        }

        private static bool CambiaStatoFascicolo(string idFascicolo, string idStato, DocsPaVO.utente.InfoUtente utente)
        {
            bool result = false;
            DocsPaVO.ProfilazioneDinamica.Templates template = BusinessLogic.ProfilazioneDinamica.ProfilazioneFascicoli.getTemplateFascDettagli(idFascicolo);
            if(template != null)
            {
                int idDiagram = BusinessLogic.DiagrammiStato.DiagrammiStato.getDiagrammaAssociatoFasc(template.ID_TIPO_FASC);
                if(idDiagram != 0)
                {
                    DocsPaVO.DiagrammaStato.DiagrammaStato stateDiagram = BusinessLogic.DiagrammiStato.DiagrammiStato.getDiagrammaById(idDiagram.ToString());
                    if(stateDiagram != null)
                    {
                        BusinessLogic.DiagrammiStato.DiagrammiStato.salvaModificaStatoFasc(idFascicolo, idStato, stateDiagram, utente.idPeople, utente, string.Empty);
                        result = true;
                    }
                }
            }
            return result;
        }

        public static void UpdateDataScadenzaProcedimento(string idProject, string tipoStato, DocsPaVO.utente.InfoUtente user)
        {
            logger.Debug("BEGIN");

            try
            {

                // Estrazione tipologia
                DocsPaVO.ProfilazioneDinamica.Templates template = BusinessLogic.ProfilazioneDinamica.ProfilazioneFascicoli.getTemplateFascDettagli(idProject);
                if (template != null && template.ELENCO_OGGETTI != null)
                {
                    string dataScadenza;
                    string terminiProcedimento = string.Empty;

                    // 1 - Estrazione termini temporali procedimento
                    foreach (DocsPaVO.ProfilazioneDinamica.OggettoCustom ogg in template.ELENCO_OGGETTI)
                    {
                        if (ogg.DESCRIZIONE.ToUpper() == DocsPaUtils.Configuration.InitConfigurationKeys.GetValue("0", "BE_PROCEDIMENTO_TERMINE").ToUpper())
                        {
                            terminiProcedimento = ogg.VALORE_DATABASE;
                            break;
                        }
                    }

                    // 2 - Calcolo nuova data di scadenza
                    foreach (DocsPaVO.ProfilazioneDinamica.OggettoCustom ogg in template.ELENCO_OGGETTI)
                    {
                        if (ogg.DESCRIZIONE.ToUpper() == DocsPaUtils.Configuration.InitConfigurationKeys.GetValue("0", "BE_PROCEDIMENTO_DATA_SCADENZA").ToUpper())
                        {
                            if (tipoStato == DocsPaVO.DiagrammaStato.TipoStato.SOSPENSIVO)
                            {
                                // Se lo stato di partenza è SOSPENSIVO il conteggio dei giorni deve ripartire dal giorno del passaggio allo stato in questione
                                DocsPaVO.fascicolazione.Fascicolo fascicolo = BusinessLogic.Fascicoli.FascicoloManager.getFascicoloById(idProject, user);

                                if (fascicolo == null || string.IsNullOrEmpty(fascicolo.dataCreazione))
                                    throw new Exception("Data creazione non presente");

                                DateTime dataSospensione = Convert.ToDateTime(DiagrammiStato.DiagrammiStato.GetDataUltimoCambioStato(idProject));
                                DateTime dataAvvio = Convert.ToDateTime(fascicolo.dataCreazione);
                                int days = (dataSospensione.Date - dataAvvio.Date).Days;

                                ogg.VALORE_DATABASE = DateTime.Now.AddDays(Convert.ToInt32(terminiProcedimento) - days).ToString("dd/MM/yyyy");

                            }
                            else if (tipoStato == DocsPaVO.DiagrammaStato.TipoStato.INTERRUTTIVO)
                            {
                                // Se lo stato di partenza è INTERRUTTIVO il conteggio dei giorni deve azzerarsi
                                ogg.VALORE_DATABASE = DateTime.Now.AddDays(Convert.ToInt32(terminiProcedimento)).ToString("dd/MM/yyyy");
                            }
                            break;
                        }
                    }

                    // Aggiornamento template
                    BusinessLogic.ProfilazioneDinamica.ProfilazioneFascicoli.salvaInserimentoUtenteProfDimFasc(template, idProject);
                }
            }
            catch(Exception ex)
            {
                logger.Debug(ex);
                throw new Exception("Errore nell'aggiornamento della data di scadenza del procedimento - " + ex.Message);
            }

            logger.Debug("END");
        }

        public static List<DocsPaVO.utente.Registro> GetAOOAssociateProcedimento(string template, string idAmm)
        {
            DocsPaDB.Query_DocsPAWS.Procedimenti proc = new DocsPaDB.Query_DocsPAWS.Procedimenti();
            return proc.GetAOOAssociateProcedimento(template, idAmm);
        }

        #region Reindirizzamento

        public static DocsPaVO.Procedimento.Reindirizzamento.ReindirizzaProcedimentoResponse ReindirizzaProcedimento(string idProject, string idAOO, DocsPaVO.utente.InfoUtente infoUtente, string note)
        {
            bool result = false;
            string msg = string.Empty;
            DocsPaVO.Procedimento.Reindirizzamento.ReindirizzaProcedimentoResponse response = new DocsPaVO.Procedimento.Reindirizzamento.ReindirizzaProcedimentoResponse();

            using (DocsPaDB.TransactionContext trContxt = new DocsPaDB.TransactionContext())
            {
                try
                {
                    // Procedimento da reindirizzare
                    Procedimento oldProc = GetProcedimentoByIdFascicolo(idProject);
                    DocsPaVO.fascicolazione.Fascicolo oldFasc = Fascicoli.FascicoloManager.getFascicoloById(idProject, infoUtente);
                    
                   
                    // AOO di destinazione
                    DocsPaVO.utente.Registro reg = Utenti.RegistriManager.getRegistro(idAOO);

                    // Utente di sistema AOO di destinazione
                    DocsPaVO.utente.InfoUtente newInfoUtente = getInfoUtenteSistema(reg.codRegistro, reg.idAmministrazione);
                    DocsPaVO.utente.Utente newUtente = Utenti.UserManager.getUtenteById(newInfoUtente.idPeople);
                    DocsPaVO.utente.Ruolo newRuolo = Utenti.UserManager.getRuoloByIdGruppo(newInfoUtente.idGruppo);

                    #region CONTROLLO PRESENZA ED EVENTUALE INSERIMENTO UTENTE IN RUBRICA
                    DocsPaVO.utente.Corrispondente oldCorr = Utenti.UserManager.getCorrispondenteBySystemID(oldProc.Autore);
                    DocsPaVO.utente.Corrispondente corr = Utenti.UserManager.getCorrispondenteByCodRubrica(oldCorr.codiceRubrica, newInfoUtente);
                    if(corr != null)
                    {
                        logger.Debug("Corrispondente già presente in rubrica");
                    }
                    else
                    {
                        logger.Debug("Corrispondente non presente in rubrica");
                        corr = new DocsPaVO.utente.Utente();
                        corr.codiceCorrispondente = oldCorr.codiceCorrispondente;
                        corr.codiceRubrica = oldCorr.codiceRubrica;
                        corr.cognome = oldCorr.cognome;
                        corr.nome = oldCorr.nome;
                        corr.descrizione = oldCorr.descrizione;
                        corr.tipoCorrispondente = "E";
                        corr.email = oldCorr.email ;
                        corr.idAmministrazione = newInfoUtente.idAmministrazione;
                        corr.idRegistro = reg.systemId;
                        corr.canalePref = oldCorr.canalePref != null ? oldCorr.canalePref : null;
                        DocsPaVO.addressbook.DettagliCorrispondente dettagli = new DocsPaVO.addressbook.DettagliCorrispondente();
                        dettagli.Corrispondente.AddCorrispondenteRow("", "", "", "", "", "", "", "", "", "", "", "", "", "", "");
                        corr.info = dettagli;
                        corr.dettagli = false;
                        corr = Utenti.addressBookManager.insertCorrispondente(corr, null);
                        if(corr != null)
                        {
                            List<DocsPaVO.utente.MailCorrispondente> mbox = new List<DocsPaVO.utente.MailCorrispondente>();
                            mbox.Add(new DocsPaVO.utente.MailCorrispondente()
                            {
                                Email = corr.email,
                                Note = string.Empty,
                                Principale = "1"
                            });
                            if (!BusinessLogic.Utenti.addressBookManager.InsertMailCorrispondente(mbox, corr.systemId))
                                throw new Exception("Errore inserimento mail corrispondente");
                        }
                        else
                        {
                            throw new Exception("Errore inserimento corrispondente");
                        }

                    }
                    #endregion

                    #region CREAZIONE NUOVO DOCUMENTO
                    string oldIdDoc = GetIdIstanzaProcedimento(idProject);
                    DocsPaVO.documento.SchedaDocumento oldDoc = Documenti.DocManager.getDettaglio(infoUtente, oldIdDoc, oldIdDoc);
                    DocsPaVO.documento.FileRequest oldFileReq = (DocsPaVO.documento.Documento)oldDoc.documenti[0];
                    DocsPaVO.documento.FileDocumento oldFile = Documenti.FileManager.getFileFirmato(oldFileReq, infoUtente, false);

                    BusinessLogic.Modelli.AsposeModelProcessor.PdfModelProcessor processor = new Modelli.AsposeModelProcessor.PdfModelProcessor(oldFile.content);

                    logger.Debug("Ricerca tipologia documento");
                    string idDocumentTypology = GetIdTemplateDocByDescProcedimento(oldDoc.template.DESCRIZIONE, newInfoUtente.idAmministrazione);
                    DocsPaVO.ProfilazioneDinamica.Templates templateDoc = processor.PopolaTemplateDocumento(idDocumentTypology);

                    string newDocnumber = reindirizzaProcedimentoCreaIstanzaPrincipale(newInfoUtente, reg, oldDoc.oggetto.descrizione, templateDoc, corr, newRuolo, corr.codiceRubrica, oldFile.content, note);
                    if (string.IsNullOrEmpty(newDocnumber))
                        throw new Exception("Errore nella creazione dell'istanza principale");

                    logger.Debug("Aggiunta allegati");
                    if (oldDoc.allegati != null)
                    {
                        foreach (DocsPaVO.documento.Allegato all in oldDoc.allegati)
                        {
                            reindirizzaProcedimentoCreaAllegati(newInfoUtente, all, newDocnumber);
                        }
                    }

                    logger.Debug("Aggiunta allegato reindirizzamento");
                    string[] parametri = new string[3];
                    parametri[0] = oldDoc.registro.descrizione;
                    parametri[1] = reg.descrizione;
                    parametri[2] = oldDoc.oggetto.descrizione;
                    DocsPaVO.documento.FileDocumento fileAllegato = Modelli.StampaRicevutaGenerica.Create(newInfoUtente, parametri, Modelli.TipoRicevuta.Reindirizzamento);
                    if (fileAllegato == null)
                        throw new Exception("Errore creazione allegato reindirizzamento");

                    DocsPaVO.documento.Allegato allegato = new DocsPaVO.documento.Allegato();
                    allegato.docNumber = newDocnumber;
                    allegato.descrizione = "Avviso di re-indirizzamento";
                    allegato.fileName = fileAllegato.nomeOriginale;
                    allegato.version = "0";
                    allegato.numeroPagine = 1;

                    DocsPaVO.documento.Allegato allResult = Documenti.AllegatiManager.aggiungiAllegato(newInfoUtente, allegato);
                    if(allResult != null)
                    {
                        Documenti.FileManager.putFile(allResult, fileAllegato, newInfoUtente);
                    }
                    else
                    {
                        throw new Exception("Errore creazione allegato reindirizzamento");
                    }



                    #endregion

                    #region CREAZIONE NUOVO FASCICOLO
                    DocsPaVO.ProfilazioneDinamica.Templates oldTemplateFasc = ProfilazioneDinamica.ProfilazioneFascicoli.getTemplateFascDettagli(idProject);
                    string idFolderTypology = GetIdTemplateFascByDescProcedimento(oldTemplateFasc.DESCRIZIONE, newInfoUtente.idAmministrazione);
                    DocsPaVO.ProfilazioneDinamica.Templates templateFasc = processor.PopolaTemplateIstanzaProcedimenti(idFolderTypology);

                    // Imposto la data di scadenza
                    string deadline = GetValoreFromProcedimento(templateFasc, "BE_PROCEDIMENTO_TERMINE");
                    foreach (DocsPaVO.ProfilazioneDinamica.OggettoCustom oggCustom in templateFasc.ELENCO_OGGETTI)
                    {
                        if (oggCustom.DESCRIZIONE.ToUpper() == DocsPaUtils.Configuration.InitConfigurationKeys.GetValue("0", "BE_PROCEDIMENTO_DATA_SCADENZA").ToUpper())
                        {
                            oggCustom.VALORE_DATABASE = DateTime.Now.AddDays(Convert.ToDouble(deadline)).ToString("dd/MM/yyyy");
                            break;
                        }
                    }

                    // Ricerca diagramma di stato
                    DocsPaVO.DiagrammaStato.DiagrammaStato stateDiagram = null;
                    DocsPaVO.DiagrammaStato.Stato initialState = null;
                    bool setDiagram = false;
                    int idDiagram = BusinessLogic.DiagrammiStato.DiagrammiStato.getDiagrammaAssociatoFasc(templateFasc.ID_TIPO_FASC);
                    if (idDiagram != 0)
                    {
                        stateDiagram = BusinessLogic.DiagrammiStato.DiagrammiStato.getDiagrammaById(idDiagram.ToString());
                        if (stateDiagram != null)
                        {
                            logger.DebugFormat("Trovato diagramma associato {0} - ID={1}", stateDiagram.DESCRIZIONE, stateDiagram.SYSTEM_ID.ToString());
                            if (stateDiagram.STATI != null && stateDiagram.STATI.Count > 0)
                            {
                                foreach (DocsPaVO.DiagrammaStato.Stato s in stateDiagram.STATI)
                                {
                                    if (s.STATO_INIZIALE)
                                    {
                                        initialState = s;
                                        break;
                                    }
                                }
                            }
                            setDiagram = true;
                        }
                    }

                    logger.Debug("Creazione fascicolo per istanza procedimento");
                    DocsPaVO.fascicolazione.Fascicolo newFascicolo = new DocsPaVO.fascicolazione.Fascicolo();

                    logger.Debug("Prelievo del titolario attivo");
                    DocsPaVO.amministrazione.OrgTitolario titolario = null;
                    ArrayList titolari = Amministrazione.TitolarioManager.getTitolariUtilizzabili(newInfoUtente.idAmministrazione);
                    if (titolari != null && titolari.Count > 0)
                    {
                        foreach (DocsPaVO.amministrazione.OrgTitolario tempTit in titolari)
                        {
                            if (tempTit.Stato == DocsPaVO.amministrazione.OrgStatiTitolarioEnum.Attivo)
                            {
                                titolario = tempTit;
                                break;
                            }
                        }
                    }

                    logger.Debug("Ricerca classifica");
                    string classCode = BusinessLogic.Procedimenti.ProcedimentiManager.GetValoreFromProcedimento(templateFasc, "BE_PROCEDIMENTO_COD_CLASS");
                    ArrayList listClassificazioni = BusinessLogic.Fascicoli.TitolarioManager.getTitolario(newInfoUtente.idAmministrazione, newInfoUtente.idGruppo, newInfoUtente.idPeople, null, classCode, false);
                    DocsPaVO.fascicolazione.Classificazione classificazione = (DocsPaVO.fascicolazione.Classificazione)listClassificazioni[0];

                    if (classificazione == null || titolario == null)
                        throw new Exception("Classificazione o titolario non trovati");

                    logger.Debug("Creazione oggetto fascicolo");
                    newFascicolo.apertura = DateTime.Now.ToString("dd/MM/yyyy");
                    newFascicolo.codiceGerarchia = classificazione.codice;
                    newFascicolo.descrizione = oldFasc.descrizione;
                    newFascicolo.idRegistro = classificazione.registro != null ? classificazione.registro.systemId : string.Empty;
                    newFascicolo.idTitolario = titolario.ID;
                    newFascicolo.privato = "0";
                    newFascicolo.codUltimo = BusinessLogic.Fascicoli.FascicoloManager.getFascNumRif(classificazione.systemID, newFascicolo.idRegistro);
                    newFascicolo.template = templateFasc;

                    logger.Debug("Salvataggio fascicolo");
                    DocsPaVO.fascicolazione.ResultCreazioneFascicolo resultFasc = DocsPaVO.fascicolazione.ResultCreazioneFascicolo.OK;
                    newFascicolo = BusinessLogic.Fascicoli.FascicoloManager.newFascicolo(classificazione, newFascicolo, newInfoUtente, newRuolo, false, out resultFasc);

                    if (resultFasc != DocsPaVO.fascicolazione.ResultCreazioneFascicolo.OK)
                    {
                        throw new Exception("Errore nella creazione del nuovo fascicolo");
                    }
                    else
                    {
                        logger.Debug("Aggiornamento associativa procedimenti");
                        DocsPaDB.Query_DocsPAWS.Procedimenti proc = new DocsPaDB.Query_DocsPAWS.Procedimenti();
                        if (!reindirizzaProcedimentoUpdate(oldProc.IdEsterno, oldFasc.systemID, newFascicolo.systemID, newDocnumber, corr.systemId, reg.systemId))
                            throw new Exception("Errore aggiornamento associativa procedimenti");

                        if(setDiagram)
                        {
                            BusinessLogic.DiagrammiStato.DiagrammiStato.salvaModificaStatoFasc(newFascicolo.systemID, initialState.SYSTEM_ID.ToString(), stateDiagram, newInfoUtente.idPeople, newInfoUtente, string.Empty);
                        }

                    }

                    logger.Debug("Inserimento documento in fascicolo");
                    string errorMsg = string.Empty;
                    if (!BusinessLogic.Fascicoli.FascicoloManager.addDocFascicolo(newInfoUtente, newDocnumber, newFascicolo.systemID, false, out errorMsg))
                    {
                        throw new Exception("Errore nell'inserimento del documento in fascicolo - " + errorMsg);
                    }

                    logger.Debug("Chiusura fascicolo");
                    newFascicolo.stato = "C";
                    newFascicolo.chiusura = DateTime.Now.ToString("dd/MM/yyyy");
                    newFascicolo.chiudeFascicolo = new DocsPaVO.fascicolazione.ChiudeFascicolo() { idPeople = newInfoUtente.idPeople, idCorrGlob_Ruolo = newInfoUtente.idCorrGlobali };
                    if (newRuolo.uo != null)
                        newFascicolo.chiudeFascicolo.idCorrGlob_UO = newRuolo.uo.systemId;
                    BusinessLogic.Fascicoli.FascicoloManager.setFascicolo(newInfoUtente, newFascicolo);
                    #endregion

                    #region TRASMISSIONE FASCICOLO
                    logger.Debug("Trasmissione fascicolo");

                    // Estrazione ruoli da notificare
                    string idCorrResponsabile = GetIdCorrDefault(templateFasc, "BE_PROCEDIMENTO_RESPONSABILE");
                    string idCorrPrimoNotificato = GetIdCorrDefault(templateFasc, "BE_PROCEDIMENTO_PRIMO_NOT");

                    DocsPaVO.trasmissione.RagioneTrasmissione reason = BusinessLogic.Trasmissioni.RagioniManager.getRagioneByCodice(newInfoUtente.idAmministrazione, "PORTALE"); // CABLATO PER ORA
                    DocsPaVO.trasmissione.Trasmissione transmissionResp = new DocsPaVO.trasmissione.Trasmissione(); // al responsabile
                    DocsPaVO.trasmissione.Trasmissione transmissionNot = new DocsPaVO.trasmissione.Trasmissione();  // al primo notificato

                    transmissionResp.tipoOggetto = DocsPaVO.trasmissione.TipoOggetto.FASCICOLO;
                    transmissionResp.infoFascicolo = new DocsPaVO.fascicolazione.InfoFascicolo(newFascicolo);
                    transmissionResp.utente = newUtente;
                    transmissionResp.ruolo = newRuolo;

                    transmissionNot.tipoOggetto = DocsPaVO.trasmissione.TipoOggetto.FASCICOLO;
                    transmissionNot.infoFascicolo = new DocsPaVO.fascicolazione.InfoFascicolo(newFascicolo);
                    transmissionNot.utente = newUtente;
                    transmissionNot.ruolo = newRuolo;

                    // Destinatari della trasmissione
                    DocsPaVO.utente.Corrispondente responsabile = BusinessLogic.Utenti.UserManager.getCorrispondente(idCorrResponsabile, true);
                    DocsPaVO.utente.Corrispondente primoNotificato = BusinessLogic.Utenti.UserManager.getCorrispondente(idCorrPrimoNotificato, true);

                    transmissionResp = BusinessLogic.Trasmissioni.TrasmManager.addTrasmissioneSingola(transmissionResp, responsabile, reason, string.Empty, "S");
                    transmissionNot = BusinessLogic.Trasmissioni.TrasmManager.addTrasmissioneSingola(transmissionNot, primoNotificato, reason, string.Empty, "S");

                    logger.Debug("Invio trasmissione al responsabile");
                    DocsPaVO.trasmissione.Trasmissione resultTrasm = BusinessLogic.Trasmissioni.ExecTrasmManager.saveExecuteTrasmMethod(string.Empty, transmissionResp);

                    logger.Debug("Invio trasmissione al primo notificato");
                    DocsPaVO.trasmissione.Trasmissione resultTrasmN = BusinessLogic.Trasmissioni.ExecTrasmManager.saveExecuteTrasmMethod(string.Empty, transmissionNot);

                    // La notifica va inviata esclusivamente al primo notificato
                    if (resultTrasm != null && resultTrasm.infoFascicolo != null && !string.IsNullOrEmpty(resultTrasm.infoFascicolo.idFascicolo))
                    {
                        foreach (DocsPaVO.trasmissione.TrasmissioneSingola single in resultTrasm.trasmissioniSingole)
                        {
                            string method = "TRASM_FOLDER_" + single.ragione.descrizione.ToUpper().Replace(" ", "_");
                            string desc = "Trasmesso Fascicolo: " + resultTrasm.infoFascicolo.codice;
                            BusinessLogic.UserLog.UserLog.WriteLog(resultTrasm.utente.userId, resultTrasm.utente.idPeople, resultTrasm.ruolo.idGruppo, newInfoUtente.idAmministrazione, method, resultTrasm.infoFascicolo.idFascicolo, desc, DocsPaVO.Logger.CodAzione.Esito.OK,
                                (newInfoUtente != null && newInfoUtente.delegato != null ? newInfoUtente.delegato : null), "0", single.systemId); // CHECK_NOTIFY=0
                        }
                    }
                    if (resultTrasmN != null && resultTrasmN.infoFascicolo != null && !string.IsNullOrEmpty(resultTrasmN.infoFascicolo.idFascicolo))
                    {
                        foreach (DocsPaVO.trasmissione.TrasmissioneSingola single in resultTrasmN.trasmissioniSingole)
                        {
                            string method = "TRASM_FOLDER_" + single.ragione.descrizione.ToUpper().Replace(" ", "_");
                            string desc = "Trasmesso Fascicolo: " + resultTrasmN.infoFascicolo.codice;
                            BusinessLogic.UserLog.UserLog.WriteLog(resultTrasmN.utente.userId, resultTrasmN.utente.idPeople, resultTrasmN.ruolo.idGruppo, newInfoUtente.idAmministrazione, method, resultTrasmN.infoFascicolo.idFascicolo, desc, DocsPaVO.Logger.CodAzione.Esito.OK,
                                (newInfoUtente != null && newInfoUtente.delegato != null ? newInfoUtente.delegato : null), "1", single.systemId); // CHECK_NOTIFY=1
                        }
                    }

                    #endregion

                    #region CHIUSURA FASCICOLO DA AOO DI PARTENZA
                    logger.Debug("Modifica stato fascicolo");
                    string statoChiusura = "Re-indirizzamento procedimento";
                    int oldIdDiagram = BusinessLogic.DiagrammiStato.DiagrammiStato.getDiagrammaAssociatoFasc(oldTemplateFasc.SYSTEM_ID.ToString());
                    if(oldIdDiagram != 0)
                    {
                        DocsPaVO.DiagrammaStato.DiagrammaStato oldDiagram = BusinessLogic.DiagrammiStato.DiagrammiStato.getDiagrammaById(oldIdDiagram.ToString());
                        if(oldDiagram != null)
                        {
                            foreach(DocsPaVO.DiagrammaStato.Stato itemStato in oldDiagram.STATI)
                            {
                                if(itemStato.DESCRIZIONE.ToUpper() == statoChiusura.ToUpper())
                                {
                                    BusinessLogic.DiagrammiStato.DiagrammiStato.salvaModificaStatoFasc(oldFasc.systemID, itemStato.SYSTEM_ID.ToString(), oldDiagram, infoUtente.idPeople, infoUtente, string.Empty);
                                    break;
                                }
                            }
                        }
                    }


                    logger.Debug("Chiusura fascicolo da AOO di origine");
                    oldFasc.stato = "C";
                    oldFasc.chiusura = DateTime.Now.ToString("dd/MM/yyyy");
                    oldFasc.chiudeFascicolo = new DocsPaVO.fascicolazione.ChiudeFascicolo() { idPeople = infoUtente.idPeople, idCorrGlob_Ruolo = infoUtente.idCorrGlobali };
                    //if (newRuolo.uo != null)
                    //    oldFasc.chiudeFascicolo.idCorrGlob_UO = oldFasc.uo.systemId;
                    BusinessLogic.Fascicoli.FascicoloManager.setFascicolo(infoUtente, oldFasc);

                    logger.Debug("Estrazione documenti");
                    ArrayList folders = BusinessLogic.Fascicoli.FascicoloManager.getListaFolderDaIdFascicolo(infoUtente, oldFasc.systemID, null, false, false);
                    foreach(DocsPaVO.fascicolazione.Folder folder in folders)
                    {
                        ArrayList listaDoc = Fascicoli.FolderManager.getDocumenti(infoUtente.idGruppo, infoUtente.idPeople, folder);
                        if(listaDoc != null && listaDoc.Count > 0)
                        {
                            foreach(DocsPaVO.documento.InfoDocumento infoDoc in listaDoc)
                            {
                                logger.Debug("Consolidamento doc id=" + infoDoc.idProfile);
                                DocsPaVO.documento.DocumentConsolidationStateInfo state = new DocsPaVO.documento.DocumentConsolidationStateInfo();
                                state = Documenti.DocumentConsolidation.ConsolidateNoSecurity(infoUtente, infoDoc.idProfile, DocsPaVO.documento.DocumentConsolidationStateEnum.Step2, true);
                            }
                        }
                    }

                    #endregion

                    #region NOTIFICA UTENTE PORTALE
                    logger.Debug("Invio mail notifica");
                    
                    if (!reindirizzaProcedimentoMailNotifica(oldDoc.registro.systemId, corr.email, oldProc.IdEsterno, oldDoc.registro.descrizione, reg.descrizione, oldDoc.oggetto.descrizione))
                        throw new Exception("Errore nell'invio della mail di notifica");
                    #endregion

                    result = true;
                    trContxt.Complete();
                }
                catch(Exception ex)
                {
                    result = false;
                    logger.Debug("Errore nel reindirizzamento di un procedimento - ", ex);
                    
                }
                trContxt.Dispose();
            }

            response.Success = result;
            response.ErrorMessage = msg;
            return response;
        }

        public static bool CheckProcedimentoReindirizzato(string idProject)
        {
            string output = string.Empty;
            string output2 = string.Empty;
            return CheckProcedimentoReindirizzato(idProject, out output, out output2);
        }

        public static bool CheckProcedimentoReindirizzato(string idProject, out string idRedirected, out string idRegRedirected)
        {
            DocsPaDB.Query_DocsPAWS.Procedimenti proc = new DocsPaDB.Query_DocsPAWS.Procedimenti();
            return proc.CheckProcedimentoReindirizzato(idProject, out idRedirected, out idRegRedirected);
        }

        private static DocsPaVO.utente.InfoUtente getInfoUtenteSistema(string codiceAOO, string idAmm)
        {
            DocsPaVO.utente.InfoUtente infoUtente = null;

            string username = codiceAOO.ToUpper() + ".PORTALE";
            DocsPaVO.utente.Utente utente = Utenti.UserManager.getUtente(username, idAmm);

            DocsPaVO.utente.Ruolo[] ruoli = (DocsPaVO.utente.Ruolo[])BusinessLogic.Utenti.UserManager.getRuoliUtente(utente.idPeople).ToArray(typeof(DocsPaVO.utente.Ruolo));
            if (ruoli != null && ruoli.Length > 0)
            {
                infoUtente = Utenti.UserManager.GetInfoUtente(utente, ruoli[0]);
            }
            else
            {
                infoUtente = null;
            }

            return infoUtente;
        }

        private static string reindirizzaProcedimentoCreaIstanzaPrincipale(DocsPaVO.utente.InfoUtente user, DocsPaVO.utente.Registro reg, string subject, DocsPaVO.ProfilazioneDinamica.Templates template, DocsPaVO.utente.Corrispondente corr, DocsPaVO.utente.Ruolo role, string addressBookCode, byte[] content, string note)
        {
            logger.Debug("START");

            string docnumber = string.Empty;

            DocsPaVO.documento.SchedaDocumento doc = new DocsPaVO.documento.SchedaDocumento();
            DocsPaVO.documento.SchedaDocumento docResult = new DocsPaVO.documento.SchedaDocumento();
            doc = BusinessLogic.Documenti.DocManager.NewSchedaDocumento(user, false);

            //logger.Debug("Registro");
            //DocsPaVO.utente.Registro reg = BusinessLogic.Utenti.RegistriManager.getRegistroByCode(regCode);

            logger.Debug("Popolamento SchedaDocumento");
            doc.oggetto.descrizione = subject;
            doc.oggetto.daAggiornare = true;
            doc.tipoProto = "A";
            doc.protocollo = new DocsPaVO.documento.ProtocolloEntrata();
            string arrivalDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            ((DocsPaVO.documento.ProtocolloEntrata)doc.protocollo).mittente = corr;
            ((DocsPaVO.documento.Documento)doc.documenti[0]).dataArrivo = arrivalDate;
            if (template != null)
            {
                doc.template = template;
                doc.tipologiaAtto = new DocsPaVO.documento.TipologiaAtto() { systemId = template.SYSTEM_ID.ToString(), descrizione = template.DESCRIZIONE };
            }

            // -----------------------------------------------------
            logger.Debug("Salvataggio scheda");
            docResult = BusinessLogic.Documenti.DocSave.addDocGrigia(doc, user, role);

            if (docResult != null)
            {
                bool dummy;
                // fix per demo 29-08 (si spera di risolvere prima o poi)
                ((DocsPaVO.documento.Documento)docResult.documenti[0]).dataArrivo = arrivalDate;
                logger.Debug("Arrival date: " + arrivalDate);

                if (reg != null)
                    docResult.registro = reg;
                docResult.tipoProto = "A";
                docResult.mezzoSpedizione = BusinessLogic.Documenti.InfoDocManager.getIdMezzoSpedizioneByDesc("PORTALE");
                docResult.descMezzoSpedizione = "PORTALE";
                docResult.predisponiProtocollazione = true;
                docResult = BusinessLogic.Documenti.DocSave.save(user, docResult, false, out dummy, role);
            }
            else
            {
                throw new Exception();
            }

            logger.Debug("Creazione nota");
            if(!string.IsNullOrEmpty(note))
            {
                DocsPaVO.Note.AssociazioneNota oggettoAssociato = new DocsPaVO.Note.AssociazioneNota();
                oggettoAssociato.TipoOggetto = DocsPaVO.Note.AssociazioneNota.OggettiAssociazioniNotaEnum.Documento;
                oggettoAssociato.Id = docResult.docNumber;

                DocsPaVO.Note.InfoNota new_note = new DocsPaVO.Note.InfoNota();
                new_note.TipoVisibilita = DocsPaVO.Note.TipiVisibilitaNotaEnum.Tutti;
                new_note.Testo = note;
                new_note.DataCreazione = System.DateTime.Now;
                new_note.UtenteCreatore = new DocsPaVO.Note.InfoUtenteCreatoreNota();
                new_note.UtenteCreatore.IdUtente = user.idPeople;
                new_note.UtenteCreatore.DescrizioneUtente = user.userId;
                new_note.UtenteCreatore.IdRuolo = user.idGruppo;

                DocsPaVO.Note.InfoNota infoNota = BusinessLogic.Note.NoteManager.InsertNota(user, oggettoAssociato, new_note);
                docResult.noteDocumento.Insert(0, infoNota);
            }

            // -----------------------------------------------------
            logger.Debug("Associazione mezzo spedizione PORTALE");
            BusinessLogic.Documenti.ProtoManager.collegaMezzoSpedizioneDocumento(user, docResult.mezzoSpedizione, docResult.docNumber);

            // -----------------------------------------------------
            logger.Debug("Upload del file");
            DocsPaVO.documento.FileRequest fileReq = (DocsPaVO.documento.FileRequest)docResult.documenti[0];
            DocsPaVO.documento.FileDocumento fileDoc = new DocsPaVO.documento.FileDocumento();
            //fileDoc.name = request.Proceeding.DocumentObject + ".pdf";
            //fileDoc.fullName = request.Proceeding.DocumentObject + ".pdf";
            fileDoc.name = addressBookCode + ".pdf";
            fileDoc.fullName = addressBookCode + ".pdf";
            fileDoc.content = content;
            fileDoc.contentType = "application/pdf";
            fileDoc.length = content.Length;
            fileDoc.bypassFileContentValidation = true;

            string errorMsg = string.Empty;
            if (!BusinessLogic.Documenti.FileManager.putFile(ref fileReq, fileDoc, user, out errorMsg))
            {
                logger.DebugFormat("Errore nel salvataggio del documento! {0}", errorMsg);
                throw new Exception("FILE_CREATION_ERROR");
            }

            docnumber = docResult.docNumber;
            logger.DebugFormat("END - ID={0}", docnumber);

            return docnumber;
        }

        private static bool reindirizzaProcedimentoCreaAllegati(DocsPaVO.utente.InfoUtente user, DocsPaVO.documento.Allegato all, string docnumber)
        {
            bool result = false;

            try
            {

                DocsPaVO.documento.Allegato newAll = new DocsPaVO.documento.Allegato();
                newAll.docNumber = docnumber;
                newAll.descrizione = all.descrizione;
                newAll.fileName = all.fileName;
                newAll.version = "0";
                newAll.numeroPagine = 1;

                DocsPaVO.documento.FileDocumento oldFileDoc = BusinessLogic.Documenti.FileManager.getFileFirmato(all, user, false);
                DocsPaVO.documento.FileDocumento newFileDoc = new DocsPaVO.documento.FileDocumento();
                newFileDoc.name = all.fileName;
                newFileDoc.fullName = all.fileName;
                newFileDoc.nomeOriginale = all.fileName;
                newFileDoc.content = oldFileDoc.content;
                newFileDoc.contentType = oldFileDoc.contentType;
                newFileDoc.estensioneFile = oldFileDoc.estensioneFile;

                DocsPaVO.documento.Allegato attResult = BusinessLogic.Documenti.AllegatiManager.aggiungiAllegato(user, newAll);
                if (attResult != null)
                {
                    BusinessLogic.Documenti.FileManager.putFile(attResult, newFileDoc, user);
                    result = true;
                }
                
            }
            catch(Exception ex)
            {
                logger.Debug("Errore in reindirizzaProcedimentoCreaAllegati - ", ex);
                result = false;
            }

            return result;
        }

        private static bool reindirizzaProcedimentoUpdate(string idProcedimento, string oldIdProject, string newIdProject, string idProfile, string idCorrGlobali, string newIdReg)
        {
            bool result = false;
            DocsPaDB.Query_DocsPAWS.Procedimenti proc = new DocsPaDB.Query_DocsPAWS.Procedimenti();

            if(proc.UpdateDocReindirizzamento(oldIdProject, newIdProject, newIdReg))
            {
                result = proc.InsertDoc(newIdProject, idProfile, idCorrGlobali, true, idProcedimento, true);
            }
            return result;
        }

        private static bool reindirizzaProcedimentoMailNotifica(string idRegistro, string mailDestinatario, string idProcedimento, string strutturaOrigine, string strutturaDestinazione, string oggetto)
        {
            bool result = false;

            // Estrazione casella principale registro
            DocsPaVO.amministrazione.CasellaRegistro casella = Amministrazione.RegistroManager.GetMailRegistro(idRegistro)[0];

            string porta = casella.PortaSMTP != 0 ? casella.PortaSMTP.ToString() : string.Empty;
            string smtp_user = !string.IsNullOrEmpty(casella.UserSMTP) ? casella.UserSMTP : null;
            string smtp_pwd = !string.IsNullOrEmpty(casella.PwdSMTP) ? casella.PwdSMTP : null;

            Interoperabilità.SvrPosta svr = new Interoperabilità.SvrPosta(casella.ServerSMTP, smtp_user, smtp_pwd, porta, System.IO.Path.GetTempPath(), Interoperabilità.CMClientType.SMTP, casella.SmtpSSL, casella.SmtpSta);

            try
            {
                svr.connect();

                string subject = "Comunicazione di re-indirizzamento istanza " + idProcedimento;
                string templateBody = "Con la presente la {0} comunica che per erronea attribuzione di competenza avvenuta in data {1} l'istanza {2} è stata re-indirizzata a {3}. Accedendo alla propria area personale nel Portale dei procedimenti sarà possibile monitorare lo stato di avanzamento della pratica.";
                string bodyMail = string.Format(templateBody, strutturaOrigine, DateTime.Now.ToString("dd/MM/yyyy"), oggetto, strutturaDestinazione);
                
                string outErr;
                svr.sendMail(casella.EmailRegistro, mailDestinatario, string.Empty, string.Empty, subject, bodyMail, Interoperabilità.CMMailFormat.HTML, null, out outErr);
                if(!string.IsNullOrEmpty(outErr))
                {
                    throw new Exception(outErr);
                }

                result = true;
            }
            catch(Exception ex)
            {
                logger.Debug("Errore invio mail notifica - ", ex);
                result = false;
            }

            return result;
        }

        #endregion
    }
}
