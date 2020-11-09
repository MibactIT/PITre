using DocsPaVO.amministrazione;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DocsPAWA.AdminTool.Gestione_RDE
{
    public partial class ScaricoRDE : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //*--- Codice per download .zip file ---                                    
                scaricaPacchetto();
            }
        }

        protected void scaricaPacchetto()
        {
            try
            {
                //*--- Codice per download .zip file ---                                    
                DocsPAWA.DocsPaWR.InfoAmministrazione infoAmm = new DocsPAWA.DocsPaWR.InfoAmministrazione();

                infoAmm.Codice      = AmmUtils.UtilsXml.GetAmmDataSession((string)Session["AMMDATASET"], "0");
                infoAmm.Descrizione = AmmUtils.UtilsXml.GetAmmDataSession((string)Session["AMMDATASET"], "1");
                infoAmm.Dominio     = AmmUtils.UtilsXml.GetAmmDataSession((string)Session["AMMDATASET"], "2");
                infoAmm.IDAmm       = AmmUtils.UtilsXml.GetAmmDataSession((string)Session["AMMDATASET"], "3");

                string fileNameToDownload = @"RdeTool.zip";

                byte[] stream = null;
                DocsPAWA.DocsPaWR.DocsPaWebService ws = new DocsPAWA.DocsPaWR.DocsPaWebService();

                if (ws.downloadSoftwareRDE(infoAmm, out stream))
                {
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.BufferOutput = false;
                    HttpContext.Current.Response.ContentType = "application/zip";
                    HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + fileNameToDownload);
                    HttpContext.Current.Response.BinaryWrite(stream);
                    
                    HttpContext.Current.Response.End();
                }
            }
            catch (Exception ex)
            {                
                HttpContext.Current.Response.Write("<script type='text/javascript'>alert('Si è verificato un errore durante il download.');window.close();</script>");
                HttpContext.Current.Response.End();
            }
        }
    }
}