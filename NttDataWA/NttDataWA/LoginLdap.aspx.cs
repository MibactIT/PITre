using System;
using System.Web;
using System.Web.UI;
using log4net;

namespace NttDataWA
{
    public partial class LoginLdap : System.Web.UI.Page
    {
        private string userid = string.Empty;
        private bool LdapOK = false;

        protected void Page_Load(object sender, EventArgs e)
        {            
            LdapOK = (Session["ldapok"] != null) ? (bool)(Session["ldapok"]) : false;
            if (LdapOK)
            {
                userid          = (Session["userid"] != null) ? (string)(Session["userid"]) : string.Empty;
                string codAmm   = (Session["codamm"] != null) ? (string)(Session["codamm"]) : string.Empty;

                DocsPaWR.UserLogin userLogin = new DocsPaWR.UserLogin();

                userLogin.UserName          = userid;
                userLogin.Password          = string.Empty;
                userLogin.IdAmministrazione = string.Empty;
                userLogin.IPAddress         = Request.UserHostAddress;
                userLogin.SessionId         = Session.SessionID;

                createBrowserInfo(userLogin);

                string msg = string.Empty;
                if (!ForcedLoginLDAP(userLogin, codAmm, out msg))
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "redirect", "sessionend('" + userid + "'); location.href='./login.htm';", true);
                }
            }
        }

        /// <summary>
        /// Force Login
        /// </summary>
        /// <param name="lgn">UserLogin</param>
        /// <returns>bool</returns>
        private bool ForcedLogin(DocsPaWR.UserLogin lgn)
        {
            bool result = false;

            DocsPaWR.LoginResult loginResult;
            DocsPaWR.Utente user = UIManager.LoginManager.ForcedLogin(this, lgn, out loginResult);

            if (loginResult == DocsPaWR.LoginResult.OK)
            {
                userid = user.idPeople;
                result = true;
                UIManager.UserManager.SetUserInSession(user);
                UIManager.RoleManager.SetRoleInSession(user.ruoli[0]);
                UIManager.RegistryManager.SetRFListInSession(UIManager.UserManager.getListaRegistriWithRF(user.ruoli[0].systemId, "1", ""));
                UIManager.RegistryManager.SetRegAndRFListInSession(UIManager.UserManager.getListaRegistriWithRF(user.ruoli[0].systemId, "", ""));
                UIManager.UserManager.SetUserLanguage(SelectedLanguage);
                UIManager.DocumentManager.GetLettereProtocolli();
                LaunchApplication();
            }

            return result;
        }

        protected void LaunchApplication()
        {
            if (Session["directLink"] != null || Session["directlinkOffice"] != null)
            {
                string link = string.Empty;
                if (Session["directLink"] != null)
                {
                    link = Session["directLink"].ToString();
                }
                else
                {
                    link = Session["directlinkOffice"].ToString();
                    Session.Remove("directlinkOffice");
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "redirect", "sessionend('" + userid + "'); location.href='" + link + "';", true);
            }
            else
            {
                HttpContext.Current.Session["IsFirstTime"] = null;
                ScriptManager.RegisterStartupScript(Page, GetType(), "redirect", "sessionend('" + userid + "'); location.href='index.aspx';", true);
            }            
        }

        /// <summary>
        /// Force Login LDAP
        /// </summary>
        /// <param name="lgn">UserLogin</param>
        /// <param name="codAmm">Codice Amministrazione</param>
        /// <param name="message">Messaggio errore</param>
        /// <returns>bool</returns>
        private bool ForcedLoginLDAP(DocsPaWR.UserLogin lgn, string codAmm, out string message)
        {
            bool resLogin = false;
            //Session["LdapOK"] = null;

            message = string.Empty;
            string ipaddress = string.Empty;

            DocsPaWR.LoginResult loginResult;
            DocsPaWR.Utente utente = UIManager.LoginManager.ForcedLoginLDAP(this, lgn.UserName, codAmm, out loginResult);

            switch (loginResult)
            {
                case DocsPaWR.LoginResult.OK:
                    lgn.IdAmministrazione = utente.idAmministrazione;
                    userid = utente.idPeople;
                    resLogin = true;
                    LaunchApplication();
                    Session["LdapOK"] = userid;
                    break;

                case DocsPaWR.LoginResult.UNKNOWN_USER:
                    if ((!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings[Utils.WebConfigKeys.POLICY_AGENT_ENABLED.ToString()]) && System.Configuration.ConfigurationManager.AppSettings[Utils.WebConfigKeys.POLICY_AGENT_ENABLED.ToString()].ToUpper() == Boolean.TrueString.ToUpper()))
                        message = Utils.Languages.GetMessageFromCode("ErrorLogInUserNotAuthorized", SelectedLanguage);
                    else
                        message = Utils.Languages.GetMessageFromCode("ErrorLogInUserPassword", SelectedLanguage);
                    break;

                case DocsPaWR.LoginResult.USER_ALREADY_LOGGED_IN:
                    string loginMode = System.Configuration.ConfigurationManager.AppSettings[Utils.WebConfigKeys.ADMINISTERED_LOGIN_MODE.ToString()];

                    //Login with administration tool
                    if (!string.IsNullOrEmpty(loginMode) && loginMode.ToUpper() == Boolean.TrueString.ToUpper())
                    {
                        message = Utils.Languages.GetMessageFromCode("ErrorLogInAlreadyConnection", SelectedLanguage);
                    }
                    else
                    {
                        // Login with user
                        message = Utils.Languages.GetMessageFromCode("ErrorLogInAlreadyConnection", SelectedLanguage);
                        // Store login object
                        Session.Add("loginData", lgn);
                        bool autoForce = false;

                        // Gabriele Melini 10-11-2014
                        // sposto la chiave AUTO_FORCE_LOGIN nel DB                        
                        if (!string.IsNullOrEmpty(Utils.InitConfigurationKeys.GetValue("0", Utils.DBKeys.FE_AUTO_FORCE_LOGIN.ToString())))
                        {                            
                            autoForce = Utils.InitConfigurationKeys.GetValue("0", Utils.DBKeys.FE_AUTO_FORCE_LOGIN.ToString()).Equals("1") ? true : false;
                        }
                        if (autoForce)
                        {
                            ForcedLogin(lgn);
                            // INC000000472586 
                            // se è attiva la chiave AUTO_FORCE_LOGIN non deve essere visualizzato il messaggio relativo alla connessione aperta
                            message = string.Empty;
                        }
                        else
                        {
                            UserLoginForced = lgn;
                            ScriptManager.RegisterStartupScript(Page, GetType(), "force", "forceLogin('" + ipaddress + "','" + Utils.Languages.GetMessageFromCode("WarningLogInForceStart", SelectedLanguage) + "','" + Utils.Languages.GetMessageFromCode("WarningLogInForceEnd", SelectedLanguage) + "');", true);
                        }
                    }
                    break;

                case DocsPaWR.LoginResult.DISABLED_USER:
                    message = Utils.Languages.GetMessageFromCode("ErrorLogInUserDisabled", SelectedLanguage);
                    break;

                case DocsPaWR.LoginResult.NO_RUOLI:
                    message = Utils.Languages.GetMessageFromCode("ErrorLogInUserNoRoles", SelectedLanguage);
                    break;

                //case DocsPaWR.LoginResult.NO_AMMIN:
                //    caricaComboAmministrazioni();
                //    UpPnlDllAdmin.Update();
                //    message = "Selezionare un'amministrazione";
                //    break;

                case DocsPaWR.LoginResult.PASSWORD_EXPIRED:
                    //OnPasswordExpired = true;
                    //OldPassword = lgn.Password;
                    //message = Utils.Languages.GetMessageFromCode("ErrorLogInChangePassword", SelectedLanguage);
                    //if (string.IsNullOrEmpty(M_idAmministrazione) || M_idAmministrazione == "0")
                    //{
                    //    string returnMsg = "";
                    //    DocsPaWR.Amministrazione[] amministrazioni = UserManager.getListaAmministrazioniByUser(this, TxtUserId.Text, ddl_Amministrazioni.Visible, out returnMsg);
                    //    if (amministrazioni.Length == 1)
                    //    {
                    //        M_idAmministrazione = amministrazioni[0].systemId;
                    //    }
                    //}
                    //SetChangePassword();
                    //break;

                case DocsPaWR.LoginResult.DTCM_SERVICE_NO_CONTACT:
                    message = Utils.Languages.GetMessageFromCode("ErrorLogInDocumentum", SelectedLanguage);
                    break;

                case DocsPaWR.LoginResult.UNKNOWN_AMMIN:
                    message = Utils.Languages.GetMessageFromCode("ErrorLogInUnknownAdministration", SelectedLanguage);
                    break;

                case DocsPaWR.LoginResult.UNKNOWN_DTCM_USER:
                    message = Utils.Languages.GetMessageFromCode("ErrorLogInDocumentumNoUser", SelectedLanguage);
                    break;

                case DocsPaWR.LoginResult.DB_ERROR:
                    message = Utils.Languages.GetMessageFromCode("ErrorLogInDBConnection", SelectedLanguage);
                    break;

                default:
                    // Application Error
                    message = Utils.Languages.GetMessageFromCode("ErrorLogInGeneric", SelectedLanguage);
                    break;
            }

            if (resLogin)
            {
                UIManager.UserManager.SetUserInSession(utente);
                UIManager.RoleManager.SetRoleInSession(utente.ruoli[0]);
                UIManager.RegistryManager.SetRFListInSession(UIManager.UserManager.getListaRegistriWithRF(utente.ruoli[0].systemId, "1", ""));
                UIManager.RegistryManager.SetRegAndRFListInSession(UIManager.UserManager.getListaRegistriWithRF(utente.ruoli[0].systemId, "", ""));
                UIManager.UserManager.SetUserLanguage(SelectedLanguage);
                UIManager.DocumentManager.GetLettereProtocolli();
            }
            return resLogin;            
        }

        private void createBrowserInfo(DocsPaWR.UserLogin userLogin)
        {
            DocsPaWR.BrowserInfo bra = new DocsPaWR.BrowserInfo();
            bra.activex = Request.Browser.ActiveXControls.ToString();
            bra.browserType = Request.Browser.Browser;
            bra.browserVersion = Request.Browser.Version;
            string clientIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (String.IsNullOrEmpty(clientIP))
                clientIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            bra.ip = clientIP;
            bra.javaApplet = Request.Browser.JavaApplets.ToString();
            bra.javascript = Request.Browser.JavaScript.ToString();
            bra.userAgent = Request.UserAgent;

            userLogin.BrowserInfo = bra;
        }

        #region Page properties

        /// <summary>
        /// Old password
        /// </summary>
        protected string OldPassword
        {
            get
            {
                string result = string.Empty;
                if (HttpContext.Current.Session["oldPassword"] != null)
                {
                    result = HttpContext.Current.Session["oldPassword"] as string;
                }

                return result;
            }
            set
            {
                HttpContext.Current.Session["oldPassword"] = value;
            }
        }

        /// <summary>
        /// True if password expired
        /// </summary>
        //protected bool OnPasswordExpired
        //{
        //    get
        //    {
        //        bool result = false;
        //        if (HttpContext.Current.Session["onPasswordExpired"] != null)
        //        {
        //            Boolean.TryParse(HttpContext.Current.Session["onPasswordExpired"].ToString(), out result);
        //        }
        //        return result;
        //    }
        //    set
        //    {
        //        HttpContext.Current.Session["onPasswordExpired"] = value;
        //    }
        //}

        /// <summary>
        /// Selected language
        /// </summary>
        protected string SelectedLanguage
        {
            get
            {
                string result = string.Empty;
                if (HttpContext.Current.Session["selectedLanguage"] != null)
                {
                    result = HttpContext.Current.Session["selectedLanguage"] as string;
                }

                return result;
            }
            set
            {
                HttpContext.Current.Session["selectedLanguage"] = value;
            }
        }

        /// <summary>
        /// Selected language
        /// </summary>
        protected DocsPaWR.UserLogin UserLoginForced
        {
            get
            {
                DocsPaWR.UserLogin result = null;
                if (HttpContext.Current.Session["userLoginForced"] != null)
                {
                    result = HttpContext.Current.Session["userLoginForced"] as DocsPaWR.UserLogin;
                }

                return result;
            }
            set
            {
                HttpContext.Current.Session["userLoginForced"] = value;
            }
        }

        //protected string M_idAmministrazione
        //{
        //    get
        //    {
        //        string result = string.Empty;
        //        if (HttpContext.Current.Session["m_idAmministrazione"] != null)
        //        {
        //            result = HttpContext.Current.Session["m_idAmministrazione"] as string;
        //        }

        //        return result;
        //    }
        //    set
        //    {
        //        HttpContext.Current.Session["m_idAmministrazione"] = value;
        //    }
        //}
        #endregion
    }
}