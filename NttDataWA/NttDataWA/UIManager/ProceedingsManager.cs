using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NttDataWA.Utils;
using NttDataWA.DocsPaWR;

namespace NttDataWA.UIManager
{
    public class ProceedingsManager
    {
        private static DocsPaWR.DocsPaWebService docsPaWS = ProxyManager.GetWS();

        public static List<string> GetTipiProcedimentoAmministrazione()
        {
            try
            {
                return docsPaWS.GetTipiProcedimentoAmministrazione(UserManager.GetInfoUser().idAmministrazione).ToList();
            }
            catch (Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return null;
            }
        }

        public static DocsPaWR.ReportProcedimentoResponse GetProcedimentiReport(DocsPaWR.ReportProcedimentoRequest request)
        {
            try
            {
                return docsPaWS.GetReportProcedimento(request);
            }
            catch (Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return null;
            }
        }

        public static List<DocsPaWR.Registro> GetAOOAssociateProcedimento(string template)
        {
            try
            {
                return new List<Registro>(docsPaWS.GetAOOAssociateProcedimento(template, UserManager.GetInfoUser().idAmministrazione));
            }
            catch(Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return null;
            }
        }

        public static bool ReindirizzaProcedimento(string idProject, string idAOO, string note)
        {
            try
            {
                DocsPaWR.ReindirizzaProcedimentoResponse response = docsPaWS.ReindirizzaProcedimento(new DocsPaWR.ReindirizzaProcedimentoRequest() { IdProject = idProject, IdAOO = idAOO, Note = note, Utente = UserManager.GetInfoUser() });
                return response.Success;
            }
            catch(Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return false;
            }
        }

        public static bool CheckProcedimentoReindirizzato(string idProject)
        {
            try
            {
                return docsPaWS.CheckProcedimentoReindirizzato(idProject);
            }
            catch(Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return false;
            }
        }
    }
}