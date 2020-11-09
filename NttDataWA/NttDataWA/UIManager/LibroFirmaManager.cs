using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NttDataWA.Utils;
using NttDataWA.DocsPaWR;

namespace NttDataWA.UIManager
{
    public class LibroFirmaManager
    {
        private static DocsPaWR.DocsPaWebService docsPaWS = ProxyManager.GetWS();

        #region Services Backend

        /// <summary>
        /// Metodo per l'estrazione degli elementi in libro firma
        /// </summary>
        /// <returns></returns>
        public static List<ElementoInLibroFirma> GetElementiLibroFirma()
        {
            try
            {
                return docsPaWS.GetElementiLibroFirma(UserManager.GetInfoUser()).ToList<ElementoInLibroFirma>();
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return null;
            }
        }

        public static List<TipoRuolo> GetTypeRole()
        {
            try
            {
                return docsPaWS.GetTypeRole(UserManager.GetInfoUser()).ToList<TipoRuolo>();
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return null;
            }
        }

        public static bool AggiornaStatoElementi(List<ElementoInLibroFirma> elementi, string nuovoStato, out string message)
        {
            try
            {
                docsPaWS.Timeout = System.Threading.Timeout.Infinite;
                return docsPaWS.AggiornaStatoElementiInLibroFirma(elementi.ToArray(), nuovoStato, RoleManager.GetRoleInSession(), UserManager.GetInfoUser(), out message);
            }
            catch (System.Exception ex)
            {            
                message = string.Empty;
                return false;
            }
        }
        public static bool AggiornaDataEsecuzioneElementoInLibroFirma(string docnumber, string stato)
        {
            try
            {
                return docsPaWS.AggiornaDataEsecuzioneElemento(docnumber, stato);
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        public static bool AggiornaStatoElemento(ElementoInLibroFirma elemento, string nuovoStato, out string message)
        {
            try
            {
                return docsPaWS.AggiornaStatoElementoInLibroFirma(elemento, nuovoStato, RoleManager.GetRoleInSession(), UserManager.GetInfoUser(), out message);
            }
            catch (System.Exception ex)
            {
                message = string.Empty;
                return false;
            }
        }


        public static bool UpdateIstanzaProcessoDiFirma(IstanzaProcessoDiFirma istanzaProcesso)
        {
            try
            {
                return docsPaWS.UpdateIstanzaProcessoDiFirma(istanzaProcesso, UserManager.GetInfoUser());
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        public static bool InterruptionSignatureProcessByProponent(IstanzaProcessoDiFirma istanzaProcesso, string noteInterruzione, string idDocPrincipale)
        {
            try
            {
                return docsPaWS.InterruptionSignatureProcessByProponent(istanzaProcesso, noteInterruzione, RoleManager.GetRoleInSession(), UserManager.GetInfoUser(), idDocPrincipale);
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        public static List<IstanzaProcessoDiFirma> GetIstanzaProcessoDiFirma(string docnumber)
        {
            try
            {
                return docsPaWS.GetIstanzaProcessoDiFirmaByDocnumber(docnumber, UserManager.GetInfoUser()).ToList();
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public static List<IstanzaProcessoDiFirma> GetIstanzaProcessoDiFirmaByIdProcesso(string idProcesso)
        {
            try
            {
                return docsPaWS.GetIstanzaProcessoDiFirmaByIdProcesso(idProcesso, UserManager.GetInfoUser()).ToList();
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public static List<string> GetElementiInLibroFirmaByDestinatario(Corrispondente corr)
        {
            try
            {
                return docsPaWS.GetElementiInLibroFirmaByDestinatario(corr, UserManager.GetInfoUser()).ToList();
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }
		
		/// <summary>
        /// Metodo per verificare se si è titolari del documento il L.F.
        /// </summary>
        /// <returns></returns>
        public static bool IsTitolare(string docNumber, InfoUtente utente)
        {
            try
            {
                return docsPaWS.IsTitolareElementoInLF(docNumber, utente);
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return false;
            }
        }

        /// <summary>
        /// Metodo per verificare il tipo di firma da apporre, nel caso in cui il documento è stato inserito in libro firma
        /// </summary>
        /// <param name="fileReq"></param>
        /// <returns></returns>
        public static string GetTypeSignatureToBeEntered(FileRequest fileReq)
        {
            try
            {
                return docsPaWS.GetTypeSignatureToBeEntered(fileReq, UserManager.GetInfoUser());
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return null;
            }
        }

        public static bool PutElectronicSignature(FileRequest approvingFile, bool isAdvancementProcess, out string message)
        {
            bool retVal = false;

            retVal = docsPaWS.PutElectronicSignature(approvingFile, UserManager.GetInfoUser(), isAdvancementProcess, out message);

            return retVal;
        }

        public static List<FirmaResult> PutElectronicSignatureMassive(List<FileRequest> approvingFile, bool isAdvancementProcess)
        {
            List<FirmaResult> firmaRsult = new List<FirmaResult>();
            try
            {
                firmaRsult = docsPaWS.PutElectronicSignatureMassive(approvingFile.ToArray(), UserManager.GetInfoUser(), isAdvancementProcess).ToList();
            }
            catch (System.Exception ex)
            {
                return null;
            }
            return firmaRsult;
        }

        public static bool InterruptionSignatureProcessByHolder(ElementoInLibroFirma elemento, out string error)
        {
            error = string.Empty;
            try
            {
                return docsPaWS.InterruptionSignatureProcessByHolder(elemento, RoleManager.GetRoleInSession(), UserManager.GetInfoUser(), out error);
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return false;
            }
        }

        public static bool InserimentoInLibroFirma(System.Web.UI.Page page, Trasmissione trasm, string idTrasmSingola)
        {
            try
            {
                SchedaDocumento sch = DocumentManager.getDocumentDetailsNoSecurity(page, trasm.infoDocumento.docNumber, trasm.infoDocumento.docNumber);
                TrasmissioneSingola trasmSingola = (from n in trasm.trasmissioniSingole where n.systemId.Equals(idTrasmSingola) select n).FirstOrDefault();
                string noteNoteSing = trasmSingola.noteSingole;
                string note = !string.IsNullOrEmpty(trasm.noteGenerali) ? (string.IsNullOrEmpty(noteNoteSing) ? trasm.noteGenerali : trasm.noteGenerali + " - " + noteNoteSing) : noteNoteSing;
                ElementoInLibroFirma elemento = new ElementoInLibroFirma()
                {
                    InfoDocumento = new InfoDocLibroFirma() { Docnumber = sch.docNumber, VersionId = sch.documenti[0].versionId, NumVersione = Convert.ToInt32(sch.documenti[0].version) },
                    DataAccettazione = DateTime.Now.ToString(),
                    Modalita = LibroFirmaManager.Modalita.MANUALE,
                    TipoFirma = trasmSingola.ragione.azioneRichiesta.ToString(),
                    StatoFirma = TipoStatoElemento.DA_FIRMARE,
                    RuoloProponente = trasm.ruolo,
                    UtenteProponente = trasm.utente,
                    IdRuoloTitolare = RoleManager.GetRoleInSession().idGruppo,
                    IdUtenteTitolare = UserManager.GetUserInSession().idPeople,
                    IdUtenteLocker = UserManager.GetUserInSession().idPeople,
                    IdTrasmSingola = idTrasmSingola,
                    Note = note
                };

                return docsPaWS.InserimentoInLibroFirma(elemento, UserManager.GetInfoUser());
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return false;
            }
        }

        public static void RejectElementsSignatureProcess(List<ElementoInLibroFirma> elements)
        {
            try
            {
                docsPaWS.RejectElementsSignatureProcess(elements.ToArray(), RoleManager.GetRoleInSession(), UserManager.GetInfoUser());
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
            }
        }

        public static bool IsDocInLibroFirma(string docnumber)
        {
            try
            {
                return docsPaWS.IsDocInLibroFirma(docnumber);
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return false;
            }
        }

        public static bool IsDocOrAllInLibroFirma(string docnumber)
        {
            try
            {
                return docsPaWS.IsDocOrAllInLibroFirma(docnumber);
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return false;
            }
        }

        public static List<IstanzaProcessoDiFirma> GetIstanzaProcessiDiFirmaByFilter(List<FiltroIstanzeProcessoFirma> filters, int numPage, int pageSize, out int numTotPage, out int nRec)
        {
            numTotPage = 0;
            nRec = 0;
            return docsPaWS.GetIstanzaProcessiDiFirmaByFilter(filters.ToArray(), UserManager.GetInfoUser(), numPage, pageSize, out numTotPage, out nRec).ToList();
        }


        public static List<IstanzaProcessoDiFirma> GetInfoProcessesStartedForDocument(string idMainDocument)
        {
            return docsPaWS.GetInfoProcessesStartedForDocument(idMainDocument).ToList();
        }

        /// <summary>
        /// Seleziona l'url dell'icona in base al tipo di evento
        /// </summary>
        /// <param name="passo"></param>
        /// <returns></returns>
        public static string GetIconEventType(PassoFirma passo)
        {
            string url = string.Empty;
            string tipoEvento = string.Empty;

            switch (passo.Evento.TipoEvento)
            {
                case LibroFirmaManager.TypeStep.EVENT:
                    //url = "../Images/Icons/LibroFirma/event.png";
                    url = passo.utenteCoinvolto != null && !string.IsNullOrEmpty(passo.utenteCoinvolto.idPeople) ? "../Images/Icons/LibroFirma/Event_People.png" : "../Images/Icons/LibroFirma/Event_Role.png";
                    break;
                case LibroFirmaManager.TypeStep.SIGN:
                    tipoEvento = passo.Evento.CodiceAzione.ToLower();
                    url = passo.utenteCoinvolto != null && !string.IsNullOrEmpty(passo.utenteCoinvolto.idPeople) ? "../Images/Icons/LibroFirma/" + tipoEvento + "_user.png" : "../Images/Icons/LibroFirma/" + tipoEvento + "_role.png";
                    break;
                case LibroFirmaManager.TypeStep.WAIT:
                    tipoEvento = passo.Evento.CodiceAzione.ToLower();
                    url = "../Images/Icons/LibroFirma/" + tipoEvento + ".png";
                    break;
            }
            return url;
        }

        /// <summary>
        /// Ritona la label del nodo di passo
        /// </summary>
        /// <param name="passo"></param>
        /// <returns></returns>
        public static string GetHolder(PassoFirma passo)
        {
            string holder = passo.numeroSequenza.ToString() + " - ";
            string user = string.Empty;
            switch (passo.Evento.TipoEvento)
            {
                case LibroFirmaManager.TypeStep.EVENT:
                    holder += passo.Evento.Descrizione + " - ";
                    if (passo.ruoloCoinvolto != null && !string.IsNullOrEmpty(passo.ruoloCoinvolto.systemId))
                    {
                        user = (passo.utenteCoinvolto != null && !string.IsNullOrEmpty(passo.utenteCoinvolto.idPeople)) ? passo.utenteCoinvolto.descrizione : string.Empty;
                        holder += string.IsNullOrEmpty(user) ? passo.ruoloCoinvolto.descrizione : user + " (" + passo.ruoloCoinvolto.descrizione + ")";
                    }
                    else if (passo.TpoRuoloCoinvolto != null && !string.IsNullOrEmpty(passo.TpoRuoloCoinvolto.descrizione))
                    {
                        holder += passo.TpoRuoloCoinvolto.descrizione;
                    }
                    else
                    {
                        holder += ". . .";
                    }
                    break;
                case LibroFirmaManager.TypeStep.SIGN:
                    if (passo.ruoloCoinvolto != null && !string.IsNullOrEmpty(passo.ruoloCoinvolto.systemId))
                    {
                        user = (passo.utenteCoinvolto != null && !string.IsNullOrEmpty(passo.utenteCoinvolto.idPeople)) ? passo.utenteCoinvolto.descrizione : string.Empty;
                        holder += string.IsNullOrEmpty(user) ? passo.ruoloCoinvolto.descrizione : user + " (" + passo.ruoloCoinvolto.descrizione + ")";
                    }
                    else if (passo.TpoRuoloCoinvolto != null && !string.IsNullOrEmpty(passo.TpoRuoloCoinvolto.descrizione))
                    {
                        holder += passo.TpoRuoloCoinvolto.descrizione;
                    }
                    else
                    {
                        holder += ". . .";
                    }
                    break;
                case LibroFirmaManager.TypeStep.WAIT:
                    holder += Utils.Languages.GetLabelFromCode("SignatureProcessWait", UserManager.GetUserLanguage());
                    break;
            }

            return holder;
        }


        /// <summary>
        /// Seleziona l'url dell'icona in base al tipo di evento
        /// </summary>
        /// <param name="passo"></param>
        /// <returns></returns>
        public static string GetIconEventType(IstanzaPassoDiFirma passo)
        {
            string url = string.Empty;
            string tipoEvento = string.Empty;

            switch (passo.Evento.TipoEvento)
            {
                case LibroFirmaManager.TypeStep.EVENT:
                    tipoEvento = passo.Evento.Gruppo.ToLower();
                    url = "../Images/Icons/LibroFirma/" + tipoEvento + ".png";
                    break;
                case LibroFirmaManager.TypeStep.SIGN:
                    tipoEvento = passo.Evento.CodiceAzione.ToLower();
                    url = passo.UtenteCoinvolto != null && !string.IsNullOrEmpty(passo.UtenteCoinvolto.idPeople) ? "../Images/Icons/LibroFirma/" + tipoEvento + "_user.png" : "../Images/Icons/LibroFirma/" + tipoEvento + "_role.png";
                    break;
                case LibroFirmaManager.TypeStep.WAIT:
                    tipoEvento = passo.Evento.CodiceAzione.ToLower();
                    url = "../Images/Icons/LibroFirma/" + tipoEvento + ".png";
                    break;
            }
            return url;
        }

        public static string GetIconEventTypeDisabled(IstanzaPassoDiFirma passo)
        {
            string url = string.Empty;
            string tipoEvento = string.Empty;

            switch (passo.Evento.TipoEvento)
            {
                case LibroFirmaManager.TypeStep.EVENT:
                    tipoEvento = passo.Evento.Gruppo.ToLower();
                    url = "../Images/Icons/LibroFirma/" + tipoEvento + "_disabled.png";
                    break;
                case LibroFirmaManager.TypeStep.SIGN:
                    tipoEvento = passo.Evento.CodiceAzione.ToLower();
                    url = passo.UtenteCoinvolto != null && !string.IsNullOrEmpty(passo.UtenteCoinvolto.idPeople) ? "../Images/Icons/LibroFirma/" + tipoEvento + "_user_disabled.png" : "../Images/Icons/LibroFirma/" + tipoEvento + "_role_disabled.png";
                    break;
                case LibroFirmaManager.TypeStep.WAIT:
                    tipoEvento = passo.Evento.CodiceAzione.ToLower();
                    url = "../Images/Icons/LibroFirma/" + tipoEvento + "_disabled.png";
                    break;
            }
            return url;
        }

        /// <summary>
        /// Seleziona l'url dell'icona in base al tipo di firma(digitale/elettronica)
        /// </summary>
        /// <param name="passo"></param>
        /// <returns></returns>
        public static string GetIconTypeSignature(ElementoInLibroFirma elemento)
        {
            string url = string.Empty;

            url = !string.IsNullOrEmpty(elemento.IdUtenteTitolare) ? "../Images/Icons/LibroFirma/" + elemento.TipoFirma.ToLower() + "_user.png" : "../Images/Icons/LibroFirma/" + elemento.TipoFirma.ToLower() + "_role.png";

            return url;
        }

        /// <summary>
        /// Ritona la label del nodo di passo
        /// </summary>
        /// <param name="passo"></param>
        /// <returns></returns>
        public static string GetHolder(IstanzaPassoDiFirma passo)
        {
            string holder = passo.numeroSequenza.ToString() + " - ";
            string user = string.Empty;
            switch (passo.Evento.TipoEvento)
            {
                case LibroFirmaManager.TypeStep.EVENT:
                    user = (passo.UtenteCoinvolto != null && !string.IsNullOrEmpty(passo.UtenteCoinvolto.idPeople)) ? passo.UtenteCoinvolto.descrizione : string.Empty;
                    holder += passo.Evento.Descrizione + " - ";
                    holder += string.IsNullOrEmpty(user) ? passo.RuoloCoinvolto.descrizione : user + " (" + passo.RuoloCoinvolto.descrizione + ")";
                    break;
                case LibroFirmaManager.TypeStep.SIGN:
                    user = (passo.UtenteCoinvolto != null && !string.IsNullOrEmpty(passo.UtenteCoinvolto.idPeople)) ? passo.UtenteCoinvolto.descrizione : string.Empty;
                    holder += string.IsNullOrEmpty(user) ? passo.RuoloCoinvolto.descrizione : user + "  (" + passo.RuoloCoinvolto.descrizione + ")";
                    break;
                case LibroFirmaManager.TypeStep.WAIT:
                    holder += Utils.Languages.GetLabelFromCode("SignatureProcessWait", UserManager.GetUserLanguage());
                    break;
            }

            return holder;
        }


        /// <summary>
        /// Estrae la label in base al tipo di firma(digitale/elettronica)
        /// </summary>
        /// <param name="passo"></param>
        /// <returns></returns>
        public static string GetLabelTypeSignature(string tipoFirma)
        {
            string label = string.Empty;
            switch (tipoFirma)
            { 
                case TypeEvent.SIGN_PADES:
                    label = Utils.Languages.GetLabelFromCode("LibroFirmaImgTypeDigitalSignaturePadesTooltip", UserManager.GetUserLanguage());
                    break;
                case TypeEvent.SIGN_CADES:
                    label = Utils.Languages.GetLabelFromCode("LibroFirmaImgTypeDigitalSignatureCadesTooltip", UserManager.GetUserLanguage());
                    break;
                case TypeEvent.VERIFIED:
                    label = Utils.Languages.GetLabelFromCode("LibroFirmaImgTypeElettronicsSignatureWithSubscriptionTooltip", UserManager.GetUserLanguage());
                    break;
                case TypeEvent.ADVANCEMENT_PROCESS:
                    label = Utils.Languages.GetLabelFromCode("LibroFirmaImgTypeElettronicsSignatureAdvancedProcessTooltip", UserManager.GetUserLanguage());
                    break;
            }
            return label;
        }

        public static bool CanInsertInLibroFirma(TrasmissioneSingola trasmSing, string docnumber)
        {
            bool value = false;

            if (trasmSing.ragione.azioneRichiesta != null && trasmSing.ragione.azioneRichiesta.ToString() != string.Empty)
            {
                string azioneRichiesta = trasmSing.ragione.azioneRichiesta.ToString();

                String sign = string.Empty;
                if (azioneRichiesta.Equals(LibroFirmaManager.TypeEvent.SIGN_CADES) || azioneRichiesta.Equals(LibroFirmaManager.TypeEvent.SIGN_PADES))
                {
                    sign = "DO_DOC_FIRMA";
                }
                if (azioneRichiesta.Equals(LibroFirmaManager.TypeEvent.VERIFIED))
                {
                    sign = "DO_DOC_FIRMA_ELETTRONICA";
                }

                value = UIManager.UserManager.IsAuthorizedFunctions("DO_LIBRO_FIRMA") && UIManager.UserManager.IsAuthorizedFunctions(sign) && !UIManager.LibroFirmaManager.IsDocInLibroFirma(docnumber);
            }
            else
            {
                value = false;
            }

            return value;
        }

        public struct Modalita
        {
            public const string AUTOMATICA = "A";
            public const string MANUALE = "M";
        }

        public struct TypeStep
        { 
            public const string SIGN = "F";
            public const string WAIT = "W";
            public const string EVENT = "E";
        }

        public struct TypeEvent
        {
            public const string SIGN_CADES = "DOC_SIGNATURE";
            public const string SIGN_PADES = "DOC_SIGNATURE_P";
            public const string VERIFIED = "DOC_VERIFIED";
            public const string ADVANCEMENT_PROCESS = "DOC_STEP_OVER";
        }
        #endregion
    }
}
