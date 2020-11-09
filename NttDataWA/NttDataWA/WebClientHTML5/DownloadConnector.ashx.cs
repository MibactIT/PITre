using NttDataWA.DocsPaWR;
using NttDataWA.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NttDataWA.WebClientHTML5
{
    /// <summary>
    /// Descrizione di riepilogo per DownloadConnector
    /// </summary>
    public class DownloadConnector : IHttpHandler
    {
        private const String LINUX_OS = "Linux";

        public void ProcessRequest(HttpContext context)
        {
            try {
                string fileName = "WebClientConnector.msi";
                if (!String.IsNullOrEmpty(context.Request.UserAgent) && context.Request.UserAgent.IndexOf(LINUX_OS) > 0)
                    fileName = "WebClientConnector.tar.gz";
                System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                response.Clear();
                response.ClearHeaders();
                response.ClearContent();
                response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                response.ContentType = "text/plain";
                response.TransmitFile(System.Web.HttpContext.Current.Server.MapPath(fileName));
                response.Flush();
                response.End();
            }catch(Exception ex){
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}