using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NttDataWA.UIManager;
using NttDataWA.DocsPaWR;
using NttDataWA.Utils;

namespace NttDataWA.Popup
{
    public partial class StartProcessSignature : System.Web.UI.Page
    {
        #region Properties

        private List<ProcessoFirma> ListaProcessiDiFirma
        {
            get
            {
                if (HttpContext.Current.Session["ListaProcessiDiFirma"] != null)
                    return (List<ProcessoFirma>)HttpContext.Current.Session["ListaProcessiDiFirma"];
                else
                    return null;
            }
            set
            {
                HttpContext.Current.Session["ListaProcessiDiFirma"] = value;
            }
        }


        protected Dictionary<String, String> ListCheck
        {
            get
            {
                Dictionary<String, String> result = null;
                if (HttpContext.Current.Session["listCheck"] != null)
                {
                    result = HttpContext.Current.Session["listCheck"] as Dictionary<String, String>;
                }
                return result;
            }
            set
            {
                HttpContext.Current.Session["listCheck"] = value;
            }
        }
        #endregion

        #region Standard method
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!this.IsPostBack)
            {
                this.InitializeLanguage();
                if (!string.IsNullOrEmpty(Request.QueryString["from"]) && Request.QueryString["from"].Equals("SearchDocument"))
                    this.InitializeList();
                this.InitializePage();
            }
            RefreshScript();
        }

        private void InitializeLanguage()
        {
            string language = UserManager.GetUserLanguage();
            this.StartProcessSignatureAssigns.Text = Utils.Languages.GetLabelFromCode("StartProcessSignatureAssigns", language);
            this.StartProcessSignatureClose.Text = Utils.Languages.GetLabelFromCode("StartProcessSignatureClose", language);
            this.lblStartProcessSignature.Text = Utils.Languages.GetLabelFromCode("lblStartProcessSignature", language);
            this.ltlNote.Text = Utils.Languages.GetLabelFromCode("StartProcessSignatureLtlNote", language);
            this.ltrNotes.Text = Utils.Languages.GetLabelFromCode("DocumentLitObjectChAv", language);
            this.ltlNotificationOption.Text = Utils.Languages.GetLabelFromCode("StartProcessSignatureltlNotificationOption", language);
            this.cbxNotificationOptionOptCP.Text = Utils.Languages.GetLabelFromCode("StartProcessSignaturecbxNotificationOptionOptCP", language);
            this.cbxNotificationOptionOptIP.Text = Utils.Languages.GetLabelFromCode("StartProcessSignaturecbxNotificationOptionOptIP", language);
            this.lblNoVisibleSignatureProcess.Text = Utils.Languages.GetLabelFromCode("StartProcessSignatureLblNoVisibleSignatureProcess", language);
            this.BtnReport.Text = Utils.Languages.GetLabelFromCode("MassiveAddAdlUserBtnReport", language);
        }

        public void InitializeList()
        {
            Dictionary<String, MassiveOperationTarget> temp = new Dictionary<string, MassiveOperationTarget>();

            // Inizializzazione della mappa con i system id degli oggetti e lo stato
            // di checking (in fase di inizializzazione tutti gli item sono deselezionati)
            foreach (KeyValuePair<string, string> item in this.ListCheck)
                if (!temp.Keys.Contains(item.Key))
                    temp.Add(item.Key, new MassiveOperationTarget(item.Key, item.Value));

            // Salvataggio del dizionario
            MassiveOperationUtils.ItemsStatus = temp;
        }


        private void InitializePage()
        {
            ClearSession();
            LoadProcessesSignature();
            TreeviewProcesses_Bind();
            EnabledButton();
        }

        private void ClearSession()
        {
            HttpContext.Current.Session.Remove("ListaProcessiDiFirma");
        }

        private void RefreshScript()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "refreshSelect", "refreshSelect();", true);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "refreshTipsy", "tooltipTipsy();", true);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "refreshPicker", "DatePicker('" + UIManager.UserManager.GetLanguageData() + "');", true);

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "refreshprojectTxtDescrizione", "charsLeft('txtNotes', '2000' , '" + this.ltrNotes.Text.Replace("'", "\'") + "');", true);
            this.txtNotes_chars.Attributes["rel"] = "txtNotes_'2000'_" + this.ltrNotes.Text;
        }

        /// <summary>
        /// Carico la lista dei processi di cui il ruolo ha visibilità
        /// </summary>
        private void LoadProcessesSignature()
        {
            try
            {
                this.ListaProcessiDiFirma = UIManager.SignatureProcessesManager.GetProcessesSignatureVisibleRole();
                if (ListaProcessiDiFirma == null || ListaProcessiDiFirma.Count == 0)
                {
                    this.plcNoSignatureProcess.Visible = true;
                    this.StartProcessSignatureAssigns.Visible = false;
                    this.plcSignatureProcesses.Visible = false;
                }
                else
                {
                    this.plcNoSignatureProcess.Visible = false;
                    this.StartProcessSignatureAssigns.Visible = true;
                    this.plcSignatureProcesses.Visible = true;
                }
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return;
            }
        }

        private void EnabledButton()
        {
            this.StartProcessSignatureAssigns.Enabled = this.TreeProcessSignature.SelectedNode != null;
            this.UpPnlButtons.Update();
        }

        /// <summary>
        /// Funzione per il caricamento delle informazioni sui documenti
        /// </summary>
        /// <param name="selectedItemSystemIdList">La lista dei system id degli elementi selezionati</param>
        /// <returns>La lista degli id dei documenti selezionati</returns>
        private List<SchedaDocumento> LoadSchedaDocumentsList(List<MassiveOperationTarget> selectedItemSystemIdList)
        {
            // La lista da restituire
            List<SchedaDocumento> toReturn = new List<SchedaDocumento>();

            if (selectedItemSystemIdList != null && selectedItemSystemIdList.Count > 0)
            {
                List<string> idDocumentList = (from temp in selectedItemSystemIdList select temp.Id).ToList<string>();
                toReturn = DocumentManager.GetSchedaDocuments(idDocumentList, this);
            }

            // Restituzione della lista di info documento
            return toReturn;

        }



        #endregion


        #region TreeView

        protected void TreeSignatureProcess_SelectedNodeChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "reallowOp", "reallowOp();", true);
            EnabledButton();
        }

        protected void TreeSignatureProcess_Collapsed(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "reallowOp", "reallowOp();", true);
        }

        private void TreeviewProcesses_Bind()
        {
            if (ListaProcessiDiFirma != null && ListaProcessiDiFirma.Count > 0)
            {
                foreach (ProcessoFirma p in ListaProcessiDiFirma)
                {
                    this.AddNode(p);
                }
                this.TreeProcessSignature.DataBind();
                this.TreeProcessSignature.CollapseAll();
            }

        }

        private TreeNode AddNode(ProcessoFirma p)
        {
            TreeNode root = new TreeNode();
            if (p.isInvalidated)
            {
                root.SelectAction = TreeNodeSelectAction.None;
                root.Text = p.isInvalidated ? "<strike>" + p.nome + "</strike>" : p.nome;
            }
            else
            {
                root.Text = p.nome;
            }
            root.Value = p.idProcesso;
            root.ToolTip = p.nome;
            foreach (PassoFirma passo in p.passi)
            {
                this.AddChildrenElements(passo, ref root);
            }

            this.TreeProcessSignature.Nodes.Add(root);
            return root;
        }

        private TreeNode AddChildrenElements(PassoFirma p, ref TreeNode root)
        {
            TreeNode nodeChild = new TreeNode();

            nodeChild.ImageUrl = LibroFirmaManager.GetIconEventType(p);
            nodeChild.Value = p.idPasso;
            nodeChild.Text = LibroFirmaManager.GetHolder(p);
            nodeChild.ToolTip = LibroFirmaManager.GetHolder(p);
            nodeChild.SelectAction = TreeNodeSelectAction.None;
            root.ChildNodes.Add(nodeChild);

            return nodeChild;
        }

        #endregion

        #region Event button

        protected void StartProcessSignatureClose_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "function", "reallowOp();", true);
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["from"]) && Request.QueryString["from"].Equals("SearchDocument"))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPnlButtons, this.UpPnlButtons.GetType(), "closeAJM", "parent.closeAjaxModal('StartProcessSignature','up');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPnlButtons, this.UpPnlButtons.GetType(), "closeAJM", "parent.closeAjaxModal('StartProcessSignature','');", true);
                }
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return;
            }
        }

        protected void StartProcessSignatureAssigns_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "function", "reallowOp();", true);
            try
            {
                TreeNode node = this.TreeProcessSignature.SelectedNode;

                if (node != null)
                {
                    ProcessoFirma selectedProcess = (from processo in this.ListaProcessiDiFirma where processo.idProcesso.Equals(node.Value) select processo).FirstOrDefault();

                    if (!string.IsNullOrEmpty(Utils.InitConfigurationKeys.GetValue("0", Utils.DBKeys.FE_TIPO_RUOLO_LIBRO_FIRMA.ToString())) && Utils.InitConfigurationKeys.GetValue("0", Utils.DBKeys.FE_TIPO_RUOLO_LIBRO_FIRMA.ToString()) != "0")
                    {
                        //Verifico se nel processo sono presenti passi a Tipo ruolo, in caso positivo verifico se esiste un ruolo con il tipo ruolo specificato e rispetto
                        //al ruolo che avvia. Se esiste un tipo ruolo per cui non è possibile determinare il ruolo, interrompo l'avvio del processo
                        List<TipoRuolo> typeRoleInSteps = (from passo in selectedProcess.passi
                                                           where passo.TpoRuoloCoinvolto != null && !string.IsNullOrEmpty(passo.TpoRuoloCoinvolto.systemId)
                                                           select passo.TpoRuoloCoinvolto).ToList();
                        if (typeRoleInSteps != null && typeRoleInSteps.Count > 0)
                        {
                            List<TipoRuolo> listTypeRoleNoMatchRole = SignatureProcessesManager.CheckExistsRoleSupByTypeRoles(typeRoleInSteps);
                            if (listTypeRoleNoMatchRole == null)
                            {
                                string msg = "ErrorStartProcessSignature";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');}", true);
                                return;
                            }
                            else if (listTypeRoleNoMatchRole.Count > 0)
                            {
                                string typeRoleNoMatch = string.Empty;
                                foreach (TipoRuolo t in listTypeRoleNoMatchRole)
                                    typeRoleNoMatch += "<li> " + t.descrizione + "</li>";

                                string msgDesc = "WarningStartProcessSignatureTypeRoleNoMatch";
                                string errFormt = typeRoleNoMatch;
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + utils.FormatJs(msgDesc) + "', 'warning', '', '" + utils.FormatJs(errFormt) + "');} else {parent.ajaxDialogModal('" + utils.FormatJs(msgDesc) + "', 'warning', '', '" + utils.FormatJs(errFormt) + "');}; ", true);
                                return;
                            }
                        }
                    }

                    #region AVVIO PROCESSO DI FIRMA MASSIVO
                    if (!string.IsNullOrEmpty(Request.QueryString["from"]) && Request.QueryString["from"].Equals("SearchDocument"))
                    {
                        List<SchedaDocumento> schedaDocumentList = LoadSchedaDocumentsList(MassiveOperationUtils.GetSelectedItems());
                        FileRequest fileReq;
                        if (schedaDocumentList != null && schedaDocumentList.Count > 0)
                        {
                            MassiveOperationReport.MassiveOperationResultEnum result;
                            MassiveOperationReport report = new MassiveOperationReport();
                            string details = string.Empty;
                            string codice = string.Empty;
                            List<FileRequest> fileRequestList = new List<FileRequest>();
                            foreach (SchedaDocumento schedaDoc in schedaDocumentList)
                            {
                                result = MassiveOperationReport.MassiveOperationResultEnum.OK;
                                details = String.Empty;

                                result = CanStartSignatureProcess(schedaDoc, out details);

                                if (result != MassiveOperationReport.MassiveOperationResultEnum.KO)
                                {
                                    fileReq = schedaDoc.documenti[0];
                                    if (schedaDoc.documentoPrincipale != null && !string.IsNullOrEmpty(schedaDoc.documentoPrincipale.docNumber))
                                    {
                                        Allegato att = new Allegato()
                                        {
                                            docNumber = fileReq.docNumber,
                                            versionId = fileReq.versionId,
                                            versionLabel = fileReq.versionLabel,
                                            descrizione = fileReq.descrizione,
                                            version = fileReq.version
                                        };
                                        fileRequestList.Add(att);
                                    }
                                    else
                                        fileRequestList.Add(fileReq);
                                }
                                else
                                {
                                    codice = MassiveOperationUtils.getItem(schedaDoc.docNumber).Codice;
                                    report.AddReportRow(
                                        codice,
                                        result,
                                        details);
                                }
                            }

                            //AVVIO IL PROCESSO DI FIRMA PER I DOC SELEZIONATI
                            if (fileRequestList != null && fileRequestList.Count > 0)
                            {
                                List<FirmaResult> firmaResult = SignatureProcessesManager.StartProccessSignatureMassive(selectedProcess, fileRequestList, this.txtNotes.Text, this.cbxNotificationOptionOptIP.Selected, this.cbxNotificationOptionOptCP.Selected);
                                if (firmaResult != null && ((firmaResult.Count > 1) || (firmaResult.Count == 1 && firmaResult[0].fileRequest != null)))
                                {
                                    foreach (FirmaResult r in firmaResult)
                                    {
                                        if (string.IsNullOrEmpty(r.errore))
                                        {
                                            result = MassiveOperationReport.MassiveOperationResultEnum.OK;
                                            details = "Avvio del processo di firma avvenuto correttamente";
                                            codice = MassiveOperationUtils.getItem(r.fileRequest.docNumber).Codice;
                                            report.AddReportRow(
                                                codice,
                                                result,
                                                details);
                                        }
                                        else
                                        {
                                            result = MassiveOperationReport.MassiveOperationResultEnum.KO;
                                            codice = MassiveOperationUtils.getItem(r.fileRequest.docNumber).Codice;
                                            details = String.Format(
                                                "Si sono verificati degli errori durante l'avvio del processo di firma. Dettagli: {0}",
                                                r.errore);
                                            report.AddReportRow(
                                                codice,
                                                result,
                                                details);
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (FileRequest fr in fileRequestList)
                                    {
                                        result = MassiveOperationReport.MassiveOperationResultEnum.KO;
                                        codice = MassiveOperationUtils.getItem(fr.docNumber).Codice;
                                        details = "Si sono verificati degli errori durante l'avvio del processo di firma.";
                                        report.AddReportRow(
                                            codice,
                                            result,
                                            details);
                                    }
                                }
                            }

                            // Introduzione della riga di summary
                            string[] pars = new string[] { "" + report.Worked, "" + report.NotWorked };
                            report.AddSummaryRow("Documenti lavorati: {0} - Documenti non lavorati: {1}", pars);

                            this.generateReport(report, "Avvio del processo massivo");
                        }
                    }
                    #endregion
                    #region AVVIO PROCESSO DI FIRMA DAL TAB PROFILO
                    else
                    {
                        DocsPaWR.FileRequest fileReq = null;

                        if (FileManager.GetSelectedAttachment() == null)
                        {
                            fileReq = UIManager.FileManager.getSelectedFile();
                            //Se stò avviando il processo dal dettaglio dell'allegato converto il fileRequest in allegato
                            if (DocumentManager.getSelectedRecord().documentoPrincipale != null && !string.IsNullOrEmpty(DocumentManager.getSelectedRecord().documentoPrincipale.docNumber))
                            {
                                fileReq = this.ConvertiFileRequestInAllegato(fileReq);
                            }
                        }
                        else
                        {
                            fileReq = FileManager.GetSelectedAttachment();
                        }
                        DocsPaWR.ResultProcessoFirma resultAvvioProcesso = ResultProcessoFirma.OK;
                        if (SignatureProcessesManager.StartProccessSignature(selectedProcess, fileReq, this.txtNotes.Text, this.cbxNotificationOptionOptIP.Selected, this.cbxNotificationOptionOptCP.Selected, out resultAvvioProcesso))
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpPnlButtons, this.UpPnlButtons.GetType(), "closeAJM", "parent.closeAjaxModal('StartProcessSignature','up');", true);
                        }
                        else
                        {
                            string msg = string.Empty;
                            switch (resultAvvioProcesso)
                            { 
                                case ResultProcessoFirma.DOCUMENTO_GIA_IN_LIBRO_FIRMA:
                                     msg = "ErrorStartProcessSignatureDocInLibroFirma";
                                     ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'warning', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'warning', '');}", true);
                                    break;
                                case ResultProcessoFirma.DOCUMENTO_CONSOLIDATO:
                                    msg = "ErrorStartProcessSignatureDocConsolidato";
                                     ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'warning', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'warning', '');}", true);
                                    break;
                                case ResultProcessoFirma.DOCUMENTO_BLOCCATO:
                                    msg = "ErrorStartProcessSignatureDocCheckout";
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'warning', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'warning', '');}", true);
                                    break;
                                default:
                                    msg = "ErrorStartProcessSignature";
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');}", true);
                                    break;

                            }
                            msg = "ErrorStartProcessSignature";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');}", true);
                            return;
                        }
                    }
                    #endregion
                }
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return;
            }
        }

        private MassiveOperationReport.MassiveOperationResultEnum CanStartSignatureProcess(SchedaDocumento schedaDoc, out string details)
        {
            // Risultato della verifica
            MassiveOperationReport.MassiveOperationResultEnum retValue = MassiveOperationReport.MassiveOperationResultEnum.OK;
            System.Text.StringBuilder detailsBS = new System.Text.StringBuilder();
            bool isPdf = (FileManager.getEstensioneIntoSignedFile(schedaDoc.documenti[0].fileName).ToUpper() == "PDF");

            string msgError = string.Empty;

            #region FILE ACQUISITO
            if (string.IsNullOrEmpty(schedaDoc.documenti[0].fileSize) || Convert.ToInt32(schedaDoc.documenti[0].fileSize) == 0)
            {
                msgError = "File non acquisito.";
                retValue = MassiveOperationReport.MassiveOperationResultEnum.KO;
                details = msgError;
                return retValue;
            }
            #endregion

            #region DOCUMENTO IN LIBRO FIRMA
            if (schedaDoc.documenti[0].inLibroFirma)
            {
                msgError = "File già presente in libro firma.";
                retValue = MassiveOperationReport.MassiveOperationResultEnum.KO;
                details = msgError;
                return retValue;
            }
            #endregion

            #region VERFICA L'ESTENSIONE DEL FILE
            if (!isPdf)
            {
                msgError = "Non è stato possibile avviare il processo di firma. Effettuare la conversione in PDF del file.";
                retValue = MassiveOperationReport.MassiveOperationResultEnum.KO;
                details = msgError;
                return retValue;
            }

            #endregion

            #region DOCUMENTO IN ATTESA DI ACCETTAZIONE

            if (!string.IsNullOrEmpty(schedaDoc.accessRights) && Convert.ToInt32(schedaDoc.accessRights) == Convert.ToInt32(HMdiritti.HDdiritti_Waiting))
            {
                msgError = "Il documento è in attesa di accetazione.";
                retValue = MassiveOperationReport.MassiveOperationResultEnum.KO;
                details = msgError;
                return retValue;
            }

            if (!string.IsNullOrEmpty(schedaDoc.accessRights) && Convert.ToInt32(schedaDoc.accessRights) == Convert.ToInt32(HMdiritti.HMdiritti_Read))
            {
                msgError = "Il documento è in sola lettura.";
                retValue = MassiveOperationReport.MassiveOperationResultEnum.KO;
                details = msgError;
                return retValue;
            }
            #endregion

            #region DOCUMENTO CONSOLIDATO

            if (schedaDoc.ConsolidationState != null && schedaDoc.ConsolidationState.State != DocsPaWR.DocumentConsolidationStateEnum.None)
            {
                msgError = "Processo di firma non avviato in quanto il documento è consolidato.";
                retValue = MassiveOperationReport.MassiveOperationResultEnum.KO;
                details = msgError;
                return retValue;
            }

            #endregion

            #region DOCUMENTO IN CHECKOUT

            if (schedaDoc.checkOutStatus != null && !string.IsNullOrEmpty(schedaDoc.checkOutStatus.ID))
            {
                msgError = "Processo di firma non avviato in quanto il documento è bloccato.";
                retValue = MassiveOperationReport.MassiveOperationResultEnum.KO;
                details = msgError;
                return retValue;
            }

            #endregion


            if (!string.IsNullOrEmpty(msgError))
            {
                retValue = MassiveOperationReport.MassiveOperationResultEnum.KO;
                detailsBS.Append(msgError);
            }

            details = detailsBS.ToString();
            return retValue;

        }

        private void generateReport(MassiveOperationReport report, string titolo)
        {
            this.grdReport.DataSource = report.GetDataSet();
            this.grdReport.DataBind();
            this.pnlReport.Visible = true;
            this.plcSignatureProcesses.Visible = false;
            this.upPnlSignatureProcesses.Update();
            this.upReport.Update();

            string template = "../xml/massiveOp_formatPdfExport.xml";
            report.GenerateDataSetForExport(Server.MapPath(template), titolo);

            this.StartProcessSignatureAssigns.Enabled = false;
            this.BtnReport.Visible = true;
            this.UpPnlButtons.Update();
        }

        protected void BtnReport_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "reallowOp", "reallowOp();", true);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "visualReport", "parent.ajaxModalPopupMassiveReport();", true);
        }

        #endregion

        private FileRequest ConvertiFileRequestInAllegato(FileRequest fileReq)
        {
            FileRequest file = new DocsPaWR.Allegato();

            file.applicazione = fileReq.applicazione;
            file.autore = fileReq.autore;
            file.autoreFile = fileReq.autoreFile;
            file.cartaceo = fileReq.cartaceo;
            file.daAggiornareFirmatari = fileReq.daAggiornareFirmatari;
            file.dataAcquisizione = fileReq.dataAcquisizione;
            file.dataInserimento = fileReq.dataInserimento;
            file.descrizione = fileReq.descrizione;
            file.docNumber = fileReq.docNumber;
            file.docServerLoc = fileReq.docServerLoc;
            file.fileName = fileReq.fileName;
            file.fileSize = fileReq.fileSize;
            file.firmatari = fileReq.firmatari;
            file.firmato = fileReq.firmato;
            file.fNversionId = fileReq.fNversionId;
            file.idPeople = fileReq.idPeople;
            file.idPeopleDelegato = fileReq.idPeopleDelegato;
            file.inLibroFirma = fileReq.inLibroFirma;
            file.msgErr = fileReq.msgErr;
            file.path = fileReq.path;
            file.repositoryContext = fileReq.repositoryContext;
            file.subVersion = fileReq.subVersion;
            file.version = fileReq.version;
            file.versionId = fileReq.versionId;
            file.versionLabel = fileReq.versionLabel;
            
            return file;
        }
    }
}