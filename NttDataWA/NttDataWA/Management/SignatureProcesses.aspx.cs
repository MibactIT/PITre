using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NttDataWA.DocsPaWR;
using NttDataWA.UIManager;
using NttDatalLibrary;
using System.Data;

namespace NttDataWA.Management
{
    public partial class SignatureProcesses : System.Web.UI.Page
    {
        #region Property

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

        private ProcessoFirma ProcessoDiFirmaSelected
        {
            get
            {
                if (HttpContext.Current.Session["ProcessoDiFirmaSelected"] != null)
                    return (ProcessoFirma)HttpContext.Current.Session["ProcessoDiFirmaSelected"];
                else
                    return null;
            }
            set
            {
                HttpContext.Current.Session["ProcessoDiFirmaSelected"] = value;
            }
        }

        private PassoFirma PassoDiFirmaSelected
        {
            get
            {
                if (HttpContext.Current.Session["PassoDiFirmaSelected"] != null)
                    return (PassoFirma)HttpContext.Current.Session["PassoDiFirmaSelected"];
                else
                    return null;
            }
            set
            {
                HttpContext.Current.Session["PassoDiFirmaSelected"] = value;
            }
        }

        private RubricaCallType CallType
        {
            get
            {
                if (HttpContext.Current.Session["callType"] != null)
                    return (RubricaCallType)HttpContext.Current.Session["callType"];
                else return RubricaCallType.CALLTYPE_PROTO_INT_DEST;
            }
            set
            {
                HttpContext.Current.Session["callType"] = value;
            }
        }

        private string IdProcesso
        {
            set
            {
                HttpContext.Current.Session["IdProcesso"] = value;
            }
        }

        #endregion

        #region Constants
        private const string CLOSE_POPUP_ADDRESS_BOOK = "closePopupAddressBook";
        private const string ELETTRONICA = "SIGN_E";
        private const string DIGITALE = "SIGN_D";
        private const string SIGN = "F";
        private const string WAIT = "W";
        private const string EVENT = "E";
        private const string CHECK_INSERT_IN_LF = "INSERIMENTO_DOCUMENTO_LF";
        private const char ROLE_DISABLED = 'R';
        private const char USER_DISABLED = 'U';
        private const string RUOLO = "R";
        private const string TIPO_RUOLO = "TR";
        #endregion

        #region Standard Method
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitializeLanguage();
                this.InitializaPage();
            }
            else
            {
                ReadRetValueFromPopup();
            }

            RefreshScript();
        }

        private void LoadKeys()
        {
            if (!string.IsNullOrEmpty(Utils.InitConfigurationKeys.GetValue("0", Utils.DBKeys.FE_TIPO_RUOLO_LIBRO_FIRMA.ToString())) && Utils.InitConfigurationKeys.GetValue("0", Utils.DBKeys.FE_TIPO_RUOLO_LIBRO_FIRMA.ToString()) != "0")
            {
                this.PnlRoleOrTypeRole.Visible = true;
                this.LoadDdlTypeRole();
            }
        }

        private void ReadRetValueFromPopup()
        {
            if (!string.IsNullOrEmpty(this.HiddenRemoveSignatureProcess.Value))
            {
                this.RimuoviProcessoDiFirma();
                this.HiddenRemoveSignatureProcess.Value = string.Empty;
                return;
            }
            if (!string.IsNullOrEmpty(this.HiddenEventWithoutWait.Value))
            {
                this.HiddenEventWithoutWait.Value = string.Empty;
                this.AddStep();
                return;
            }
            if (!string.IsNullOrEmpty(this.HiddenRemoveStepSignatureProcess.Value))
            {
                this.RimuoviPassoProcessoDiFirma();
                this.HiddenRemoveStepSignatureProcess.Value = string.Empty;
                return;
            }
            if (this.Request.Form["__EVENTARGUMENT"] != null && (this.Request.Form["__EVENTARGUMENT"].Equals(CLOSE_POPUP_ADDRESS_BOOK)))
            {
                string addressBookCallFrom = HttpContext.Current.Session["AddressBook.from"].ToString();
                switch (addressBookCallFrom)
                {
                    case "VISIBILITY_SIGNATURE_PROCESS":
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "popupObject", "document.getElementById('ifrm_VisibilitySignatureProcess').contentWindow.closeAddressBookPopup();", true);
                        break;
                    case "SIGNATURE_PROCESS":
                        btnAddressBookPostback();
                        break;
                }
                HttpContext.Current.Session["AddressBook.from"] = null;
                return;
            }
        }

        private void InitializeLanguage()
        {
            string language = UIManager.UserManager.GetUserLanguage();
            this.ManagementSignatureProcesses.Text = Utils.Languages.GetLabelFromCode("ManagementSignatureProcesses", language);
            this.linkSignatureProcesses.Text = Utils.Languages.GetLabelFromCode("linkSignatureProcesses", language);
            this.SignatureProcessesBtnNew.Text = Utils.Languages.GetLabelFromCode("SignatureProcessesBtnNew", language);
            this.SignatureProcessesBtnSave.Text = Utils.Languages.GetLabelFromCode("SignatureProcessesBtnSave", language);
            this.SignatureProcessesBtnRemove.Text = Utils.Languages.GetLabelFromCode("SignatureProcessesBtnRemove", language);
            this.lblNameSignatureProcesses.Text = Utils.Languages.GetLabelFromCode("lblNameSignatureProcesses", language);
            this.LitSignatureProcessesRole.Text = Utils.Languages.GetLabelFromCode("LitSignatureProcessesRole", language);
            this.ltlSignatureProcessesTypeSignature.Text = Utils.Languages.GetLabelFromCode("ltlSignatureProcessesTypeSignature", language);
            this.ltlSignatureProcessesTypeSignatureD.Text = Utils.Languages.GetLabelFromCode("ltlSignatureProcessesTypeSignatureD", language);
            this.ltlSignatureProcessesTypeSignatureE.Text = Utils.Languages.GetLabelFromCode("ltlSignatureProcessesTypeSignatureE", language);
            this.ltlOptionNotify.Text = Utils.Languages.GetLabelFromCode("ltlSignatureProcessesltlOptionNotify", language);
            this.ltlNotes.Text = Utils.Languages.GetLabelFromCode("ltlSignatureProcessesltlNotes", language);
            this.ltrNotes.Text = Utils.Languages.GetLabelFromCode("DocumentLitObjectChAv", language);
            this.ltlUtenteCoinvolto.Text = Utils.Languages.GetLabelFromCode("ltlUtenteCoinvolto", language);
            this.ddlUtenteCoinvolto.Attributes.Add("data-placeholder", Utils.Languages.GetLabelFromCode("ddlUtenteCoinvolto", language));
            this.btnAddStep.ToolTip = Utils.Languages.GetLabelFromCode("SignatureProcessesBtnAddStep", language);
            this.ltlNr.Text = Utils.Languages.GetLabelFromCode("SignatureProcessesLtlNr", language);
            this.lblSectionDocument.Text = Utils.Languages.GetLabelFromCode("SignatureProcessesLblSectionDocument", language);
            this.btnAddStep.ToolTip = Utils.Languages.GetLabelFromCode("SignatureProcessesBtnAddStepToolTip", language);
            this.BtnDeleteStep.ToolTip = Utils.Languages.GetLabelFromCode("SignatureProcessesBtnDeleteStepToolTip", language);
            this.SignatureProcessesBtnVisibility.Text = Utils.Languages.GetLabelFromCode("SignatureProcessesBtnVisibility", language);
            this.VisibilitySignatureProcess.Title = Utils.Languages.GetLabelFromCode("SignatureProcessesPopupVisibility", language);
            this.AddressBook.Title = Utils.Languages.GetLabelFromCode("AddFilterAddressBookTitle", language);
            //this.optDigitale.Text = Utils.Languages.GetLabelFromCode("SignatureProcessOptDigitale", language);
            //this.optElettronica.Text = Utils.Languages.GetLabelFromCode("SignatureProcessOptElettronica", language);
            this.SignatureProcessesStatistics.Text = Utils.Languages.GetLabelFromCode("SignatureProcessesStatistics", language);
            this.StatisticsSignatureProcess.Title = Utils.Languages.GetLabelFromCode("SignatureProcessesStatistics", language);
            this.LtlTypeStep.Text = Utils.Languages.GetLabelFromCode("SignatureProcessesTypeStep", language);
            this.optSign.Text = Utils.Languages.GetLabelFromCode("SignatureProcessesOptSign", language);
            this.optWait.Text = Utils.Languages.GetLabelFromCode("SignatureProcessesOptWait", language);
            this.optEvent.Text = Utils.Languages.GetLabelFromCode("SignatureProcessesOptEvent", language);
            this.LtlTypeEvent.Text = Utils.Languages.GetLabelFromCode("SignatureProcessesLtlTypeEvent", language);
            this.DdlTypeEvent.Attributes.Add("data-placeholder", Utils.Languages.GetLabelFromCode("SignatureProcessesDdlTypeEvent", language));
            this.MessangerWarning.Text = Utils.Languages.GetLabelFromCode("MessangerWarning", language);
            this.optRole.Text = Utils.Languages.GetLabelFromCode("SignatureProcessOptRole", language);
            this.optTypeRole.Text = Utils.Languages.GetLabelFromCode("SignatureProcessOptTypeRole", language);
            this.DdlTypeRole.Attributes.Add("data-placeholder", Utils.Languages.GetLabelFromCode("SignatureProcessesDdlTypeRole", language));
            this.LtlTypeRole.Text = Utils.Languages.GetLabelFromCode("SignatureProcessesLtlTypeRole", language);
        }

        private void InitializaPage()
        {
            try
            {
                ClearSession();
                LoadProcessiDiFirma();
                TreeviewProcesses_Bind();
                this.LoadEventNotification();
                this.LoadEventTypes();
                this.LoadKeys();
                this.Bind_RblSignature();
                this.UpdateContentPage();
                string dataUser = RoleManager.GetRoleInSession().systemId;

                Registro reg = RegistryManager.GetRegistryInSession();
                if (reg == null)
                {
                    reg = RoleManager.GetRoleInSession().registri[0];
                }
                dataUser = dataUser + "-" + reg.systemId;
                this.RapidRole.ContextKey = dataUser + "-" + UIManager.UserManager.GetUserInSession().idAmministrazione + "-" + "CALLTYPE_IN_ONLY_ROLE";
            }
            catch (Exception ex)
            {
                string msg = "ErrorSignatureProcess";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');}", true);
                return;
            }
        }

        private void RefreshScript()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "refreshSelect", "refreshSelect();", true);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "refreshTipsy", "tooltipTipsy();", true);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "refreshPicker", "DatePicker('" + UIManager.UserManager.GetLanguageData() + "');", true);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "OnlyNumbers", "OnlyNumbers();", true);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "refreshprojectTxtDescrizione", "charsLeft('txtNotes', '2000' , '" + this.ltrNotes.Text.Replace("'", "\'") + "');", true);
            this.txtNotes_chars.Attributes["rel"] = "txtNotes_'2000'_" + this.ltrNotes.Text;
        }


        private void ClearSession()
        {
            HttpContext.Current.Session.Remove("ProcessoDiFirmaSelected");
            HttpContext.Current.Session.Remove("PassoDiFirmaSelected");
            HttpContext.Current.Session.Remove("ListaProcessiDiFirma");
        }
        #endregion

        #region Event Buttons

        protected void SignatureProcessesBtnNew_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearFieldsStep();
                this.ClearFieldsProcess();

                this.ProcessoDiFirmaSelected = new ProcessoFirma();
                if (this.TreeSignatureProcess.SelectedNode != null)
                {
                    this.TreeSignatureProcess.SelectedNode.Selected = false;
                }
                this.UpdateContentPage();
                this.upPnlTreeSignatureProcess.Update();
            }
            catch (Exception ex)
            {
                string msg = "ErrorSignatureProcess";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');}", true);
                return;
            }
        }

        protected void SignatureProcessesBtnSave_Click(object sender, EventArgs e)
        {
            string msg = string.Empty;
            if (string.IsNullOrEmpty(this.txtNameSignatureProcesses.Text))
            {
                msg = "WarningRequiredFieldNameProcess";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'warning', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'warning', '');}", true);
                return;
            }

            this.ProcessoDiFirmaSelected.nome = this.txtNameSignatureProcesses.Text;
            try
            {
                //Creazione di un nuovo processo di firma
                if (string.IsNullOrEmpty(this.ProcessoDiFirmaSelected.idProcesso))
                {
                    this.ProcessoDiFirmaSelected = SignatureProcessesManager.InsertProcessoDiFirma(this.ProcessoDiFirmaSelected);

                    if (this.ProcessoDiFirmaSelected != null)
                    {
                        this.CalcolaProssimoPasso();
                        this.ListaProcessiDiFirma.Add(ProcessoDiFirmaSelected);
                        this.AddNode(ProcessoDiFirmaSelected).Select();
                        this.TreeSignatureProcess.SelectedNode.Expand();
                        this.UpdateContentPage();
                    }
                    else
                    {
                        msg = "ErrorCreationProcess";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');}", true);
                        return;
                    }

                }
                else //salvataggio processo di firma già esistente
                {
                    ProcessoFirma processoAggiornato = UIManager.SignatureProcessesManager.AggiornaProcessoDiFirma(ProcessoDiFirmaSelected);
                    if (processoAggiornato != null)
                    {
                        //Aggiorno il processo in sessione
                        ProcessoFirma processo = (from pr in this.ListaProcessiDiFirma where pr.idProcesso.Equals(this.ProcessoDiFirmaSelected.idProcesso) select pr).FirstOrDefault();
                        processo = processoAggiornato;

                        //Aggiorno il Treeview
                        TreeNode nodoParent = this.TreeSignatureProcess.SelectedNode;
                        nodoParent.Text = processoAggiornato.nome;
                        nodoParent.Select();

                        /*
                        msg = "ConfirmProcessChange";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'check', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'check', '');}", true);
                        return;
                         * */
                    }
                    else
                    {
                        msg = "ErrorProcessChange";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');}", true);
                        return;
                    }
                }
                this.UpdateContentPage();
                this.upPnlTreeSignatureProcess.Update();
            }
            catch (Exception ex)
            {
                msg = "ErrorProcessChange";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');}", true);
                return;
            }
        }

        protected void SignatureProcessesBtnRemove_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxConfirmModal", "parent.fra_main.ajaxConfirmModal('ConfirmRemoveSignatureProcess', 'HiddenRemoveSignatureProcess', '','" + this.ProcessoDiFirmaSelected.nome + "');", true);
            return;
        }

        protected void SignatureProcessesStatistics_Click(object sender, EventArgs e)
        {
            try
            {
                this.IdProcesso = this.ProcessoDiFirmaSelected.idProcesso;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxModalPopupStatisticsSignatureProcess", "ajaxModalPopupStatisticsSignatureProcess();", true);
                return;
            }
            catch (Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return;
            }
        }

        protected void RblTypeStep_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.cbxOptionNotify.Items.FindByValue(CHECK_INSERT_IN_LF).Enabled = true;
                if (this.RblTypeStep.SelectedValue.Equals(SIGN))
                {
                    this.PnlTypeStep.Visible = true;
                    this.pnlSign.Visible = true;
                    this.rblTypeSignature.SelectedIndex = 0;
                    this.RblTypeSignature_SelectedIndexChanged(null, null);
                    this.PnlTypeEvent.Visible = false;
                }

                if (this.RblTypeStep.SelectedValue.Equals(WAIT))
                {
                    this.PnlTypeStep.Visible = false;
                    this.TxtCodeRole.Text = string.Empty;
                    this.TxtDescriptionRole.Text = string.Empty;
                    this.ddlUtenteCoinvolto.Items.Clear();
                    this.ddlUtenteCoinvolto.Enabled = false;
                    this.DdlTypeEvent.SelectedIndex = -1;
                    this.txtNotes.Text = string.Empty;
                    foreach (ListItem item in this.cbxOptionNotify.Items)
                    {
                        item.Selected = false;
                    }
                }

                if (this.RblTypeStep.SelectedValue.Equals(EVENT))
                {
                    this.PnlTypeStep.Visible = true;
                    this.PnlTypeEvent.Visible = true;
                    this.pnlSign.Visible = false;

                    //Disabilito il check di notifica per inserimento in libro firma
                    this.cbxOptionNotify.Items.FindByValue(CHECK_INSERT_IN_LF).Selected = false;
                    this.cbxOptionNotify.Items.FindByValue(CHECK_INSERT_IN_LF).Enabled = false;
                }

                this.UpTypeStep.Update();
            }
            catch (Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return;
            }
        }

        protected void RblTypeSignature_SelectedIndexChanged(object sender, EventArgs e)
        {
            setVisibilityTypeEvent();
            if (!string.IsNullOrEmpty(this.TxtCodeRole.Text))
            {
                if (!IsRoleEnabledSignature(this.PassoDiFirmaSelected.ruoloCoinvolto))
                {
                    string msg = "WarningRoleNotEnabledSign";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) { parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'warning', '');} else { parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'warning', '');}", true);
                }
            }
        }


        protected void RblRoleOrTypeRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.RblRoleOrTypeRole.SelectedValue.Equals(RUOLO))
            {
                this.DdlTypeRole.SelectedIndex = -1;
                this.PnlTypeRole.Attributes.Add("style", "display:none");
                this.PnlRole.Attributes.Add("style", "display:block");
                this.PnlUenteCoinvolto.Attributes.Add("style", "display:block");
            }
            else
            {
                this.PnlTypeRole.Attributes.Add("style", "display:block");
                this.PnlRole.Attributes.Add("style", "display:none");
                this.PnlUenteCoinvolto.Attributes.Add("style", "display:none");
                this.TxtCodeRole.Text = string.Empty;
                this.TxtDescriptionRole.Text = string.Empty;
                this.ddlUtenteCoinvolto.Items.Clear();
                this.ddlUtenteCoinvolto.Enabled = false;
            }
            this.UpdPnlRole.Update();
            this.UpdPnlTypeRole.Update();
            this.UpdPnlUtenteCoinvolto.Update();
        }

        protected void BtnAddressBook_Click(object sender, EventArgs e)
        {
            this.CallType = RubricaCallType.CALLTYPE_CORR_INT;
            HttpContext.Current.Session["AddressBook.from"] = "SIGNATURE_PROCESS";
            HttpContext.Current.Session["AddressBook.EnableOnly"] = "R";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxModalPopupAddressBook", "ajaxModalPopupAddressBook();", true);
        }

        protected void btnAddressBookPostback()
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "function", "<script>reallowOp();</script>", false);
                List<NttDataWA.Popup.AddressBook.CorrespondentDetail> atList = (List<NttDataWA.Popup.AddressBook.CorrespondentDetail>)HttpContext.Current.Session["AddressBook.At"];
                if (atList != null && atList.Count > 0)
                {
                    Corrispondente corr = new Corrispondente() { systemId = atList[0].SystemID, codiceRubrica = atList[0].CodiceRubrica, descrizione = atList[0].Descrizione };
                    Ruolo ruolo = RoleManager.GetRuolo(corr.systemId);
                    if (IsRoleEnabledSignature(ruolo))
                    {
                        this.TxtCodeRole.Text = corr.codiceRubrica;
                        this.TxtDescriptionRole.Text = corr.descrizione;
                        this.idRuolo.Value = ruolo.idGruppo;
                        this.PassoDiFirmaSelected.ruoloCoinvolto = ruolo;
                        this.UpdPnlRole.Update();
                        this.LoadDllUtenteCoinvolto(UIManager.UserManager.getUserInRoleByIdGruppo(ruolo.idGruppo));
                    }
                    else
                    {
                        this.TxtCodeRole.Text = string.Empty;
                        this.TxtDescriptionRole.Text = string.Empty;
                        this.idRuolo.Value = string.Empty;
                        this.UpdPnlRole.Update();
                        string msg = "WarningRoleNotEnabledSign";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) { parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'warning', '');} else { parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'warning', '');}", true);
                    }
                }
                HttpContext.Current.Session["AddressBook.At"] = null;
                HttpContext.Current.Session["AddressBook.Cc"] = null;
                HttpContext.Current.Session["AddressBook.type"] = null;
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return;
            }
        }

        /// <summary>
        /// Abilita il panel per la creazione di un nuovo passo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnAddStep_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearFieldsStep();
                this.CalcolaProssimoPasso();
                this.PassoDiFirmaSelected = new PassoFirma();
                this.PassoDiFirmaSelected.Invalidated = '0';
                this.UpdateContentPage();
            }
            catch (Exception ex)
            {
                string msg = "ErrorSignatureProcess";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');}", true);
                return;
            }
        }

        /// <summary>
        /// Effettua la rimozione del passo selezionato
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnDeleteStep_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxConfirmModal", "parent.fra_main.ajaxConfirmModal('ConfirmRemoveStepSignatureProcess', 'HiddenRemoveStepSignatureProcess', '','" + this.PassoDiFirmaSelected.numeroSequenza + "');", true);
            return;
        }

        /// <summary>
        /// Effettua la creazione/modifica del passo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnConfirmAddStep_Click(object sender, EventArgs e)
        {
            //Sto inserendo un nuvo passo
            string msg = string.Empty;
            try
            {
                msg = this.CheckFields();
                if (!string.IsNullOrEmpty(msg))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'warning', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'warning', '');}", true);
                    return;
                }
                else
                {                     
                    //Nel caso di passo di tipo evento, controllo che sia preceduto da un passo di tipo attesa, in caso contrario dò un messaggio di conferma
                    bool existWait = (from i in this.ProcessoDiFirmaSelected.passi
                                      where i.Evento.TipoEvento.Equals(LibroFirmaManager.TypeStep.WAIT)
                                      select i).FirstOrDefault() != null;
                    if (string.IsNullOrEmpty(this.PassoDiFirmaSelected.idPasso) && RblTypeStep.SelectedValue == LibroFirmaManager.TypeStep.EVENT && !existWait)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxConfirmModal", "parent.fra_main.ajaxConfirmModal('ConfirmEventWithoutWaitSignatureProcess', 'HiddenEventWithoutWait', '','" + this.PassoDiFirmaSelected.numeroSequenza + "');", true);
                        return;
                    }
                    this.AddStep();
                }
            }
            catch (Exception ex)
            {
                msg = "ErrorSignatureProcess";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');}", true);
                return;
            }
        }

        private void AddStep()
        {
            string msg = string.Empty;
            int oldNumeroSequenza = this.PassoDiFirmaSelected.numeroSequenza;
            try
            {
                this.PopolaPassoDiFirma();
                if (string.IsNullOrEmpty(this.PassoDiFirmaSelected.idPasso))
                {
                    PassoFirma nuovoPasso = UIManager.SignatureProcessesManager.InserisciPassoDiFirma(this.PassoDiFirmaSelected);
                    if (nuovoPasso != null)
                    {
                        this.PassoDiFirmaSelected = nuovoPasso;

                        //Aggiorno la lista dei passi del processo selezionato
                        ProcessoFirma processo = (from p in this.ListaProcessiDiFirma where p.idProcesso.Equals(this.PassoDiFirmaSelected.idProcesso) select p).FirstOrDefault();
                        IncrementaNumeroSequenzaPassi(ref processo, nuovoPasso);
                        List<PassoFirma> listaPassi = new List<PassoFirma>();
                        if (processo.passi != null && processo.passi.Length > 0)
                        {
                            listaPassi.AddRange(processo.passi.ToList<PassoFirma>());
                        }
                        listaPassi.Add(this.PassoDiFirmaSelected);
                        listaPassi = (from p in listaPassi orderby p.numeroSequenza ascending select p).ToList<PassoFirma>();
                        processo.passi = listaPassi.ToArray();

                        //Aggiorno il nodo padre inserendo il nuovo passo
                        TreeNode parentNode = this.TreeSignatureProcess.FindNode(this.ProcessoDiFirmaSelected.idProcesso);
                        parentNode.ChildNodes.Clear();
                        foreach (PassoFirma p in processo.passi)
                        {
                            this.AddChildrenElements(p, ref parentNode);
                        }
                        parentNode.Select();
                        this.upPnlTreeSignatureProcess.Update();
                        this.PassoDiFirmaSelected = null;
                    }
                    else
                    {
                        msg = "ErrorInsertStep";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');}", true);
                        return;
                    }
                    this.CalcolaProssimoPasso();
                }
                else //Sto aggiornando un passo esistente
                {
                    if (UIManager.SignatureProcessesManager.AggiornaPassoDiFirma(PassoDiFirmaSelected, oldNumeroSequenza))
                    {
                        //Aggiorno il passo in sessione
                        ProcessoFirma processo = (from p in this.ListaProcessiDiFirma where p.idProcesso.Equals(this.PassoDiFirmaSelected.idProcesso) select p).FirstOrDefault();
                        PassoFirma passo = (from p in processo.passi where p.idPasso.Equals(this.PassoDiFirmaSelected.idPasso) select p).FirstOrDefault();
                        passo.numeroSequenza = this.PassoDiFirmaSelected.numeroSequenza;
                        passo.Evento = this.PassoDiFirmaSelected.Evento;
                        passo.note = this.PassoDiFirmaSelected.note;
                        passo.dataScadenza = this.PassoDiFirmaSelected.dataScadenza;
                        passo.idEventiDaNotificare = this.PassoDiFirmaSelected.idEventiDaNotificare;
                        passo.TpoRuoloCoinvolto = this.PassoDiFirmaSelected.TpoRuoloCoinvolto;
                        passo.ruoloCoinvolto = this.PassoDiFirmaSelected.ruoloCoinvolto;
                        passo.utenteCoinvolto = this.PassoDiFirmaSelected.utenteCoinvolto;

                        //Se il passo era invalidato, lo aggiorno a valido e se, non esistono altri passi invalidi, aggiorno anche il processo
                        if (passo.Invalidated == ROLE_DISABLED || passo.Invalidated == USER_DISABLED)
                        {
                            passo.Invalidated = '0';
                            this.PnlWarningRoleUserDisabled.Visible = false;
                            if ((from p in processo.passi where p.Invalidated == ROLE_DISABLED || p.Invalidated == USER_DISABLED select p).FirstOrDefault() == null)
                            {
                                processo.isInvalidated = false;
                                this.UpdateRootElement(processo);
                            }
                        }

                        //Aggiorno il nodo nel treeview
                        this.UpdateChildrenElement(passo);

                        //Se il numero di sequenza è stato cambiato aggiorno il numero di sequenza degli altri passi
                        if (passo.numeroSequenza != oldNumeroSequenza)
                        {
                            AggiornaNumeroSequenzaPassi(ref processo, passo, oldNumeroSequenza);

                            processo.passi = (from p in processo.passi orderby p.numeroSequenza ascending select p).ToArray();

                            //Aggiorno il nodo padre inserendo il nuovo passo
                            TreeNode parentNode = this.TreeSignatureProcess.FindNode(this.ProcessoDiFirmaSelected.idProcesso);
                            parentNode.ChildNodes.Clear();
                            foreach (PassoFirma p in processo.passi)
                            {
                                this.AddChildrenElements(p, ref parentNode);
                            }
                            TreeNode node = (from n in parentNode.ChildNodes.Cast<TreeNode>() where n.Value.Equals(passo.idPasso) select n).FirstOrDefault();
                            node.Select();
                            this.upPnlTreeSignatureProcess.Update();
                        }

                        this.lblSectionDocument.Text = Utils.Languages.GetLabelFromCode("SignatureProcessesLblSectionDocument", UIManager.UserManager.GetUserLanguage()) + passo.numeroSequenza.ToString();
                        this.txtNr.Text = passo.numeroSequenza.ToString();
                        this.UpdPnlStep.Update();

                        msg = "ConfirmStepChange";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'check', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'check', '');}", true);
                    }
                    else
                    {
                        msg = "ErrorStepChange";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');}", true);
                        return;
                    }
                }
                this.UpdateContentPage();
            }
            catch (Exception ex)
            {
                msg = "ErrorSignatureProcess";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');}", true);
                return;
            }

        }

        protected void TxtCode_OnTextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.TxtCodeRole.Text))
                {
                    RubricaCallType calltype = RubricaCallType.CALLTYPE_PROTO_INT_MITT;
                    ElementoRubrica[] listaCorr = null;
                    Corrispondente corr = null;
                    UIManager.RegistryManager.SetRegistryInSession(RoleManager.GetRoleInSession().registri[0]);
                    listaCorr = UIManager.AddressBookManager.getElementiRubricaMultipli(TxtCodeRole.Text, calltype, true);
                    if (listaCorr != null && (listaCorr.Count() == 1))
                    {
                        if (listaCorr.Count() == 1)
                        {
                            corr = UIManager.AddressBookManager.getCorrispondenteRubrica(this.TxtCodeRole.Text, calltype);
                        }
                        if (corr == null)
                        {
                            this.TxtCodeRole.Text = string.Empty;
                            this.TxtDescriptionRole.Text = string.Empty;
                            this.idRuolo.Value = string.Empty;
                            this.UpdPnlRole.Update();
                            string msg = "ErrorTransmissionCorrespondentNotFound";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'warning', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'warning', '');}", true);
                        }
                        if (!corr.tipoCorrispondente.Equals("R"))
                        {
                            this.TxtCodeRole.Text = string.Empty;
                            this.TxtDescriptionRole.Text = string.Empty;
                            this.idRuolo.Value = string.Empty;
                            this.UpdPnlRole.Update();
                            string msg = "WarningCorrespondentAsRole";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) { parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'warning', '');} else { parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'warning', '');}", true);
                        }
                        else
                        {
                            Ruolo ruolo = RoleManager.GetRuolo(corr.systemId);
                            if (IsRoleEnabledSignature(ruolo))
                            {
                                this.TxtCodeRole.Text = corr.codiceRubrica;
                                this.TxtDescriptionRole.Text = corr.descrizione;
                                this.idRuolo.Value = ruolo.idGruppo;
                                this.PassoDiFirmaSelected.ruoloCoinvolto = ruolo;
                                this.UpdPnlRole.Update();
                                this.LoadDllUtenteCoinvolto(UIManager.UserManager.getUserInRoleByIdGruppo(ruolo.idGruppo));
                            }
                            else
                            {
                                this.TxtCodeRole.Text = string.Empty;
                                this.TxtDescriptionRole.Text = string.Empty;
                                this.idRuolo.Value = string.Empty;
                                this.UpdPnlRole.Update();
                                string msg = "WarningRoleNotEnabledSign";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) { parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'warning', '');} else { parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'warning', '');}", true);
                            }
                        }
                    }
                    else
                    {
                        this.TxtCodeRole.Text = string.Empty;
                        this.TxtDescriptionRole.Text = string.Empty;
                        this.idRuolo.Value = string.Empty;
                        this.UpdPnlRole.Update();
                        string msg = "ErrorTransmissionCorrespondentNotFound";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'warning', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'warning', '');}", true);
                    }
                    //else
                    //{
                    //    corr = null;
                    //    this.FoundCorr = listaCorr;
                    //    this.TypeChooseCorrespondent = "Sender";
                    //    this.TypeRecord = "A";
                    //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "chooseCorrespondent", "ajaxModalPopupChooseCorrespondent();", true);
                    //}
                }
                else
                {
                    this.TxtCodeRole.Text = string.Empty;
                    this.TxtDescriptionRole.Text = string.Empty;
                    this.idRuolo.Value = string.Empty;
                    this.LoadDllUtenteCoinvolto(null);
                    this.UpdPnlRole.Update();
                }
            }
            catch (Exception ex)
            {
                string msg = "ErrorSignatureProcess";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');}", true);
                return;
            }
        }

        #endregion

        #region Treeview

        private void TreeviewProcesses_Bind()
        {
            if (ListaProcessiDiFirma != null && ListaProcessiDiFirma.Count > 0)
            {
                foreach (ProcessoFirma p in ListaProcessiDiFirma)
                {
                    this.AddNode(p);
                }
                this.TreeSignatureProcess.DataBind();
            }

        }

        private TreeNode AddNode(ProcessoFirma p)
        {
            TreeNode root = new TreeNode();
            root.Text = p.isInvalidated ? "<strike>" + p.nome + "</strike>" : p.nome;
            root.Value = p.idProcesso;
            root.ToolTip = p.nome;
            foreach (PassoFirma passo in p.passi)
            {
                this.AddChildrenElements(passo, ref root);
            }

            this.TreeSignatureProcess.Nodes.Add(root);
            return root;
        }

        private TreeNode AddChildrenElements(PassoFirma p, ref TreeNode root)
        {
            TreeNode nodeChild = new TreeNode();

            nodeChild.ImageUrl = LibroFirmaManager.GetIconEventType(p);
            nodeChild.Value = p.idPasso;
            nodeChild.Text = p.Invalidated != '0' ? "<strike>" + LibroFirmaManager.GetHolder(p) + "</strike>" : LibroFirmaManager.GetHolder(p);
            nodeChild.ToolTip = LibroFirmaManager.GetHolder(p);
            root.ChildNodes.Add(nodeChild);

            return nodeChild;
        }

        private void UpdateChildrenElement(PassoFirma p)
        {
            TreeNode nodeChild = this.TreeSignatureProcess.SelectedNode;

            nodeChild.ImageUrl = LibroFirmaManager.GetIconEventType(p);
            nodeChild.Value = p.idPasso;
            nodeChild.Text = LibroFirmaManager.GetHolder(p);
            nodeChild.ToolTip = LibroFirmaManager.GetHolder(p);

            this.upPnlTreeSignatureProcess.Update();
        }

        private void UpdateRootElement(ProcessoFirma p)
        {
            TreeNode root = this.TreeSignatureProcess.SelectedNode.Parent;

            root.Text = p.isInvalidated ? "<strike>" + p.nome + "</strike>" : p.nome;
            root.Value = p.idProcesso;
            root.ToolTip = p.nome;

            this.upPnlTreeSignatureProcess.Update();
        }

        protected void TreeSignatureProcess_SelectedNodeChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "reallowOp", "reallowOp();", true);
            try
            {
                TreeNode node = this.TreeSignatureProcess.SelectedNode;

                if (node.Parent == null)
                {
                    this.ProcessoDiFirmaSelected = (from processo in this.ListaProcessiDiFirma where processo.idProcesso.Equals(node.Value) select processo).FirstOrDefault();
                    this.txtNameSignatureProcesses.Text = this.ProcessoDiFirmaSelected.nome;
                    this.PassoDiFirmaSelected = null;
                    this.CalcolaProssimoPasso();
                }
                else
                {
                    TreeNode nodeParent = node.Parent;
                    this.ClearFieldsStep();
                    this.ProcessoDiFirmaSelected = (from processo in this.ListaProcessiDiFirma where processo.idProcesso.Equals(nodeParent.Value) select processo).FirstOrDefault();
                    this.txtNameSignatureProcesses.Text = this.ProcessoDiFirmaSelected.nome;
                    this.PassoDiFirmaSelected = (from p in this.ProcessoDiFirmaSelected.passi where p.idPasso.Equals(node.Value) select p).FirstOrDefault();
                    this.PopolaCampiPasso(PassoDiFirmaSelected);
                }
                this.UpdateContentPage();
            }
            catch (Exception ex)
            {
                string msg = "ErrorSignatureProcess";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');}", true);
                return;
            }
        }

        protected void TreeSignatureProcess_Collapsed(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "reallowOp", "reallowOp();", true);
        }


        #endregion

        #region Utils

        /// <summary>
        /// Visualizzo il panel del tipo firma digitale solo se è selezionato tipo firma digitale
        /// </summary>
        private void setVisibilityTypeEvent()
        {
            if (this.rblTypeSignature.SelectedValue.Equals(DIGITALE))
            {
                this.plcTypeSignatureD.Visible = true;
                this.plcTypeSignatureE.Visible = false;
                this.rblTypeSignatureD.SelectedIndex = 0;
            }
            else
            {
                this.plcTypeSignatureD.Visible = false;
                this.plcTypeSignatureE.Visible = true;
                this.rblTypeSignatureE.SelectedIndex = 0;
            }
            this.UpdPnlTypeSignature.Update();
        }

        /// <summary>
        /// Carica gli eventi legati al libro firma
        /// </summary>
        private void LoadEventNotification()
        {
            List<AnagraficaEventi> listEventTypes = SignatureProcessesManager.GetEventNotification();
            if (listEventTypes != null && listEventTypes.Count > 0)
            {
                List<ListItem> listItem = new List<ListItem>();
                foreach (AnagraficaEventi evento in listEventTypes)
                {
                    listItem.Add(new ListItem()
                        {
                            Text = Utils.Languages.GetLabelFromCode(evento.gruppo, UserManager.GetUserLanguage()),
                            Value = evento.gruppo
                        });
                }

                this.cbxOptionNotify.Items.Clear();
                this.cbxOptionNotify.Items.AddRange(listItem.ToArray());
            }
        }

        private void LoadEventTypes()
        {
            this.DdlTypeEvent.Items.Clear();
            List<AnagraficaEventi> listEventTypes = SignatureProcessesManager.GetEventTypes(EVENT);
            if (listEventTypes != null && listEventTypes.Count > 0)
            {
                ListItem empty = new ListItem("", "");
                this.DdlTypeEvent.Items.Add(empty);
                this.DdlTypeEvent.SelectedIndex = -1;

                List<ListItem> listItem = new List<ListItem>();
                foreach (AnagraficaEventi evento in listEventTypes)
                {
                    listItem.Add(new ListItem()
                    {
                        Text = evento.descrizione,
                        Value = evento.codiceAzione
                    });
                }
                this.DdlTypeEvent.Items.AddRange(listItem.ToArray());
            }
        }



        /// <summary>
        /// Imposta la visibilità dei bottoni
        /// </summary>
        private void SetButtons()
        {
            //I bottoni salva ed elimina sono applicabili solo per il processo, quindi sono abilitati solo se 
            //nel treeview è selezionato un processo
            TreeNode selectedNode = this.TreeSignatureProcess.SelectedNode;
            this.SignatureProcessesBtnNew.Enabled = true;
            if (selectedNode == null)
            {
                this.SignatureProcessesBtnSave.Enabled = false;
                this.SignatureProcessesStatistics.Enabled = false;
                this.SignatureProcessesBtnRemove.Enabled = false;
                this.SignatureProcessesBtnVisibility.Enabled = false;
                if (this.ProcessoDiFirmaSelected != null && string.IsNullOrEmpty(this.ProcessoDiFirmaSelected.idProcesso))
                {
                    this.SignatureProcessesBtnNew.Enabled = false;
                    this.SignatureProcessesBtnSave.Enabled = true;
                    this.SignatureProcessesStatistics.Enabled = true;
                    this.SignatureProcessesBtnVisibility.Enabled = false;
                }
            }
            else if (selectedNode.Parent != null)
            {
                this.SignatureProcessesBtnSave.Enabled = false;
                this.SignatureProcessesStatistics.Enabled = false;
                this.SignatureProcessesBtnRemove.Enabled = false;
                this.SignatureProcessesBtnVisibility.Enabled = false;

                this.BtnConfirmAddStep.ToolTip = Utils.Languages.GetLabelFromCode("SignatureProcessesBtnSaveStepToolTip", UIManager.UserManager.GetUserLanguage());
                this.BtnDeleteStep.Visible = true;
                this.PnlBtnAddStep.Visible = false;
            }
            else
            {
                this.btnAddStep.Enabled = true;
                this.PnlBtnAddStep.Visible = true;
                this.SignatureProcessesBtnSave.Enabled = true;
                this.SignatureProcessesStatistics.Enabled = true;
                this.SignatureProcessesBtnRemove.Enabled = true;
                this.SignatureProcessesBtnVisibility.Enabled = true;

                if (this.PassoDiFirmaSelected != null && string.IsNullOrEmpty(this.PassoDiFirmaSelected.idPasso))
                {
                    this.SignatureProcessesBtnSave.Enabled = false;
                    this.SignatureProcessesStatistics.Enabled = false;
                    this.SignatureProcessesBtnRemove.Enabled = false;
                    this.SignatureProcessesBtnVisibility.Enabled = false;
                    this.btnAddStep.Enabled = false;
                    this.BtnConfirmAddStep.ToolTip = Utils.Languages.GetLabelFromCode("SignatureProcessesBtnConfirmAddStepToolTip", UIManager.UserManager.GetUserLanguage());
                    this.BtnDeleteStep.Visible = false;
                }
                if (this.ProcessoDiFirmaSelected.passi == null || this.ProcessoDiFirmaSelected.passi.Length < 1)
                {
                    this.SignatureProcessesBtnVisibility.Enabled = false;
                }
            }
            this.UplAddStep.Update();
            this.UpPnlButtons.Update();
        }

        private void ClearFieldsStep()
        {
            this.PnlWarningRoleUserDisabled.Visible = false;
            this.txtNr.Text = string.Empty;
            this.TxtCodeRole.Text = string.Empty;
            this.TxtDescriptionRole.Text = string.Empty;
            this.ddlUtenteCoinvolto.Items.Clear();
            this.ddlUtenteCoinvolto.Enabled = false;

            this.RblTypeStep.SelectedValue = SIGN;
            this.RblTypeStep_SelectedIndexChanged(null, null);
            this.DdlTypeEvent.SelectedIndex = -1;

            this.rblTypeSignature.SelectedValue = DIGITALE;
            this.rblTypeSignatureD.SelectedIndex = 0;
            this.plcTypeSignatureD.Visible = true;
            this.plcTypeSignatureE.Visible = false;

            this.RblRoleOrTypeRole.SelectedIndex = 0;

            if (!string.IsNullOrEmpty(Utils.InitConfigurationKeys.GetValue("0", Utils.DBKeys.FE_TIPO_RUOLO_LIBRO_FIRMA.ToString()))
                && Utils.InitConfigurationKeys.GetValue("0", Utils.DBKeys.FE_TIPO_RUOLO_LIBRO_FIRMA.ToString()) != "0")
                this.DdlTypeRole.SelectedIndex = -1;

            this.PnlTypeRole.Attributes.Add("style", "display:none");
            this.PnlRole.Attributes.Add("style", "display:block");
            this.PnlUenteCoinvolto.Attributes.Add("style", "display:block");

            this.txtNotes.Text = string.Empty;

            foreach (ListItem item in this.cbxOptionNotify.Items)
            {
                item.Selected = false;
            }
            this.PassoDiFirmaSelected = null;
            this.UpdPnlDetailsStep.Update();
        }

        private void ClearFieldsProcess()
        {
            this.ProcessoDiFirmaSelected = new ProcessoFirma();

            this.txtNameSignatureProcesses.Text = string.Empty;
            this.UpdPlnProcessName.Update();
        }

        /// <summary>
        /// Aggiorna l'intero contenuto della pagina
        /// </summary>
        private void UpdateContentPage()
        {
            this.pnlStep.Visible = true;
            if (this.TreeSignatureProcess.SelectedNode == null)//Se non c'è alcun nodo selezionato non vedo nulla
            {
                this.pnlStep.Visible = false;
                this.pnlProcessName.Visible = false;
                if (this.ProcessoDiFirmaSelected != null && string.IsNullOrEmpty(this.ProcessoDiFirmaSelected.idProcesso))
                {
                    this.pnlProcessName.Visible = true;
                }
            }
            else if (this.TreeSignatureProcess.SelectedNode.Parent != null) //Se è selezionato il passo non vedo il nome del processo
            {
                this.pnlDetailsSteps.Visible = true;
                this.pnlProcessName.Visible = false;
            }
            else if (this.TreeSignatureProcess.SelectedNode.Parent == null) //Selezionato un processo
            {
                this.pnlStep.Visible = true;
                this.pnlProcessName.Visible = true;

                if (this.PassoDiFirmaSelected != null && string.IsNullOrEmpty(this.PassoDiFirmaSelected.idPasso)) //Ho selezionato un processo e stò creando un nuovo passo
                {
                    this.pnlDetailsSteps.Visible = true;
                }
                else
                {
                    this.pnlDetailsSteps.Visible = false;
                }
            }
            this.SetButtons();
            this.UpdPnlDetailsStep.Update();
            this.UplWarningRoleUserDisabled.Update();
            this.UpdPnlStep.Update();
            this.UplAddStep.Update();
            this.UpdPlnProcessName.Update();
        }

        /// <summary>
        /// Estrazione dei processi di firma creati dall'utente
        /// </summary>
        private void LoadProcessiDiFirma()
        {
            ListaProcessiDiFirma = SignatureProcessesManager.GetProcessiDiFirma();
        }

        private void LoadDdlTypeRole()
        {
            List<DocsPaWR.TipoRuolo> listTypeRole = LibroFirmaManager.GetTypeRole();
            if (listTypeRole != null && listTypeRole.Count > 0)
            {
                ListItem empty = new ListItem("", "");
                this.DdlTypeRole.Items.Add(empty);
                this.DdlTypeRole.SelectedIndex = -1;

                for (int i = 0; i < listTypeRole.Count; i++)
                {
                    ListItem item = new ListItem(listTypeRole[i].descrizione, listTypeRole[i].systemId);
                    this.DdlTypeRole.Items.Add(item);
                }

                this.DdlTypeRole.Enabled = true;
            }
        }

        private void LoadDllUtenteCoinvolto(List<Utente> listUserInRole)
        {
            this.ddlUtenteCoinvolto.Items.Clear();

            if (listUserInRole != null && listUserInRole.Count > 0)
            {
                ListItem empty = new ListItem("", "");
                this.ddlUtenteCoinvolto.Items.Add(empty);
                this.ddlUtenteCoinvolto.SelectedIndex = -1;

                for (int i = 0; i < listUserInRole.Count; i++)
                {
                    ListItem item = new ListItem(listUserInRole[i].descrizione, listUserInRole[i].systemId);
                    this.ddlUtenteCoinvolto.Items.Add(item);
                }

                this.ddlUtenteCoinvolto.Enabled = true;
            }
            else
            {
                this.ddlUtenteCoinvolto.Enabled = false;
            }

            this.UpdPnlUtenteCoinvolto.Update();

        }

        private string CheckFields()
        {
            string errorMessage = string.Empty;
            PassoFirma p;
            bool isAuthorizedLibroFirma = true;
            int numPassi = string.IsNullOrEmpty(this.PassoDiFirmaSelected.idPasso) ? this.ProcessoDiFirmaSelected.passi.Length + 1 : this.ProcessoDiFirmaSelected.passi.Length;
            if (Convert.ToInt32(this.txtNr.Text) > numPassi || Convert.ToInt32(this.txtNr.Text) < 1)
            {
                errorMessage = "WarningSequenceNumberNoValid";
                return errorMessage;
            }

            switch (RblTypeStep.SelectedValue)
            {
                case LibroFirmaManager.TypeStep.SIGN:
                    if (this.RblRoleOrTypeRole.SelectedValue.Equals(RUOLO))
                    {
                        if (string.IsNullOrEmpty(this.idRuolo.Value))
                        {
                            errorMessage = "WarningRequiredFieldRole";
                            return errorMessage;
                        }

                        if (!IsRoleEnabledSignature(this.PassoDiFirmaSelected.ruoloCoinvolto))
                        {
                            errorMessage = "WarningRoleNotEnabledSign";
                            return errorMessage;
                        }
                        isAuthorizedLibroFirma = ((from function in this.PassoDiFirmaSelected.ruoloCoinvolto.funzioni
                                                   where function.codice.ToUpper().Equals("DO_LIBRO_FIRMA")
                                                   select function.systemId).Count() > 0);
                        if (!isAuthorizedLibroFirma)
                        {
                            errorMessage = "WarningNotAuthorizedLibroFirma";
                            return errorMessage;
                        }
                    }
                    else if (this.RblRoleOrTypeRole.Equals(TIPO_RUOLO))
                    {
                        errorMessage = "WarningRequiredFieldTypeRole";
                        return errorMessage;
                    }
                    if (string.IsNullOrEmpty(this.PassoDiFirmaSelected.idPasso))
                    {
                        //Un passo di tipo firma non può essere inserito dopo un passo di tipo evento o di tipo attesa
                        bool canInsertStepSign = (from i in this.ProcessoDiFirmaSelected.passi
                                                  where i.numeroSequenza < Convert.ToInt32(this.txtNr.Text) && (i.Evento.TipoEvento.Equals(LibroFirmaManager.TypeStep.EVENT)
                                                  || (i.Evento.TipoEvento.Equals(LibroFirmaManager.TypeStep.WAIT)))
                                                  select i).FirstOrDefault() == null;
                        if (!canInsertStepSign)
                        {
                            errorMessage = "WarningNotStepSignAfterWaitOrEvent";
                            return errorMessage;
                        }

                        //Una passo di firma pades non può essere preceduto da un passo di firma CADES
                        if (rblTypeSignature.SelectedValue.Equals(DIGITALE) && rblTypeSignatureD.SelectedValue.Equals(LibroFirmaManager.TypeEvent.SIGN_PADES))
                        {
                            canInsertStepSign = (from i in this.ProcessoDiFirmaSelected.passi
                                                 where i.numeroSequenza < Convert.ToInt32(this.txtNr.Text) && i.Evento.CodiceAzione.Equals(LibroFirmaManager.TypeEvent.SIGN_CADES)
                                                 select i).FirstOrDefault() == null;
                            if (!canInsertStepSign)
                            {
                                errorMessage = "WarningNotStepSignAfterCades";
                                return errorMessage;
                            }
                        }
                    }
                    else
                    {
                        //Un passo di tipo firma non può essere inserito dopo un passo di tipo evento o di tipo attesa
                        bool canInsertStepSign = (from i in this.ProcessoDiFirmaSelected.passi
                                                  where i.numeroSequenza <= Convert.ToInt32(this.txtNr.Text) && i.numeroSequenza != this.PassoDiFirmaSelected.numeroSequenza
                                                     && (i.Evento.TipoEvento.Equals(LibroFirmaManager.TypeStep.EVENT) || (i.Evento.TipoEvento.Equals(LibroFirmaManager.TypeStep.WAIT)))
                                                  select i).FirstOrDefault() == null;
                        if (!canInsertStepSign)
                        {
                            errorMessage = "WarningNotStepSignAfterWaitOrEvent";
                            return errorMessage;
                        }

                        //Una passo di firma pades non può essere preceduto da un passo di firma CADES
                        if (rblTypeSignature.SelectedValue.Equals(DIGITALE) && rblTypeSignatureD.SelectedValue.Equals(LibroFirmaManager.TypeEvent.SIGN_PADES))
                        {
                            canInsertStepSign = (from i in this.ProcessoDiFirmaSelected.passi
                                                 where i.numeroSequenza <= Convert.ToInt32(this.txtNr.Text) && i.numeroSequenza != this.PassoDiFirmaSelected.numeroSequenza
                                                 && i.Evento.CodiceAzione.Equals(LibroFirmaManager.TypeEvent.SIGN_CADES)
                                                 select i).FirstOrDefault() == null;
                            if (!canInsertStepSign)
                            {
                                errorMessage = "WarningNotStepSignAfterCades";
                                return errorMessage;
                            }
                        }
                    }
                    break;
                case LibroFirmaManager.TypeStep.EVENT:

                    if (this.RblRoleOrTypeRole.SelectedValue.Equals(RUOLO) && string.IsNullOrEmpty(this.idRuolo.Value))
                    {
                        errorMessage = "WarningRequiredFieldRole";
                        return errorMessage;
                    }
                    else if (this.RblRoleOrTypeRole.SelectedValue.Equals(TIPO_RUOLO) && string.IsNullOrEmpty(this.DdlTypeRole.SelectedValue))
                    {
                        errorMessage = "WarningRequiredFieldTypeRole";
                        return errorMessage;
                    }
                    if (DdlTypeEvent.SelectedIndex == -1)
                    {
                        errorMessage = "WarningRequiredEventType";
                        return errorMessage;
                    }
                    //NEL CASO DI TIPO EVENTO, NON ESSENDOCI L'INSERIMENTO IN LF, IL RUOLO PUò NON ESSERE AUTORIZZATO AL LIBRO FIRMA
                    //isAuthorizedLibroFirma = ((from function in this.PassoDiFirmaSelected.ruoloCoinvolto.funzioni
                    //                           where function.codice.ToUpper().Equals("DO_LIBRO_FIRMA")
                    //                           select function.systemId).Count() > 0);
                    //if (!isAuthorizedLibroFirma)
                    //{
                    //    errorMessage = "WarningNotAuthorizedLibroFirma";
                    //    return errorMessage;
                    //}

                    //Un passo di tipo EVENTO non può essere inserito prima di un passo di attesa o di un passo di firma
                    bool canInsertStepEvent = (from i in this.ProcessoDiFirmaSelected.passi
                                               where i.numeroSequenza >= Convert.ToInt32(this.txtNr.Text)
                                                  && (string.IsNullOrEmpty(this.PassoDiFirmaSelected.idPasso) || i.numeroSequenza != this.PassoDiFirmaSelected.numeroSequenza)
                                                  && (i.Evento.TipoEvento.Equals(LibroFirmaManager.TypeStep.SIGN) || (i.Evento.TipoEvento.Equals(LibroFirmaManager.TypeStep.WAIT)))
                                               select i).FirstOrDefault() == null;
                    if (!canInsertStepEvent)
                    {
                        errorMessage = "WarningNotStepEvent";
                        return errorMessage;
                    }

                    break;
                case LibroFirmaManager.TypeStep.WAIT:

                    if (string.IsNullOrEmpty(this.PassoDiFirmaSelected.idPasso))
                    {
                        //Verifico, in caso di inserimento, se è già presente un passo di WAIT
                        bool existStepWait = (from i in this.ProcessoDiFirmaSelected.passi
                                              where i.Evento.TipoEvento.Equals(LibroFirmaManager.TypeStep.WAIT)
                                              select i).FirstOrDefault() != null;
                        if (existStepWait)
                        {
                            errorMessage = "WarningExistsStepWait";
                            return errorMessage;
                        }

                        //Verifico che il passo di WAIT non venga inserito prima di un passo di FIRMA( >=) o dopo un passo di EVENTO(<)
                        bool canInsertStepWait = (from i in this.ProcessoDiFirmaSelected.passi
                                                  where (i.numeroSequenza >= Convert.ToInt32(this.txtNr.Text) && i.Evento.TipoEvento.Equals(LibroFirmaManager.TypeStep.SIGN))
                                                    || (i.numeroSequenza < Convert.ToInt32(this.txtNr.Text) && i.Evento.TipoEvento.Equals(LibroFirmaManager.TypeStep.EVENT))
                                                  select i).FirstOrDefault() == null;
                        if (!canInsertStepWait)
                        {
                            errorMessage = "WarningInsertStepWait";
                            return errorMessage;
                        }
                    }//stò modificando il passo
                    else
                    {
                        //Verifico se è già presente un passo di WAIT
                        bool existStepWait = (from i in this.ProcessoDiFirmaSelected.passi
                                              where i.Evento.TipoEvento.Equals(LibroFirmaManager.TypeStep.WAIT) && i.numeroSequenza != this.PassoDiFirmaSelected.numeroSequenza
                                              select i).FirstOrDefault() != null;
                        if (existStepWait)
                        {
                            errorMessage = "WarningExistsStepWait";
                            return errorMessage;
                        }

                        //Verifico che il passo di WAIT non venga inserito prima di un passo di FIRMA( >=) o dopo un passo di EVENTO(<=)
                        bool canModifyStepWait = (from i in this.ProcessoDiFirmaSelected.passi
                                                  where i.numeroSequenza != this.PassoDiFirmaSelected.numeroSequenza &&
                                                      ((i.numeroSequenza >= Convert.ToInt32(this.txtNr.Text) && i.Evento.TipoEvento.Equals(LibroFirmaManager.TypeStep.SIGN))
                                                    || (i.numeroSequenza <= Convert.ToInt32(this.txtNr.Text) && i.Evento.TipoEvento.Equals(LibroFirmaManager.TypeStep.EVENT)))
                                                  select i).FirstOrDefault() == null;
                        if (!canModifyStepWait)
                        {
                            errorMessage = "WarningInsertStepWait";
                            return errorMessage;
                        }
                    }

                    break;
            }

            return errorMessage;
        }


        private void PopolaPassoDiFirma()
        {
            PassoFirma passo = new PassoFirma();

            passo.idProcesso = ProcessoDiFirmaSelected.idProcesso;
            if (!string.IsNullOrEmpty(this.txtNr.Text))
            {
                passo.numeroSequenza = Convert.ToInt32(this.txtNr.Text);
            }
            else if (ProcessoDiFirmaSelected.passi != null)
            {
                passo.numeroSequenza = this.ProcessoDiFirmaSelected.passi.Length + 1;
            }
            else
            {
                passo.numeroSequenza = 1;
            }

            if (this.PassoDiFirmaSelected != null)
            {
                passo.idPasso = this.PassoDiFirmaSelected.idPasso;
                passo.Invalidated = PassoDiFirmaSelected.Invalidated;
            }


            passo.Evento = new Evento();
            passo.Evento.TipoEvento = this.RblTypeStep.SelectedValue;

            #region TIPO FIRMA o TIPO EVENTO
            if (!RblTypeStep.SelectedValue.Equals(WAIT))
            {
                if (this.RblTypeStep.SelectedValue.Equals(SIGN))
                {
                    passo.Evento.Gruppo = this.rblTypeSignature.SelectedValue;
                    if (rblTypeSignature.SelectedValue.Equals(DIGITALE))
                    {
                        passo.Evento.CodiceAzione = this.rblTypeSignatureD.SelectedValue;
                    }
                    else
                    {
                        passo.Evento.CodiceAzione = this.rblTypeSignatureE.SelectedValue;
                    }
                }
                else
                {
                    passo.Evento.CodiceAzione = this.DdlTypeEvent.SelectedValue;
                    passo.Evento.Descrizione = this.DdlTypeEvent.SelectedItem.Text;
                }
                //Verifico se ho selezionato un ruolo o Tipo ruolo
                if (this.RblRoleOrTypeRole.SelectedValue.Equals(RUOLO))
                {
                    if (this.PassoDiFirmaSelected != null)
                    {
                        passo.ruoloCoinvolto = PassoDiFirmaSelected.ruoloCoinvolto;
                    }

                    passo.utenteCoinvolto = new Utente();
                    passo.utenteCoinvolto.idPeople = this.ddlUtenteCoinvolto.SelectedValue;
                    passo.utenteCoinvolto.descrizione = this.ddlUtenteCoinvolto.SelectedItem.Text;
                }
                else
                {
                    passo.TpoRuoloCoinvolto = new TipoRuolo();
                    passo.TpoRuoloCoinvolto.systemId = this.DdlTypeRole.SelectedValue;
                    passo.TpoRuoloCoinvolto.descrizione = this.DdlTypeRole.SelectedItem.Text;
                }
                passo.note = this.txtNotes.Text;
                passo.idEventiDaNotificare = (from i in this.cbxOptionNotify.Items.Cast<ListItem>() where i.Selected select i.Value).ToArray();
            }
            #endregion

            #region TIPO ATTESA
            if (this.RblTypeStep.SelectedValue.Equals(LibroFirmaManager.TypeStep.WAIT))
            {
                List<AnagraficaEventi> eventWaiting = SignatureProcessesManager.GetEventTypes(WAIT);
                if (eventWaiting != null && eventWaiting.Count > 0)
                {
                    passo.Evento.CodiceAzione = eventWaiting[0].codiceAzione;
                    passo.Evento.Gruppo = eventWaiting[0].gruppo;
                }
            }

            #endregion

            this.PassoDiFirmaSelected = passo;
        }

        private void PopolaCampiPasso(PassoFirma p)
        {
            this.txtNr.Text = p.numeroSequenza.ToString();
            string language = UIManager.UserManager.GetUserLanguage();
            this.lblSectionDocument.Text = Utils.Languages.GetLabelFromCode("SignatureProcessesLblSectionDocument", language) + this.txtNr.Text;
            this.RblTypeStep.SelectedValue = p.Evento.TipoEvento;
            this.RblTypeStep_SelectedIndexChanged(null, null);

            //Controllo se il passo è invalidato. Il passo può essere invalidato se
            // 1.Utente disabilitato o non più nel ruolo
            // 2.Ruolo modificato o disabilitato

            this.PnlWarningRoleUserDisabled.Visible = false;
            if(p.Invalidated == ROLE_DISABLED)
            {
                this.msgTextInvalidated.Text = Utils.Languages.GetLabelFromCode("SignatureProcessesRoleDisabled", UserManager.GetUserLanguage());
                this.PnlWarningRoleUserDisabled.Visible = true;
            }               
            if(p.Invalidated == USER_DISABLED)
            {
                this.msgTextInvalidated.Text = Utils.Languages.GetLabelFromCode("SignatureProcessesUserDisabled", UserManager.GetUserLanguage());
                this.PnlWarningRoleUserDisabled.Visible = true;
            }

            switch (p.Evento.TipoEvento)
            {
                case LibroFirmaManager.TypeStep.SIGN:
                    if (p.Evento.Gruppo.Equals(DIGITALE))
                    {
                        this.rblTypeSignature.SelectedValue = DIGITALE;
                        this.rblTypeSignatureD.SelectedValue = p.Evento.CodiceAzione;
                        this.plcTypeSignatureD.Visible = true;
                        this.plcTypeSignatureE.Visible = false;
                    }
                    else
                    {
                        this.rblTypeSignature.SelectedValue = ELETTRONICA;
                        this.rblTypeSignatureE.SelectedValue = p.Evento.CodiceAzione;
                        this.plcTypeSignatureD.Visible = false;
                        this.plcTypeSignatureE.Visible = true;
                    }
                    FillPanelDetailEvent_Sign(p);
                    break;
                case LibroFirmaManager.TypeStep.EVENT:
                    this.DdlTypeEvent.SelectedValue = p.Evento.CodiceAzione;
                    FillPanelDetailEvent_Sign(p);
                    break;
            }
        }

        private void FillPanelDetailEvent_Sign(PassoFirma p)
        {
            if (p.TpoRuoloCoinvolto != null && !string.IsNullOrEmpty(p.TpoRuoloCoinvolto.systemId))
            {
                this.DdlTypeRole.SelectedValue = p.TpoRuoloCoinvolto.systemId;
                this.RblRoleOrTypeRole.SelectedValue = TIPO_RUOLO;
                this.PnlTypeRole.Attributes.Add("style", "display:block");
                this.PnlRole.Attributes.Add("style", "display:none");
                this.PnlUenteCoinvolto.Attributes.Add("style", "display:none");
            }
            else
            {
                this.RblRoleOrTypeRole.SelectedValue = RUOLO;
                this.PnlRole.Attributes.Add("style", "display:block");
                this.PnlUenteCoinvolto.Attributes.Add("style", "display:block");
                this.PnlTypeRole.Attributes.Add("style", "display:none");
                if (p.Invalidated != USER_DISABLED && p.Invalidated != ROLE_DISABLED)
                {
                    this.TxtCodeRole.Text = p.ruoloCoinvolto.codiceRubrica;
                    this.TxtDescriptionRole.Text = p.ruoloCoinvolto.descrizione;
                    this.idRuolo.Value = p.ruoloCoinvolto.idGruppo;

                    this.LoadDllUtenteCoinvolto(UIManager.UserManager.getUserInRoleByIdGruppo(p.ruoloCoinvolto.idGruppo));
                    this.ddlUtenteCoinvolto.SelectedValue = p.utenteCoinvolto.idPeople;
                }
                else if (p.Invalidated == USER_DISABLED)
                {
                    this.TxtCodeRole.Text = p.ruoloCoinvolto.codiceRubrica;
                    this.TxtDescriptionRole.Text = p.ruoloCoinvolto.descrizione;
                    this.idRuolo.Value = p.ruoloCoinvolto.idGruppo;

                    this.LoadDllUtenteCoinvolto(UIManager.UserManager.getUserInRoleByIdGruppo(p.ruoloCoinvolto.idGruppo));
                }
                else
                {
                    this.TxtCodeRole.Text = string.Empty;
                    this.TxtDescriptionRole.Text = string.Empty;
                    this.idRuolo.Value = string.Empty;
                    this.ddlUtenteCoinvolto.ClearSelection();
                }
            }
            this.txtNotes.Text = p.note;

            foreach (ListItem item in this.cbxOptionNotify.Items)
            {
                item.Selected = (from e in p.idEventiDaNotificare where e.Equals(item.Value) select e).FirstOrDefault() != null;
            }
        }

        private void RimuoviProcessoDiFirma()
        {
            string msg = string.Empty;
            if (UIManager.SignatureProcessesManager.RimuoviProcessoDiFirma(this.ProcessoDiFirmaSelected))
            {
                this.TreeSignatureProcess.Nodes.Remove(this.TreeSignatureProcess.SelectedNode);
                this.upPnlTreeSignatureProcess.Update();
                this.ProcessoDiFirmaSelected = null;
                this.UpdateContentPage();
            }
            else
            {
                msg = "ErrorDeleteSignatureProcess";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');}", true);
                return;
            }
        }

        private void RimuoviPassoProcessoDiFirma()
        {
            string msg = string.Empty;
            if (UIManager.SignatureProcessesManager.RimuoviPassoDiFirma(this.PassoDiFirmaSelected))
            {
                //Rimuovo il passo dall'oggetto in sessione
                ProcessoFirma processo = (from p in this.ListaProcessiDiFirma where p.idProcesso.Equals(this.PassoDiFirmaSelected.idProcesso) select p).FirstOrDefault();
                DecrementaNumeroSequenzaPassi(ref processo, PassoDiFirmaSelected);
                List<PassoFirma> listaPassi = (from p in processo.passi where !p.idPasso.Equals(PassoDiFirmaSelected.idPasso) select p).ToList();
                processo.passi = listaPassi.ToArray();

                //Se il passo era invalidato, lo aggiorno a valido e se, non esistono altri passi invalidi, aggiorno anche il processo
                if (PassoDiFirmaSelected.Invalidated == ROLE_DISABLED || PassoDiFirmaSelected.Invalidated == USER_DISABLED)
                {
                    if ((from p in processo.passi where p.Invalidated == ROLE_DISABLED || p.Invalidated == USER_DISABLED select p).FirstOrDefault() == null)
                    {
                        processo.isInvalidated = false;
                        this.UpdateRootElement(processo);
                    }
                }

                //Rimuovo il nodo dal treeview e aggiorno la numerazione dei passi
                TreeNode nodoParent = this.TreeSignatureProcess.SelectedNode.Parent;
                nodoParent.ChildNodes.Clear();
                foreach (PassoFirma p in processo.passi)
                {
                    this.AddChildrenElements(p, ref nodoParent);
                }
                nodoParent.Select();

                this.upPnlTreeSignatureProcess.Update();

                this.PassoDiFirmaSelected = null;
                this.CalcolaProssimoPasso();
                this.UpdateContentPage();
            }
            else
            {
                msg = "ErrorDeleteStepSignatureProcess";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');}", true);
                return;
            }
        }

        private void DecrementaNumeroSequenzaPassi(ref ProcessoFirma processo, PassoFirma passoRimosso)
        {
            if (passoRimosso.numeroSequenza < processo.passi.Length)
            {
                foreach (PassoFirma p in processo.passi)
                {
                    if (p.numeroSequenza > passoRimosso.numeroSequenza)
                        p.numeroSequenza -= 1;
                }
            }
        }


        private void IncrementaNumeroSequenzaPassi(ref ProcessoFirma processo, PassoFirma nuovoPasso)
        {
            if (nuovoPasso.numeroSequenza <= processo.passi.Length)
            {
                foreach (PassoFirma p in processo.passi)
                {
                    if (p.numeroSequenza >= nuovoPasso.numeroSequenza)
                        p.numeroSequenza += 1;
                }
            }
        }

        private void AggiornaNumeroSequenzaPassi(ref ProcessoFirma processo, PassoFirma passoAggiornato, int oldNumeroSequenza)
        {
            if (passoAggiornato.numeroSequenza > oldNumeroSequenza)
            {
                foreach (PassoFirma p in processo.passi)
                {
                    if (!p.idPasso.Equals(passoAggiornato.idPasso) && p.numeroSequenza > oldNumeroSequenza && p.numeroSequenza <= passoAggiornato.numeroSequenza)
                        p.numeroSequenza -= 1;
                }
            }
            else if (passoAggiornato.numeroSequenza < oldNumeroSequenza)
            {
                foreach (PassoFirma p in processo.passi)
                {
                    if (!p.idPasso.Equals(passoAggiornato.idPasso) && p.numeroSequenza >= passoAggiornato.numeroSequenza && p.numeroSequenza <= oldNumeroSequenza)
                        p.numeroSequenza += 1;
                }
            }
        }


        //private void RblTypeSignature_Bind()
        //{
        //    List<ListItem> listItem = new List<ListItem>();

        //    listItem.Add(new ListItem()
        //    {
        //        Text = Utils.Languages.GetLabelFromCode("rblSignatureProcessesOptDigitale", UIManager.UserManager.GetUserLanguage()),
        //        Value = TipoFirma.DIGITALE_CADES.ToString()
        //    });

        //    listItem.Add(new ListItem()
        //    {
        //        Text = Utils.Languages.GetLabelFromCode("rblSignatureProcessesOptElettronica", UIManager.UserManager.GetUserLanguage()),
        //        Value = TipoFirma.ELETTRONICA.ToString()
        //    });

        //    this.rblTypeSignature.Items.Clear();
        //    this.rblTypeSignature.Items.AddRange(listItem.ToArray());
        //    this.rblTypeSignature.SelectedValue = TipoFirma.DIGITALE_CADES.ToString();
        //}

        /// <summary>
        /// Costruisce i radio button per la firma
        /// </summary>
        private void Bind_RblSignature()
        {
            List<AnagraficaEventi> listSignatureEvent = SignatureProcessesManager.GetEventTypes(SIGN);

            #region TIPO FIRMA
            List<AnagraficaEventi> typeSignature = listSignatureEvent.GroupBy(e => e.gruppo).Select(g => g.First()).OrderBy(i => i.gruppo).ToList();
            if (typeSignature != null && typeSignature.Count > 0)
            {
                List<ListItem> listItem = new List<ListItem>();
                foreach (AnagraficaEventi evento in typeSignature)
                {
                    listItem.Add(new ListItem()
                    {
                        Text = Utils.Languages.GetLabelFromCode(evento.gruppo, UserManager.GetUserLanguage()),
                        Value = evento.gruppo
                    });
                }
                this.rblTypeSignature.Items.Clear();
                this.rblTypeSignature.Items.AddRange(listItem.ToArray());
            }

            #endregion

            #region TIPO FIRMA DIGITALE

            List<AnagraficaEventi> typeDigitalSignature = listSignatureEvent.Where(e => e.gruppo.Equals(DIGITALE)).OrderBy(g => g.idEvento).ToList(); ;
            if (typeDigitalSignature != null && typeDigitalSignature.Count > 0)
            {
                List<ListItem> listItem = new List<ListItem>();
                foreach (AnagraficaEventi evento in typeDigitalSignature)
                {
                    listItem.Add(new ListItem()
                    {
                        Text = evento.descrizione,
                        Value = evento.codiceAzione
                    });
                }
                this.rblTypeSignatureD.Items.Clear();
                this.rblTypeSignatureD.Items.AddRange(listItem.ToArray());
            }

            #endregion

            #region  TIPO FIRMA ELETTRONICA

            List<AnagraficaEventi> typeElectronicsSignature = listSignatureEvent.Where(e => e.gruppo.Equals(ELETTRONICA)).OrderBy(g => g.idEvento).ToList();
            if (typeElectronicsSignature != null && typeElectronicsSignature.Count > 0)
            {
                List<ListItem> listItem = new List<ListItem>();
                foreach (AnagraficaEventi evento in typeElectronicsSignature)
                {
                    listItem.Add(new ListItem()
                    {
                        Text = Utils.Languages.GetLabelFromCode(evento.codiceAzione, UserManager.GetUserLanguage()),
                        Value = evento.codiceAzione
                    });
                }
                this.rblTypeSignatureE.Items.Clear();
                this.rblTypeSignatureE.Items.AddRange(listItem.ToArray());
            }


            #endregion
        }

        private void CalcolaProssimoPasso()
        {
            this.lblSectionDocument.Text = Utils.Languages.GetLabelFromCode("SignatureProcessesLblSectionDocument", UIManager.UserManager.GetUserLanguage()) +
                 (this.ProcessoDiFirmaSelected.passi.Length + 1).ToString();

            this.txtNr.Text = (this.ProcessoDiFirmaSelected.passi.Length + 1).ToString();

            this.pnlStep.Visible = true;
            this.UpdPnlStep.Update();
        }

        /// <summary>
        /// Verifica se il ruolo è abilitato ad effettuare la firma selezionata
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        private bool IsRoleEnabledSignature(Ruolo role)
        {
            bool isAuthorizedSign = true;
            if (this.RblTypeStep.SelectedValue.Equals(LibroFirmaManager.TypeStep.SIGN))
            {
                if (this.rblTypeSignature.SelectedValue.Equals(DIGITALE))
                {
                    isAuthorizedSign = ((from function in role.funzioni
                                         where function.codice.ToUpper().Equals("DO_DOC_FIRMA")
                                         select function.systemId).Count() > 0 ||
                                    (from function in role.funzioni
                                     where function.codice.ToUpper().Equals("FIRMA_HSM")
                                     select function.systemId).Count() > 0);
                }
                if (this.rblTypeSignature.SelectedValue.Equals(ELETTRONICA))
                {
                    if (this.rblTypeSignatureE.SelectedValue.Equals(LibroFirmaManager.TypeEvent.VERIFIED))
                    {
                        isAuthorizedSign = ((from function in role.funzioni
                                             where function.codice.ToUpper().Equals("DO_DOC_FIRMA_ELETTRONICA")
                                             select function.systemId).Count() > 0);
                    }
                    else if (this.rblTypeSignatureE.SelectedValue.Equals(LibroFirmaManager.TypeEvent.ADVANCEMENT_PROCESS))
                    {
                        isAuthorizedSign = ((from function in role.funzioni
                                             where function.codice.ToUpper().Equals("DO_DOC_AVANZAMENTO_ITER")
                                             select function.systemId).Count() > 0);
                    }
                }
            }
            return isAuthorizedSign;
        }
        #endregion
    }
}
