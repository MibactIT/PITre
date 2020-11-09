using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using log4net;

namespace Amministrazione
{
    /// <summary>
    /// Summary description for Exit.
    /// </summary>
    public class Exit : System.Web.UI.Page
    {
        private ILog logger = LogManager.GetLogger(typeof(Exit));
        private void Page_Load(object sender, System.EventArgs e)
        {
            this.Session.Abandon();

            string sessionID = Session.SessionID;

            DocsPAWA.AdminTool.Manager.SessionManager sessionManager = new DocsPAWA.AdminTool.Manager.SessionManager();
            
            AmmUtils.WebServiceLink web = new AmmUtils.WebServiceLink();
            DocsPAWA.DocsPaWR.InfoUtenteAmministratore user = sessionManager.getUserAmmSession();
            if (user != null)
                web.Logout(user);

            switch (Request.QueryString["FROM"])
            {
                case "ABORT":
                    //Response.Redirect("login.htm");
                    break;

                case "EXPIRED":
                    FormsAuthentication.SignOut();
                    //Response.Redirect("login.htm");
                    break;
            }
            string logoutRedirectUrl = "";
           // non fuziona   logoutRedirectUrl = DocsPaUtils.Configuration.InitConfigurationKeys.GetValue("0", "LOGOUT_REDIRECT_URL");
            logoutRedirectUrl = System.Configuration.ConfigurationManager.AppSettings["LOGOUT_REDIRECT_URL"];

            logger.Debug("exit url ldap redirect  " + logoutRedirectUrl);
            if(!string.IsNullOrEmpty(logoutRedirectUrl))
            {
                logger.Debug("exit url ldap redirect  logoutRedirectUrl");
                
                Response.Redirect(logoutRedirectUrl);
            }
           else
                {
                logger.Debug("exit url ldap redirect  login.htm");
                //Response.Redirect("login.htm");
                string httpFullPath = DocsPAWA.Utils.getHttpFullPath();

                //Response.Redirect(httpFullPath + "/AdminTool/Gestione_Login/login.aspx");
                Response.Redirect("Gestione_Login/login.aspx");
            }

        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
        }
        #endregion
    }
}
