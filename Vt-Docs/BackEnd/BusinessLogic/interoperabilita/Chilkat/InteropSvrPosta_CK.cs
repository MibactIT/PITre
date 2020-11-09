﻿using System;
using System.Collections;
using System.IO;
using Chilkat;
using log4net;

namespace BusinessLogic.Interoperabilità 
{
    public class SvrPosta_CK : IMailConnector
    {
        private static ILog logger = LogManager.GetLogger(typeof(SvrPosta_CK));

        public MailMan mm = null;
        public Imap imap = null;
        private MessageSet messageSet = null;
        public bool processEmailByHeaders = true;

        #region Dichiarazioni

        private string server;
        private string password;
        private string user;
        private string port;
        private string workingDir;
        private CMClientType clientType;
        private string SmtpSsl;
        private string PopSsl; //valido anche per imapSsl
        public string codifica;
        private string smtpSTA;
        //private string connTimeout;
        //private string readTimeout;

        private EmailBundle mbx;
        //modifica
        private string mailbox;
        private string mailelaborate;
        private string mailNonElaborate;
        //fine modifica



        #endregion

        #region Costruttori

        public SvrPosta_CK(string sServer, string sUser, string sPassword, string sPort, string sWorkingDir, CMClientType clientType, string SmtpSsl, string PopSsl, string smtpSTA, string Mailbox, string Mailelaborate, string MailNonElaborate)
        {
            this.server = sServer;
            this.user = sUser;
            this.password = sPassword;
            this.port = sPort;
            this.workingDir = sWorkingDir;
            this.clientType = clientType;
            this.SmtpSsl = SmtpSsl;
            this.PopSsl = PopSsl;
            this.smtpSTA = smtpSTA;
            mm = null;
            mbx = null;
            imap = null;
            mailbox = Mailbox;
            mailelaborate = Mailelaborate;
            mailNonElaborate = MailNonElaborate;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sServer"></param>
        /// <param name="sUser"></param>
        /// <param name="sPassword"></param>
        /// <param name="sPort"></param>
        /// <param name="sWorkingDir"></param>
        /// <param name="clientType"></param>
        /// <param name="Ssl"></param>
        public SvrPosta_CK(string sServer, string sUser, string sPassword, string sPort, string sWorkingDir, CMClientType clientType, string SmtpSsl, string PopSsl, string smtpSTA)
        {

            this.server = sServer;
            this.user = sUser;
            this.password = sPassword;
            this.port = sPort;
            this.workingDir = sWorkingDir;
            this.clientType = clientType;
            this.SmtpSsl = SmtpSsl;
            this.PopSsl = PopSsl;
            this.smtpSTA = smtpSTA;
            mm = null;
            mbx = null;
            imap = null;
        }

        public SvrPosta_CK(string sServer, string sUser, string sPassword, string sPort, string sWorkingDir, CMClientType clientType, string SmtpSsl, string smtpSTA)
        {

            this.server = sServer;
            this.user = sUser;
            this.password = sPassword;
            this.port = sPort;
            this.workingDir = sWorkingDir;
            this.clientType = clientType;
            this.SmtpSsl = SmtpSsl;
            this.PopSsl = "";
            this.smtpSTA = smtpSTA;
            this.mmInitialize();

        }

        public SvrPosta_CK()
        {
        }

        #endregion

        private void mmInitialize()
        {
            mm = new MailMan();
            mm.UnlockComponent(ChilkatKeys.Mail);
            mm.MailHost = this.server;
            mm.PopUsername = this.user;
            mm.PopPassword = this.password;
            mm.SmtpHost = this.server;

            mm.SmtpSsl = (this.SmtpSsl == "1") ? true : false;
            if (!this.PopSsl.Equals(""))
                mm.PopSsl = (this.PopSsl == "1") ? true : false;

            if (System.Configuration.ConfigurationManager.AppSettings["POP3_CONN_TIMEOUT"] != null &&
                System.Configuration.ConfigurationManager.AppSettings["POP3_CONN_TIMEOUT"].ToString() != "")
                mm.ConnectTimeout = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["POP3_CONN_TIMEOUT"].ToString());

            if (System.Configuration.ConfigurationManager.AppSettings["POP3_READ_TIMEOUT"] != null &&
                System.Configuration.ConfigurationManager.AppSettings["POP3_READ_TIMEOUT"].ToString() != "")
                mm.ReadTimeout = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["POP3_READ_TIMEOUT"].ToString());
            if (!this.smtpSTA.Equals(""))
                mm.StartTLS = (this.smtpSTA == "1") ? true : false;

        }


        private void mmInitializeImap()
        {
            imap = new Chilkat.Imap();
            imap.UnlockComponent(ChilkatKeys.IMAP);
            ////imap.Connect(this.server);
            ////imap.Login(this.user, this.password);
            if (System.Configuration.ConfigurationManager.AppSettings["IMAP_CONN_TIMEOUT"] != null &&
              System.Configuration.ConfigurationManager.AppSettings["IMAP_CONN_TIMEOUT"].ToString() != "")
                mm.ConnectTimeout = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["IMAP_CONN_TIMEOUT"].ToString());

            if (System.Configuration.ConfigurationManager.AppSettings["IMAP_READ_TIMEOUT"] != null &&
                System.Configuration.ConfigurationManager.AppSettings["IMAP_READ_TIMEOUT"].ToString() != "")
                mm.ReadTimeout = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["IMAP_READ_TIMEOUT"].ToString());

            imap.Ssl = (this.PopSsl == "1") ? true : false;//this.mm.PopSsl;


        }

        #region Metodi pubblici

        public bool cancellaMailImap()
        {
            try
            {
                return imap.Expunge();

            }
            catch (Exception e)
            {
                logger.ErrorFormat("cancellaMailImap - errore: {0}" ,e.Message);
                return false;
            }
        }

        public bool moveImap(int index, bool elaborata)
        {

            try
            {
                int id = messageSet.GetId(index - 1);//index - 1); // id della mail da spostare e cancellare dalla inbox
                string cartella = mailbox + ".";      //cartella su cui spostare le mail
                bool copied = false;                  // indica se la mail è stata copiata

                if (!imap.IsConnected())
                    if (!imap.Login(user, password))
                        return false;


                cartella = cartella + (elaborata ? mailelaborate : mailNonElaborate);

                copied = imap.Copy(id, true, cartella);

                if (copied)
                {
                    return imap.SetFlag(id, true, "Deleted", 1);

                }
            }
            catch (Exception e)
            {
                logger.Error("errore durante il move imap");
                logger.ErrorFormat("{0} ; errore:{1}" ,imap.SessionLog ,e.Message );
                return false;
            }

            return false;
        }

        public bool moveImap(string uid, bool elaborata)
        {
            try
            {

                int id = Convert.ToInt32(uid);

                //string cartella = mailbox + ".";      //cartella su cui spostare le mail
                string cartella = string.Empty;      //cartella su cui spostare le mail
                bool copied = false;                  // indica se la mail è stata copiata

                if (!imap.IsConnected())
                    if (!imap.Login(user, password))
                        return false;


                //cartella = cartella + (elaborata ? mailelaborate : mailNonElaborate);
                cartella = (elaborata ? mailelaborate : mailNonElaborate);

                copied = imap.Copy(id, true, cartella);

                if (copied)
                {
                    return imap.SetFlag(id, true, "Deleted", 1);

                }
            }
            catch (Exception e)
            {
                logger.Error("errore durante il move imap");
                logger.ErrorFormat("{0} ; errore:{1}", imap.SessionLog, e.Message);
                return false;
            }

            return false;
        }

        public bool salvaMailInLocale(int indexMail, string pathFile, string NomeDellaMail)
        {
            bool retval = false;
            try
            {
                logger.Debug("path: " + pathFile);
                logger.Debug("NomeDellaMail: " + NomeDellaMail);

                indexMail--;
                Email msgComponent = mbx.GetEmail(indexMail);
                object eml = null;
                if (processEmailByHeaders)
                {
                    if (this.clientType == CMClientType.POP)
                    {
                        eml = (byte[])mm.FetchMime(msgComponent.Uidl);
                        //msgComponent = mm.GetFullEmail(msgComponent);
                    }
                    else
                    {
                        eml = imap.FetchSingleAsMime(msgComponent.GetImapUid(), true);

                        //msgComponent = imap.FetchSingle(msgComponent.GetImapUid(), true);
                    }
                }

                if (eml != null)
                {
                    try
                    {
                        if (eml.GetType() == typeof(String))
                        {
                            File.WriteAllText(@pathFile + "\\" + NomeDellaMail, (String)eml);
                        }
                        else
                        {
                            File.WriteAllBytes(@pathFile + "\\" + NomeDellaMail, (byte[])eml);
                        }

                        retval = true;
                    }
                    catch (Exception e)
                    {
                        logger.ErrorFormat("Errore durante il salvataggio della mail su disco: {0}", e.Message);
                        retval = false;
                        throw new Exception("Errore durante il salvataggio della mail su disco");
                    }
                }
            }
            catch (Exception e)
            {
                logger.ErrorFormat("Errore durante reperimento per il salvataggio della mail: {0}", e.Message);
                throw new Exception("Errore durante reperimento per il salvataggio della mail");
            }
            return retval;
        }

        public bool salvaMailInLocale(string uidl, string pathFile, string NomeDellaMail)
        {
            bool retval = false;
            try
            {
                logger.Debug("path: " + pathFile);
                logger.Debug("NomeDellaMail: " + NomeDellaMail);

                object eml = null;
                if (this.clientType == CMClientType.POP)
                {
                    //il Fetch da Pop Torna Byte[]
                    eml = mm.FetchMime(uidl);
                }
                else
                {
                    //il fetch da IMAP torna stringa
                    Email msgComponent = mm.FetchEmail(uidl);
                    eml = imap.FetchSingleAsMime(msgComponent.GetImapUid(), true);
                }
                if (eml != null)
                {
                    try
                    {
                        if (eml.GetType() == typeof(String))
                        {
                            //Imap
                            File.WriteAllText(@pathFile + "\\" + NomeDellaMail, (String)eml);
                        }
                        else
                        {
                            //Pop
                            File.WriteAllBytes(@pathFile + "\\" + NomeDellaMail, (byte[])eml);
                        }
                        retval = true;
                    }
                    catch (Exception e)
                    {
                        logger.ErrorFormat("Errore durante il salvataggio della mail su disco: {0}", e.Message);
                        retval = false;
                        throw new Exception("Errore durante il salvataggio della mail su disco");
                    }
                }
            }
            catch (Exception e)
            {
                logger.ErrorFormat("Errore durante il salvataggio della mail: {0}", e.Message);
                throw new Exception("Errore durante il salvataggio della mail");
            }
            return retval;

        }

        public bool getMessagePec(int index)
        {

            bool retval = false;

            if (this.clientType == CMClientType.POP)
            {
                if (mm == null)
                    mmInitialize();
            }
            else
                if (this.clientType == CMClientType.IMAP)
                {
                    if (imap == null)
                        mmInitializeImap();

                    try
                    {
                        if (processEmailByHeaders)
                            mbx = imap.FetchHeaders(messageSet);
                        else
                            mbx = imap.FetchBundle(messageSet);

                    }
                    catch (Exception eccezione)
                    {
                        throw new Exception(String.Format(("Errore nel recupero della mail dal server IMAP [{0}]. {1}"), server, eccezione.Message));
                    }
                }

            index--;
            try
            {
                if ((this.clientType == CMClientType.POP) && !mm.VerifyPopConnection())
                    throw new Exception("(connerr)errore di connessione alla casella pop: " + mm.LastErrorText);

                Email msgComponent = mbx.GetEmail(index);

                if (processEmailByHeaders)
                {
                    if (this.clientType == CMClientType.POP)
                    {
                        Email eml = new Email();
                        eml.AttachMessage((byte[])mm.FetchMime(msgComponent.Uidl));
                        msgComponent = eml.GetAttachedMessage(0);
                        //msgComponent = mm.GetFullEmail(msgComponent);
                    }
                    else
                    {
                        Email eml = new Email();
                        Mime mi = new Mime();
                        mi.LoadMime(imap.FetchSingleAsMime(msgComponent.GetImapUid(), true));
                        eml.AttachMessage(mi.GetMimeBytes());
                        msgComponent = eml.GetAttachedMessage(0);
                        //msgComponent = imap.FetchSingle(msgComponent.GetImapUid(), true);
                    }
                }
 
                if ((msgComponent.ReceivedSigned) && (!msgComponent.SignaturesValid))
                    return false;

                retval = extractMailPec(msgComponent);

            }
            catch (Exception exc)
            {
                logger.Debug(String.Format("Errore nel recupero del tipo della mail dal server [{0}]. {1}", server, exc.Message));
            }

            return retval;
        }
        private CMMsg extractMessage(Email msgComponent)
        {
            CMMsg msg = null;
            DateTime ReceiveDate= DateTime.MinValue;
            msg = extractMail(msgComponent);
            if (msg != null)
            {
                try{ReceiveDate= msgComponent.LocalDate;}catch{ }
                if (msg.isPECDelivered() ||
                    (msg.isFromNonPEC() &&
                    (msgComponent.NumAttachedMessages == 1)))
                {
                    msgComponent = msgComponent.GetAttachedMessage(0);
                    msg = extractMail(msgComponent);
                    if (ReceiveDate!=DateTime.MinValue)
                        msg.date = ReceiveDate;

                    int dsnRootLeaf = isDSN(msgComponent);
                    if (dsnRootLeaf != 0)
                    {
                        CMMsg msgNew = extractDSNInfo(msgComponent, dsnRootLeaf);
                       foreach (string h in msgNew.headers.Keys)
                           msg.headers.Add(h, msgNew.getHeader(h));
                       
                       msg.subject = msgNew.subject;
                       msg.HTMLBody = msgNew.body;
                       msg.body = msgNew.body;
                       msg.recipients = msgNew.recipients;
                       
                      
                    }
                }
            }
            return msg;
        }

        private int isDSN (Email e)
        {

            //Chilkat.Mime mimemail = e.GetMimeObject();

            Chilkat.Mime mimemail = new Mime();
            mimemail.LoadMime(e.GetMime());

            int parts = mimemail.NumParts;

            for (int i = 1; i < parts; i++)
            {
                Chilkat.Mime dstat = mimemail.GetPart(1);
                if (dstat == null)
                    return 0;
                if (dstat.ContentType.Equals("message/delivery-status"))
                    return i;
            }
            return 0;
        }

        private CMMsg extractDSNInfo(Email msgComponent, int dsnRootLeaf)
        {
            CMMsg retval = new CMMsg();

            //Chilkat.Mime mimemail = msgComponent.GetMimeObject();

            Chilkat.Mime mimemail = new Mime();
            mimemail.LoadMime(msgComponent.GetMime());

            Chilkat.Mime dstat = mimemail.GetPart(dsnRootLeaf);
            retval.body = string.Format("<PRE> {0} </PRE>", msgComponent.GetMime());

            if (dstat.ContentType.Equals("message/delivery-status"))
            {


                string msgDstat = dstat.GetMime();
                string dsnErrorCode = CMMsg.findSmptErrorCode(msgDstat);
                retval.headers.Add("IsDSN", "TRUE");
                if (dsnErrorCode == null)
                    dsnErrorCode = "550";

                retval.headers.Add("Status", dsnErrorCode);
                string dsnErrorText = CMMsg.decodeSmtpErrorCode(dsnErrorCode);
                if (dsnErrorText != null)
                    retval.headers.Add("Diagnostic", dsnErrorText);
                else
                    retval.headers.Add("Diagnostic", "Generic smpt error");


                retval.headers.Add("FailedRecepients", getfailedRecepients(msgDstat));

                //if (msgComponent.NumAttachedMessages > 0)
                //{
                //    Chilkat.Email originalEmail = msgComponent.GetAttachedMessage(0);
                //    retval.subject = originalEmail.Subject;
                //    retval.from = originalEmail.From;
                //}
                //else
                {

                    //Mime mime = null;

                    // Access the email via the ChilkatMime API:
                    //mime = msgComponent.GetMimeObject();

                    Chilkat.Mime mime = new Mime();
                    mime.LoadMime(msgComponent.GetMime());

                    // Now get the 3rd sub-part, which is at index 2.
                    Chilkat.Mime part3 = mime.GetPart(dsnRootLeaf+1);


                    // Is this a text/rfc822-headers part?
                    if (part3.ContentType.Equals("text/rfc822-headers") || part3.ContentType.Equals("message/rfc822"))
                    {
                        // OK, the body of this part is a collection of headers.
                        // If we get the body, we can append a few blank lines
                        // and then read it as a MIME message and access any
                        // header.
                        string msg = part3.GetBodyDecoded() + "\r\n\r\n";

                        // Load it into a MIME object.
                        Chilkat.Mime mime2 = new Chilkat.Mime();
                        mime2.LoadMime(msg);

                        // Fetch some of the headers:

                        retval.subject = mime2.GetHeaderField("subject");
                        retval.from = mime2.GetHeaderField("from");
                        string toField = mime2.GetHeaderField("to");
                        retval.recipients.Add(new CMRecipient { mail = toField, name = toField });
                    }
                }
            }

            return retval;

        }

        private string getfailedRecepients(string mailText)
        {
            string retval = string.Empty;

            string[] lines = mailText.Split('\r');
            foreach (string line in lines)
            {
                if (line.Contains("Final-Recipient"))
                {
                    string[] lineArr = line.Split(';');
                    if (lineArr.Length > 1)
                        retval += lineArr[1] + "§";
                    else
                        retval += lineArr[0] + "§";
                }
                if (line.Contains("Status: "))
                {
                    retval += line.Replace("Status: ", string.Empty) + "§";
                }
                if (line.Contains("Diagnostic-Code:"))
                {
                    string[] lineArr = line.Split(';');
                    if (lineArr.Length > 1)
                        retval += lineArr[1] + ";";
                    else
                        retval += lineArr[0] + ";";
                }



            }

            retval = retval.Replace("<", string.Empty).Replace(">", string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty);
            return retval;


        }

        public CMMsg getMessage(byte[] email)
        {

            CMMsg msg = null;
            Email msgComponent = new Email();
            if (msgComponent.SetFromMimeBytes(email))
            {
                if ((msgComponent.ReceivedSigned) && (!msgComponent.SignaturesValid))
                {
                    //return null;
                    try
                    {
                        logger.ErrorFormat("Errore nella firma del file .eml: Segnatura non valida. Soggetto:{0}. Firmato da {1}. Indirizzo Mittente: {2} ", msgComponent.Subject, msgComponent.SignedBy, msgComponent.FromAddress);
                    }
                    catch (Exception e) { }
                }
            }
            return extractMessage(msgComponent);
        }

        public CMMsg getMessage(int index)
        {


            if (this.clientType == CMClientType.POP)
            {
                if (mm == null)
                    mmInitialize();
            }
            else
                if (this.clientType == CMClientType.IMAP)
                {
                    if (imap == null)
                        mmInitializeImap();

                    try
                    {
                        if (processEmailByHeaders)
                            mbx = imap.FetchHeaders(messageSet);
                        else
                            mbx = imap.FetchBundle(messageSet);

                    }
                    catch (Exception eccezione)
                    {
                        throw new Exception(String.Format(("Errore nel recupero della mail dal server IMAP [{0}]. {1}"), server, eccezione.Message));
                    }
                }

            index--;
            try
            {
                if ((this.clientType == CMClientType.POP) && !mm.VerifyPopConnection())
                    throw new Exception("(connerr)errore di connessione alla casella pop: " + mm.LastErrorText);


                Email msgComponent = mbx.GetEmail(index);
                if (processEmailByHeaders)
                {
                    if (this.clientType == CMClientType.POP)
                    {

                        Email eml = new Email();
                        eml.AttachMessage((byte[])mm.FetchMime(msgComponent.Uidl));
                        msgComponent = eml.GetAttachedMessage(0);
                        //msgComponent = mm.GetFullEmail(msgComponent);


                    }
                    else
                    {
                        Email eml = new Email();
                        Mime mi = new Mime();
                        mi.LoadMime(imap.FetchSingleAsMime(msgComponent.GetImapUid(), true));
                        eml.AttachMessage(mi.GetMimeBytes());
                        msgComponent = eml.GetAttachedMessage(0);

                        //msgComponent = imap.FetchSingle(msgComponent.GetImapUid(), true);
                    }
                }

                if ((msgComponent.ReceivedSigned) && (!msgComponent.SignaturesValid))
                    return null;



                return extractMessage(msgComponent);
            }
            catch (Exception exc)
            {
                throw new Exception(String.Format("Errore nel recupero della mail dal server [{0}]. {1}", server, exc.Message));
            }
        }

        #region GESTIONE SCARICO CASELLA CON UIDL
        public CMMsg getMessage(string uidl)
        {

            if (this.clientType == CMClientType.POP)
            {
                if (mm == null)
                    mmInitialize();
            }
            else if (this.clientType == CMClientType.IMAP)
            {
                if (imap == null)
                    mmInitializeImap();
            }

            try
            {
                if ((this.clientType == CMClientType.POP) && !mm.VerifyPopConnection())
                    throw new Exception("(connerr)errore di connessione alla casella pop: " + mm.LastErrorText);

                Email msgComponent = mm.FetchEmail(uidl);
                if ((msgComponent.ReceivedSigned) && (!msgComponent.SignaturesValid))
                    return null;

                return extractMessage(msgComponent);
            }
            catch (Exception exc)
            {
                throw new Exception(String.Format("Errore nel recupero della mail dal server [{0}]. {1}", server, exc.Message));
            }
        }


        public string[] getUidls()
        {
            Chilkat.StringArray uilds = mbx.GetUidls();
            int n = uilds.Count;
            string[] list = new string[n];

            for (int i = 0; i <= uilds.Count - 1; i++)
            {
                list[i] = uilds.GetString(i);
            }
            return list;
        }

        public void deleteSingleMessage(string uidl)
        {
            if (this.clientType != CMClientType.POP)
                throw new Exception("Il client di posta non è inizializzato correttamente");

            try
            {
                mm.DeleteByUidl(uidl);
            }
            catch (Exception exc)
            {
                throw new Exception(String.Format("La cancellazione del messaggio con uidl {0} è fallita sul server POP [{1}]. {2}", uidl, server, exc.Message));
            }
        }

        public bool getMessagePec(string uidl)
        {

            bool retval = false;

            if (this.clientType == CMClientType.POP)
            {
                if (mm == null)
                    mmInitialize();
            }
            else if (this.clientType == CMClientType.IMAP)
            {
                if (imap == null)
                    mmInitializeImap();
            }

            try
            {
                if ((this.clientType == CMClientType.POP) && !mm.VerifyPopConnection())
                    throw new Exception("(connerr)errore di connessione alla casella pop: " + mm.LastErrorText);

                Email msgComponent = mm.FetchEmail(uidl);

                if ((msgComponent.ReceivedSigned) && (!msgComponent.SignaturesValid))
                    return false;

                retval = extractMailPec(msgComponent);

            }
            catch (Exception exc)
            {
                logger.Debug(String.Format("Errore nel recupero del tipo della mail dal server [{0}]. {1}", server, exc.Message));
            }

            return retval;
        }
        #endregion
        //public bool deleteSingleMessagePop(int i)
        //{
        //    bool retval = false;
        //    if (this.clientType != CMClientType.POP)
        //        logger.Debug("Il client di posta non è inizializzato correttamente");

        //    i--;

        //    try
        //    {
        //        string uidl = mbx.GetEmail(i).Uidl;

        //        retval=  mm.DeleteByUidl(uidl);
        //    }
        //    catch (Exception exc)
        //    {
        //        logger.Debug(String.Format("La cancellazione del messaggio numero {0} è fallita sul server POP [{1}]. {2}", i, server, exc.Message));
        //    }

        //    return retval;
        //}


        public void deleteSingleMessage(int i)
        {
            if (this.clientType != CMClientType.POP)
                throw new Exception("Il client di posta non è inizializzato correttamente");

            i--;

            try
            {
                string uidl = mbx.GetEmail(i).Uidl;
                mm.DeleteByUidl(uidl);

            }
            catch (Exception exc)
            {
                throw new Exception(String.Format("La cancellazione del messaggio numero {0} è fallita sul server POP [{1}]. {2}", i, server, exc.Message));
            }
        }

        public bool connectImap()
        {
            bool retval = false;
            try
            {
                if (imap.Connect(this.server))
                    if (imap.Login(this.user, this.password))
                    {
                        logger.Debug("la connessione al server IMAP con l'utente: " + this.user + " è stata effettuata con successo");
                        retval = true;
                    }

                if (!retval)
                    throw new ApplicationException("errore nella connessione IMAP: " + imap.LastErrorText);

                imap.CreateMailbox(mailbox);
                imap.SelectMailbox(mailbox);
                imap.CreateMailbox(mailbox + "." + mailelaborate);
                imap.CreateMailbox(mailbox + "." + mailNonElaborate);
            }
            catch (Exception e)
            {
                logger.Debug("connessione al server imap" + e.Message);

                retval = false;
            }

            return retval;
        }

        EmailBundle copyMail()
        {
            if (processEmailByHeaders)
            {
                StringArray sa = mm.GetUidls();
                return mm.FetchMultipleHeaders(sa, 2);
            }
            else
            {
                return mm.CopyMail();
            }
        }

        public void connect()
        {
            switch (clientType)
            {
                case CMClientType.POP:

                    try
                    {
                        if (mm == null)
                        {
                            mmInitialize();
                        }

                        int PopPort = Int32.Parse(this.port);
                        mm.PopSsl = this.mm.PopSsl;//(PopPort != 110 && InteroperabilitaUtils.Cfg_Pop3OverSsl);
                        mm.MailPort = PopPort;

                        int n = InteroperabilitaUtils.Cfg_NumeroTentativiLogServerPosta;

                        for (int i = 1; i <= n; i++)
                        {
                            try
                            {
                                logger.Debug("Tentativo di connessione al server " + mm.MailHost + "con Userid: " + mm.PopUsername + " per controllo casella, numero " + i);
                                mbx = copyMail();
                                if (mbx != null)
                                {
                                    logger.Debug("Tentativo di connessione al server " + mm.MailHost + "con Userid: " + mm.PopUsername + " per controllo casella, numero " + i + " effettuato con successo");
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                if (!string.IsNullOrEmpty(mm.LastErrorText))
                                    throw new Exception(mm.LastErrorText);
                                else
                                    throw ex;
                            }
                        }
                        if (mbx == null)
                        {

                            if (!string.IsNullOrEmpty(mm.LastErrorText))
                                throw new Exception(mm.LastErrorText);
                            else
                                throw new Exception("");
                        }

                    }
                    catch (Exception exc)
                    {
                        throw new Exception(String.Format("Connessione al server POP [{0}] fallita. {1}", server, exc.Message));
                    }

                    break;

                case CMClientType.SMTP:

                    try
                    {
                        if (mm == null)
                        {
                            mmInitialize();
                        }

                    }
                    catch (Exception exc)
                    {
                        throw new Exception(String.Format("Connessione al server SMTP [{0}] fallita. {1}", server, exc.Message));
                    }

                    break;
                case CMClientType.IMAP:

                    bool retval = true;
                    try
                    {
                        if (imap == null)
                        {
                            mmInitializeImap();
                        }
                        if (retval)
                        {
                            imap.Port = Int32.Parse(this.port);

                            int n = InteroperabilitaUtils.Cfg_NumeroTentativiLogServerPosta;

                            for (int i = 1; i <= n; i++)
                            {
                                try
                                {
                                    if (connectImap())
                                    {
                                        logger.Debug("Tentativo di connessione al server " + imap.Domain + " per controllo casella, numero " + i + " effettuato con successo");
                                        break;
                                    }
                                }
                                catch (Exception ex)
                                { logger.Debug("errore connessione imap: " + ex.Message + " ; " + imap.LastErrorText); }
                            }
                        }
                    }

                    catch (Exception exc)
                    {
                        throw new Exception(String.Format("Connessione al server IMAP [{0}] fallita. {1};{2}", server, exc.Message, imap.LastErrorText));
                    }

                    break;
            }
        }


        #region prova Connessione
        public bool provaConnessione(DocsPaVO.amministrazione.OrgRegistro.MailRegistro mailRegistro, out string errore, string tipoConnessione)
        {
            bool retval = false;
            errore = string.Empty;
            switch (tipoConnessione)
            {
                case "POP":

                    try
                    {
                        mm = new MailMan();
                        mm.UnlockComponent(ChilkatKeys.Mail);
                        mm.MailHost = mailRegistro.ServerPOP;
                        mm.PopUsername = mailRegistro.UserID;
                        mm.PopPassword = mailRegistro.Password;

                        mm.SmtpSsl = (mailRegistro.SMTPssl == "1") ? true : false;

                        mm.PopSsl = (mailRegistro.POPssl == "1") ? true : false;

                        mm.MailPort = mailRegistro.PortaPOP;

                        try
                        {
                            mbx = mm.CopyMail();
                            if (mbx != null)
                                retval = true;
                            else
                                errore = mm.LastErrorText;


                        }
                        catch
                        {
                            logger.Debug("errore in prva connessione di tipo pop: " + mm.LastErrorText);
                        }

                    }
                    catch (Exception exc)
                    {
                        errore = mm.LastErrorText;
                        logger.Debug("errore in provaConnessione: " + exc.Message);
                    }
                    if (mm != null)
                        mm.Dispose();
                    break;

                case "SMTP":

                    try
                    {
                        Email msg = new Email();
                        mm = new MailMan();
                        //msg.UnlockComponent(ChilkatKeys.Mail);
                        if (!string.IsNullOrEmpty(mailRegistro.UserSMTP) && mailRegistro.Password != null)
                        {
                            mm.SmtpUsername = mailRegistro.UserSMTP;
                            mm.SmtpPassword = mailRegistro.PasswordSMTP;
                        }
                        else
                        {
                            errore = "Nome utente o password errati";
                            break;
                        }
                        mm.SmtpSsl = (mailRegistro.SMTPssl == "1") ? true : false;
                        mm.SmtpPort = mailRegistro.PortaSMTP;
                        mm.StartTLS = (mailRegistro.SMTPsslSTA == "1") ? true : false;
                        mm.SmtpHost = mailRegistro.ServerSMTP;

                        msg.Body = "TEST CONNESSIONE";
                        if (!string.IsNullOrEmpty(mailRegistro.Email))
                            msg.From = mailRegistro.Email;


                        msg.AddTo("test COnnessione", mailRegistro.Email);

                        if (mm.SendEmail(msg))
                            retval = true;
                        else
                            errore = mm.LastErrorText;


                    }
                    catch
                    {
                        errore = mm.LastErrorText;
                        logger.Debug("errore in prova connessione di tipo SMTP: " + mm.LastErrorText);
                    }

                    if (mm != null)
                        mm.CloseSmtpConnection();
                    break;

                case "IMAP":
                    try
                    {
                        imap = new Chilkat.Imap();
                        imap.UnlockComponent(ChilkatKeys.IMAP);
                        imap.Ssl = (mailRegistro.POPssl == "1") ? true : false;

                        imap.Port = mailRegistro.portaIMAP;

                        try
                        {

                            if (imap.Connect(mailRegistro.serverImap))
                                if (imap.Login(mailRegistro.UserID, mailRegistro.Password))
                                    retval = true;
                                else
                                    errore = imap.LastErrorText;
                            else
                                errore = imap.LastErrorText;


                        }
                        catch (Exception ex)
                        {
                            errore = imap.LastErrorText;
                            logger.Debug("errore connessione imap: " + ex.Message + " ; ");
                        }

                    }


                    catch (Exception exc)
                    {
                        errore = imap.LastErrorText;
                        logger.Debug("errore connessione imap: " + exc.Message + " ; ");
                    }
                    if (imap != null)
                        imap.Disconnect();
                    break;
            }


            return retval;
        }
        #endregion




        public void disconnect()
        {
            switch (clientType)
            {
                case CMClientType.POP:
                    try
                    {
                        mm.Dispose();

                    }
                    catch (Exception exc)
                    {
                        throw new Exception(String.Format("Disonnessione al server [{0}] fallita. {1}", server, exc.Message));
                    }
                    break;
                case CMClientType.SMTP:
                    try
                    {
                        mm.Dispose();
                    }
                    catch (Exception exc)
                    {
                        throw new Exception(String.Format("Disconnessione al server SMTP [{0}] fallita. {1}", server, exc.Message));
                    }
                    break;
                case CMClientType.IMAP:
                    try
                    {
                        if (imap != null && imap.IsConnected())
                            imap.Dispose();
                    }
                    catch (Exception exc)
                    {
                        throw new Exception(String.Format("Disconnessione al server [{0}] fallita. {1}", server, exc.Message));
                    }
                    break;
            }
        }

        public int messageCount()
        {
            int cnt = -1;

            try
            {
                if (this.clientType != CMClientType.POP)
                {  // throw new Exception("Il client di posta non è inizializzato correttamente");

                    if (imap != null)
                    {
                        logger.Debug("reperimento delle mail nel server IMAP");
                        this.messageSet = null;
                        bool fetchUids = true;
                        if (!imap.IsConnected())
                        {
                            logger.Debug("Connessione al server IMAP per recupere i messaggi");

                            if (imap.Login(user, password))
                            {
                                logger.Debug("il login è avvuto per il count dei messaggi");
                                imap.SelectMailbox(mailbox);
                                messageSet = imap.Search("ALL", fetchUids);
                            }
                        }
                        else
                        {
                            logger.Debug("già loggato eseguo il count dei messaggi");
                            imap.SelectMailbox(mailbox);
                            messageSet = imap.Search("ALL", fetchUids);
                        }

                        if (messageSet != null)
                            logger.Debug("messageset= " + messageSet.Count + " ;messageset: " + messageSet.HasUids);
                        else
                            logger.Debug("il messageset è null perchè il nome della casella inbox è errata");
                        logger.Debug("ho recuperato tutte le informazioni delle mail presenti sul server di posta eseguio unb fetch dei messaggi la posta:");
                         
                        EmailBundle mbx=null;

                        if (processEmailByHeaders)
                            mbx = imap.FetchHeaders(messageSet);
                        else
                            mbx = imap.FetchBundle(messageSet);

                        logger.Debug("controllo se il fetch è andato a buon fine");
                        if (mbx != null)
                        {
                            logger.Debug("il fetch è andato a buon fine");
                            cnt = mbx.MessageCount;
                        }
                        else
                        {
                            logger.Debug("il fetch non andato a buon fine");
                            cnt = -1;
                        }

                    }

                }
                else
                {
                    logger.Debug("recupero i messaggi dal server di tipo pop");
                    cnt = mm.CheckMail();
                }

                if (cnt < 0)
                    throw new Exception("errore nel reperimento delle mail -- errore: " + mm.LastErrorText);
            }
            catch (Exception exc)
            {
                logger.Debug(exc.Message);
                throw new Exception(String.Format("Connessione al server [{0}] non riuscita", server));
            }

            return cnt;
        }


        public void sendMail(string sFrom, string sTo, string sCC, string sBCC, string sSubject, string sBody, CMMailFormat format, CMAttachment[] attachments, CMMailHeaders[] headers, out string outError)
        {
            outError = string.Empty;
            if (this.clientType != CMClientType.SMTP)
                throw new Exception("Il client di posta non è inizializzato correttamente");

            Email msg = new Email();
            //msg.UnlockComponent(ChilkatKeys.Mail);

            try
            {
                switch (format)
                {
                    case CMMailFormat.HTML:
                        msg.SetHtmlBody(sBody);
                        break;
                    case CMMailFormat.Text:
                        msg.Body = sBody;
                        break;
                }

                msg.From = sFrom;
                //commentato 11 nov 2005. Viene preso da web.config
                //se l'indirizzo mittente è nullo
                //				if(!(msg.From!=null && !msg.From.Equals("")))
                //					msg.From="protoetno@etnoteam.it";

                msg.AddMultipleTo(sTo);
                msg.Subject = sSubject;
                //				msg.Priority = ?;

                if (sCC != "")
                    msg.AddMultipleCC(sCC);

                if (sBCC != "")
                    msg.AddMultipleBcc(sBCC);

                // Aggiunta gestione Header
                if (headers != null)
                    foreach (CMMailHeaders header in headers)
                        msg.AddHeaderField(header.header, header.value);

                // Gestione attachments
                // 20161103 - CR
                // Nel caso ci sia un allegato iCalendar viene usata la funzione specifica di Chilkat
                if (attachments != null)
                {
                    foreach (CMAttachment attachment in attachments)
                    {
                        if (!string.IsNullOrEmpty(attachment.name) && attachment.name.EndsWith(".ics"))
                        {
                            string ical = System.Text.Encoding.ASCII.GetString(attachment._data);
                            msg.AddiCalendarAlternativeBody(ical, attachment._method);
                        }
                        else
                            msg.AddDataAttachment2(attachment.name, attachment._data, attachment.contentType);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(String.Format("Errore nella creazione del messaggio. {0}", exc.Message));
            }

            try
            {
                if (user != null && user != "" && password != null)
                {
                    mm.SmtpUsername = user;
                    mm.SmtpPassword = password;
                }
                //				mm.SmtpSsl = InteroperabilitaUtils.Cfg_SmtpOverSsl;
                //				mm.SmtpPort = InteroperabilitaUtils.Cfg_SmtpOverSsl ? 465 : Int32.Parse(this.port);

                int SmtpPort = Int32.Parse(this.port);
                mm.SmtpSsl = this.mm.SmtpSsl;
                mm.SmtpPort = SmtpPort;

                //alcune connessioni smtp richiedono questo parametro, di solito quando 
                //questo parametro else true IList this.mm.SmtpSsl=false;
                mm.StartTLS = this.mm.StartTLS;

                int n = InteroperabilitaUtils.Cfg_NumeroTentativiLogServerPosta;
                bool res = false;

                for (int i = 1; i <= n; i++)
                {
                    logger.Debug("Tentativo di connessione al server " + mm.SmtpHost + "con Userid: " + mm.SmtpUsername + " numero " + i);
                    res = mm.SendEmail(msg);
                    if (res)
                    {
                        logger.Debug("Tentativo di connessione al server " + mm.SmtpHost + "con Userid: " + mm.SmtpUsername + " numero " + i + " effettuato con successo");
                        break;
                    }
                }

                if (!res)
                {
                    outError = "Impossibile contattare il server SMTP";
                    throw new Exception(mm.LastErrorText);
                }
            }
            catch (Exception exc)
            {
                if (outError != "Impossibile contattare il server SMTP")
                    outError = "Errore generico";
                throw new Exception(String.Format("Errore nell'invio del messaggio tramite il server SMTP [{0}]. {1}", server, exc.Message));
            }
        }

        public void sendMail(string sFrom, string sTo, string sCC, string sBCC, string sSubject, string sBody, CMMailFormat format, CMAttachment[] attachments, out string outError)
        {
            sendMail(sFrom, sTo, sCC, sBCC, sSubject, sBody, format, attachments, null, out outError);
        }
       
        public void sendMail(string sFrom, string sTo, string sSubject, string sBody, CMAttachment[] attachments)
        {
            string errors;
            sendMail(sFrom, sTo, "", "", sSubject, sBody, CMMailFormat.HTML, attachments, null, out errors);
        }
   
        public void sendMail(string sFrom, string sTo, string sSubject, string sBody)
        {
            string errors;
            sendMail(sFrom, sTo, "", "", sSubject, sBody, CMMailFormat.HTML, (CMAttachment[])null, out errors);
        }

        #region Metodo Commentato
        /* Metodo sendMail ripetuto rispetto a quello sopra con l'outError*/
        /*
        public void sendMail(string sFrom, string sTo, string sCC, string sBCC, string sSubject, string sBody, CMMailFormat format, CMAttachment[] attachments, string tipoRicevutaPec)
        {
            if (this.clientType != CMClientType.SMTP)
                throw new Exception("Il client di posta non è inizializzato correttamente");

            Email msg = new Email();
            //msg.UnlockComponent(ChilkatKeys.Mail);
            CMMsg cmmsg = new CMMsg();
            
            try
            {
                switch (format)
                {
                    case CMMailFormat.HTML:
                        msg.SetHtmlBody(sBody);
                        break;
                    case CMMailFormat.Text:
                        msg.Body = sBody;
                        break;
                }

                msg.From = sFrom;
                //commentato 11 nov 2005. Viene preso da web.config
                //se l'indirizzo mittente è nullo
                //				if(!(msg.From!=null && !msg.From.Equals("")))
                //					msg.From="protoetno@etnoteam.it";

                msg.AddTo(sTo, sTo);
                msg.Subject = sSubject;
                //				msg.Priority = ?;

                if (sCC != "")
                    msg.AddCC(sCC, sCC);

                if (sBCC != "")
                    msg.AddBcc(sBCC, sBCC);

                if (attachments != null)
                    foreach (CMAttachment attachment in attachments)
                        msg.AddDataAttachment2(attachment.name, attachment._data,attachment.contentType);
            }
            catch (Exception exc)
            {
                throw new Exception(String.Format("Errore nella creazione del messaggio. {0}", exc.Message));
            }

            try
            {
                if (user != null && user != "" && password != null)
                {
                    mm.SmtpUsername = user;
                    mm.SmtpPassword = password;
                }
                //				mm.SmtpSsl = InteroperabilitaUtils.Cfg_SmtpOverSsl;
                //				mm.SmtpPort = InteroperabilitaUtils.Cfg_SmtpOverSsl ? 465 : Int32.Parse(this.port);

                int SmtpPort = Int32.Parse(this.port);
                mm.SmtpSsl = this.mm.SmtpSsl;
                mm.SmtpPort = SmtpPort;

                //alcune connessioni smtp richiedono questo parametro, di solito quando 
                //questo parametro else true IList this.mm.SmtpSsl=false;
                mm.StartTLS = this.mm.StartTLS;

                int n = InteroperabilitaUtils.Cfg_NumeroTentativiLogServerPosta;
                bool res = false;

                for (int i = 1; i <= n; i++)
                {
                    logger.Debug("Tentativo di connessione al server " + mm.SmtpHost + "con Userid: " + mm.SmtpUsername + " numero " + i);
                    res = mm.SendEmail(msg);

                    if (res)
                    {
                        logger.Debug("Tentativo di connessione al server " + mm.SmtpHost + "con Userid: " + mm.SmtpUsername + " numero " + i + " effettuato con successo");
                        break;
                    }
                }

                if (!res)
                {
                    throw new Exception(mm.LastErrorText);
                }
            }
            catch (Exception exc)
            {
                throw new Exception(String.Format("Errore nell'invio del messaggio tramite il server SMTP [{0}]. {1}", server, exc.Message));
            }
        }
        */
        #endregion




        #region Metodi privati

        # region codice commentato
        //		private void getBody(Mime component, ref string body, ref string bodyHTML)
        //		{
        //			string contentType = component.ContentType;
        //
        //			for (int i = 0; i < component.NumParts; i++)
        //			{
        //				Mime part = new Mime();
        //				part.UnlockComponent(ChilkatKeys.SMime);
        //				part = component.GetPart (i);
        //
        //				if (part.ContentType.StartsWith("multipart")) 
        //					getBody(part, ref body,ref bodyHTML);
        //				else
        //				{
        //					if (part.ContentType.ToLower()  == "text/plain")
        //						body = part.GetEntireBody();
        //
        //					if (part.ContentType.ToLower() == "text/html")
        //					{
        //						bodyHTML = part.GetBodyDecoded();
        //						//						if (part.Header[1].Value.ToLower() == "quoted-printable")
        //						//						{
        //						//							QuotedPrintableDecoder PD = new QuotedPrintableDecoder(); 
        //						//							System.IO.MemoryStream So = new MemoryStream();  
        //						//							part.BodyStream.Position = 0;
        //						//							PD.Decode(part.BodyStream, So);
        //						//							bodyHTML = System.Text.Encoding.UTF8.GetString(So.ToArray(),0,So.ToArray().Length); 
        //						//						}
        //
        //						//						if (part.Header[1].Value.ToLower()  == "base64")
        //						//						{
        //						//							bodyHTML = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(part.Body),0,Convert.FromBase64String(part.Body).Length); 
        //						//						}
        //					}
        //					Console.WriteLine();
        //				}
        //			}
        //		}
        //
        #endregion
        //ridoni 25/10/2005 dopo interpvento in simest
        //ridoni 2/11/2005 dopo intervento per fix ANAS
        private void getBody(Mime component, ref string body, ref string
            bodyHTML, string codifica)
        {
            string contentType = component.ContentType;

            if (component.NumParts <= 0)
                return;
            # region codice commentato
            //			Mime part = new Mime();
            //			part.UnlockComponent(ChilkatKeys.SMime);
            //			part = component.GetPart (0);
            //			if (part.ContentType.ToLower()  == "text/plain")
            //				body = part.GetEntireBody();
            //
            //			if (part.ContentType.ToLower() == "text/html")
            //			{
            //				bodyHTML = part.GetBodyDecoded();
            //			}
            #endregion
            for (int i = 0; i < component.NumParts; i++)
            {
                Mime part = new Mime();
                part.UnlockComponent(ChilkatKeys.SMime);
                part = component.GetPart(i);
                part.Encoding = "codifica";

                string prova = part.GetBodyEncoded();

                if (part.ContentType.StartsWith("multipart"))
                    getBody(part, ref body, ref bodyHTML, codifica);
                else
                {
                    if (part.ContentType.ToLower() == "text/plain" &&
                        (part.Filename == null || part.Filename == ""))
                        body += part.GetEntireBody();

                    if (part.ContentType.ToLower() == "text/html" &&
                        (part.Filename == null || part.Filename == ""))
                    {
                        bodyHTML += part.GetBodyDecoded();
                    }
                }
            }
        }
        #region ExtractMail Della Chillkat

        private CMMsg extractMail(Email msgComponent)
        {
            // declarations
            string sTemp;
            //string formato = "ddd\\, dd MMM yyyy HH:mm:ss zzz";
            System.Globalization.CultureInfo MyCultureInfo = new System.Globalization.CultureInfo("en-US");
            CMMsg msg = new CMMsg();
            try
            {
                if (msgComponent != null)
                {
                    sTemp = ExtractEmail(msgComponent, msg);
                    //se la mail è una ricevuta di ritorno non eseguo il forward di una mail
                    //Attachments Message - Forward di una email alla casella istituzionale 
                    if (!(msg.isPECDelivered() || msg.isFromNonPEC()) &&
                        !BusinessLogic.interoperabilita.InteroperabilitaManager.isRicevutaPec(msg))
                    {
                        if (msgComponent.NumAttachedMessages != 0)
                        {
                            Chilkat.Email mailAtt = msgComponent.GetAttachedMessage(0);
                            msg.attachments.Clear();
                            //int temp = mailAtt.NumAttachments;
                            // if (temp != 0)
                            //   {
                            for (int j = 0; j < mailAtt.NumAttachments; j++)    //NumAttachments
                            {
                                string a_filename = mailAtt.GetAttachmentFilename(j);
                                string a_conttype = mailAtt.GetAttachmentContentType(j);
                                byte[] a_content = mailAtt.GetAttachmentData(j);
                                CMAttachment att = new CMAttachment(a_filename, a_conttype, a_content);
                                msg.attachments.Add(att);
                            }
                            msg.headers.Add("utenteDocspa", msg.from);
                            msg.from = mailAtt.FromAddress;
                            msg.subject = mailAtt.Subject;
                            // Mime mime_msg_1 = mailAtt.GetMimeObject();

                            Chilkat.Mime mime_msg_1 = new Mime();
                            mime_msg_1.LoadMime(mailAtt.GetMime());

                            mime_msg_1.UnlockComponent(ChilkatKeys.SMime);
                            //   }
                            if (mime_msg_1.ContentType.ToLower().StartsWith("multipart"))
                                getBody(mime_msg_1, ref msg.body, ref msg.HTMLBody, codifica);
                            else
                            {
                                if ((mailAtt.Body != "") && (mailAtt.Body != null))
                                    msg.body = mailAtt.Body;
                            }
                        }
                    }
                    // 	Recipients (TO)
                    for (int i = 0; i < msgComponent.NumTo; i++)
                    {
                        CMRecipient rcp = new CMRecipient();
                        rcp.mail = msgComponent.GetToAddr(i);
                        rcp.name = msgComponent.GetTo(i);
                        msg.recipients.Add(rcp);
                    }

                    return msg;
                }
                else
                    return null;
            }
            catch (Exception exc)
            {
                throw new Exception(String.Format("Errore durante l'estrazione della mail. {1} ", exc.Message));
            }
        }
        private bool extractMailPec(Email msgComponent)
        {
            // declarations
            string sTemp;
            //string formato = "ddd\\, dd MMM yyyy HH:mm:ss zzz";
            System.Globalization.CultureInfo MyCultureInfo = new System.Globalization.CultureInfo("en-US");
            CMMsg msg = new CMMsg();

            try
            {
                if (msgComponent != null)
                {
                    sTemp = ExtractEmail(msgComponent, msg);
                    //se la mail è una ricevuta di ritorno non eseguo il forward di una mail
                    //Attachments Message - Forward di una email alla casella istituzionale 
                    return msg.isPECDelivered();
                }
                else
                    return false;
            }
            catch (Exception exc)
            {
                throw new Exception(String.Format("Errore durante l'estrazione della mail. {1} ", exc.Message));
            }
        }


        private string ExtractEmail(Email msgComponent, CMMsg msg)
        {
            string retval = null;
            // mail info

            //Mime mime_msg = msgComponent.GetMimeObject();

            Chilkat.Mime mime_msg = new Mime();
            mime_msg.LoadMime(msgComponent.GetMime());

            mime_msg.UnlockComponent(ChilkatKeys.SMime);
            string codifica = msgComponent.Charset;
            if (mime_msg.ContentType.ToLower().StartsWith("multipart"))
                getBody(mime_msg, ref msg.body, ref msg.HTMLBody, codifica);
            else
            {
                if ((msgComponent.Body != "") && (msgComponent.Body != null))
                    msg.body = msgComponent.Body;
            }
            msg.subject = msgComponent.Subject;

            try
            {
                msg.date = msgComponent.LocalDate;
            }
            catch
            { }

            msg.from = msgComponent.FromAddress;

            // BCC
            for (int i = 0; i < msgComponent.NumBcc; i++)
                msg.hideRecipients += (msgComponent.GetBccAddr(i) + ";");

            // Headers
            msg.headers.Add("message-id", msgComponent.Uidl);

            for (int i = 0; i < msgComponent.NumHeaderFields; i++)
            {
                string hname = msgComponent.GetHeaderFieldName(i);
                string hval = msgComponent.GetHeaderFieldValue(i);
                if (msg.headers[hname] != null)
                {
                    retval = msg.headers[hname] + " ";
                    msg.headers.Remove(hname);
                }
                else
                    retval = "";

                msg.headers.Add(hname, retval + hval);
            }


            // Attachments
            for (int i = 0; i < msgComponent.NumAttachments; i++)
            {
                string a_filename = msgComponent.GetAttachmentFilename(i);
                string a_conttype = msgComponent.GetAttachmentContentType(i);
                byte[] a_content = msgComponent.GetAttachmentData(i);
                if (a_conttype != "application/ms-tnef")
                {
                    CMAttachment att = new CMAttachment(a_filename,
                        a_conttype, a_content);

                    msg.attachments.Add(att);
                }
            }
            return retval;
        }

        #endregion


        #endregion

        #endregion

        public string getBodyFromMail(string email)
        {
            string retval = null;
            Email em = new Email();
            if (em.SetFromMimeText(email))
            {
                retval = em.GetHtmlBody();
                if ((retval == null) || (retval == String.Empty))
                    retval = em.GetPlainTextBody();
            }

            return retval;
        }
    }

}

