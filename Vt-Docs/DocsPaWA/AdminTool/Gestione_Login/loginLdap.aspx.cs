using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

namespace DocsPAWA.AdminTool.Gestione_Login
{
    public partial class loginLdap : System.Web.UI.Page
    {
        private ILog logger = LogManager.GetLogger(typeof(loginLdap));
        private string userid = string.Empty;
        private bool LdapOK = false;
        protected string _userID = string.Empty;
        protected string _userPWD = string.Empty;
        protected DocsPAWA.DocsPaWR.InfoUtenteAmministratore datiAmministratore = new DocsPAWA.DocsPaWR.InfoUtenteAmministratore();

        protected void Page_Load(object sender, EventArgs e)
        {
            LdapOK = (Request.Form.Get("LDapOk") != null) ? Convert.ToBoolean(Request.Form.Get("LDapOk")) : false;
            Session["LdapOK"] = LdapOK;
            if (LdapOK)
            {
                userid = Request.Form.Get("userId");
                string codAmm = Request.Form.Get("amm");

                DocsPaWR.UserLogin userLogin = new DocsPaWR.UserLogin();

                userLogin.UserName = userid;
                userLogin.Password = string.Empty;
                userLogin.IdAmministrazione = string.Empty;
                userLogin.IPAddress = Request.UserHostAddress;
                userLogin.SessionId = Session.SessionID;

                //createBrowserInfo(userLogin);

                bool loginOk;
                LoginProfilataLDAP(userLogin, codAmm, out loginOk);
                if (!loginOk)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "redirect", "location.href='login.aspx';", true);
                }
            }
            else
            {
                Session.Remove("LdapOK");
            }
        }

        private void LoginProfilataLDAP(DocsPaWR.UserLogin userLogin, string codAmm, out bool loginOk)
        {
            logger.Debug("Enter in LoginProfilataLDAP ");
            loginOk = false;

            try
            {
                string userId = userLogin.UserName;
                string userPassword = userLogin.Password;
                logger.Debug("login per " + userId);
                if (!string.IsNullOrEmpty(userId))
                {
                    DocsPAWA.AdminTool.Manager.AmministrazioneManager manager = new DocsPAWA.AdminTool.Manager.AmministrazioneManager();
                    DocsPAWA.DocsPaWR.EsitoOperazione esito = new DocsPAWA.DocsPaWR.EsitoOperazione();

                    userLogin.UserName = userId;
                    userLogin.Password = userPassword;
                    userLogin.SessionId = Session.SessionID;
                    userLogin.IPAddress = this.Request.UserHostAddress;
                    logger.Debug("Chiamata il manager");
                    esito = manager.LoginLDAP(userLogin, true, out datiAmministratore);

                    // gestione della sessione dei dati dell'utente amministratore
                    DocsPAWA.AdminTool.Manager.SessionManager session = new DocsPAWA.AdminTool.Manager.SessionManager();
                    session.setUserAmmSession(datiAmministratore);
                    logger.Debug("Esito codice: " + esito.Codice);
                    switch (esito.Codice)
                    {
                        case 0: // tutto ok
                            this._userID = userId;
                            this._userPWD = userPassword;
                            loginOk = true;
                            logger.Debug("Login ok - codice 0");
                            this.gotoHomePageProfilataLDAP();
                            break;
                        case 1: // errore generico
                            logger.Debug("errore generico - codice 1");
                            session.removeUserAmmSession();
                            break;
                        case 99: // utente non riconosciuto  
                            logger.Debug("utente non riconosciuto - codice 99");
                            session.removeUserAmmSession();
                            break;
                        case 100: // utente già connesso
                            logger.Debug("gia connesso - codice 100");
                            break;
                        case 200: // ....NON GESTITO!... utente presente su più amministrazioni (non vale per il SYSTEM ADMIN [tipo = 1])
                            logger.Debug("non gestito  utente presente su più amministrazioni (non vale per il SYSTEM ADMIN [tipo = 1]- codice 200");
                            break;
                    }
                }
            }
            catch(Exception ex)
            {
                logger.Debug("Catturo eccezione" + ex.Message);
            }
        }

        /// <summary>
        /// reindirizzamento rispetto alla tipologia utente
        /// </summary>
        private void gotoHomePageProfilataLDAP()
        {
            try
            {
                /*
				* GESTIONE DELLA SESSIONE:
				* -----------------------------------------------------------------------------
				* sia il tool di amministrazione sia Docspa si trovano sotto lo stesso progetto 
				* quindi hanno in comune il presente Global.asax .
				* 
				* Esiste una sessione denominata "AppWA" che all'accesso del tool di amm.ne 
				* viene impostata a "ADMIN"; all'accesso di Docspa viene impostata a "DOCSPA".
				* 
				* Vedi >>>>>>>     Global.asax.cs > Session_End(Object sender, EventArgs e)
				*/
                Session["AppWA"] = "ADMIN";
                Session["UserIdAdmin"] = datiAmministratore.userId; //utile per la gestione del cambia password				
                                                                    // -----------------------------------------------------------------------------
                string script = string.Empty;
                string tipoAmministratore = string.Empty;
                string redirectUrl = string.Empty;

                tipoAmministratore = datiAmministratore.tipoAmministratore;

                switch (tipoAmministratore)
                {
                    case "3":
                        if (!this.setCurrAmmAndMenu())
                        {
                            //this.GUI("error");
                            return;
                        }
                        //MIBACT: richiesta abilitazione HomePage pe gli utenti amministratori
                        redirectUrl = "../Gestione_Homepage/Home2.aspx?from=HP1";
                        break;
                    default:
                        redirectUrl = "../Gestione_Homepage/Home.aspx";
                        break;
                }
                script = "<script>window.location.href = '" + redirectUrl + "'</script>";
                this.scriptJP(script);
            }
            catch
            {
                //this.GUI("error");
            }
        }

        /// <summary>
        /// nuovo per la gestione degli utenti profilati:
        /// se l'utente è di tipo USER ADMIN, imposta l'amministrazione corrente
        /// </summary>
        /// <returns></returns>
        private bool setCurrAmmAndMenu()
        {
            bool retValue = true;

            try
            {
                // reperimento dati dell'amministrazione alla quale appartiene l'utente loggato
                DocsPAWA.AdminTool.Manager.AmministrazioneManager manager = new DocsPAWA.AdminTool.Manager.AmministrazioneManager();
                //manager.GetAmmAppartenenza(this._userID, this._userPWD);
                if (datiAmministratore != null && !string.IsNullOrEmpty(datiAmministratore.idAmministrazione))
                    manager.InfoAmmCorrente(datiAmministratore.idAmministrazione);
                else
                    manager.GetAmmAppartenenza(this._userID, this._userPWD);
                if (manager.getCurrentAmm() != null)
                {
                    DocsPAWA.DocsPaWR.InfoAmministrazione amm = manager.getCurrentAmm();

                    string codice = amm.Codice;
                    string descrizione = amm.Descrizione;
                    string dominio = "";
                    string idAmm = amm.IDAmm;

                    // imposta la sessione come se l'utente fosse passato dalla homepage ed avesse impostato l'amministrazione da gestire
                    Session["AMMDATASET"] = codice + "@" + descrizione + "@" + dominio + "@" + idAmm;

                    // prende le voci di menu associate a questo USER ADMIN
                    manager.GetAmmListaVociMenu(datiAmministratore.idCorrGlobali, amm.IDAmm);
                    DocsPAWA.DocsPaWR.Menu[] listaVociMenu = manager.getListaVociMenu();

                    if (listaVociMenu != null && listaVociMenu.Length > 0)
                    {
                        datiAmministratore.VociMenu = listaVociMenu;

                        DocsPAWA.AdminTool.Manager.SessionManager session = new DocsPAWA.AdminTool.Manager.SessionManager();
                        session.removeUserAmmSession();
                        session.setUserAmmSession(datiAmministratore);
                    }
                }
                else
                {
                    retValue = false;
                }
            }
            catch
            {
                retValue = false;
            }

            return retValue;
        }

        #region Utility
        private void scriptJP(string script)
        {
            if (!this.Page.IsStartupScriptRegistered("scriptJavaScript"))
                this.ClientScript.RegisterStartupScript(this.GetType(), "scriptJavaScript", script);
            //this.Page.RegisterStartupScript("scriptJavaScript", script);				
        }
        #endregion
    }
}