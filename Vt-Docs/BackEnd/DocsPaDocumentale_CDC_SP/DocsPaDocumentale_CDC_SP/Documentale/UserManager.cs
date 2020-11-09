using System;
using System.Collections.Generic;
using System.Text;
using DocsPaDocumentale.Interfaces;
using DocsPaVO;
using DocsPaVO.documento;
using DocsPaVO.utente;
using log4net;

namespace DocsPaDocumentale_CDC_SP.Documentale
{
    /// <summary>
    /// 
    /// </summary>
    public class UserManager : IUserManager
    {
        private static ILog logger = LogManager.GetLogger(typeof(UserManager));

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        private IUserManager _userManagerETDOCS = null;



        /// <summary>
        /// </summary>
        public UserManager()
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Effettua il login di un utente amministratore
        /// </summary>
        /// <param name="utente"></param>
        /// <param name="loginResult"></param>
        /// <returns></returns>
        public bool LoginAdminUser(DocsPaVO.utente.UserLogin userLogin, bool forceLogin, out DocsPaVO.amministrazione.InfoUtenteAmministratore utente, out DocsPaVO.utente.UserLogin.LoginResult loginResult)
        {
            utente = null;
            loginResult = UserLogin.LoginResult.UNKNOWN_USER;

            try
            {
                // Connessione al sistema ETDOCS
                bool connected = this.UserManagerETDOCS.LoginAdminUser(userLogin, forceLogin, out utente, out loginResult);



                return connected;
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Errore nella login dell'utente al sistema documentale: {0}", ex.Message);
                logger.Debug(errorMessage);

                throw new ApplicationException(errorMessage, ex);
            }
        }

        /// <summary>
        /// Effettua il login di un utente amministratore
        /// </summary>
        /// <param name="utente"></param>
        /// <param name="loginResult"></param>
        /// <returns></returns>
        public bool LoginAdminUserLDAP(DocsPaVO.utente.UserLogin userLogin, bool forceLogin, out DocsPaVO.amministrazione.InfoUtenteAmministratore utente, out DocsPaVO.utente.UserLogin.LoginResult loginResult)
        {
            utente = null;
            loginResult = UserLogin.LoginResult.UNKNOWN_USER;

            try
            {
                // Connessione al sistema ETDOCS
                bool connected = this.UserManagerETDOCS.LoginAdminUserLDAP(userLogin, forceLogin, out utente, out loginResult);

                return connected;
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Errore nella login dell'utente al sistema documentale: {0}", ex.Message);
                logger.Debug(errorMessage);

                throw new ApplicationException(errorMessage, ex);
            }
        }


        /// <summary>
        /// Connessione dell'utente al sistema documentale
        /// </summary>
        /// <param name="utente"></param>
        /// <param name="loginResult"></param>
        /// <returns></returns>
        public bool LoginUser(DocsPaVO.utente.UserLogin userLogin, out DocsPaVO.utente.Utente utente, out DocsPaVO.utente.UserLogin.LoginResult loginResult)
        {
            try
            {
                // Connessione al sistema ETDOCS
                bool connected = this.UserManagerETDOCS.LoginUser(userLogin, out utente, out loginResult);



                return connected;
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Errore nella login dell'utente al sistema documentale: {0}", ex.Message);
                logger.Debug(errorMessage);
                throw new ApplicationException(errorMessage, ex);
            }
        }

        public virtual bool Checkconnection()
        {
            return true;
        }

        /// <summary>
        /// Modifica password utente
        /// </summary>
        /// <param name="newPassword"/></param>
        /// <param name="utente"></param>
        ///// <returns></returns>
        public DocsPaVO.Validations.ValidationResultInfo ChangeUserPwd(DocsPaVO.utente.UserLogin user, string oldPassword)
        {
            DocsPaVO.Validations.ValidationResultInfo result = null;

            using (DocsPaDB.TransactionContext transactionContext = new DocsPaDB.TransactionContext())
            {
                result = this.UserManagerETDOCS.ChangeUserPwd(user, oldPassword);

                if (result.Value)
                    transactionContext.Complete();
            }
            return result;
        }

        /// <summary>
        /// Disconnessione dell'utente dal sistema documentale
        /// </summary>
        /// <param name="dst">Identificativo univoco della sessione utente</param>
        /// <returns></returns>
        public bool LogoutUser(string dst)
        {
            try
            {
                // Connessione al sistema ETDOCS
                bool disconnected = this.UserManagerETDOCS.LogoutUser(dst);

                return disconnected;
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Errore nella logout dell'utente al sistema documentale: {0}", ex.Message);
                logger.Debug(errorMessage);
                throw new ApplicationException(errorMessage, ex);
            }
        }


        /// <summary>
        /// Effettua il login di un utente LDAP
        /// </summary>
        /// <param name="utente">Oggetto Utente connesso</param>
        /// <returns>True = OK; False = Si è verificato un errore</returns>
        public bool LoginUserLDAP(DocsPaVO.utente.UserLogin userLogin, out DocsPaVO.utente.Utente utente, out DocsPaVO.utente.UserLogin.LoginResult loginResult)
        {
            bool result = true;
            utente = null;
            loginResult = DocsPaVO.utente.UserLogin.LoginResult.OK;

            try
            {
                DocsPaDB.Query_DocsPAWS.Utenti utenti = new DocsPaDB.Query_DocsPAWS.Utenti();

                string name = System.String.Empty;
                int idAmm = 0;

                if (!string.IsNullOrEmpty(userLogin.UserName))
                    name = userLogin.UserName;
                if (!string.IsNullOrEmpty(userLogin.IdAmministrazione))
                    idAmm = Convert.ToInt32(userLogin.IdAmministrazione);

                if (utenti.IsUtenteDisabled(userLogin.UserName, userLogin.Modulo, userLogin.IdAmministrazione))
                {
                    loginResult = DocsPaVO.utente.UserLogin.LoginResult.DISABLED_USER;
                    result = false;
                    logger.Debug("Utente disabilitato");
                }

                //verifica userId su tabella utenti
                string peopleId = string.Empty;

                if (result && !utenti.UserLogin(out peopleId, name, idAmm.ToString(), userLogin.Modulo))
                {
                    loginResult = DocsPaVO.utente.UserLogin.LoginResult.UNKNOWN_USER;
                    result = false;
                    logger.Debug("Utente sconosciuto");
                }

                if (result && !string.IsNullOrEmpty(peopleId))
                {
                    if (!utenti.CheckLdapLogin(userLogin.UserName))
                    {
                        result = false;
                    }
                }

                if (result)
                {
                    // Reperimento metadati dell'utente
                    utente = utenti.GetUtente(name, userLogin.IdAmministrazione, userLogin.Modulo);

                    // Associazione token di autenticazione
                    //utente.dst = this.CreateUserToken();
                }

            }
            catch (Exception ex)
            {
                logger.Debug("Errore nella login.", ex);
                result = false;
                utente = null;
            }

            return result;
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Documentale etdocs
        /// </summary>
        protected IUserManager UserManagerETDOCS
        {
            get
            {
                if (this._userManagerETDOCS == null)
                    this._userManagerETDOCS = new DocsPaDocumentale_ETDOCS.Documentale.UserManager();
                return this._userManagerETDOCS;
            }
        }



        /// <summary>
        /// Aggiornamento token di autenticazione del documentale esterno in ETDOCS
        /// </summary>
        /// <param name="infoUtente"></param>
        protected virtual bool UpdateUserToken(string newDst, string oldDst)
        {
            bool retValue = false;

            using (DocsPaDB.DBProvider dbProvider = new DocsPaDB.DBProvider())
            {
                string commandText = string.Format("UPDATE DPA_LOGIN SET DST = '{0}' WHERE DST = '{1}'", newDst, oldDst);

                int rowsAffected;
                if (dbProvider.ExecuteNonQuery(commandText, out rowsAffected))
                    retValue = (rowsAffected == 1);
            }

            return retValue;
        }

        public string GetSuperUserAuthenticationToken()
        {
            return this.UserManagerETDOCS.GetSuperUserAuthenticationToken();
        }

        #endregion
    }
}
