using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NttDataWA.Popup
{
    public partial class RedirectProject : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                this.InitializePage();
            }
        }

        protected void InitializePage()
        {
            this.InitializeLanguage();
            this.PopolaDdlAOO();
        }

        protected void InitializeLanguage()
        {
            string language = UIManager.UserManager.GetUserLanguage();

            this.RedirectProjectBtnConfirm.Text = Utils.Languages.GetLabelFromCode("RedirectProjectBtnConfirm", language);
            this.RedirectProjectBtnClose.Text = Utils.Languages.GetLabelFromCode("RedirectProjectBtnCancel", language);
            this.RedirectLitAoo.Text = Utils.Languages.GetLabelFromCode("RedirectProjectLitAoo", language);
            this.RedirectLitNotes.Text = Utils.Languages.GetLabelFromCode("RedirectProjectLitNotes", language);
        }

        protected void PopolaDdlAOO()
        {
            DocsPaWR.Fascicolo fascicolo = UIManager.ProjectManager.getProjectInSession();
            if(fascicolo != null && fascicolo.template != null)
            {
                string idAmm = UIManager.UserManager.GetInfoUser().idAmministrazione;
                List<DocsPaWR.Registro> list = UIManager.ProceedingsManager.GetAOOAssociateProcedimento(fascicolo.template.DESCRIZIONE);
                if(list == null)
                {
                    this.ddlAoo.Enabled = false;
                    this.RedirectProjectBtnConfirm.Enabled = false;
                    string msg = "RedirectProjectLoadError";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');}", true);
                }
                else if(list.Count == 0)
                {
                    this.ddlAoo.Enabled = false;
                    this.RedirectProjectBtnConfirm.Enabled = false;
                    string msg = "RedirectProjectLoadWarning";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'warning', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'warning', '');}", true);
                }
                else
                {
                    foreach(DocsPaWR.Registro item in list)
                    {
                        this.ddlAoo.Items.Add(new ListItem() { Value = item.systemId, Text = string.Format("({0}) {1}", item.codice, item.descrizione) });
                    }
                }
            }

        }

        protected void RedirectProjectBtnConfirm_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "function", "reallowOp();", true);

            bool result = UIManager.ProceedingsManager.ReindirizzaProcedimento(UIManager.ProjectManager.getProjectInSession().systemID, this.ddlAoo.SelectedValue, this.RedirectTxtNotes.Text);

            if(result)
            {
                this.RedirectProjectBtnConfirm.Enabled = false;
                string msg = "RedirectProjectOK";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'info', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'info', '');}", true);
            }
            else
            {
                string msg = "RedirectProjectError";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'error', '');}", true);
            }
        }

        protected void RedirectProjectBtnClose_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "function", "reallowOp();", true);
            try
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "closeMask", "if (parent.fra_main) {parent.fra_main.closeAjaxModal('RedirectProject', '');} else {parent.closeAjaxModal('RedirectProject', '');};", true);
            }
            catch(Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return;
            }
        }
    }
}