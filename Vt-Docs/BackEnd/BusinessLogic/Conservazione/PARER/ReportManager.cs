using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocsPaVO.Conservazione.PARER.Report;

namespace BusinessLogic.Conservazione.PARER
{
    public class ReportManager
    {
        public static ReportSingolaAmmResponse GetDataReportSingolaAmm(ReportSingolaAmmRequest request)
        {
            DocsPaDB.Query_DocsPAWS.Conservazione cons = new DocsPaDB.Query_DocsPAWS.Conservazione();

            ReportSingolaAmmResponse response = new ReportSingolaAmmResponse();

            string idRespCons = cons.GetIdUtenteResponsabileConservazione(request.IdAmm);

            if (!string.IsNullOrEmpty(idRespCons))
            {
                response = cons.GetDataReportSingolaAmm(request);

                if (response != null)
                {
                    DocsPaDB.Query_DocsPAWS.Utenti u = new DocsPaDB.Query_DocsPAWS.Utenti();
                    response.MailResponsabile = u.GetEmailUtente(idRespCons);
                }
            }

            return response;
        }
    }
}
