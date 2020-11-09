using DocsPaVO.documento;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Documenti
{
    public class SegnaturaManager
    {
        private static ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string REASON = "SegnaturaPermanente";

        /// <summary>
        /// Calcolo compelto della segnatura
        /// </summary>
        /// <param name="schedaDocumento"></param>
        /// <param name="infoUtente"></param>
        /// <returns></returns>
        public static DocsPaVO.documento.DettaglioSegnatura CalcolaDettaglioSegnatura(DocsPaVO.documento.SchedaDocumento schedaDocumento, DocsPaVO.utente.InfoUtente infoUtente)
        {
            _logger.Info("START");
            DocsPaVO.documento.DettaglioSegnatura _dettaglio = null;
            DocsPaVO.documento.DettaglioSegnaturaRepertorio _segnaturaRepertorio = null;
            DocsPaVO.utente.Ruolo _ruolo = null;
            DocsPaVO.amministrazione.InfoAmministrazione _infoAmministrazione;
            DocsPaVO.documento.SchedaDocumento _documentoPrincipale = null;
            DocsPaDB.Query_DocsPAWS.Segnatura _segnaturaDB = null;
            DocsPaDB.Query_DocsPAWS.Documentale _documentale;
            DocsPaDB.Query_DocsPAWS.Utenti _utenti;
            bool _isPermanente = false;
            try
            {
                _segnaturaDB = new DocsPaDB.Query_DocsPAWS.Segnatura();
                _ruolo = BusinessLogic.Utenti.UserManager.getRuoloByIdGruppo(infoUtente.idGruppo);
                _infoAmministrazione = BusinessLogic.Amministrazione.AmministraManager.AmmGetInfoAmmCorrente(infoUtente.idAmministrazione);
                if (!"1".Equals(_infoAmministrazione.Segnatura_IsPermanente)) { throw new DocsPaVO.documento.Exceptions.SegnaturaPermanenteDisabledException("Segnatura Permanente non impostata in Amministrazione"); }

                if (schedaDocumento.documentoPrincipale != null)
                {
                    _logger.Debug("Il Documento è un allegato, recupero il documento principale");
                    _documentoPrincipale = BusinessLogic.Documenti.DocManager.getDettaglio(infoUtente, schedaDocumento.documentoPrincipale.docNumber, schedaDocumento.documentoPrincipale.docNumber);
                }



                // Recupero il primo fascicolo se esiste:
                _logger.Info("Recupero il codice fascicolo");
                try
                {
                    // quando si effettua il "riproponi" potrebbe lanciare eccezione
                    if (schedaDocumento.documentoPrincipale == null)
                        schedaDocumento.codiceFascicolo = BusinessLogic.Fascicoli.FascicoloManager.GetClassificaPerSegnatura(schedaDocumento.docNumber, infoUtente, _infoAmministrazione);
                    else
                    {
                        _documentoPrincipale.codiceFascicolo = BusinessLogic.Fascicoli.FascicoloManager.GetClassificaPerSegnatura(_documentoPrincipale.docNumber, infoUtente, _infoAmministrazione);
                    }
                    _logger.Debug("Codice Fascicolo: " + schedaDocumento.codiceFascicolo ?? _documentoPrincipale?.codiceFascicolo);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message, ex);
                }


                SchedaDocumento _tempDoc = _documentoPrincipale ?? schedaDocumento;
                _dettaglio = new DettaglioSegnatura();
                _dettaglio.ProfileID = schedaDocumento.systemId;
                _dettaglio.Segnato = "0";


                if (_tempDoc.template != null)
                {
                    string _idVersione;
                    string _versione = String.Empty;
                    string _codiceRegistro = String.Empty;
                    DocsPaVO.utente.Registro _registro = null;
                    _dettaglio.DettaglioSegnaturaRepertorio = _segnaturaDB.GetSegnaturaRepertorio(_tempDoc.docNumber);
                    _dettaglio.IsPermanenteRepertorio = (_dettaglio.DettaglioSegnaturaRepertorio != null) ? _dettaglio.DettaglioSegnaturaRepertorio.IsPermanente : "0";
                    if ("1".Equals(_dettaglio.IsPermanenteRepertorio))
                    {
                        /* CALCOLO VERSIONE*/
                        _documentale = new DocsPaDB.Query_DocsPAWS.Documentale();
                        _idVersione = _documentale.GetLatestVersionId(schedaDocumento.docNumber);
                        if (!string.IsNullOrWhiteSpace(_idVersione))
                        {
                            string versioneTemp = _documentale.GetVersionFromVersionId(_idVersione);
                            if (int.TryParse(versioneTemp, out int v))
                            {
                                _versione = v > 0 ? v.ToString("D2") : String.Empty;
                            }
                        }

                        /* RECUPERO REGISTRO*/
                        if (_dettaglio.DettaglioSegnaturaRepertorio.IdAooRf.HasValue)
                        {
                            _utenti = new DocsPaDB.Query_DocsPAWS.Utenti();
                            _utenti.GetRegistro(_dettaglio.DettaglioSegnaturaRepertorio.IdAooRf.ToString(), ref _registro);
                            _codiceRegistro = _registro?.codRegistro;
                        }

                        _dettaglio.SegnaturaRepertorio = _calcolaSegnaturaRepertorio(_dettaglio.DettaglioSegnaturaRepertorio, _infoAmministrazione.Codice, _versione, _codiceRegistro);
                    }
                    
                }


                switch (_tempDoc.tipoProto.ToUpper())
                {
                    case "A":
                    case "P":
                    case "I":
                        // *** Documento protocollato ***
                        _logger.Debug("Documento di tipo PROTOCOLLATO");
                        if (String.IsNullOrWhiteSpace(_tempDoc.protocollo.numero))
                        {
                            _dettaglio = null;
                        }
                        else
                        {
                            _dettaglio.SegnaturaProtocollo = _calcolaSegnatura(schedaDocumento, _ruolo, _documentoPrincipale, false);
                            _dettaglio.IsPermanenteProtocollo = _infoAmministrazione.Segnatura_IsPermanente;
                        }
                        
                        break;
                    case "G":
                        _logger.Debug("Documento di tipo NON PROTOCOLLATO");
                        // *** documento NON protocollato ***
                        if (_tempDoc.template == null)
                        {
                            _dettaglio = null;
                        }
                        else if(!"1".Equals(_dettaglio.IsPermanenteRepertorio))
                        {
                            _dettaglio = null;
                        }
                        break;
                    default:
                        _dettaglio = null;
                        break;
                }


            }
            catch(DocsPaVO.documento.Exceptions.SegnaturaPermanenteDisabledException ex)
            {
                _logger.Warn(ex.Message, ex);
                _dettaglio = null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw ex;
            }
            finally
            {
                _segnaturaDB?.Dispose();
            }

            _logger.Info("END");

            return _dettaglio;

        }

        /// <summary>
        /// Recupera dal Databse il dettaglio di segnatura
        /// </summary>
        /// <param name="systemIdProfile"></param>
        /// <returns></returns>
        public static DocsPaVO.documento.DettaglioSegnatura GetDettaglioSegnaturaDocumento(string systemIdProfile)
        {
            _logger.Info("START");
            DocsPaVO.documento.DettaglioSegnatura _dettaglio = null;
            DocsPaDB.Query_DocsPAWS.Segnatura _segnaturaDB = null;
            try
            {
                _segnaturaDB = new DocsPaDB.Query_DocsPAWS.Segnatura();
                _dettaglio = _segnaturaDB.DettaglioSegnatura_Select(systemIdProfile);
                if (_dettaglio != null)
                {
                    _dettaglio.DettaglioSegnaturaRepertorio = _segnaturaDB.GetSegnaturaRepertorio(systemIdProfile);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw ex;
            }
            finally
            {
                _segnaturaDB?.Dispose();
            }
            _logger.Info("END");
            return _dettaglio;
        }


        /// <summary>
        /// Appone la segnatura permanete ai file del documento (principale ed allegati) ed aggiora il DB
        /// </summary>
        /// <param name="schedaDocumento"></param>
        /// <param name="dettaglioSegnatura"></param>
        /// <param name="infoUtente"></param>
        /// <returns></returns>
        public static bool ApponiSegnaturaPermanenteCompleta(DocsPaVO.documento.SchedaDocumento schedaDocumento, DettaglioSegnatura dettaglioSegnatura, DocsPaVO.utente.InfoUtente infoUtente)
        {
            _logger.Info("START");
            bool _result = true;
            bool _filePresente = true;
            bool _allegatoSegnato;
            string _extention = String.Empty;
            string _idLastVersion;
            DocsPaDB.Query_DocsPAWS.Segnatura _segnaturaDB = null;
            try
            {
                if (dettaglioSegnatura == null) { throw new ArgumentNullException("DettaglioSegnatura"); }
                _segnaturaDB = new DocsPaDB.Query_DocsPAWS.Segnatura();

                DocsPaVO.documento.FileRequest _lastDocumento = (DocsPaVO.documento.Documento)schedaDocumento.documenti[0];

                dettaglioSegnatura.DettaglioSegnaturaPosition = _segnaturaDB.DettaglioSegnaturaPosition_Select(schedaDocumento.docNumber);
                dettaglioSegnatura.Segnato = "1";
                _extention = System.IO.Path.GetExtension(_lastDocumento?.fileName ?? string.Empty);
                if (_extention?.IndexOf("pdf", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _result = _apponiSegnaturaToFile(_lastDocumento, infoUtente, dettaglioSegnatura, out _filePresente);
                    if (_result)
                    {
                        dettaglioSegnatura.VersionId = BusinessLogic.Documenti.VersioniManager.getLatestVersionID(schedaDocumento.docNumber, infoUtente);
                        _segnaturaDB.DettaglioSegnatura_Insert(dettaglioSegnatura);
                    }
                }
                else
                {
                    _segnaturaDB.DettaglioSegnatura_Insert(dettaglioSegnatura);
                }

                if (!string.IsNullOrEmpty(DocsPaUtils.Configuration.InitConfigurationKeys.GetValue("0", "BE_SEG_PERM_ALLEGATI")) && DocsPaUtils.Configuration.InitConfigurationKeys.GetValue("0", "BE_SEG_PERM_ALLEGATI") == "1")
                {
                    foreach (DocsPaVO.documento.FileRequest _allegato in schedaDocumento.allegati)
                    {
                        dettaglioSegnatura.ProfileID = _allegato.docNumber;
                        _extention = System.IO.Path.GetExtension(_allegato.fileName ?? string.Empty);
                        if (_extention?.IndexOf("pdf", StringComparison.InvariantCultureIgnoreCase) < 0)
                        {
                            _segnaturaDB.DettaglioSegnatura_Insert(dettaglioSegnatura);
                            continue;
                        }
                        _allegatoSegnato = _apponiSegnaturaToFile(_allegato, infoUtente, dettaglioSegnatura, out _filePresente);
                        // il file è stato segnato?
                        if (_allegatoSegnato && _filePresente)
                        {
                            dettaglioSegnatura.VersionId = BusinessLogic.Documenti.VersioniManager.getLatestVersionID(_allegato.docNumber, infoUtente);
                            _segnaturaDB.DettaglioSegnatura_Insert(dettaglioSegnatura);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                _result = false;
            }
            finally
            {
                _segnaturaDB?.Dispose();
            }
            _logger.Info("END");
            return _result;
        }


        public static bool? CheckIfPdfDocumentIsSigned(DocsPaVO.documento.FileDocumento fileDoc)
        {
            _logger.Info("START");
            bool? result = null;
            //return null; // DISABILITA

            System.IO.Stream pdfStreamFile = null;
            try
            {
                pdfStreamFile = new System.IO.MemoryStream(fileDoc.content);
                Aspose.Pdf.Document _pdf = new Aspose.Pdf.Document(pdfStreamFile);
                result = false;
                foreach (Aspose.Pdf.Forms.Field field in _pdf.Form)
                {
                    Aspose.Pdf.Forms.SignatureField sf = field as Aspose.Pdf.Forms.SignatureField;
                    if(sf != null)
                    {
                        if (sf.Signature.Reason != SegnaturaManager.REASON) { result = true; }
                    }
                }
                
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message, ex);
                result = null;
            }
            finally
            {
                pdfStreamFile?.Dispose();
            }
            _logger.Info("END");
            return result;
        }


        


        // POSITION

        public static DocsPaVO.documento.DettaglioSegnaturaPosition DettaglioSegnaturaPosition_Select(string systemIdProfile)
        {
            _logger.Info("START");
            DocsPaVO.documento.DettaglioSegnaturaPosition _dettaglio = null;
            DocsPaDB.Query_DocsPAWS.Segnatura _segnaturaDB = null;
            try
            {

                _segnaturaDB = new DocsPaDB.Query_DocsPAWS.Segnatura();
                _dettaglio = _segnaturaDB.DettaglioSegnaturaPosition_Select(systemIdProfile);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw ex;
            }
            finally
            {
                _segnaturaDB?.Dispose();
            }
            _logger.Info("END");
            return _dettaglio;
        }

        public static bool DettaglioSegnaturaPosition_Insert(DocsPaVO.documento.DettaglioSegnaturaPosition segnaturaPostion)
        {
            _logger.Info("START");
            bool result = true;
            DocsPaDB.Query_DocsPAWS.Segnatura _segnaturaDB = null;
            try
            {
                _segnaturaDB = new DocsPaDB.Query_DocsPAWS.Segnatura();
                _segnaturaDB.DettaglioSegnaturaPosition_Insert(segnaturaPostion);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                result = false;
            }
            finally
            {
                _segnaturaDB?.Dispose();
            }

            _logger.Info("END");
            return result;
        }

        public static bool DettaglioSegnatura_Update_SetPosition(DocsPaVO.documento.DettaglioSegnaturaPosition dettaglioSegnatura)
        {
            _logger.Info("START");
            bool _result = true;
            DocsPaDB.Query_DocsPAWS.Segnatura _segnaturaDB = null;
            try
            {
                _segnaturaDB = new DocsPaDB.Query_DocsPAWS.Segnatura();
                _segnaturaDB.DettaglioSegnaturaPosition_Update_SePosition(dettaglioSegnatura);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                _result = false;
            }
            finally
            {
                _segnaturaDB?.Dispose();
            }

            _logger.Info("END");
            return _result;
        }



        #region DEPRECATI

        /// <summary>
        /// Inserisce nel DB il dettaglio della segnatura calcolato
        /// </summary>
        /// <param name="dettaglioSegnatura"></param>
        /// <returns></returns>
        //public static bool DettaglioSegnatura_Insert(DocsPaVO.documento.DettaglioSegnatura dettaglioSegnatura)
        //{
        //    _logger.Info("START");
        //    bool _result = true;
        //    DocsPaDB.Query_DocsPAWS.Segnatura _segnaturaDB = null;
        //    try
        //    {
        //        _segnaturaDB = new DocsPaDB.Query_DocsPAWS.Segnatura();
        //        _segnaturaDB.DettaglioSegnatura_Insert(dettaglioSegnatura);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex.Message, ex);
        //        _result = false;
        //    }

        //    _logger.Info("END");
        //    return _result;
        //}


        //public static bool ApponiSegnaturaPermanenteToPredisposto(DettaglioSegnatura dettaglioSegnatura, DocsPaVO.utente.InfoUtente infoUtente)
        //{
        //    _logger.Info("START");
        //    bool _result = true;

        //    DocsPaVO.documento.SchedaDocumento _schedaDoc = null;
        //    DocsPaDB.Query_DocsPAWS.Documenti _queryDoc = null;
        //    DocsPaDB.Query_DocsPAWS.Segnatura _segnaturaDB = null;
        //    try
        //    {
        //        _segnaturaDB = new DocsPaDB.Query_DocsPAWS.Segnatura();
        //        _schedaDoc = BusinessLogic.Documenti.DocManager.getDettaglio(infoUtente, dettaglioSegnatura.ProfileID, dettaglioSegnatura.ProfileID);
        //        _queryDoc = new DocsPaDB.Query_DocsPAWS.Documenti();
        //        string _idLastVersion = BusinessLogic.Documenti.VersioniManager.getLatestVersionID(_schedaDoc.docNumber, infoUtente);


        //        DocsPaVO.documento.FileRequest _lastDocumento = (DocsPaVO.documento.Documento)_schedaDoc.documenti[0];

        //        string[] _testiSegnatura = _getTestoFromDettaglioSegnatura(dettaglioSegnatura);
        //        bool _filePresente = true;
        //        _result = _apponiSegnaturaToFile(_lastDocumento, infoUtente, _testiSegnatura, out _filePresente);

        //        if (_result)
        //        {
        //            // aggiorna DB
        //            dettaglioSegnatura.Segnato = "1";
        //            _segnaturaDB.DettaglioSegnatura_Update_SetSegnato(dettaglioSegnatura.ProfileID);
        //        }

        //        bool _allegatoSegnato = false;
        //        foreach (DocsPaVO.documento.FileRequest _allegato in _schedaDoc.allegati)
        //        {
        //            _allegatoSegnato = _apponiSegnaturaToFile(_allegato, infoUtente, _testiSegnatura, out _filePresente);
        //            dettaglioSegnatura.ProfileID = _allegato.docNumber;
        //            // il file è stato segnato?
        //            if (_allegatoSegnato && _filePresente)
        //            {
        //                _segnaturaDB.DettaglioSegnatura_Insert(dettaglioSegnatura);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex.Message, ex);
        //        throw ex;
        //    }

        //    _logger.Info("END");
        //    return _result;
        //}


        //public static bool ApponiSegnaturaPermanente(string docNumber, DocsPaVO.utente.InfoUtente infoUtente)
        //{
        //    // 1 - recuperare l'ultima versione del documento
        //    // 2 - estrapolare il file
        //    // 3 - apporre la segnatura
        //    // 4 - greare una  nuova versione del documento
        //    // 5 - creare il nuovo pdf con la firma
        //    _logger.Info("START");

        //    DocsPaDB.Query_DocsPAWS.Documenti _queryDoc = null;
        //    DocsPaDB.Query_DocsPAWS.Segnatura _segnaturaDB = null;
        //    DocsPaVO.utente.Ruolo _ruolo = null;

        //    DocsPaVO.documento.SchedaDocumento _schedaDoc = null;
        //    DocsPaVO.documento.DettaglioSegnatura _dettaglio = null;
        //    DocsPaVO.documento.SchedaDocumento _documentoPrincipale = null;
        //    DocsPaVO.amministrazione.InfoAmministrazione _amministrazioneCorrente = null;

        //    try
        //    {
        //        _segnaturaDB = new DocsPaDB.Query_DocsPAWS.Segnatura();
        //        _queryDoc = new DocsPaDB.Query_DocsPAWS.Documenti();
        //        _ruolo = BusinessLogic.Utenti.UserManager.getRuoloByIdGruppo(infoUtente.idGruppo);

        //        #region SchedaDocumento
        //        _logger.Info("Recupero i dettagli del documento");
        //        _schedaDoc = BusinessLogic.Documenti.DocManager.getDettaglio(infoUtente, docNumber, docNumber);
        //        //string _detailSchedaDoc = DocsPaUtils.Functions.Functions.DisplayObjectInfo(_schedaDoc);
        //        //_logger.Debug(_detailSchedaDoc);
        //        #endregion

        //        //#region Dettagli Segnatura
        //        //_logger.Info("Recupero i dettagli della segnatura di repertorio");
        //        //_dettaglio = GetDettaglioSegnaturaDocumento(_schedaDoc.systemId);
        //        ////var _detailDettSegn = DocsPaUtils.Functions.Functions.DisplayObjectInfo(_dettaglio);
        //        ////_logger.Debug(_detailDettSegn);

        //        //// Nel caso il documento non abbia gia un dettaglio segnatura 
        //        //if (_dettaglio == null)
        //        //{
        //        //    if (_schedaDoc.documentoPrincipale != null)
        //        //    {
        //        //        _logger.Debug("Il Documento è un allegato, recupero il documento principale");
        //        //        _documentoPrincipale = BusinessLogic.Documenti.DocManager.getDettaglio(infoUtente, _schedaDoc.documentoPrincipale.docNumber, _schedaDoc.documentoPrincipale.docNumber);
        //        //        //string _detailDocPrincipale = DocsPaUtils.Functions.Functions.DisplayObjectInfo(_documentoPrincipale);
        //        //        //_logger.Debug(_detailDocPrincipale);
        //        //    }

        //        //    _logger.Info("Recupero i dettagli dell'amministrazione");
        //        //    _amministrazioneCorrente = Amministrazione.AmministraManager.AmmGetInfoAmmCorrente(infoUtente.idAmministrazione);
        //        //    //string _detailAmm = DocsPaUtils.Functions.Functions.DisplayObjectInfo(_amministrazioneCorrente);
        //        //    //_logger.Debug(_detailAmm);

        //        //    // Recupero il primo fascicolo se esiste:
        //        //    _logger.Info("Recupero il codice fascicolo");
        //        //    if (_schedaDoc.documentoPrincipale == null)
        //        //        _schedaDoc.codiceFascicolo = Fascicoli.FascicoloManager.GetClassificaPerSegnatura(_schedaDoc.docNumber, infoUtente, _amministrazioneCorrente);
        //        //    else
        //        //    {
        //        //        _documentoPrincipale.codiceFascicolo = Fascicoli.FascicoloManager.GetClassificaPerSegnatura(_documentoPrincipale.docNumber, infoUtente, _amministrazioneCorrente);
        //        //    }
        //        //    _logger.Debug("Codice Fascicolo: " + _schedaDoc.codiceFascicolo ?? _documentoPrincipale?.codiceFascicolo);

        //        //    // recupero la segnatura di repertorio per verificare o meno se il documento è stato repertoriato
        //        //    _logger.Info("recupero la segnatura di repertorio");
        //        //    _segnaturaRepertorio = BusinessLogic.Documenti.DocManager.GetSegnaturaRepertorio(docNumber, _amministrazioneCorrente.Codice, false, out string dataAnnullamento);
        //        //    _logger.Debug("Dattagli segnatura: " + _segnaturaRepertorio);

        //        //}
        //        //#endregion

        //        //#region Testo della Segnatura

        //        //switch (_schedaDoc.tipoProto.ToUpper())
        //        //{
        //        //    case "A":
        //        //    case "P":
        //        //    case "I":
        //        //        // *** Documento protocollato ***
        //        //        _logger.Debug("Documento di tipo PROTOCOLLATO");
        //        //        if (_dettaglio == null)
        //        //        {
        //        //            // crea dettaglio per protocollati
        //        //            _dettaglio = CalcolaDettaglioSegnaturaProtocolli(_schedaDoc, _ruolo);
        //        //            //_segnaturaDB.InsertDettaglioSegnatura(_dettaglio);
        //        //        }

        //        //        if (!string.IsNullOrWhiteSpace(_segnaturaRepertorio))
        //        //        {
        //        //            ///////////////////////////////////////////////
        //        //        }
        //        //        else
        //        //        {
        //        //            if (!_dettaglio.IsPermanente.Equals("1") || _dettaglio.Segnato.Equals("1"))
        //        //            {
        //        //                _logger.DebugFormat("IsPermanente: {0} - Segnato: {1}", _dettaglio.IsPermanente, _dettaglio.Segnato);
        //        //                return false;
        //        //            }
        //        //            _segnatureText = _dettaglio.SegnaturaProtocollo;
        //        //        }
        //        //        break;
        //        //    case "G":
        //        //        _logger.Debug("Documento di tipo NON PROTOCOLLATO");
        //        //        // *** documento NON protocollato ***
        //        //        if (_dettaglio == null)
        //        //        {
        //        //            // crea dettaglio per NonProtocollati
        //        //            _dettaglio = _calcolaDettaglioSegnaturaNP(_schedaDoc, _ruolo, _documentoPrincipale);
        //        //            // _segnaturaDB.InsertDettaglioSegnatura(_dettaglio);
        //        //        }

        //        //        if (!string.IsNullOrWhiteSpace(_segnaturaRepertorio))
        //        //        {
        //        //            /////////////////////////////////////
        //        //        }
        //        //        else
        //        //        {
        //        //            if (!_dettaglio.IsPermanente.Equals("1") || _dettaglio.Segnato.Equals("1"))
        //        //            {
        //        //                _logger.DebugFormat("IsPermanenteNP: {0} - Segnato: {1}", _dettaglio.IsPermanente, _dettaglio.Segnato);
        //        //                return false;
        //        //            }
        //        //            _segnatureText = _dettaglio.SegnaturaNP;
        //        //        }
        //        //        break;
        //        //}

        //        //#endregion

        //        //if (String.IsNullOrWhiteSpace(_segnatureText))
        //        //{
        //        //    throw new Exception("Nessuna segnatura calcolata");
        //        //}

        //        _dettaglio = CalcolaDettaglioSegnatura(_schedaDoc, infoUtente);
        //        String[] _testiSegnatura = _getTestoFromDettaglioSegnatura(_dettaglio);

        //        // GESTIONE REPERTORIO


        //        string _idLastVersion = BusinessLogic.Documenti.VersioniManager.getLatestVersionID(_schedaDoc.docNumber, infoUtente);

        //        string extOriginale = String.Empty;
        //        bool _isDocumentoFirmato = _queryDoc.isDocFirmato(docNumber, _idLastVersion, out extOriginale);


        //        DocsPaVO.documento.FileRequest _lastDocumento = (DocsPaVO.documento.Documento)_schedaDoc.documenti[0];
        //        var _file = FileManager.getFile(_lastDocumento, infoUtente);
        //        var _contenuto = _file.content;




        //        byte[] _nuovoContenuto = AsposePDF.AsposeManager.ApponiSegnaturaPermanente(_testiSegnatura, _contenuto, _isDocumentoFirmato);


        //        _logger.Debug($"Nuovo file con segnatura { (_nuovoContenuto?.Length > 0 ? "" : "NON") } generato");

        //        bool result;
        //        // ASPOSE
        //        if (_isDocumentoFirmato)
        //        {
        //            result = Documenti.SignedFileManager.AppendDocumentoFirmato(_nuovoContenuto, false, ref _lastDocumento, infoUtente);
        //        }
        //        else
        //        {
        //            DocsPaVO.documento.FileRequest _r2 = new DocsPaVO.documento.FileRequest()
        //            {
        //                docNumber = docNumber,
        //                descrizione = "versione con segnatura"
        //            };
        //            // crea contenitore per la nuova versione
        //            DocsPaVO.documento.FileRequest _fileReq = Documenti.VersioniManager.addVersion(_r2, infoUtente, false);
        //            if (_fileReq != null)
        //            {
        //                // acquisisce il documento e lo inserisce nel documento
        //                DocsPaVO.documento.FileDocumento _fileDoc = new DocsPaVO.documento.FileDocumento()
        //                {
        //                    content = _nuovoContenuto,
        //                    estensioneFile = extOriginale,
        //                    length = _nuovoContenuto.Length,
        //                    name = _lastDocumento.fileName,
        //                    fullName = _lastDocumento.fileName
        //                };
        //                FileManager.putFile(_fileReq, _fileDoc, infoUtente);
        //                result = true;
        //            }
        //            else
        //            {
        //                result = false;
        //            }
        //        }


        //        if (result)
        //        {
        //            // aggiorna DB
        //            _dettaglio.Segnato = "1";
        //            _segnaturaDB.DettaglioSegnatura_Insert(_dettaglio);
        //            // UpdateDettaglioSegnatura_SetSegnato(_dettaglio);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex.Message, ex);
        //        throw ex;
        //    }
        //    _logger.Info("END");

        //    return true;

        //}


        //public static DettaglioSegnatura CalcolaDettaglioSegnaturaNP(DocsPaVO.documento.SchedaDocumento schedaDocumento, DocsPaVO.utente.InfoUtente infoUtente)
        //{
        //    _logger.Info("START");
        //    if (schedaDocumento.template != null) { return null; }
        //    DettaglioSegnatura _dettaglio = null;
        //    DocsPaVO.amministrazione.InfoAmministrazione _infoAmministrazione;
        //    DocsPaVO.utente.Ruolo _ruolo = null;
        //    DocsPaDB.Query_DocsPAWS.Segnatura _segnaturaDB;
        //    try
        //    {
        //        _infoAmministrazione = BusinessLogic.Amministrazione.AmministraManager.AmmGetInfoAmmCorrente(infoUtente.idAmministrazione);
        //        _ruolo = BusinessLogic.Utenti.UserManager.getRuoloByIdGruppo(infoUtente.idGruppo);
        //        _segnaturaDB = new DocsPaDB.Query_DocsPAWS.Segnatura();

        //        _dettaglio = new DettaglioSegnatura()
        //        {
        //            SegnaturaNP = _calcolaSegnaturaNP(schedaDocumento, _ruolo),
        //            IsPermanenteNP = _infoAmministrazione.SegnaturaNP_IsPermanente,
        //            SegnaturaRepertorio = BusinessLogic.Documenti.DocManager.GetSegnaturaRepertorio(schedaDocumento.docNumber, _infoAmministrazione.Codice, false, out string dataAnnullamento),
        //            IsPermanenteRepertorio = _segnaturaDB.SegnaturaRepertorio_IsPermanente(schedaDocumento.docNumber) ? "1" : "0",
        //            Segnato = "0"
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex.Message, ex);
        //        throw ex;
        //    }
        //    _logger.Info("END");
        //    return _dettaglio;
        //}


        //public static byte[] ApponiSegnaturaPermanenteToFileContent(byte[] contenutoFile, DettaglioSegnatura dettaglioSegnatura, bool isFirmato)
        //{
        //    _logger.Info("START");
        //    byte[] _newContent = null;
        //    DocsPaVO.documento.DettaglioSegnatura _dettaglio = null;
        //    DocsPaDB.Query_DocsPAWS.Segnatura _segnaturaDB = null;
        //    try
        //    {
        //        String[] _testiSegnatura = _getTestoFromDettaglioSegnatura(dettaglioSegnatura);
        //        _newContent = AsposePDF.AsposeManager.ApponiSegnaturaPermanente(_testiSegnatura, contenutoFile, isFirmato);

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex.Message, ex);
        //        throw ex;
        //    }

        //    _logger.Info("END");
        //    return _newContent;
        //}


        //public static DettaglioSegnatura CalcolaDettaglioSegnaturaProtocollo(DocsPaVO.documento.SchedaDocumento schedaDocumento, DocsPaVO.utente.InfoUtente infoUtente)
        //{
        //    _logger.Info("END");
        //    DettaglioSegnatura _dettaglio = null;
        //    DocsPaVO.amministrazione.InfoAmministrazione _infoAmministrazione;
        //    DocsPaVO.utente.Ruolo _ruolo = null;
        //    DocsPaDB.Query_DocsPAWS.Segnatura _segnaturaDB;
        //    try
        //    {
        //        _infoAmministrazione = BusinessLogic.Amministrazione.AmministraManager.AmmGetInfoAmmCorrente(infoUtente.idAmministrazione);
        //        _ruolo = BusinessLogic.Utenti.UserManager.getRuoloByIdGruppo(infoUtente.idGruppo);
        //        _segnaturaDB = new DocsPaDB.Query_DocsPAWS.Segnatura();

        //        switch (schedaDocumento.tipoProto.ToUpper())
        //        {
        //            case "A":
        //            case "P":
        //            case "I":
        //                // *** Documento protocollato ***
        //                _dettaglio = new DettaglioSegnatura()
        //                {
        //                    SegnaturaProtocollo = _calcolaSegnatura(schedaDocumento, _ruolo),
        //                    IsPermanenteProtocollo = _infoAmministrazione.Segnatura_IsPermanente,
        //                    ProfileID = schedaDocumento.systemId
        //                };
        //                if (schedaDocumento.template != null)
        //                {
        //                    _dettaglio.SegnaturaRepertorio = BusinessLogic.Documenti.DocManager.GetSegnaturaRepertorio(schedaDocumento.docNumber, _infoAmministrazione.Codice, false, out string dataAnnullamento);
        //                    _dettaglio.IsPermanenteProtocollo = _segnaturaDB.SegnaturaRepertorio_IsPermanente(schedaDocumento.docNumber) ? "1" : "0";
        //                }
        //                _dettaglio.Segnato = "0";
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex.Message, ex);
        //        throw ex;
        //    }
        //    _logger.Info("END");
        //    return _dettaglio;
        //}



        //public static void DettaglioSegnatura_Update_SetSegnato(DocsPaVO.documento.DettaglioSegnatura dettaglioSegnatura)
        //{
        //    _logger.Info("START");
        //    try
        //    {
        //        DocsPaDB.Query_DocsPAWS.Segnatura _segnaturaDB = new DocsPaDB.Query_DocsPAWS.Segnatura();
        //        _segnaturaDB.DettaglioSegnatura_Update_SetSegnato(dettaglioSegnatura);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex.Message, ex);
        //        throw ex;
        //    }
        //    _logger.Info("END");
        //}


        //public static DocsPaVO.documento.DettaglioSegnatura CalcolaDettaglioSegnaturaNP(string docNumber, DocsPaVO.utente.InfoUtente infoUtente)
        //{
        //    _logger.Info("START");
        //    DocsPaVO.utente.Ruolo _ruolo = null;
        //    DocsPaVO.documento.SchedaDocumento _schedaDocumento = null;
        //    DocsPaVO.documento.SchedaDocumento _documentoPrincipale = null;
        //    DocsPaVO.documento.DettaglioSegnatura _dettaglioSegnaturaNP = null;
        //    DocsPaDB.Query_DocsPAWS.Segnatura _segnaturaDB = null;
        //    try
        //    {
        //        _logger.Info("Calcolo Segnatura Permanente");
        //        _segnaturaDB = new DocsPaDB.Query_DocsPAWS.Segnatura();
        //        _logger.Debug("Recupero i dettagli del documento");
        //        _schedaDocumento = BusinessLogic.Documenti.DocManager.getDettaglio(infoUtente, docNumber, docNumber);
        //        _ruolo = BusinessLogic.Utenti.UserManager.getRuoloByIdGruppo(infoUtente.idGruppo);

        //        if (_schedaDocumento.documentoPrincipale != null)
        //        {
        //            _logger.Debug("Il Documento è un allegato, recupero il documento principale");
        //            _documentoPrincipale = BusinessLogic.Documenti.DocManager.getDettaglio(infoUtente, _schedaDocumento.documentoPrincipale.docNumber, _schedaDocumento.documentoPrincipale.docNumber);
        //            //string _detailDocPrincipale = DocsPaUtils.Functions.Functions.DisplayObjectInfo(_documentoPrincipale);
        //            //_logger.Debug(_detailDocPrincipale);
        //        }

        //        _logger.Info("Recupero i dettagli dell'amministrazione");
        //        DocsPaVO.amministrazione.InfoAmministrazione currAmm = BusinessLogic.Amministrazione.AmministraManager.AmmGetInfoAmmCorrente(infoUtente.idAmministrazione);

        //        // Recupero il primo fascicolo se esiste:
        //        _logger.Info("Recupero il codice fascicolo");
        //        if (_schedaDocumento.documentoPrincipale == null)
        //            _schedaDocumento.codiceFascicolo = BusinessLogic.Fascicoli.FascicoloManager.GetClassificaPerSegnatura(_schedaDocumento.docNumber, infoUtente, currAmm);
        //        else
        //        {
        //            _documentoPrincipale.codiceFascicolo = BusinessLogic.Fascicoli.FascicoloManager.GetClassificaPerSegnatura(_documentoPrincipale.docNumber, infoUtente, currAmm);
        //        }
        //        _logger.Debug("Codice Fascicolo: " + _schedaDocumento.codiceFascicolo ?? _documentoPrincipale?.codiceFascicolo);



        //        _dettaglioSegnaturaNP = _calcolaDettaglioSegnaturaNP(_schedaDocumento, _ruolo, _documentoPrincipale);
        //        _logger.Debug("SEGNATURANP: " + _dettaglioSegnaturaNP.SegnaturaNP);
        //        _logger.Debug("SystemId: " + _dettaglioSegnaturaNP.ProfileID + " isPermenente: " + _dettaglioSegnaturaNP.IsPermanenteProtocollo);

        //        _logger.Debug("Inserisco i dettagli della sengatura NP nel DB");
        //        // _segnaturaDB.InsertDettaglioSegnatura(_dettaglioSegnaturaNP);

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex.Message, ex);
        //        _dettaglioSegnaturaNP = null;
        //    }
        //    _logger.Info("END");

        //    return _dettaglioSegnaturaNP;
        //}


        //public string CalcolaSegnatura(DocsPaVO.documento.SchedaDocumento schedaDoc, DocsPaVO.utente.Ruolo ruolo, bool addSeganturaRepertorio = true)
        //{
        //    _logger.Info("START");
        //    string _segnatura = String.Empty;
        //    try
        //    {
        //        _segnatura = _calcolaSegnatura(schedaDoc, ruolo, null, addSeganturaRepertorio);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex.Message, ex);
        //        throw ex;
        //    }

        //    return _segnatura;
        //}


        //public static DocsPaVO.documento.DettaglioSegnatura CalcolaDettaglioSegnaturaProtocolli(DocsPaVO.documento.SchedaDocumento schedaDoc, DocsPaVO.utente.Ruolo ruolo, bool addSegnaturaRepertorio = true)
        //{
        //    _logger.Info("START");
        //    DocsPaVO.documento.DettaglioSegnatura _dettaglio = null;
        //    string _segnatura = String.Empty;
        //    try
        //    {
        //        var _infoAmministrazione = DocsPaDB.Utils.Personalization.getInstance(ruolo.idAmministrazione);

        //        _segnatura = _calcolaSegnatura(schedaDoc, ruolo, null, addSegnaturaRepertorio);
        //        _logger.Debug($"Segantura: '{_segnatura}'");
        //        _dettaglio = new DettaglioSegnatura()
        //        {
        //            IsPermanenteProtocollo = _infoAmministrazione.SegnaturaIsPermenente,
        //            SegnaturaProtocollo = _segnatura,
        //            ProfileID = schedaDoc.systemId,
        //            Segnato = "0"
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex.Message, ex);
        //        throw ex;
        //    }

        //    _logger.Info("END");
        //    return _dettaglio;
        //}

        //public static byte[] ApponiSegnaturaPermanenteToFileContent(string docNumber, byte[] contenutoFile, bool isFirmato, bool throweExceptionIfNotFound = true)
        //{
        //    _logger.Info("START");
        //    byte[] _newContent = null;
        //    DocsPaVO.documento.DettaglioSegnatura _dettaglio = null;
        //    DocsPaDB.Query_DocsPAWS.Segnatura _segnaturaDB = null;
        //    try
        //    {
        //        _segnaturaDB = new DocsPaDB.Query_DocsPAWS.Segnatura();
        //        _dettaglio = _segnaturaDB.DettaglioSegnatura_Select(docNumber);
        //        if (_dettaglio == null) { throw new Exception("Nessun Dettaglio Segnatura Permanente recuperato"); }
        //        String[] _testiSegnatura = _getTestoFromDettaglioSegnatura(_dettaglio);
        //        _newContent = AsposePDF.AsposeManager.ApponiSegnaturaPermanente(_testiSegnatura, contenutoFile, isFirmato);

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex.Message, ex);
        //        _newContent = null;
        //        if (throweExceptionIfNotFound) { throw ex; }
        //    }

        //    _logger.Info("END");
        //    return _newContent;
        //}


        //private static DocsPaVO.documento.DettaglioSegnatura _calcolaDettaglioSegnaturaNP(DocsPaVO.documento.SchedaDocumento schedaDoc, DocsPaVO.utente.Ruolo ruolo, DocsPaVO.documento.SchedaDocumento schedaDocPrincipale = null)
        //{
        //    _logger.Info("START");
        //    DocsPaVO.documento.DettaglioSegnatura _dettaglio = null;
        //    string _segnatura = String.Empty;
        //    DocsPaDB.Utils.Personalization _infoAmministrazione = null;
        //    DocsPaDB.Query_DocsPAWS.Documenti _documentQuery = null;

        //    try
        //    {
        //        _documentQuery = new DocsPaDB.Query_DocsPAWS.Documenti();
        //        _infoAmministrazione = DocsPaDB.Utils.Personalization.getInstance(ruolo.idAmministrazione);

        //        if (schedaDoc.template != null)
        //        {
        //            string _dataAnnullamento;
        //            _segnatura = _documentQuery.GetSegnaturaRepertorio(schedaDoc.docNumber, _infoAmministrazione.getCodiceAmministrazione(), false, out _dataAnnullamento);

        //        }
        //        else if (schedaDocPrincipale != null && schedaDocPrincipale.template != null)
        //        {
        //            // ----- da completare
        //        }

        //        if (String.IsNullOrWhiteSpace(_segnatura))
        //        {
        //            _segnatura = _infoAmministrazione.FormatoSegnaturaNP;
        //            _segnatura = _segnatura.Replace("COD_AMM", _infoAmministrazione.getCodiceAmministrazione());
        //            _segnatura = _segnatura.Replace("COD_UO", ruolo.uo.codice);
        //            _segnatura = _segnatura.Replace("DATA_ORA_CREAZ", schedaDoc.dataCreazione?.Trim());
        //            _segnatura = _segnatura.Replace("ID", schedaDoc.docNumber);

        //            string _codiceFascicolo = schedaDocPrincipale == null ? schedaDoc.codiceFascicolo : schedaDocPrincipale.codiceFascicolo;


        //            if (!String.IsNullOrWhiteSpace(_codiceFascicolo))
        //                _segnatura = _segnatura.Replace("CLASSIFICA", _codiceFascicolo);
        //            else
        //            {
        //                _logger.Warn("Codice fascicolo non presente");
        //                int _indexClassifica = _segnatura.IndexOf("CLASSIFICA");
        //                if (_indexClassifica > 0)  // non è il primo elemento
        //                {
        //                    _segnatura = _segnatura.Remove(_indexClassifica - 1, "CLASSIFICA".Length + 1); // include il separatore che precede
        //                }
        //                else if (_indexClassifica == 0)
        //                {
        //                    _segnatura = _segnatura.Remove(_indexClassifica, "CLASSIFICA".Length + 1);  // include il separatore che antecede
        //                }
        //            }
        //        }

        //        _dettaglio = new DocsPaVO.documento.DettaglioSegnatura()
        //        {
        //            IsPermanenteProtocollo = _infoAmministrazione.SegnaturaNPIsPermenente,
        //            SegnaturaNP = _segnatura,
        //            ProfileID = schedaDoc.systemId,
        //            Segnato = "0"
        //        };

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex.Message, ex);
        //        throw ex;
        //    }
        //    _logger.Info("END");

        //    return _dettaglio;
        //}


        /// <summary>
        /// Calcolo della Segnatura Permanente per i tipi protocollo
        /// </summary>
        /// <param name="schedaDocumento"></param>
        /// <param name="ruolo"></param>
        /// <param name="fascicolo"></param>
        /// <returns></returns>
        //private static string _calcolaSegnatura(DocsPaVO.documento.SchedaDocumento schedaDocumento, DocsPaVO.utente.Ruolo ruolo)
        //{
        //    _logger.Info("START");
        //    string _segnatura = String.Empty;
        //    DocsPaVO.documento.EtichettaInfo[] eti = null;
        //    DocsPaVO.utente.InfoUtente temput = null;
        //    DocsPaDB.Query_DocsPAWS.Documenti _documentiDB = null;
        //    DocsPaDB.Utils.Personalization _amministrazioneCorrente = null;
        //    Protocollo _protocollo = null;

        //    int MAX_LENGTH = 7;
        //    try
        //    {
        //        _protocollo = schedaDocumento.protocollo;

        //        _amministrazioneCorrente = DocsPaDB.Utils.Personalization.getInstance(schedaDocumento.registro.idAmministrazione);
        //        temput = new DocsPaVO.utente.InfoUtente();
        //        _documentiDB = new DocsPaDB.Query_DocsPAWS.Documenti();
        //        eti = _documentiDB.getLettereDocumento(temput, ruolo.idAmministrazione);

        //        string _idAmm = schedaDocumento.registro.idAmministrazione;
        //        string _numProto = _protocollo.numero.PadLeft(MAX_LENGTH, '0');
        //        string _data = _protocollo.dataProtocollazione.TrimEnd();
        //        string _codReg = schedaDocumento.registro.codRegistro;
        //        string _codAmm = _amministrazioneCorrente.getCodiceAmministrazione();
        //        string _classifica = schedaDocumento.codiceFascicolo ?? string.Empty;
        //        _segnatura = _amministrazioneCorrente.FormatoSegnatura;

        //        string arrPart = "";

        //        if (_protocollo.GetType() == typeof(DocsPaVO.documento.ProtocolloEntrata))
        //        {
        //            // arrPart = "A";
        //            arrPart = eti[0].Descrizione;
        //        }
        //        else if (_protocollo.GetType() == typeof(DocsPaVO.documento.ProtocolloUscita))
        //        {
        //            // arrPart = "P";
        //            arrPart = eti[1].Descrizione;
        //        }
        //        else if (_protocollo.GetType() == typeof(DocsPaVO.documento.ProtocolloInterno))
        //        {
        //            // arrPart = "I";
        //            arrPart = eti[2].Descrizione;
        //        }

        //        _segnatura = _segnatura.Replace("NUM_PROTO", _numProto);
        //        _segnatura = _segnatura.Replace("COD_UO", ruolo.uo.codice);
        //        _segnatura = _segnatura.Replace("DATA_ANNO", _protocollo.anno.Trim());
        //        _segnatura = _segnatura.Replace("DATA_COMP", _data);
        //        _segnatura = _segnatura.Replace("COD_REG", _codReg);
        //        _segnatura = _segnatura.Replace("COD_AMM", _codAmm);
        //        _segnatura = _segnatura.Replace("IN_OUT", arrPart);

        //        if (String.IsNullOrWhiteSpace(_classifica))
        //        {
        //            _segnatura = _segnatura.Remove(_segnatura.IndexOf("CLASSIFICA") - 1, "CLASSIFICA".Length + 1);
        //        }
        //        else
        //        {
        //            _segnatura = _segnatura.Replace("CLASSIFICA", _classifica);
        //        }

        //        if (!String.IsNullOrWhiteSpace(schedaDocumento.oraCreazione))
        //        {
        //            _segnatura = _segnatura.Replace("ORA", schedaDocumento.oraCreazione.Length > 5 ? schedaDocumento.oraCreazione.Substring(0, 5) : schedaDocumento.oraCreazione);
        //        }
        //        else if (_segnatura.Contains("ORA"))
        //        {
        //            //in caso di proto in giallo l'ora è null dunque nella segnatura appare un doppio separatore
        //            _segnatura = _segnatura.Remove(_segnatura.IndexOf("ORA") - 1, 4);
        //        }

        //        if (_segnatura.Contains("COD_RF_PROT"))
        //        {
        //            if (!string.IsNullOrEmpty(schedaDocumento.cod_rf_prot))
        //            {
        //                if (schedaDocumento.cod_rf_prot.Equals(schedaDocumento.registro.codRegistro))
        //                {
        //                    if (_segnatura.IndexOf("COD_RF_PROT") == 0)
        //                    {
        //                        _segnatura = _segnatura.Replace("COD_RF_PROT", "");
        //                        _segnatura = _segnatura.Substring(1);
        //                    }
        //                    else
        //                    {
        //                        _segnatura = _segnatura.Substring(0, _segnatura.IndexOf("COD_RF_PROT") - 1) + _segnatura.Substring(_segnatura.IndexOf("COD_RF_PROT"));
        //                        _segnatura = _segnatura.Replace("COD_RF_PROT", "");
        //                    }
        //                }
        //                else
        //                    _segnatura = _segnatura.Replace("COD_RF_PROT", schedaDocumento.cod_rf_prot);
        //            }
        //            else
        //            {
        //                if (_segnatura.IndexOf("COD_RF_PROT") == 0)
        //                {
        //                    _segnatura = _segnatura.Replace("COD_RF_PROT", "");
        //                    _segnatura = _segnatura.Substring(1);
        //                }
        //                else
        //                {
        //                    _segnatura = _segnatura.Substring(0, _segnatura.IndexOf("COD_RF_PROT") - 1) + _segnatura.Substring(_segnatura.IndexOf("COD_RF_PROT"));
        //                    _segnatura = _segnatura.Replace("COD_RF_PROT", "");
        //                }
        //            }
        //        }

        //        //ABBATANGELI - CODICE DISGUSTOSO IMPOSTO DAL GRUPPO PANZERA-LUCIANI
        //        if (_segnatura.ToUpper().StartsWith("MIBAC|MIBAC_"))
        //        {
        //            _segnatura = _segnatura.Substring(0, 6) + _segnatura.Substring(12);
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex.Message, ex);
        //        throw ex;
        //    }

        //    return _segnatura;
        //}

        //private static string _calcolaSegnaturaNP(DocsPaVO.documento.SchedaDocumento schedaDoc, DocsPaVO.utente.Ruolo ruolo)
        //{
        //    _logger.Info("START");
        //    string _segnaturaNP = String.Empty;
        //    DocsPaDB.Utils.Personalization _infoAmministrazione = null;
        //    try
        //    {
        //        _infoAmministrazione = DocsPaDB.Utils.Personalization.getInstance(ruolo.idAmministrazione);

        //        _segnaturaNP = _infoAmministrazione.FormatoSegnaturaNP;
        //        _segnaturaNP = _segnaturaNP.Replace("COD_AMM", _infoAmministrazione.getCodiceAmministrazione());
        //        _segnaturaNP = _segnaturaNP.Replace("COD_UO", ruolo.uo.codice);
        //        _segnaturaNP = _segnaturaNP.Replace("DATA_ORA_CREAZ", schedaDoc.dataCreazione?.Trim());
        //        _segnaturaNP = _segnaturaNP.Replace("ID", schedaDoc.docNumber);

        //        string _codiceFascicolo = schedaDoc.codiceFascicolo;


        //        if (!String.IsNullOrWhiteSpace(_codiceFascicolo))
        //            _segnaturaNP = _segnaturaNP.Replace("CLASSIFICA", _codiceFascicolo);
        //        else
        //        {
        //            _logger.Warn("Codice fascicolo non presente");
        //            int _indexClassifica = _segnaturaNP.IndexOf("CLASSIFICA");
        //            if (_indexClassifica > 0)  // non è il primo elemento
        //            {
        //                _segnaturaNP = _segnaturaNP.Remove(_indexClassifica - 1, "CLASSIFICA".Length + 1); // include il separatore che precede
        //            }
        //            else if (_indexClassifica == 0)
        //            {
        //                _segnaturaNP = _segnaturaNP.Remove(_indexClassifica, "CLASSIFICA".Length + 1);  // include il separatore che antecede
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex.Message, ex);
        //        throw ex;
        //    }
        //    _logger.Info("END");
        //    return _segnaturaNP;
        //}


        #endregion






        private static byte[] _firmaPdf(byte[] pdfContent, ArubaSignServiceRest.Model.JsonPdfSignApparence jsonPdfSignApparence)
        {
            _logger.Info("END");
            byte[] _result;
            try
            {
                string URL_ARSS = ConfigurationManager.AppSettings["URL_ARSS"];
                string _optPW = ConfigurationManager.AppSettings["OTP_PWD"]; // dsign
                string _typeOtpAuth = ConfigurationManager.AppSettings["TYPE_OTP_AUTH"]; // demoprod 
                string _user = ConfigurationManager.AppSettings["USER_ARSS"]; // titolare
                string _passw = ConfigurationManager.AppSettings["PWD_ARSS"]; // password22 
                string _delegate = ConfigurationManager.AppSettings["USER_DELEGATO"];
                string _delegatePWD = ConfigurationManager.AppSettings["PWD_DELEGATO"];
                string _delegateDom = ConfigurationManager.AppSettings["DELEGATE_DOMAIN"];


                //var apiInstance = new ArubaSignServiceRest.ArubaSignServiceApi("https://arss.demo.firma-automatica.it/ArubaSignService/rest");
                var apiInstance = new ArubaSignServiceRest.ArubaSignServiceApi(URL_ARSS);
                // Automatica  //Per effettuare una firma automatica è sufficiente utilizzare il metodo pkcs7signV2()
                //ArubaSignServiceRest.Model.JsonAuth jsonAuth = new ArubaSignServiceRest.Model.JsonAuth()
                //{

                //    DelegatedDomain = "demoprod",
                //    DelegatedPassword = "password11",
                //    DelegatedUser = "delegato",
                //    OtpPwd = "dsign",
                //    TypeOtpAuth = "demoprod",
                //    User = "titolare"
                //};

                // Autenticazione per firma remota
                ArubaSignServiceRest.Model.JsonAuth jsonAuth = new ArubaSignServiceRest.Model.JsonAuth()
                {
                    OtpPwd = _optPW,
                    TypeOtpAuth = _typeOtpAuth,
                    User = _user,
                    UserPWD = _passw,
                    DelegatedDomain = _delegateDom,
                    DelegatedPassword = _delegatePWD,
                    DelegatedUser = _delegate
                };

                ArubaSignServiceRest.Model.JsonSignRequestV2 jsonBaseSignRequest = new ArubaSignServiceRest.Model.JsonSignRequestV2()
                {
                    CertID = "AS0",
                    Identity = jsonAuth,
                    Requiredmark = false,
                    Binaryinput = Convert.ToBase64String(pdfContent),
                    Transport = ArubaSignServiceRest.Model.JsonTypeTransport.BYNARYNET
                };

                ArubaSignServiceRest.Model.JsonBodyRequestPdfSignatureV2 jsonBodyRequestPdfSignatureV2 = new ArubaSignServiceRest.Model.JsonBodyRequestPdfSignatureV2()
                {
                    Apparence = jsonPdfSignApparence,
                    Request = jsonBaseSignRequest,
                    Pdfprofile = ArubaSignServiceRest.Model.JsonPDFProfile.BASIC,
                    DictSignedAttributes = new ArubaSignServiceRest.Model.JsonDictionarySignedAttributes() { T = string.Empty }
                };

                // This method add a joint signature to a document (a document already signed).
                ArubaSignServiceRest.Model.JsonSignReturnV2 jsonSignReturnV2 = apiInstance.ResourceArubaSignServiceRestPdfsignatureV2POST(jsonBodyRequestPdfSignatureV2);
                switch (jsonSignReturnV2.Status)
                {
                    case "OK":
                        _result = Convert.FromBase64String(jsonSignReturnV2.Binaryoutput);
                        break;
                    case "KO":
                        throw new Exception($"Firma fallita: Code:'{jsonSignReturnV2.Code}'  Description: '{jsonSignReturnV2.Description}'");
                    default:
                        _result = Convert.FromBase64String(jsonSignReturnV2.Binaryoutput);
                        break;
                }

                _result = Convert.FromBase64String(jsonSignReturnV2.Binaryoutput);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw ex;
            }
            _logger.Info("END");
            return _result;
        }


        private static ArubaSignServiceRest.Model.JsonPdfSignApparence _getPdfSignApparence(PdfService.Model.PdfPageInfo pageInfo, DettaglioSegnatura dettaglioSegnatura)
        {
            _logger.Info("START");
            ArubaSignServiceRest.Model.JsonPdfSignApparence _pdfSignApparence;
            string _position;
            //string[] _testiSegnatura = null;
            int _altezzaSegnatura = 30; // pixel
            try
            {
                //_testiSegnatura = GetTestoFromDettaglioSegnatura(dettaglioSegnatura);
                _pdfSignApparence = new ArubaSignServiceRest.Model.JsonPdfSignApparence()
                {
                    Page = 1,
                    Leftx = 10,
                    //Rightx = Convert.ToDecimal(pageInfo.Rectangle.Width - 10),
                    //Rightx = Convert.ToDecimal((pageInfo.Rectangle.Width / 3)),
                    Rightx = Convert.ToDecimal((pageInfo.Rectangle.Width / 2)),
                    ShowDateTime = false,
                    ImageOnly = false,
                    Testo = " " + dettaglioSegnatura.ToString() + " ", // String.Join(" ", _testiSegnatura),
                    Reason = SegnaturaManager.REASON
                };

                _logger.Warn($"Testo da stampare: '{ _pdfSignApparence.Testo}'");

                _position = dettaglioSegnatura.DettaglioSegnaturaPosition?.SegnaturaPosition ?? "";

                //ABBATANGELI - Mibact - Nuovo posizionamento
                switch (_position)
                {
                    case "TOP_L":
                        _pdfSignApparence.Lefty = Convert.ToDecimal(pageInfo.Rectangle.Height - 10 - _altezzaSegnatura);
                        _pdfSignApparence.Righty = Convert.ToDecimal(pageInfo.Rectangle.Height - 10);
                        _pdfSignApparence.Rightx = _pdfSignApparence.Rightx + 5;
                        break;
                    case "TOP_C":
                    case "TOP_R":
                        _pdfSignApparence.Lefty = Convert.ToDecimal(pageInfo.Rectangle.Height - 10 - _altezzaSegnatura);
                        _pdfSignApparence.Righty = Convert.ToDecimal(pageInfo.Rectangle.Height - 10);

                        _pdfSignApparence.Leftx = Convert.ToDecimal((pageInfo.Rectangle.Width / 2)) + 5;
                        _pdfSignApparence.Rightx = _pdfSignApparence.Rightx + _pdfSignApparence.Leftx - 7;
                        break;
                    //case "TOP_R":
                    //    _pdfSignApparence.Lefty = Convert.ToDecimal(pageInfo.Rectangle.Height - 10 - _altezzaSegnatura);
                    //    _pdfSignApparence.Righty = Convert.ToDecimal(pageInfo.Rectangle.Height - 10);

                    //    _pdfSignApparence.Leftx = Convert.ToDecimal((pageInfo.Rectangle.Width / 1.5));
                    //    _pdfSignApparence.Rightx = _pdfSignApparence.Rightx + _pdfSignApparence.Leftx;
                    //    break;
                    case "BOTTOM_L":
                        _pdfSignApparence.Lefty = 10;
                        _pdfSignApparence.Righty = 10 + _altezzaSegnatura;

                        _pdfSignApparence.Rightx = _pdfSignApparence.Rightx + 5;
                        break;
                    case "BOTTOM_C":
                    case "BOTTOM_R":
                        _pdfSignApparence.Lefty = 10;
                        _pdfSignApparence.Righty = 10 + _altezzaSegnatura;

                        _pdfSignApparence.Leftx = Convert.ToDecimal((pageInfo.Rectangle.Width / 2)) + 5;
                        _pdfSignApparence.Rightx = _pdfSignApparence.Rightx + _pdfSignApparence.Leftx - 7;
                        break;
                    //case "BOTTOM_R":
                    //    _pdfSignApparence.Lefty = 10;
                    //    _pdfSignApparence.Righty = 10 + _altezzaSegnatura;

                    //    _pdfSignApparence.Leftx = Convert.ToDecimal((pageInfo.Rectangle.Width / 1.5));
                    //    _pdfSignApparence.Rightx = _pdfSignApparence.Rightx + _pdfSignApparence.Leftx;
                    //    break;
                    case "LEFT":
                        break;
                    case "RIGHT":
                        break;
                    default:
                        _logger.Warn("Posizione Segnatura non imposta, uso quella di default: TOP");
                        _pdfSignApparence.Lefty = Convert.ToDecimal(pageInfo.Rectangle.Height - 10 - _altezzaSegnatura);
                        _pdfSignApparence.Righty = Convert.ToDecimal(pageInfo.Rectangle.Height - 10);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw ex;
            }
            _logger.Info("END");
            return _pdfSignApparence;
        }




        private static string _calcolaSegnatura(DocsPaVO.documento.SchedaDocumento schedaDoc, DocsPaVO.utente.Ruolo ruolo, DocsPaVO.documento.SchedaDocumento schedaDocumentoPrincipale, bool addSegnaturaRepertorio)
        {
            _logger.Info("START");
            string _segnatura = String.Empty;
            DocsPaVO.documento.EtichettaInfo[] eti = null;
            DocsPaVO.utente.InfoUtente temput = null;
            DocsPaDB.Query_DocsPAWS.Documenti _documentiDB = null;
            DocsPaDB.Utils.Personalization _amministrazioneCorrente = null;
            Protocollo _protocollo = null;

            int MAX_LENGTH = 7;
            try
            {
                _protocollo = schedaDocumentoPrincipale == null ? schedaDoc.protocollo : schedaDocumentoPrincipale.protocollo;
                SchedaDocumento _tempDoc = schedaDocumentoPrincipale ?? schedaDoc;

                _amministrazioneCorrente = DocsPaDB.Utils.Personalization.getInstance(_tempDoc.registro.idAmministrazione);
                temput = new DocsPaVO.utente.InfoUtente();
                _documentiDB = new DocsPaDB.Query_DocsPAWS.Documenti();
                eti = _documentiDB.getLettereDocumento(temput, ruolo.idAmministrazione);

                string _idAmm = _tempDoc.registro.idAmministrazione;
                string _numProto = _protocollo.numero.PadLeft(MAX_LENGTH, '0');
                string _data = _protocollo.dataProtocollazione.TrimEnd();
                string _codReg = _tempDoc.registro.codRegistro;
                string _codAmm = _amministrazioneCorrente.getCodiceAmministrazione();
                string _classifica = _tempDoc.codiceFascicolo ?? string.Empty;
                _segnatura = _amministrazioneCorrente.FormatoSegnatura;

                string arrPart = "";

                if (_protocollo.GetType() == typeof(DocsPaVO.documento.ProtocolloEntrata))
                {
                    // arrPart = "A";
                    arrPart = eti[0].Descrizione;
                }
                else if (_protocollo.GetType() == typeof(DocsPaVO.documento.ProtocolloUscita))
                {
                    // arrPart = "P";
                    arrPart = eti[1].Descrizione;
                }
                else if (_protocollo.GetType() == typeof(DocsPaVO.documento.ProtocolloInterno))
                {
                    // arrPart = "I";
                    arrPart = eti[2].Descrizione;
                }

                _segnatura = _segnatura.Replace("NUM_PROTO", _numProto);
                _segnatura = _segnatura.Replace("COD_UO", ruolo.uo.codice);
                _segnatura = _segnatura.Replace("DATA_ANNO", _tempDoc.protocollo.anno.Trim());
                _segnatura = _segnatura.Replace("DATA_COMP", _data);
                _segnatura = _segnatura.Replace("COD_REG", _codReg);
                _segnatura = _segnatura.Replace("COD_AMM", _codAmm);
                _segnatura = _segnatura.Replace("IN_OUT", arrPart);

                if (String.IsNullOrWhiteSpace(_classifica))
                {
                    if (_segnatura.Contains("CLASSIFICA"))
                    {
                        _segnatura = _segnatura.Remove(_segnatura.IndexOf("CLASSIFICA") - 1, "CLASSIFICA".Length + 1);
                    }
                }
                else
                {
                    _segnatura = _segnatura.Replace("CLASSIFICA", _classifica);
                }

                if (!String.IsNullOrWhiteSpace(schedaDoc.oraCreazione))
                {
                    _segnatura = _segnatura.Replace("ORA", schedaDoc.oraCreazione.Length > 5 ? schedaDoc.oraCreazione.Substring(0, 5) : schedaDoc.oraCreazione);
                }
                else if (_segnatura.Contains("ORA"))
                {
                    //in caso di proto in giallo l'ora è null dunque nella segnatura appare un doppio separatore
                    if(_segnatura.Contains("ORA"))
                    {
                        _segnatura = _segnatura.Remove(_segnatura.IndexOf("ORA") - 1, 4);
                    }
                    
                }

                if (_segnatura.Contains("COD_RF_PROT"))
                {
                    if (!string.IsNullOrEmpty(_tempDoc.cod_rf_prot))
                    {
                        if (_tempDoc.cod_rf_prot.Equals(_tempDoc.registro.codRegistro))
                        {
                            if (_segnatura.IndexOf("COD_RF_PROT") == 0)
                            {
                                _segnatura = _segnatura.Replace("COD_RF_PROT", "");
                                _segnatura = _segnatura.Substring(1);
                            }
                            else
                            {
                                _segnatura = _segnatura.Substring(0, _segnatura.IndexOf("COD_RF_PROT") - 1) + _segnatura.Substring(_segnatura.IndexOf("COD_RF_PROT"));
                                _segnatura = _segnatura.Replace("COD_RF_PROT", "");
                            }
                        }
                        else
                            _segnatura = _segnatura.Replace("COD_RF_PROT", _tempDoc.cod_rf_prot);
                    }
                    else
                    {
                        if (_segnatura.IndexOf("COD_RF_PROT") == 0)
                        {
                            _segnatura = _segnatura.Replace("COD_RF_PROT", "");
                            _segnatura = _segnatura.Substring(1);
                        }
                        else
                        {
                            _segnatura = _segnatura.Substring(0, _segnatura.IndexOf("COD_RF_PROT") - 1) + _segnatura.Substring(_segnatura.IndexOf("COD_RF_PROT"));
                            _segnatura = _segnatura.Replace("COD_RF_PROT", "");
                        }
                    }
                }

                //ABBATANGELI - CODICE DISGUSTOSO IMPOSTO DAL GRUPPO PANZERA-LUCIANI
                if (_segnatura.ToUpper().StartsWith("MIBAC|MIBAC_"))
                {
                    _segnatura = _segnatura.Substring(0, 6) + _segnatura.Substring(12);
                }

                if (addSegnaturaRepertorio)
                {
                    string _docNumberTemp = _tempDoc.docNumber;
                    string _segnaturRepertorio = _documentiDB.GetSegnaturaRepertorio(_docNumberTemp, _codAmm, false, out string dataAnnullamento);
                    if (String.IsNullOrWhiteSpace(_segnaturRepertorio))
                    {
                        _logger.Debug("Segnatura repertorio: " + _segnaturRepertorio);
                        _segnatura += "|" + _segnaturRepertorio;
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw ex;
            }
            finally
            {
                _documentiDB?.Dispose();
            }

            return _segnatura;
        }

        private static string _calcolaSegnaturaRepertorio(DettaglioSegnaturaRepertorio dettaglioSegnaturaRepertorio, string codiceAmm, string versione, string codiceRegistro)
        {
            _logger.Info("START");
            string _segnatura = null;
            try
            {
                if (String.IsNullOrWhiteSpace(dettaglioSegnaturaRepertorio.Contatore)) { throw new ArgumentNullException("Contatore assente"); }

                _segnatura = dettaglioSegnaturaRepertorio.FormatoContatore;
                _segnatura = _segnatura.ToUpper().Replace("TIPOLOGIA", dettaglioSegnaturaRepertorio.Tipologia);
                _segnatura = _segnatura.ToUpper().Replace("CLASSIFICA", dettaglioSegnaturaRepertorio.Classifica);

                string _anno = dettaglioSegnaturaRepertorio.Anno?.ToString("D4") ?? String.Empty;
                _segnatura = _segnatura.ToUpper().Replace("ANNO", _anno);
                _segnatura = _segnatura.Replace("YY", _anno.Substring(2, 2));
                _segnatura = _segnatura.ToUpper().Replace("CONTATORE", dettaglioSegnaturaRepertorio.Contatore);

                _segnatura = _segnatura.Replace("AOO", codiceRegistro);

                _segnatura = _segnatura.ToUpper().Replace("COD_AMM", codiceAmm);
                
                _segnatura = _segnatura.ToUpper().Replace("COD_UO", dettaglioSegnaturaRepertorio.CodiceUO);

                _segnatura = _segnatura.ToUpper().Replace("GG/MM/AAAA HH:MM", dettaglioSegnaturaRepertorio.DataInserimento?.ToString("dd/MM/yyyy HH:mm"));
                _segnatura = _segnatura.ToUpper().Replace("GG/MM/AAAA", dettaglioSegnaturaRepertorio.DataInserimento?.ToString("dd/MM/yyyy"));

                _segnatura = _segnatura.ToUpper().Replace("VERSIONE", versione);

                _segnatura = _segnatura.Replace("RF", codiceRegistro);
                
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                _segnatura = null;
            }
            _logger.Info("END");
            return _segnatura;
        }

        public static string[] GetTestoFromDettaglioSegnatura(DettaglioSegnatura dettaglioSegnatura)
        {
            _logger.Info("START");
            string[] _testiSegnatura = null;
            List<string> _tempTesti = new List<string>();
            try
            {
                if (!String.IsNullOrWhiteSpace(dettaglioSegnatura.SegnaturaProtocollo) && dettaglioSegnatura.IsPermanenteProtocollo.Equals("1"))
                {
                    _tempTesti.Add(dettaglioSegnatura.SegnaturaProtocollo);
                }
                if (!String.IsNullOrEmpty(dettaglioSegnatura.SegnaturaRepertorio) && "1".Equals(dettaglioSegnatura.IsPermanenteRepertorio))
                {
                    if (_tempTesti.Count == 0 || "1".Equals(dettaglioSegnatura.DettaglioSegnaturaRepertorio?.Onnicomprensiva))
                    {
                        _tempTesti.Add(dettaglioSegnatura.SegnaturaRepertorio);
                    }
                }
                _testiSegnatura = _tempTesti.ToArray();

                //if (!String.IsNullOrEmpty(dettaglioSegnatura.SegnaturaRepertorio) && "1".Equals(dettaglioSegnatura.IsPermanenteRepertorio))
                //{
                //    if (!String.IsNullOrWhiteSpace(dettaglioSegnatura.SegnaturaProtocollo) && "1".Equals(dettaglioSegnatura.IsPermanenteProtocollo))
                //    {
                //        _testiSegnatura = new string[2] { dettaglioSegnatura.SegnaturaProtocollo, dettaglioSegnatura.SegnaturaRepertorio };
                //    }
                //    else
                //    {
                //        _testiSegnatura = new string[1] { dettaglioSegnatura.SegnaturaRepertorio };
                //    }
                //}
                //else if (!String.IsNullOrWhiteSpace(dettaglioSegnatura.SegnaturaProtocollo) && dettaglioSegnatura.IsPermanenteProtocollo.Equals("1"))
                //{
                //    _testiSegnatura = new string[1] { dettaglioSegnatura.SegnaturaProtocollo };
                //}
                //else if(!String.IsNullOrWhiteSpace(dettaglioSegnatura.SegnaturaNP))
                //{
                //    _testiSegnatura = new string[1] { dettaglioSegnatura.SegnaturaNP };
                //}
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                _testiSegnatura = null;
            }


            _logger.Info("END");
            return _testiSegnatura;
        }

        private static bool _apponiSegnaturaToFile(DocsPaVO.documento.FileRequest document, DocsPaVO.utente.InfoUtente infoUtente, DettaglioSegnatura dettaglioSegnatura, out bool filePresente)
        {
            _logger.Info("START");
            bool _result = true;
            DocsPaDB.Query_DocsPAWS.Documenti _queryDoc = null;
            PdfService.Model.PdfPageInfo _pageInfo;
            ArubaSignServiceRest.Model.JsonPdfSignApparence _segnatureApparence;

            try
            {
                if (String.IsNullOrWhiteSpace(document.fileName)) { filePresente = false; return true; }
                filePresente = true;

                _queryDoc = new DocsPaDB.Query_DocsPAWS.Documenti();

                string _idLastVersion = BusinessLogic.Documenti.VersioniManager.getLatestVersionID(document.docNumber, infoUtente);

                string extOriginale = String.Empty;

                var _file = FileManager.getFileFirmato(document, infoUtente, false);
                byte[] _contenuto = _file.content;

                // Apponi segnatura permante con servizio di firma Aruba

                _pageInfo = BusinessLogic.Pdf.PdfManager.GetPdfPageInfo(new System.IO.MemoryStream(_contenuto), 1);
                _segnatureApparence = _getPdfSignApparence(_pageInfo, dettaglioSegnatura);
                byte[] _nuovoContenuto = _firmaPdf(_contenuto, _segnatureApparence);

                // test
                // string _basePath = @"C:\_ROOT\temp\test_segnatura\";
                // System.IO.File.WriteAllBytes(_basePath + "firmato_BE.pdf", _nuovoContenuto);

                // test


                //bool _isDocumentoFirmato = _queryDoc.isDocFirmato(document.docNumber, _idLastVersion, out extOriginale);

                //byte[] _nuovoContenuto = AsposePDF.AsposeManager.ApponiSegnaturaPermanente(_testiSegnatura, _contenuto, _isDocumentoFirmato);





                _result = Documenti.FileManager.AppendDocumento(document, infoUtente, _nuovoContenuto);

                //if (_isDocumentoFirmato)
                //{
                //    _result = Documenti.SignedFileManager.AppendDocumentoFirmato(_nuovoContenuto, false, ref document, infoUtente);
                //}
                //else
                //{
                //    _result = Documenti.FileManager.AppendDocumento(document, infoUtente, _nuovoContenuto);
                //}

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw ex;
            }
            finally
            {
                _queryDoc?.Dispose();
            }
            _logger.Info("END");
            return _result;
        }
    }
}
