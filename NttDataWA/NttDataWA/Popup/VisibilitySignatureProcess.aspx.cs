using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NttDataWA.DocsPaWR;
using NttDataWA.UIManager;
using NttDatalLibrary;

namespace NttDataWA.Popup
{
    public partial class VisibilitySignatureProcess : System.Web.UI.Page
    {
        private List<Corrispondente> ListaVisibilitaProcesso
        {
            get
            {
                if (HttpContext.Current.Session["ListaVisibilitaProcesso"] != null)
                    return (List<Corrispondente>)HttpContext.Current.Session["ListaVisibilitaProcesso"];
                else
                    return null;
            }
            set
            {
                HttpContext.Current.Session["ListaVisibilitaProcesso"] = value;
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
        }

        public RubricaCallType CallType
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

        private int SelectedPage
        {
            get
            {
                int toReturn = 1;
                if (HttpContext.Current.Session["selectedPageVisibilityProcess"] != null) Int32.TryParse(HttpContext.Current.Session["selectedPageVisibilityProcess"].ToString(), out toReturn);
                if (toReturn < 1) toReturn = 1;

                return toReturn;
            }
            set
            {
                HttpContext.Current.Session["selectedPageVisibilityProcess"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitializeLanguage();
                this.InitializaPage();
            }
            else
            {
                RefreshScript();
            }
        }

        private void InitializeLanguage()
        {
            string language = UIManager.UserManager.GetUserLanguage();
            this.BtnClose.Text = Utils.Languages.GetLabelFromCode("VisibilitySignatureProcessBtnClose", language);
            //this.BtnConfirm.Text = Utils.Languages.GetLabelFromCode("VisibilitySignatureProcessBtnConfirm", language);
            this.LitVisibilitySignatureProcessCorr.Text = Utils.Languages.GetLabelFromCode("LitVisibilitySignatureProcessCorr", language);
        }

        private void InitializaPage()
        {
            LoadListCorr();
            SetAjaxAddressBook();
            GridViewResult_Bind();
        }

        private void RefreshScript()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "refreshTipsy", "tooltipTipsy();", true);
        }

        protected void BtnConfirm_Click(object sender, EventArgs e)
        {

        }

        protected void SetAjaxAddressBook()
        {
            string dataUser = UIManager.RoleManager.GetRoleInSession().systemId;
            Registro reg = RegistryManager.GetRegistryInSession();
            if (reg == null)
            {
                reg = RoleManager.GetRoleInSession().registri[0];
            }
            dataUser = dataUser + "-" + reg.systemId;

            string callType = "CALLTYPE_IN_ONLY_ROLE";
            this.RapidCorr.ContextKey = dataUser + "-" + UIManager.UserManager.GetUserInSession().idAmministrazione + "-" + callType;
        }

        protected void BtnAddRole_Click(object sender, EventArgs e)
        {
            try
            {
                Corrispondente corr = new Corrispondente();
                corr.systemId = this.idCorr.Value;
                corr.codiceRubrica = this.TxtCodeCorr.Text;
                corr.descrizione = this.TxtDescriptionCorr.Text;

                //AGGIUNGO SUL DB
                //Verifico che il corrispondente selezionato non sia già presente nella lista
                Corrispondente cor = (from c in this.ListaVisibilitaProcesso where c.systemId.Equals(corr.systemId) select c).FirstOrDefault();
                if (cor == null)
                {
                    if (UIManager.SignatureProcessesManager.InsertVisibilitaProcesso(new List<Corrispondente>() { corr }, this.ProcessoDiFirmaSelected.idProcesso))
                    {
                        //AGGIORNO LA LISTA IN SESSIONE ED AGGIORNO LA GRIGLIA
                        this.TxtCodeCorr.Text = string.Empty;
                        this.TxtDescriptionCorr.Text = string.Empty;
                        this.BtnAddRole.Enabled = false;
                        this.UpdPnlCorr.Update();

                        if (ListaVisibilitaProcesso == null)
                        {
                            ListaVisibilitaProcesso = new List<Corrispondente>();
                        }

                        this.ListaVisibilitaProcesso.Add(corr);
                        this.ListaVisibilitaProcesso = (from p in ListaVisibilitaProcesso orderby p.descrizione ascending select p).ToList<Corrispondente>();
                        GridViewResult_Bind();
                    }
                    else
                    {
                        string msg = "ErrorVisibilitySignatureProcessCorrispondent";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');}", true);
                        return;
                    }
                }
                else
                {
                    this.TxtCodeCorr.Text = string.Empty;
                    this.TxtDescriptionCorr.Text = string.Empty;
                    this.BtnAddRole.Enabled = false;
                    this.UpdPnlCorr.Update();
                }
            }
            catch(Exception ex)
            {
                string msg = "ErrorVisibilitySignatureProcessCorrispondent";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');}", true);
                return;
            }
        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlButtons, this.UpPnlButtons.GetType(), "closeAJM", "parent.closeAjaxModal('VisibilitySignatureProcess','');", true);
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return;
            }
        }

        protected void BtnAddressBook_Click(object sender, EventArgs e)
        {
            this.CallType = RubricaCallType.CALLTYPE_CORR_INT;
            HttpContext.Current.Session["AddressBook.from"] = "VISIBILITY_SIGNATURE_PROCESS";
            HttpContext.Current.Session["AddressBook.EnableOnly"] = "R";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxModalPopupAddressBook", "parent.ajaxModalPopupAddressBook();", true);
        }

        protected void btnAddressBookPostback_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "function", "<script>reallowOp();</script>", false);
                List<NttDataWA.Popup.AddressBook.CorrespondentDetail> atList = (List<NttDataWA.Popup.AddressBook.CorrespondentDetail>)HttpContext.Current.Session["AddressBook.At"];
                if (atList != null && atList.Count > 0)
                {
                    List<Corrispondente> listaCorr = new List<Corrispondente>();
                    foreach (NttDataWA.Popup.AddressBook.CorrespondentDetail corr in atList)
                    {
                        if ((from c in this.ListaVisibilitaProcesso where c.systemId.Equals(corr.SystemID) select c).FirstOrDefault() == null)
                        {
                            listaCorr.Add(new Corrispondente() { systemId = corr.SystemID, descrizione = corr.Descrizione, codiceRubrica = corr.CodiceRubrica });
                        }
                    }
                    if (UIManager.SignatureProcessesManager.InsertVisibilitaProcesso(listaCorr, this.ProcessoDiFirmaSelected.idProcesso))
                    {
                        if (ListaVisibilitaProcesso == null)
                        {
                            ListaVisibilitaProcesso = new List<Corrispondente>();
                        }

                        this.ListaVisibilitaProcesso.AddRange(listaCorr);
                        this.ListaVisibilitaProcesso = (from p in ListaVisibilitaProcesso orderby p.descrizione ascending select p).ToList<Corrispondente>();
                        GridViewResult_Bind();
                    }
                }
                HttpContext.Current.Session["AddressBook.At"] = null;
                HttpContext.Current.Session["AddressBook.Cc"] = null;
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return;
            }
        }

        private void GridViewResult_Bind()
        {
            this.GridViewResult.DataSource = this.ListaVisibilitaProcesso;
            this.GridViewResult.DataBind();
            this.UpnlGrid.Update();
        }

        protected void GridViewResult_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void GridViewResult_PreRender(object sender, EventArgs e)
        {
        }

        

        protected void GridViewResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.GridViewResult.PageIndex = e.NewPageIndex;
                GridViewResult_Bind();
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return;
            }
        }

        protected void BuildGridNavigator()
        {
            try
            {
                this.plcNavigator.Controls.Clear();

                int countPage = (int)Math.Round(((double)(this.ListaVisibilitaProcesso.Count * 2) / (double)this.GridViewResult.PageSize) + 0.49);

                int val = (this.ListaVisibilitaProcesso.Count * 2) % this.GridViewResult.PageSize;
                //if (val == 0)
                //{
                //    countPage = countPage - 1;
                //}

                if (countPage > 1)
                {
                    Panel panel = new Panel();
                    panel.EnableViewState = true;
                    panel.CssClass = "recordNavigator";

                    int startFrom = 1;
                    if (int.Parse(this.grid_pageindex.Value) > 6) startFrom = int.Parse(this.grid_pageindex.Value) -5;

                    int endTo = 10;
                    if (int.Parse(this.grid_pageindex.Value) > 6) endTo = int.Parse(this.grid_pageindex.Value) +5;
                    if (endTo > countPage) endTo = countPage;

                    if (startFrom > 1)
                    {
                        LinkButton btn = new LinkButton();
                        btn.EnableViewState = true;
                        btn.Text = "...";
                        btn.Attributes["onclick"] = " $('#grid_pageindex').val(" + (startFrom - 1) + "); __doPostBack('upPnlGridIndexes', ''); return false;";
                        panel.Controls.Add(btn);
                    }

                    for (int i = startFrom; i <= endTo; i++)
                    {
                        if (i == int.Parse(this.grid_pageindex.Value))
                        {
                            Literal lit = new Literal();
                            lit.Text = "<span>" + i.ToString() + "</span>";
                            panel.Controls.Add(lit);
                        }
                        else
                        {
                            LinkButton btn = new LinkButton();
                            btn.EnableViewState = true;
                            btn.Text = i.ToString();
                            btn.Attributes["onclick"] = " $('#grid_pageindex').val($(this).text()); __doPostBack('upPnlGridIndexes', ''); return false;";
                            panel.Controls.Add(btn);
                        }
                    }

                    if (endTo < countPage)
                    {
                        LinkButton btn = new LinkButton();
                        btn.EnableViewState = true;
                        btn.Text = "...";
                        btn.Attributes["onclick"] = " $('#grid_pageindex').val(" + endTo + "); __doPostBack('upPnlGridIndexes', ''); return false;";
                        panel.Controls.Add(btn);
                    }

                    this.plcNavigator.Controls.Add(panel);
                }
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return;
            }
        }

        protected void GridViewResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void ImgDeleteVisibility_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "reallowOp", "reallowOp();", true);
            try
            {
                CustomImageButton btnIm = (CustomImageButton)sender;
                GridViewRow row = (GridViewRow)btnIm.Parent.Parent;
                //int rowIndex = row.RowIndex;

                string idCorr = (row.FindControl("systemIdCorr") as Label).Text;
                if (UIManager.SignatureProcessesManager.RimuoviVisibilitaProcesso(this.ProcessoDiFirmaSelected.idProcesso, idCorr))
                {
                    this.ListaVisibilitaProcesso = (from c in this.ListaVisibilitaProcesso where !c.systemId.Equals(idCorr) select c).ToList();
                    this.GridViewResult_Bind();
                }
                else
                {
                    string msg = "ErrorVisibilitySignatureProcessDeleteCorr";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');}", true);
                    return;
                }
            }
            catch(Exception ex)
            {
                string msg = "ErrorVisibilitySignatureProcessDeleteCorr";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');}", true);
                return;
            }
        }

        protected void TxtCode_OnTextChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "reallowOp", "reallowOp();", true);
            try
            {
                if (!string.IsNullOrEmpty(this.TxtCodeCorr.Text))
                {
                    RubricaCallType calltype = RubricaCallType.CALLTYPE_PROTO_INT_MITT;
                    ElementoRubrica[] listaCorr = null;
                    Corrispondente corr = null;
                    UIManager.RegistryManager.SetRegistryInSession(RoleManager.GetRoleInSession().registri[0]);
                    listaCorr = UIManager.AddressBookManager.getElementiRubricaMultipli(TxtCodeCorr.Text, calltype, true);
                    if (listaCorr != null && (listaCorr.Count() == 1))
                    {
                        if (listaCorr.Count() == 1)
                        {
                            corr = UIManager.AddressBookManager.getCorrispondenteRubrica(this.TxtCodeCorr.Text, calltype);
                        }
                        if (corr == null)
                        {
                            this.TxtCodeCorr.Text = string.Empty;
                            this.TxtDescriptionCorr.Text = string.Empty;
                            //this.idRuolo.Value = string.Empty;
                            this.UpdPnlCorr.Update();
                            string msg = "ErrorTransmissionCorrespondentNotFound";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'warning', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'warning', '');}", true);
                        }
                        if (!corr.tipoCorrispondente.Equals("R"))
                        {
                            this.TxtCodeCorr.Text = string.Empty;
                            this.TxtDescriptionCorr.Text = string.Empty;
                            //this.idRuolo.Value = string.Empty;
                            this.UpdPnlCorr.Update();
                            string msg = "WarningCorrespondentAsRole";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) { parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'warning', '');} else { parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'warning', '');}", true);
                        }
                        else
                        {
                            this.TxtCodeCorr.Text = corr.codiceRubrica;
                            this.TxtDescriptionCorr.Text = corr.descrizione;
                            this.idCorr.Value = corr.systemId;
                            this.BtnAddRole.Enabled = true;
                            //this.PassoDiFirmaSelected.ruoloCoinvolto = ruolo;
                            this.UpdPnlCorr.Update();
                        }
                    }
                    else
                    {
                        this.TxtCodeCorr.Text = string.Empty;
                        this.TxtDescriptionCorr.Text = string.Empty;
                        //this.idRuolo.Value = string.Empty;
                        this.UpdPnlCorr.Update();
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
                    this.TxtCodeCorr.Text = string.Empty;
                    this.TxtDescriptionCorr.Text = string.Empty;
                    //this.idRuolo.Value = string.Empty;
                    this.UpdPnlCorr.Update();
                }
            }
            catch (Exception ex)
            {
                string msg = "ErrorSignatureProcess";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');}", true);
                return;
            }
        }

        private void LoadListCorr()
        {
            this.ListaVisibilitaProcesso = UIManager.SignatureProcessesManager.GetVisibilitaProcesso(this.ProcessoDiFirmaSelected.idProcesso);
        }

        protected string GetDescriptionCorr(Corrispondente corr)
        {
            return corr.descrizione + "(" + corr.codiceRubrica + ")";
        }
    }
}