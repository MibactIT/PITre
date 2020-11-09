using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace NttDataWA.CheckInOutApplet
{
    /// <summary>
    /// Pagina necessaria per effetturare il download di un documento
    /// </summary>
    public partial class SaveFilePage : System.Web.UI.Page
    {

        public Boolean IsSocket{
            get { 

                return !String.IsNullOrEmpty(Request.QueryString["issocket"]);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.DownloadFile();
        }

        /// <summary>
        /// Download del file richiesto
        /// </summary>
        protected virtual void DownloadFile()
        {
            string filePath = Request.Params["filePath"];

            byte[] fileContent = null;

            // Reperimento contenuto del file, verificando se reperire
            // il file firmato o il file originale
            if (
                filePath.ToUpper().EndsWith(".P7M") ||
                filePath.ToUpper().EndsWith(".TSD") ||
                filePath.ToUpper().EndsWith(".M7M") ||
                filePath.ToUpper().EndsWith(".TSR")
                )
                fileContent = this.GetSignedFileContent();
            else
                fileContent = this.GetFileContent();

            if (fileContent == null)
            {   
                // Il contenuto non � stato reperito
                Response.StatusCode = -1;
                Response.StatusDescription = "Il file potrebbe non essere stato acquisito";
            }
            else
            {
                if (IsSocket)
                {
                    string base64String = System.Convert.ToBase64String(fileContent, 0, fileContent.Length);
                    Response.Write(base64String);
                }
                else
                {
                    Response.BinaryWrite(fileContent);
                }

               // Response.Flush();
            }
        }

        /// <summary>
        /// Download del contenuto del file richiesto (firmato)
        /// </summary>
        /// <returns></returns>
        protected byte[] GetSignedFileContent()
        {
            return SaveFileServices.GetSignedFileContent();
        }

        /// <summary>
        /// Download del contenuto del file richiesto
        /// </summary>
        /// <returns></returns>
        protected byte[] GetFileContent()
        {
            return SaveFileServices.GetFileContent();
        }
    }
}
