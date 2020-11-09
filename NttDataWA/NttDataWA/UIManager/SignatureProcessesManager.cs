using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NttDataWA.Utils;
using NttDataWA.DocsPaWR;

namespace NttDataWA.UIManager
{
    public class SignatureProcessesManager
    {
        private static DocsPaWR.DocsPaWebService docsPaWS = ProxyManager.GetWS();

        #region Services Backend

        /// <summary>
        /// Metodo per l'estrazione degli eventi di notifica
        /// </summary>
        /// <returns></returns>
        public static List<AnagraficaEventi> GetEventNotification()
        {
            try
            {
                return docsPaWS.GetEventNotification(UserManager.GetInfoUser()).ToList<AnagraficaEventi>();
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return null;
            }
        }

        /// <summary>
        /// Metodo per l'estrazione degli eventi
        /// </summary>
        /// <returns></returns>
        public static List<AnagraficaEventi> GetEventTypes(string eventType)
        {
            try
            {
                return docsPaWS.GetEventTypes(eventType, UserManager.GetInfoUser()).ToList<AnagraficaEventi>();
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return null;
            }
        }

        /// <summary>
        /// Estrazione dei processi di firma dell'utente
        /// </summary>
        /// <returns></returns>
        public static List<ProcessoFirma> GetProcessiDiFirma()
        {
            try
            {
                return docsPaWS.GetProcessiDiFirma(UserManager.GetInfoUser()).ToList<ProcessoFirma>();
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Estrazione dei processi di firma visibili al ruolo
        /// </summary>
        /// <returns></returns>
        public static List<ProcessoFirma> GetProcessesSignatureVisibleRole()
        {
            try
            {
                return docsPaWS.GetProcessesSignatureVisibleRole(UserManager.GetInfoUser()).ToList<ProcessoFirma>();
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Creazione del processo di firma
        /// </summary>
        /// <param name="processoDiFirma"></param>
        /// <returns></returns>
        public static ProcessoFirma InsertProcessoDiFirma(ProcessoFirma processoDiFirma)
        {
            try
            {
                return docsPaWS.InsertProcessoDiFirma(processoDiFirma, UserManager.GetInfoUser());
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public static PassoFirma InserisciPassoDiFirma(PassoFirma passo)
        {
            try
            {
                return docsPaWS.InserisciPassoDiFirma(passo, UserManager.GetInfoUser());
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public static ProcessoFirma GetProcessoDiFirma(string idProcesso)
        {
            try
            {
                return docsPaWS.GetProcessoDiFirma(idProcesso, UserManager.GetInfoUser());
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Rimozione del processo di firma
        /// </summary>
        /// <param name="processo"></param>
        /// <returns></returns>
        public static bool RimuoviProcessoDiFirma(ProcessoFirma processo)
        {
            try
            {
                return docsPaWS.RimuoviProcessoDiFirma(processo, UserManager.GetInfoUser());
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Rimozione del processo di firma
        /// </summary>
        /// <param name="processo"></param>
        /// <returns></returns>
        public static bool RimuoviPassoDiFirma(PassoFirma passo)
        {
            try
            {
                return docsPaWS.RimuoviPassoDiFirma(passo, UserManager.GetInfoUser());
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Rimuove la visibilità per il corrispondente in input
        /// </summary>
        /// <param name="idProcesso"></param>
        /// <param name="idCorr"></param>
        /// <returns></returns>
        public static bool RimuoviVisibilitaProcesso(string idProcesso, string idCorr)
        {
            try
            {
                return docsPaWS.RimuoviVisibilitaProcesso(idProcesso, idCorr, UserManager.GetInfoUser());
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Aggiorna passo di firma
        /// </summary>
        /// <param name="passo"></param>
        /// <returns></returns>
        public static bool AggiornaPassoDiFirma(PassoFirma passo, int oldNumeroSequenza)
        {
            try
            {
                return docsPaWS.AggiornaPassoDiFirma(passo,oldNumeroSequenza, UserManager.GetInfoUser());
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// Aggiornamento del processo di firma
        /// </summary>
        /// <param name="processoDiFirma"></param>
        /// <returns></returns>
        public static ProcessoFirma AggiornaProcessoDiFirma(ProcessoFirma processoDiFirma)
        {
            try
            {
                return docsPaWS.AggiornaProcessoDiFirma(processoDiFirma, UserManager.GetInfoUser());
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Imposta la visibilita del processo per i corrispondenti in input
        /// </summary>
        /// <param name="listaCorr"></param>
        /// <param name="idProcesso"></param>
        /// <returns></returns>
        public static bool InsertVisibilitaProcesso(List<Corrispondente>listaCorr, string idProcesso)
        {
            try
            {
                return docsPaWS.InsertVisibilitaProcesso(listaCorr.ToArray(), idProcesso, UserManager.GetInfoUser());
            }
            catch(Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Estrae la lista dei corrispondenti aventi visibilita sul processo
        /// </summary>
        /// <param name="idProcesso"></param>
        /// <returns></returns>
        public static List<Corrispondente> GetVisibilitaProcesso(string idProcesso)
        {
            try
            {
                return docsPaWS.GetVisibilitaProcesso(idProcesso, UserManager.GetInfoUser()).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static bool StartProccessSignature(ProcessoFirma process, FileRequest fileReq, string note, bool notiFicaInterruzione, bool notificaConclusione, out DocsPaWR.ResultProcessoFirma resultAvvioProcesso)
        {
            bool result = false;
            resultAvvioProcesso = ResultProcessoFirma.OK;
            try
            {
                result = docsPaWS.StartProcessoDiFirma(process, fileReq, UserManager.GetInfoUser(), LibroFirmaManager.Modalita.AUTOMATICA, note, notiFicaInterruzione, notificaConclusione, out resultAvvioProcesso);
            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }

        public static List<FirmaResult> StartProccessSignatureMassive(ProcessoFirma process, List<FileRequest> fileReq, string note, bool notiFicaInterruzione, bool notificaConclusione)
        {
            List<FirmaResult> firmaRsult = new List<FirmaResult>();
            try
            {
                docsPaWS.Timeout = System.Threading.Timeout.Infinite;
                firmaRsult = docsPaWS.StartProcessoDiFirmaMassive(process, fileReq.ToArray(), UserManager.GetInfoUser(), LibroFirmaManager.Modalita.AUTOMATICA, note, notiFicaInterruzione, notificaConclusione).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
            return firmaRsult;
        }

        /// <summary>
        /// Restituisce i tipi ruolo per cui non è presente un ruolo associato e gerarchicamente legato al ruolo che avvia il processo
        /// </summary>
        /// <param name="listTypeRoleToCheck"></param>
        /// <returns></returns>
        public static List<TipoRuolo> CheckExistsRoleSupByTypeRoles(List<TipoRuolo> listTypeRoleToCheck)
        {
            List<TipoRuolo> listTypeRole = new List<TipoRuolo>();
            try
            {
                listTypeRole = docsPaWS.CheckExistsRoleSupByTypeRoles(listTypeRoleToCheck.ToArray(), UserManager.GetInfoUser()).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
            return listTypeRole;
        }

        #endregion
    }
}