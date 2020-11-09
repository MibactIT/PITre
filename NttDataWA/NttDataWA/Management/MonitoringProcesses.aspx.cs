using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NttDataWA.DocsPaWR;
using NttDataWA.UIManager;
using NttDataWA.Utils;
using NttDatalLibrary;

namespace NttDataWA.Management
{
    public partial class MonitoringProcesses : System.Web.UI.Page
    {
        #region Properties

        private List<IstanzaProcessoDiFirma> ListaIstanzaProcessiFirmaFiltered
        {
            get
            {
                if (HttpContext.Current.Session["ListaIstanzaProcessiFirmaFiltered"] != null)
                    return (List<IstanzaProcessoDiFirma>)HttpContext.Current.Session["ListaIstanzaProcessiFirmaFiltered"];
                else
                    return null;
            }
            set
            {
                HttpContext.Current.Session["ListaIstanzaProcessiFirmaFiltered"] = value;
            }
        }

        private List<FiltroIstanzeProcessoFirma> FiltersInstanceProcesses
        {
            get
            {
                return (List<FiltroIstanzeProcessoFirma>)HttpContext.Current.Session["FiltersInstanceProcesses"];
            }
            set
            {
                HttpContext.Current.Session["FiltersInstanceProcesses"] = value;
            }
        }

        private string SelectedRow
        {
            get
            {
                string result = string.Empty;
                if (HttpContext.Current.Session["selectedRowMonitoringProcesses"] != null)
                {
                    result = HttpContext.Current.Session["selectedRowMonitoringProcesses"] as String;
                }
                return result;
            }
            set
            {
                HttpContext.Current.Session["selectedRowMonitoringProcesses"] = value;
            }
        }

        private int RecordCount
        {
            get
            {
                int toReturn = 0;
                if (HttpContext.Current.Session["recordCountMonitoringProcesses"] != null) Int32.TryParse(HttpContext.Current.Session["recordCountMonitoringProcesses"].ToString(), out toReturn);
                return toReturn;
            }
            set
            {
                HttpContext.Current.Session["recordCountMonitoringProcesses"] = value;
            }
        }

        private int SelectedPage
        {
            get
            {
                int toReturn = 1;
                if (HttpContext.Current.Session["selectedPageMonitoringProcesses"] != null) Int32.TryParse(HttpContext.Current.Session["selectedPageMonitoringProcesses"].ToString(), out toReturn);
                if (toReturn < 1) toReturn = 1;

                return toReturn;
            }
            set
            {
                HttpContext.Current.Session["selectedPageMonitoringProcesses"] = value;
            }
        }

        /// <summary>
        /// Numero di pagine restituiti dalla ricerca
        /// </summary>
        public int PageCount
        {
            get
            {
                int toReturn = 1;
                if (HttpContext.Current.Session["PageCountMonitoringProcesses"] != null)
                {
                    Int32.TryParse(
                        HttpContext.Current.Session["PageCountMonitoringProcesses"].ToString(),
                        out toReturn);
                }
                return toReturn;
            }
            set
            {
                HttpContext.Current.Session["PageCountMonitoringProcesses"] = value;
            }
        }
        #endregion

        private const string ALLEGATO = "A";
        private const string PANEL_GRID_INDEXES = "upPnlGridIndexes";

        #region Standard method
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitializeLanguage();
                this.InitializaPage();
            }
            else
            {
                if (this.Request.Form["__EVENTTARGET"] != null && this.Request.Form["__EVENTTARGET"].Equals(PANEL_GRID_INDEXES))
                {
                    this.SelectedRow = string.Empty;
                    if (!string.IsNullOrEmpty(this.grid_pageindex.Value))
                    {
                        this.SelectedPage = int.Parse(this.grid_pageindex.Value);
                    }
                    int numTotPage = 0;
                    int nRec = 0;
                    this.ListaIstanzaProcessiFirmaFiltered = LibroFirmaManager.GetIstanzaProcessiDiFirmaByFilter(FiltersInstanceProcesses, this.SelectedPage, gridViewResult.PageSize, out numTotPage, out nRec);
                    this.RecordCount = nRec;
                    this.PageCount = (int)Math.Round(((double)nRec / (double)gridViewResult.PageSize) + 0.49);
                    GridViewResult_Bind();
                    this.UpPnlGridView.Update();
                }
            }

            RefreshScript();
        }

        private void InitializaPage()
        {
            this.txt_initStartDate.ReadOnly = false;
            this.txt_finedataStartDate.Visible = false;
            this.LtlAStartDate.Visible = false;

            this.txt_initCompletitionDate.ReadOnly = false;
            this.txt_finedataCompletitionDate.Visible = false;
            this.LtlACompletitionDate.Visible = false;

            this.txt_initInterruptionDate.ReadOnly = false;
            this.txt_finedataInterruptionDate.Visible = false;
            this.LtlAInterruptionDate.Visible = false;

            this.txt_initIdDoc.ReadOnly = false;
            this.txt_fineIdDoc.Visible = false;
            this.LtlAIdDoc.Visible = false;
            this.LtlDaIdDoc.Visible = false;

            //Back
            if (this.Request.QueryString["back"] != null && this.Request.QueryString["back"].Equals("1"))
            {
                List<Navigation.NavigationObject> navigationList = Navigation.NavigationUtils.GetNavigationList();
                Navigation.NavigationObject obj = navigationList.Last();

                List<ProcessoFirma> listSignatureProcesses = LoadSignatureProcessesVisible();
                if (listSignatureProcesses != null && listSignatureProcesses.Count > 0)
                {
                    this.TreeviewProcesses_Bind(listSignatureProcesses);
                }
                else
                {
                    this.DisabledSearch();
                }
                if (!string.IsNullOrEmpty(obj.NumPage))
                {
                    this.gridViewResult.PageIndex = Int32.Parse(obj.NumPage);
                }

                if (!string.IsNullOrEmpty(obj.IdObject))
                {
                    this.FillFieldsFilter();
                    if (FiltersInstanceProcesses != null)
                    {
                        int numTotPage = 0;
                        int nRec = 0;
                        this.ListaIstanzaProcessiFirmaFiltered = LibroFirmaManager.GetIstanzaProcessiDiFirmaByFilter(FiltersInstanceProcesses, Int32.Parse(obj.NumPage), gridViewResult.PageSize, out numTotPage, out nRec);

                        this.RecordCount = nRec;
                        this.PageCount = (int)Math.Round(((double)nRec / (double)gridViewResult.PageSize) + 0.49);

                        GridViewResult_Bind();

                        string idProfile = string.Empty;
                        foreach (GridViewRow grd in this.gridViewResult.Rows)
                        {
                            idProfile = string.Empty;

                            if (grd.FindControl("idDocumento") != null)
                            {
                                idProfile = (grd.FindControl("idDocumento") as Label).Text.ToString();
                            }

                            if (idProfile.Equals(obj.IdObject))
                            {
                                this.SelectedRow = grd.RowIndex.ToString();
                            }
                        }
                        this.HighlightSelectedRow();
                        this.UpPnlGridView.Update();
                    }
                }
            }
            else
            {

                this.ClearSession();
                List<ProcessoFirma> listSignatureProcesses = LoadSignatureProcessesVisible();
                if (listSignatureProcesses != null && listSignatureProcesses.Count > 0)
                {
                    this.TreeviewProcesses_Bind(listSignatureProcesses);
                }
                else
                {
                    this.DisabledSearch();
                }
                this.ListaIstanzaProcessiFirmaFiltered = new List<IstanzaProcessoDiFirma>();
                this.SelectedRow = null;
                this.GridViewResult_Bind();
            }
        }

        private void DisabledSearch()
        {
            this.pnlNoProcesses.Visible = true;

            this.ddl_CompletitionDate.Enabled = false;
            this.ddl_InterruptionDate.Enabled = false;
            this.ddl_idDoc.Enabled = false;
            this.ddl_StartDate.Enabled = false;
            this.cbxState.Enabled = false;

            this.txt_initStartDate.ReadOnly = true;
            this.txt_finedataStartDate.ReadOnly = true;

            this.txt_initCompletitionDate.ReadOnly = true;
            this.txt_finedataCompletitionDate.ReadOnly = true;

            this.txt_initInterruptionDate.ReadOnly = true;
            this.txt_finedataInterruptionDate.ReadOnly = true;

            this.txt_initIdDoc.ReadOnly = true;
            this.txt_fineIdDoc.ReadOnly = true;
            this.txt_object.ReadOnly = true;
            this.txtNotesSturtup.ReadOnly = true;
            this.txtNotesInterruption.ReadOnly = true;
            this.MonitoringProcessesSearch.Enabled = false;
            this.MonitoringProcessesClearFilter.Enabled = false;
        }

        private void ClearSession()
        {
            HttpContext.Current.Session.Remove("ListaIstanzaProcessiFirmaFiltered");
            HttpContext.Current.Session.Remove("FiltersInstanceProcesses");
            HttpContext.Current.Session.Remove("selectedRowMonitoringProcesses");
        }

        private void InitializeLanguage()
        {
            string language = UIManager.UserManager.GetUserLanguage();
            this.ManagementMonitoringProcesses.Text = Utils.Languages.GetLabelFromCode("ManagementMonitoringProcesses", language);
            this.MonitoringProcessesSearch.Text = Utils.Languages.GetLabelFromCode("MonitoringProcessesSearch", language);
            this.LtlStartDate.Text = Utils.Languages.GetLabelFromCode("StatisticsSignatureProcessStartDate", language);
            this.LtlDaStartDate.Text = Utils.Languages.GetLabelFromCode("VisibilityOneField", language);
            this.LtlAStartDate.Text = Utils.Languages.GetLabelFromCode("SearchDocumentAdvancedLtlA", language);
            this.ddl_StartDate.Items[0].Text = Utils.Languages.GetLabelFromCode("ddl_data0", language);
            this.ddl_StartDate.Items[1].Text = Utils.Languages.GetLabelFromCode("ddl_data1", language);
            this.ddl_StartDate.Items[2].Text = Utils.Languages.GetLabelFromCode("ddl_data2", language);
            this.ddl_StartDate.Items[3].Text = Utils.Languages.GetLabelFromCode("ddl_data3", language);
            this.ddl_StartDate.Items[4].Text = Utils.Languages.GetLabelFromCode("ddl_data4", language);
            this.LtlNotesStartup.Text = Utils.Languages.GetLabelFromCode("StatisticsSignatureProcessNotesStartup", language);
            this.LtlCompletitionDate.Text = Utils.Languages.GetLabelFromCode("StatisticsSignatureProcessCompletitionDate", language);
            this.LtlDaCompletitionDate.Text = Utils.Languages.GetLabelFromCode("VisibilityOneField", language);
            this.LtlACompletitionDate.Text = Utils.Languages.GetLabelFromCode("SearchDocumentAdvancedLtlA", language);
            this.ddl_CompletitionDate.Items[0].Text = Utils.Languages.GetLabelFromCode("ddl_data0", language);
            this.ddl_CompletitionDate.Items[1].Text = Utils.Languages.GetLabelFromCode("ddl_data1", language);
            this.ddl_CompletitionDate.Items[2].Text = Utils.Languages.GetLabelFromCode("ddl_data2", language);
            this.ddl_CompletitionDate.Items[3].Text = Utils.Languages.GetLabelFromCode("ddl_data3", language);
            this.ddl_CompletitionDate.Items[4].Text = Utils.Languages.GetLabelFromCode("ddl_data4", language);
            this.LtlInterruptionDate.Text = Utils.Languages.GetLabelFromCode("StatisticsSignatureProcessInterruptionDate", language);
            this.LtlNotesInterruption.Text = Utils.Languages.GetLabelFromCode("StatisticsSignatureProcessNotesInterruption", language);
            this.LtlDaInterruptionDate.Text = Utils.Languages.GetLabelFromCode("VisibilityOneField", language);
            this.LtlAInterruptionDate.Text = Utils.Languages.GetLabelFromCode("SearchDocumentAdvancedLtlA", language);
            this.ddl_InterruptionDate.Items[0].Text = Utils.Languages.GetLabelFromCode("ddl_data0", language);
            this.ddl_InterruptionDate.Items[1].Text = Utils.Languages.GetLabelFromCode("ddl_data1", language);
            this.ddl_InterruptionDate.Items[2].Text = Utils.Languages.GetLabelFromCode("ddl_data2", language);
            this.ddl_InterruptionDate.Items[3].Text = Utils.Languages.GetLabelFromCode("ddl_data3", language);
            this.ddl_InterruptionDate.Items[4].Text = Utils.Languages.GetLabelFromCode("ddl_data4", language);
            this.LtlState.Text = Utils.Languages.GetLabelFromCode("StatisticsSignatureProcessState", language);
            this.opIN_EXEC.Text = Utils.Languages.GetLabelFromCode("IN_EXEC", language);
            this.opSTOPPED.Text = Utils.Languages.GetLabelFromCode("STOPPED", language);
            this.opCLOSED.Text = Utils.Languages.GetLabelFromCode("CLOSED", language);
            this.opTRUNCATED.Text = Utils.Languages.GetLabelFromCode("TRUNCATED", language);
            this.monitoringProcessesResultCount.Text = Utils.Languages.GetLabelFromCode("monitoringProcessesResultCount", language);
            this.LtlIdDoc.Text = Utils.Languages.GetLabelFromCode("LtlIdDoc", language);
            this.ddl_idDoc.Items[0].Text = Utils.Languages.GetLabelFromCode("ddl_numProt_E0", language);
            this.ddl_idDoc.Items[1].Text = Utils.Languages.GetLabelFromCode("ddl_numProt_E1", language);
            this.LtlDaIdDoc.Text = Utils.Languages.GetLabelFromCode("SearchDocumentAdvancedLtlDa", language);
            this.LtlAIdDoc.Text = Utils.Languages.GetLabelFromCode("SearchDocumentAdvancedLtlA", language);
            this.LtlObject.Text = Utils.Languages.GetLabelFromCode("MonitoringProcessesObject", language);
            this.MonitoringProcessesClearFilter.Text = Utils.Languages.GetLabelFromCode("MonitoringProcessesClearFilter", language);
            this.lblNoProcesses.Text = Utils.Languages.GetLabelFromCode("NoVisibleProcesses", language);
        }

        private void RefreshScript()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "refreshSelect", "refreshSelect();", true);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "refreshTipsy", "tooltipTipsy();", true);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "refreshPicker", "DatePicker('" + UIManager.UserManager.GetLanguageData() + "');", true);
        }

        private List<ProcessoFirma> LoadSignatureProcessesVisible()
        {
            List<ProcessoFirma> listSignatureProcesses = new List<ProcessoFirma>();
            try
            {
                listSignatureProcesses = UIManager.SignatureProcessesManager.GetProcessesSignatureVisibleRole();
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return null;
            }
            return listSignatureProcesses;
        }
        #endregion

        #region Event Button

        protected void MonitoringProcessesSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.SelectedRow = null;
                bool result = BindFilters();
                if (result)
                {
                    int numPage = 1;
                    int numTotPage = 0;
                    int nRec = 0;
                    this.SelectedPage = 0;
                    this.ListaIstanzaProcessiFirmaFiltered = LibroFirmaManager.GetIstanzaProcessiDiFirmaByFilter(FiltersInstanceProcesses, numPage, gridViewResult.PageSize, out numTotPage, out nRec);

                    this.RecordCount = nRec;
                    this.PageCount = (int)Math.Round(((double)nRec / (double)gridViewResult.PageSize) + 0.49);

                    GridViewResult_Bind();
                    this.UpPnlGridView.Update();

                }
                else
                    return;
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return;
            }
        }

        protected void MonitoringProcessesClearFilter_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearFilter();
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return;
            }
        }


        protected void ddl_idDoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (this.ddl_idDoc.SelectedIndex)
                {
                    case 0: //Valore singolo
                        this.txt_initIdDoc.ReadOnly = false;
                        this.txt_fineIdDoc.Visible = false;
                        this.LtlAIdDoc.Visible = false;
                        this.LtlDaIdDoc.Visible = false;
                        this.txt_fineIdDoc.Text = string.Empty;
                        break;
                    case 1: //Intervallo
                        this.txt_initIdDoc.ReadOnly = false;
                        this.txt_fineIdDoc.ReadOnly = false;
                        this.LtlAIdDoc.Visible = true;
                        this.LtlDaIdDoc.Visible = true;
                        this.txt_fineIdDoc.Visible = true;
                        break;
                }
                this.UpPnlIdDoc.Update();
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return;
            }
        }


        protected void ddl_StartDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string language = UIManager.UserManager.GetUserLanguage();
                switch (this.ddl_StartDate.SelectedIndex)
                {
                    case 0: //Valore singolo
                        this.txt_initStartDate.ReadOnly = false;
                        this.txt_finedataStartDate.Visible = false;
                        this.LtlAStartDate.Visible = false;
                        this.LtlDaStartDate.Text = Utils.Languages.GetLabelFromCode("VisibilityOneField", language);
                        break;
                    case 1: //Intervallo
                        this.txt_initStartDate.ReadOnly = false;
                        this.txt_finedataStartDate.ReadOnly = false;
                        this.LtlAStartDate.Visible = true;
                        this.LtlDaStartDate.Visible = true;
                        this.txt_finedataStartDate.Visible = true;
                        this.LtlDaStartDate.Text = Utils.Languages.GetLabelFromCode("SearchDocumentAdvancedLtlDa", language);
                        this.LtlAStartDate.Text = Utils.Languages.GetLabelFromCode("SearchDocumentAdvancedLtlA", language);
                        break;
                    case 2: //Oggi
                        this.LtlAStartDate.Visible = false;
                        this.txt_finedataStartDate.Visible = false;
                        this.txt_initStartDate.ReadOnly = true;
                        this.txt_initStartDate.Text = NttDataWA.Utils.dateformat.toDay();
                        this.LtlDaStartDate.Text = Utils.Languages.GetLabelFromCode("VisibilityOneField", language);
                        break;
                    case 3: //Settimana corrente
                        this.LtlAStartDate.Visible = true;
                        this.txt_finedataStartDate.Visible = true;
                        this.txt_initStartDate.Text = NttDataWA.Utils.dateformat.getFirstDayOfWeek();
                        this.txt_finedataStartDate.Text = NttDataWA.Utils.dateformat.getLastDayOfWeek();
                        this.txt_finedataStartDate.ReadOnly = true;
                        this.txt_initStartDate.ReadOnly = true;
                        this.LtlDaStartDate.Text = Utils.Languages.GetLabelFromCode("SearchDocumentAdvancedLtlDa", language);
                        this.LtlAStartDate.Text = Utils.Languages.GetLabelFromCode("SearchDocumentAdvancedLtlA", language);
                        break;
                    case 4: //Mese corrente
                        this.LtlAStartDate.Visible = true;
                        this.txt_finedataStartDate.Visible = true;
                        this.txt_initStartDate.Text = NttDataWA.Utils.dateformat.getFirstDayOfMonth();
                        this.txt_finedataStartDate.Text = NttDataWA.Utils.dateformat.getLastDayOfMonth();
                        this.txt_finedataStartDate.ReadOnly = true;
                        this.txt_initStartDate.ReadOnly = true;
                        this.LtlDaStartDate.Text = Utils.Languages.GetLabelFromCode("SearchDocumentAdvancedLtlDa", language);
                        this.LtlAStartDate.Text = Utils.Languages.GetLabelFromCode("SearchDocumentAdvancedLtlA", language);
                        break;
                }
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return;
            }
        }


        protected void ddl_CompletitionDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string language = UIManager.UserManager.GetUserLanguage();
                switch (this.ddl_CompletitionDate.SelectedIndex)
                {
                    case 0: //Valore singolo
                        this.txt_initCompletitionDate.ReadOnly = false;
                        this.txt_finedataCompletitionDate.Visible = false;
                        this.LtlACompletitionDate.Visible = false;
                        this.LtlDaCompletitionDate.Text = Utils.Languages.GetLabelFromCode("VisibilityOneField", language);
                        break;
                    case 1: //Intervallo
                        this.txt_initCompletitionDate.ReadOnly = false;
                        this.txt_finedataCompletitionDate.ReadOnly = false;
                        this.LtlACompletitionDate.Visible = true;
                        this.LtlDaCompletitionDate.Visible = true;
                        this.txt_finedataCompletitionDate.Visible = true;
                        this.LtlDaCompletitionDate.Text = Utils.Languages.GetLabelFromCode("SearchDocumentAdvancedLtlDa", language);
                        this.LtlACompletitionDate.Text = Utils.Languages.GetLabelFromCode("SearchDocumentAdvancedLtlA", language);
                        break;
                    case 2: //Oggi
                        this.LtlACompletitionDate.Visible = false;
                        this.txt_finedataCompletitionDate.Visible = false;
                        this.txt_initCompletitionDate.ReadOnly = true;
                        this.txt_initCompletitionDate.Text = NttDataWA.Utils.dateformat.toDay();
                        this.LtlDaCompletitionDate.Text = Utils.Languages.GetLabelFromCode("VisibilityOneField", language);
                        break;
                    case 3: //Settimana corrente
                        this.LtlACompletitionDate.Visible = true;
                        this.txt_finedataCompletitionDate.Visible = true;
                        this.txt_initCompletitionDate.Text = NttDataWA.Utils.dateformat.getFirstDayOfWeek();
                        this.txt_finedataCompletitionDate.Text = NttDataWA.Utils.dateformat.getLastDayOfWeek();
                        this.txt_finedataCompletitionDate.ReadOnly = true;
                        this.txt_initCompletitionDate.ReadOnly = true;
                        this.LtlDaCompletitionDate.Text = Utils.Languages.GetLabelFromCode("SearchDocumentAdvancedLtlDa", language);
                        this.LtlACompletitionDate.Text = Utils.Languages.GetLabelFromCode("SearchDocumentAdvancedLtlA", language);
                        break;
                    case 4: //Mese corrente
                        this.LtlACompletitionDate.Visible = true;
                        this.txt_finedataCompletitionDate.Visible = true;
                        this.txt_initCompletitionDate.Text = NttDataWA.Utils.dateformat.getFirstDayOfMonth();
                        this.txt_finedataCompletitionDate.Text = NttDataWA.Utils.dateformat.getLastDayOfMonth();
                        this.txt_finedataCompletitionDate.ReadOnly = true;
                        this.txt_initCompletitionDate.ReadOnly = true;
                        this.LtlDaCompletitionDate.Text = Utils.Languages.GetLabelFromCode("SearchDocumentAdvancedLtlDa", language);
                        this.LtlACompletitionDate.Text = Utils.Languages.GetLabelFromCode("SearchDocumentAdvancedLtlA", language);
                        break;
                }
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return;
            }
        }

        protected void ddl_InterruptionDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string language = UIManager.UserManager.GetUserLanguage();
                switch (this.ddl_InterruptionDate.SelectedIndex)
                {
                    case 0: //Valore singolo
                        this.txt_initInterruptionDate.ReadOnly = false;
                        this.txt_finedataInterruptionDate.Visible = false;
                        this.LtlAInterruptionDate.Visible = false;
                        this.LtlDaInterruptionDate.Text = Utils.Languages.GetLabelFromCode("VisibilityOneField", language);
                        break;
                    case 1: //Intervallo
                        this.txt_initInterruptionDate.ReadOnly = false;
                        this.txt_finedataInterruptionDate.ReadOnly = false;
                        this.LtlAInterruptionDate.Visible = true;
                        this.LtlDaInterruptionDate.Visible = true;
                        this.txt_finedataInterruptionDate.Visible = true;
                        this.LtlDaInterruptionDate.Text = Utils.Languages.GetLabelFromCode("SearchDocumentAdvancedLtlDa", language);
                        this.LtlAInterruptionDate.Text = Utils.Languages.GetLabelFromCode("SearchDocumentAdvancedLtlA", language);
                        break;
                    case 2: //Oggi
                        this.LtlAInterruptionDate.Visible = false;
                        this.txt_finedataInterruptionDate.Visible = false;
                        this.txt_initInterruptionDate.ReadOnly = true;
                        this.txt_initInterruptionDate.Text = NttDataWA.Utils.dateformat.toDay();
                        this.LtlDaInterruptionDate.Text = Utils.Languages.GetLabelFromCode("VisibilityOneField", language);
                        break;
                    case 3: //Settimana corrente
                        this.LtlAInterruptionDate.Visible = true;
                        this.txt_finedataInterruptionDate.Visible = true;
                        this.txt_initInterruptionDate.Text = NttDataWA.Utils.dateformat.getFirstDayOfWeek();
                        this.txt_finedataInterruptionDate.Text = NttDataWA.Utils.dateformat.getLastDayOfWeek();
                        this.txt_finedataInterruptionDate.ReadOnly = true;
                        this.txt_initInterruptionDate.ReadOnly = true;
                        this.LtlDaInterruptionDate.Text = Utils.Languages.GetLabelFromCode("SearchDocumentAdvancedLtlDa", language);
                        this.LtlAInterruptionDate.Text = Utils.Languages.GetLabelFromCode("SearchDocumentAdvancedLtlA", language);
                        break;
                    case 4: //Mese corrente
                        this.LtlAInterruptionDate.Visible = true;
                        this.txt_finedataInterruptionDate.Visible = true;
                        this.txt_initInterruptionDate.Text = NttDataWA.Utils.dateformat.getFirstDayOfMonth();
                        this.txt_finedataInterruptionDate.Text = NttDataWA.Utils.dateformat.getLastDayOfMonth();
                        this.txt_finedataInterruptionDate.ReadOnly = true;
                        this.txt_initInterruptionDate.ReadOnly = true;
                        this.LtlDaInterruptionDate.Text = Utils.Languages.GetLabelFromCode("SearchDocumentAdvancedLtlDa", language);
                        this.LtlAInterruptionDate.Text = Utils.Languages.GetLabelFromCode("SearchDocumentAdvancedLtlA", language);
                        break;
                }
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return;
            }
        }

        private bool BindFilters()
        {
            List<FiltroIstanzeProcessoFirma> filters = new List<FiltroIstanzeProcessoFirma>();
            FiltroIstanzeProcessoFirma filter;

            #region ID_PROCESSO

            string idIstanzaProcesso = this.TreeProcessSignature.SelectedValue;
            filter = new FiltroIstanzeProcessoFirma();
            filter.Argomento = DocsPaWR.FiltriElementoLibroFirma.ID_PROCESSO.ToString();
            filter.Valore = idIstanzaProcesso;
            filters.Add(filter);

            #endregion;

            #region ID DOCUMENTO

            if (this.ddl_idDoc.SelectedIndex == 0)
            {
                if (this.txt_initIdDoc.Text != null && !this.txt_initIdDoc.Text.Equals(""))
                {
                    filter = new DocsPaWR.FiltroIstanzeProcessoFirma();
                    filter.Argomento = DocsPaWR.FiltriDocumento.DOCNUMBER.ToString();
                    filter.Valore = this.txt_initIdDoc.Text;
                    filters.Add(filter);
                }
            }
            else
            {
                if (this.txt_initIdDoc.Text != null && !this.txt_initIdDoc.Text.Equals(""))
                {
                    filter = new DocsPaWR.FiltroIstanzeProcessoFirma();
                    filter.Argomento = DocsPaWR.FiltriDocumento.DOCNUMBER_DAL.ToString();
                    filter.Valore = this.txt_initIdDoc.Text;
                    filters.Add(filter);
                }
                if (this.txt_fineIdDoc.Text != null && !this.txt_fineIdDoc.Text.Equals(""))
                {
                    filter = new DocsPaWR.FiltroIstanzeProcessoFirma();
                    filter.Argomento = DocsPaWR.FiltriDocumento.DOCNUMBER_AL.ToString();
                    filter.Valore = this.txt_fineIdDoc.Text;
                    filters.Add(filter);
                }
            }

            #endregion

            #region OGGETTO

            if (!string.IsNullOrEmpty(this.txt_object.Text))
            {
                filter = new DocsPaWR.FiltroIstanzeProcessoFirma();
                filter.Argomento = DocsPaWR.FiltriDocumento.OGGETTO.ToString();
                filter.Valore = utils.DO_AdattaString(this.txt_object.Text);
                filters.Add(filter);
            }

            #endregion

            #region DATA DI AVVIO

            if (this.ddl_StartDate.SelectedIndex == 2)
            {
                filter = new DocsPaWR.FiltroIstanzeProcessoFirma();
                filter.Argomento = DocsPaWR.FiltriElementoLibroFirma.DATA_AVVIO_TODAY.ToString();
                filter.Valore = "1";
                filters.Add(filter);
            }
            if (this.ddl_StartDate.SelectedIndex == 3)
            {
                // siamo nel caso di Settimana corrente
                filter = new DocsPaWR.FiltroIstanzeProcessoFirma();
                filter.Argomento = DocsPaWR.FiltriElementoLibroFirma.DATA_AVVIO_SC.ToString();
                filter.Valore = "1";
                filters.Add(filter);
            }
            if (this.ddl_StartDate.SelectedIndex == 4)
            {
                // siamo nel caso di Mese corrente
                filter = new DocsPaWR.FiltroIstanzeProcessoFirma();
                filter.Argomento = DocsPaWR.FiltriElementoLibroFirma.DATA_AVVIO_MC.ToString();
                filter.Valore = "1";
                filters.Add(filter);
            }
            if (this.ddl_StartDate.SelectedIndex == 0)

                if (this.ddl_StartDate.SelectedIndex == 0)
                {
                    if (!this.txt_initStartDate.Text.Equals(""))
                    {
                        filter = new DocsPaWR.FiltroIstanzeProcessoFirma();
                        filter.Argomento = DocsPaWR.FiltriElementoLibroFirma.DATA_AVVIO_IL.ToString();
                        filter.Valore = this.txt_initStartDate.Text;
                        filters.Add(filter);
                    }
                }

            if (this.ddl_StartDate.SelectedIndex == 1)
            {
                if (!string.IsNullOrEmpty(txt_initStartDate.Text) &&
                   !string.IsNullOrEmpty(txt_finedataStartDate.Text) &&
                   utils.verificaIntervalloDate(txt_initStartDate.Text, txt_finedataStartDate.Text))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('ErrorSearchProjectFilterDateCreateInterval', 'warning', '');} else {parent.ajaxDialogModal('ErrorSearchProjectFilterDateCreateInterval', 'warning', '');};", true);
                    return false;
                }
                if (!this.txt_initStartDate.Text.Equals(""))
                {

                    filter = new DocsPaWR.FiltroIstanzeProcessoFirma();
                    filter.Argomento = DocsPaWR.FiltriElementoLibroFirma.DATA_AVVIO_SUCCESSIVA_AL.ToString();
                    filter.Valore = this.txt_initStartDate.Text;
                    filters.Add(filter);
                }
                if (!this.txt_finedataStartDate.Text.Equals(""))
                {

                    filter = new DocsPaWR.FiltroIstanzeProcessoFirma();
                    filter.Argomento = DocsPaWR.FiltriElementoLibroFirma.DATA_AVVIO_PRECEDENTE_IL.ToString();
                    filter.Valore = this.txt_finedataStartDate.Text;
                    filters.Add(filter);
                }
            }

            #endregion

            #region NOTE DI AVVIO

            if (!string.IsNullOrEmpty(this.txtNotesSturtup.Text))
            {
                filter = new DocsPaWR.FiltroIstanzeProcessoFirma();
                filter.Argomento = DocsPaWR.FiltriElementoLibroFirma.NOTE_AVVIO.ToString();
                filter.Valore = utils.DO_AdattaString(this.txtNotesSturtup.Text);
                filters.Add(filter);
            }

            #endregion

            #region DATA CONCLUSIONE

            if (this.ddl_CompletitionDate.SelectedIndex == 2)
            {
                filter = new DocsPaWR.FiltroIstanzeProcessoFirma();
                filter.Argomento = DocsPaWR.FiltriElementoLibroFirma.DATA_CONCLUSIONE_TODAY.ToString();
                filter.Valore = "1";
                filters.Add(filter);
            }
            if (this.ddl_CompletitionDate.SelectedIndex == 3)
            {
                // siamo nel caso di Settimana corrente
                filter = new DocsPaWR.FiltroIstanzeProcessoFirma();
                filter.Argomento = DocsPaWR.FiltriElementoLibroFirma.DATA_CONCLUSIONE_SC.ToString();
                filter.Valore = "1";
                filters.Add(filter);
            }
            if (this.ddl_CompletitionDate.SelectedIndex == 4)
            {
                // siamo nel caso di Mese corrente
                filter = new DocsPaWR.FiltroIstanzeProcessoFirma();
                filter.Argomento = DocsPaWR.FiltriElementoLibroFirma.DATA_CONCLUSIONE_MC.ToString();
                filter.Valore = "1";
                filters.Add(filter);
            }
            if (this.ddl_CompletitionDate.SelectedIndex == 0)

                if (this.ddl_CompletitionDate.SelectedIndex == 0)
                {
                    if (!this.txt_initCompletitionDate.Text.Equals(""))
                    {
                        filter = new DocsPaWR.FiltroIstanzeProcessoFirma();
                        filter.Argomento = DocsPaWR.FiltriElementoLibroFirma.DATA_CONCLUSIONE_IL.ToString();
                        filter.Valore = this.txt_initCompletitionDate.Text;
                        filters.Add(filter);
                    }
                }

            if (this.ddl_CompletitionDate.SelectedIndex == 1)
            {
                if (!string.IsNullOrEmpty(txt_initCompletitionDate.Text) &&
                   !string.IsNullOrEmpty(txt_finedataCompletitionDate.Text) &&
                   utils.verificaIntervalloDate(txt_initCompletitionDate.Text, txt_finedataCompletitionDate.Text))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('ErrorSearchProjectFilterDateCreateInterval', 'warning', '');} else {parent.ajaxDialogModal('ErrorSearchProjectFilterDateCreateInterval', 'warning', '');};", true);
                    return false;
                }
                if (!this.txt_initCompletitionDate.Text.Equals(""))
                {

                    filter = new DocsPaWR.FiltroIstanzeProcessoFirma();
                    filter.Argomento = DocsPaWR.FiltriElementoLibroFirma.DATA_CONCLUSIONE_SUCCESSIVA_AL.ToString();
                    filter.Valore = this.txt_initCompletitionDate.Text;
                    filters.Add(filter);
                }
                if (!this.txt_finedataCompletitionDate.Text.Equals(""))
                {

                    filter = new DocsPaWR.FiltroIstanzeProcessoFirma();
                    filter.Argomento = DocsPaWR.FiltriElementoLibroFirma.DATA_CONCLUSIONE_PRECEDENTE_IL.ToString();
                    filter.Valore = this.txt_finedataCompletitionDate.Text;
                    filters.Add(filter);
                }
            }

            #endregion

            #region DATA INTERRUZIONE

            if (this.ddl_InterruptionDate.SelectedIndex == 2)
            {
                filter = new DocsPaWR.FiltroIstanzeProcessoFirma();
                filter.Argomento = DocsPaWR.FiltriElementoLibroFirma.DATA_INTERRUZIONE_TODAY.ToString();
                filter.Valore = "1";
                filters.Add(filter);
            }
            if (this.ddl_InterruptionDate.SelectedIndex == 3)
            {
                // siamo nel caso di Settimana corrente
                filter = new DocsPaWR.FiltroIstanzeProcessoFirma();
                filter.Argomento = DocsPaWR.FiltriElementoLibroFirma.DATA_INTERRUZIONE_SC.ToString();
                filter.Valore = "1";
                filters.Add(filter);
            }
            if (this.ddl_InterruptionDate.SelectedIndex == 4)
            {
                // siamo nel caso di Mese corrente
                filter = new DocsPaWR.FiltroIstanzeProcessoFirma();
                filter.Argomento = DocsPaWR.FiltriElementoLibroFirma.DATA_INTERRUZIONE_MC.ToString();
                filter.Valore = "1";
                filters.Add(filter);
            }
            if (this.ddl_InterruptionDate.SelectedIndex == 0)

                if (this.ddl_InterruptionDate.SelectedIndex == 0)
                {
                    if (!this.txt_initInterruptionDate.Text.Equals(""))
                    {
                        filter = new DocsPaWR.FiltroIstanzeProcessoFirma();
                        filter.Argomento = DocsPaWR.FiltriElementoLibroFirma.DATA_INTERRUZIONE_IL.ToString();
                        filter.Valore = this.txt_initInterruptionDate.Text;
                        filters.Add(filter);
                    }
                }

            if (this.ddl_InterruptionDate.SelectedIndex == 1)
            {
                if (!string.IsNullOrEmpty(txt_initInterruptionDate.Text) &&
                   !string.IsNullOrEmpty(txt_finedataInterruptionDate.Text) &&
                   utils.verificaIntervalloDate(txt_initInterruptionDate.Text, txt_finedataInterruptionDate.Text))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('ErrorSearchProjectFilterDateCreateInterval', 'warning', '');} else {parent.ajaxDialogModal('ErrorSearchProjectFilterDateCreateInterval', 'warning', '');};", true);
                    return false;
                }
                if (!this.txt_initInterruptionDate.Text.Equals(""))
                {

                    filter = new DocsPaWR.FiltroIstanzeProcessoFirma();
                    filter.Argomento = DocsPaWR.FiltriElementoLibroFirma.DATA_INTERRUZIONE_SUCCESSIVA_AL.ToString();
                    filter.Valore = this.txt_initInterruptionDate.Text;
                    filters.Add(filter);
                }
                if (!this.txt_finedataInterruptionDate.Text.Equals(""))
                {

                    filter = new DocsPaWR.FiltroIstanzeProcessoFirma();
                    filter.Argomento = DocsPaWR.FiltriElementoLibroFirma.DATA_INTERRUZIONE_PRECEDENTE_IL.ToString();
                    filter.Valore = this.txt_finedataInterruptionDate.Text;
                    filters.Add(filter);
                }
            }

            #endregion

            #region MOTIVO INTERRUZIONE

            if (!string.IsNullOrEmpty(this.txtNotesInterruption.Text))
            {
                filter = new DocsPaWR.FiltroIstanzeProcessoFirma();
                filter.Argomento = DocsPaWR.FiltriElementoLibroFirma.NOTE_RESPINGIMENTO.ToString();
                filter.Valore = utils.DO_AdattaString(this.txtNotesInterruption.Text);
                filters.Add(filter);
            }

            #endregion

            #region STATO

            if (this.cbxState.Items.FindByValue("IN_EXEC") != null)
            {
                filter = new DocsPaWR.FiltroIstanzeProcessoFirma();
                filter.Argomento = DocsPaWR.FiltriElementoLibroFirma.STATO_IN_ESECUZIONE.ToString();
                if (this.cbxState.Items.FindByValue("IN_EXEC").Selected)
                    filter.Valore = "true";
                else
                    filter.Valore = "false";
                filters.Add(filter);
            }

            if (this.cbxState.Items.FindByValue("STOPPED") != null)
            {
                filter = new DocsPaWR.FiltroIstanzeProcessoFirma();
                filter.Argomento = DocsPaWR.FiltriElementoLibroFirma.STATO_INTERROTTO.ToString();
                if (this.cbxState.Items.FindByValue("STOPPED").Selected)
                    filter.Valore = "true";
                else
                    filter.Valore = "false";
                filters.Add(filter);
            }

            if (this.cbxState.Items.FindByValue("CLOSED") != null)
            {
                filter = new DocsPaWR.FiltroIstanzeProcessoFirma();
                filter.Argomento = DocsPaWR.FiltriElementoLibroFirma.STATO_CONCLUSO.ToString();
                if (this.cbxState.Items.FindByValue("CLOSED").Selected)
                    filter.Valore = "true";
                else
                    filter.Valore = "false";
                filters.Add(filter);
            }
            if (this.opTRUNCATED.Checked)
            {
                filter = new DocsPaWR.FiltroIstanzeProcessoFirma();
                filter.Argomento = DocsPaWR.FiltriElementoLibroFirma.TRONCATO.ToString();
                filter.Valore = "true";
                filters.Add(filter);
            }

            #endregion

            this.FiltersInstanceProcesses = filters;

            return true;
        }

        private void FillFieldsFilter()
        {
            try
            {
                foreach (DocsPaWR.FiltroIstanzeProcessoFirma item in this.FiltersInstanceProcesses)
                {
                    #region ID_PROCESSO

                    if (item.Argomento == DocsPaWR.FiltriElementoLibroFirma.ID_PROCESSO.ToString())
                    {
                        foreach (TreeNode node in this.TreeProcessSignature.Nodes)
                        {
                            if (node.Value == item.Valore)
                            {
                                node.Selected = true;
                            }
                        }
                    }

                    #endregion

                    #region ID_DOCUMENTO

                    #region DOCNUMBER
                    else if (item.Argomento == DocsPaWR.FiltriDocumento.DOCNUMBER.ToString())
                    {
                        if (this.ddl_idDoc.SelectedIndex != 0)
                            this.ddl_idDoc.SelectedIndex = 0;
                        this.ddl_idDoc_SelectedIndexChanged(null, new System.EventArgs());
                        this.txt_initIdDoc.Text = item.Valore;
                    }
                    #endregion DOCNUMBER
                    #region DOCNUMBER_DAL
                    else if (item.Argomento == DocsPaWR.FiltriDocumento.DOCNUMBER_DAL.ToString())
                    {
                        if (this.ddl_idDoc.SelectedIndex != 1)
                            this.ddl_idDoc.SelectedIndex = 1;
                        this.ddl_idDoc_SelectedIndexChanged(null, new System.EventArgs());
                        this.txt_initIdDoc.Text = item.Valore;
                    }
                    #endregion DOCNUMBER_DAL
                    #region DOCNUMBER_AL
                    else if (item.Argomento == DocsPaWR.FiltriDocumento.DOCNUMBER_AL.ToString())
                    {
                        if (this.ddl_idDoc.SelectedIndex != 1)
                            this.ddl_idDoc.SelectedIndex = 1;
                        this.ddl_idDoc_SelectedIndexChanged(null, new System.EventArgs());
                        this.txt_fineIdDoc.Text = item.Valore;
                    }
                    #endregion DOCNUMBER_AL

                    #endregion

                    #region OGGETTO
                    else if (item.Argomento == DocsPaWR.FiltriDocumento.OGGETTO.ToString())
                    {
                        this.txt_object.Text = item.Valore;
                    }
                    #endregion OGGETTO

                    #region DATA_AVVIO
                    #region DATA_AVVIO_IL
                    else if (item.Argomento == DocsPaWR.FiltriElementoLibroFirma.DATA_AVVIO_IL.ToString())
                    {
                        if (this.ddl_StartDate.SelectedIndex != 0)
                            ddl_StartDate.SelectedIndex = 0;
                        ddl_StartDate_SelectedIndexChanged(null, new System.EventArgs());
                        this.txt_initStartDate.Text = item.Valore;
                    }
                    #endregion

                    #region DATA_AVVIO_SUCCESSIVA_AL

                    else if (item.Argomento == DocsPaWR.FiltriElementoLibroFirma.DATA_AVVIO_SUCCESSIVA_AL.ToString())
                    {
                        if (ddl_StartDate.SelectedIndex != 1)
                            ddl_StartDate.SelectedIndex = 1;
                        ddl_StartDate_SelectedIndexChanged(null, new System.EventArgs());
                        this.txt_initStartDate.Text = item.Valore;
                    }

                    #endregion

                    #region DATA_AVVIO_PRECEDENTE_IL

                    else if (item.Argomento == DocsPaWR.FiltriElementoLibroFirma.DATA_AVVIO_PRECEDENTE_IL.ToString())
                    {
                        if (ddl_StartDate.SelectedIndex != 1)
                            ddl_StartDate.SelectedIndex = 1;
                        ddl_StartDate_SelectedIndexChanged(null, new System.EventArgs());
                        this.txt_finedataStartDate.Text = item.Valore;
                    }

                    #endregion

                    #region DATA_AVVIO_SC

                    else if (item.Argomento == DocsPaWR.FiltriElementoLibroFirma.DATA_AVVIO_SC.ToString() && item.Valore == "1")
                    {
                        this.ddl_StartDate.SelectedIndex = 3;
                        this.ddl_StartDate_SelectedIndexChanged(null, new System.EventArgs());
                    }

                    #endregion

                    #region DATA_AVVIO_MC

                    else if (item.Argomento == DocsPaWR.FiltriElementoLibroFirma.DATA_AVVIO_MC.ToString() && item.Valore == "1")
                    {
                        this.ddl_StartDate.SelectedIndex = 4;
                        this.ddl_StartDate_SelectedIndexChanged(null, new System.EventArgs());
                    }

                    #endregion

                    #region DATA_AVVIO_TODAY

                    else if (item.Argomento == DocsPaWR.FiltriElementoLibroFirma.DATA_AVVIO_TODAY.ToString() && item.Valore == "1")
                    {
                        this.ddl_StartDate.SelectedIndex = 2;
                        this.ddl_StartDate_SelectedIndexChanged(null, new System.EventArgs());
                    }

                    #endregion

                    #endregion

                    #region NOTE_AVVIO

                    else if (item.Argomento == DocsPaWR.FiltriElementoLibroFirma.NOTE_AVVIO.ToString())
                    {
                        this.txtNotesSturtup.Text = item.Valore;
                    }

                    #endregion

                    #region DATA_CONCLUSIONE

                    #region DATA_CONCLUSIONE_IL
                    else if (item.Argomento == DocsPaWR.FiltriElementoLibroFirma.DATA_CONCLUSIONE_IL.ToString())
                    {
                        if (this.ddl_CompletitionDate.SelectedIndex != 0)
                            ddl_CompletitionDate.SelectedIndex = 0;
                        ddl_CompletitionDate_SelectedIndexChanged(null, new System.EventArgs());
                        this.txt_initCompletitionDate.Text = item.Valore;
                    }
                    #endregion

                    #region DATA_CONCLUSIONE_SUCCESSIVA_AL

                    else if (item.Argomento == DocsPaWR.FiltriElementoLibroFirma.DATA_CONCLUSIONE_SUCCESSIVA_AL.ToString())
                    {
                        if (ddl_CompletitionDate.SelectedIndex != 1)
                            ddl_CompletitionDate.SelectedIndex = 1;
                        ddl_CompletitionDate_SelectedIndexChanged(null, new System.EventArgs());
                        this.txt_initCompletitionDate.Text = item.Valore;
                    }

                    #endregion

                    #region DATA_CONCLUSIONE_PRECEDENTE_IL

                    else if (item.Argomento == DocsPaWR.FiltriElementoLibroFirma.DATA_CONCLUSIONE_PRECEDENTE_IL.ToString())
                    {
                        if (ddl_CompletitionDate.SelectedIndex != 1)
                            ddl_CompletitionDate.SelectedIndex = 1;
                        ddl_CompletitionDate_SelectedIndexChanged(null, new System.EventArgs());
                        this.txt_finedataCompletitionDate.Text = item.Valore;
                    }

                    #endregion

                    #region DATA_CONCLUSIONE_SC

                    else if (item.Argomento == DocsPaWR.FiltriElementoLibroFirma.DATA_CONCLUSIONE_SC.ToString() && item.Valore == "1")
                    {
                        this.ddl_CompletitionDate.SelectedIndex = 3;
                        this.ddl_CompletitionDate_SelectedIndexChanged(null, new System.EventArgs());
                    }

                    #endregion

                    #region DATA_CONCLUSIONE_MC

                    else if (item.Argomento == DocsPaWR.FiltriElementoLibroFirma.DATA_CONCLUSIONE_MC.ToString() && item.Valore == "1")
                    {
                        this.ddl_CompletitionDate.SelectedIndex = 4;
                        this.ddl_CompletitionDate_SelectedIndexChanged(null, new System.EventArgs());
                    }

                    #endregion

                    #region DATA_CONCLUSIONE_TODAY

                    else if (item.Argomento == DocsPaWR.FiltriElementoLibroFirma.DATA_CONCLUSIONE_TODAY.ToString() && item.Valore == "1")
                    {
                        this.ddl_CompletitionDate.SelectedIndex = 2;
                        this.ddl_CompletitionDate_SelectedIndexChanged(null, new System.EventArgs());
                    }

                    #endregion

                    #endregion

                    #region DATA_INTERRUZIONE

                    #region DATA_INTERRUZIONE_IL
                    else if (item.Argomento == DocsPaWR.FiltriElementoLibroFirma.DATA_INTERRUZIONE_IL.ToString())
                    {
                        if (this.ddl_InterruptionDate.SelectedIndex != 0)
                            ddl_InterruptionDate.SelectedIndex = 0;
                        ddl_InterruptionDate_SelectedIndexChanged(null, new System.EventArgs());
                        this.txt_initInterruptionDate.Text = item.Valore;
                    }
                    #endregion

                    #region DATA_INTERRUZIONE_SUCCESSIVA_AL

                    else if (item.Argomento == DocsPaWR.FiltriElementoLibroFirma.DATA_INTERRUZIONE_SUCCESSIVA_AL.ToString())
                    {
                        if (ddl_InterruptionDate.SelectedIndex != 1)
                            ddl_InterruptionDate.SelectedIndex = 1;
                        ddl_InterruptionDate_SelectedIndexChanged(null, new System.EventArgs());
                        this.txt_initInterruptionDate.Text = item.Valore;
                    }

                    #endregion

                    #region DATA_INTERRUZIONE_PRECEDENTE_IL

                    else if (item.Argomento == DocsPaWR.FiltriElementoLibroFirma.DATA_INTERRUZIONE_PRECEDENTE_IL.ToString())
                    {
                        if (ddl_InterruptionDate.SelectedIndex != 1)
                            ddl_InterruptionDate.SelectedIndex = 1;
                        ddl_InterruptionDate_SelectedIndexChanged(null, new System.EventArgs());
                        this.txt_finedataInterruptionDate.Text = item.Valore;
                    }

                    #endregion

                    #region DATA_INTERRUZIONE_SC

                    else if (item.Argomento == DocsPaWR.FiltriElementoLibroFirma.DATA_INTERRUZIONE_SC.ToString() && item.Valore == "1")
                    {
                        this.ddl_InterruptionDate.SelectedIndex = 3;
                        this.ddl_InterruptionDate_SelectedIndexChanged(null, new System.EventArgs());
                    }

                    #endregion

                    #region DATA_INTERRUZIONE_MC

                    else if (item.Argomento == DocsPaWR.FiltriElementoLibroFirma.DATA_INTERRUZIONE_MC.ToString() && item.Valore == "1")
                    {
                        this.ddl_InterruptionDate.SelectedIndex = 4;
                        this.ddl_InterruptionDate_SelectedIndexChanged(null, new System.EventArgs());
                    }

                    #endregion

                    #region DATA_INTERRUZIONE_TODAY

                    else if (item.Argomento == DocsPaWR.FiltriElementoLibroFirma.DATA_INTERRUZIONE_TODAY.ToString() && item.Valore == "1")
                    {
                        this.ddl_CompletitionDate.SelectedIndex = 2;
                        this.ddl_CompletitionDate_SelectedIndexChanged(null, new System.EventArgs());
                    }

                    #endregion

                    #endregion

                    #region NOTE_INTERRUZIONE

                    else if (item.Argomento == DocsPaWR.FiltriElementoLibroFirma.NOTE_RESPINGIMENTO.ToString())
                    {
                        this.txtNotesInterruption.Text = item.Valore;
                    }

                    #endregion

                    #region STATO

                    else if (item.Argomento == DocsPaWR.FiltriElementoLibroFirma.STATO_IN_ESECUZIONE.ToString())
                    {
                        this.cbxState.Items.FindByValue("IN_EXEC").Selected = Convert.ToBoolean(item.Valore);
                    }
                    else if (item.Argomento == DocsPaWR.FiltriElementoLibroFirma.STATO_CONCLUSO.ToString())
                    {
                        this.cbxState.Items.FindByValue("CLOSED").Selected = Convert.ToBoolean(item.Valore);
                    }
                    else if (item.Argomento == DocsPaWR.FiltriElementoLibroFirma.STATO_INTERROTTO.ToString())
                    {
                        this.cbxState.Items.FindByValue("STOPPED").Selected = Convert.ToBoolean(item.Valore);
                    }
                    #endregion
                }
            }
            catch (Exception e)
            {

            }
        }


        private void ClearFilter()
        {
            this.TreeProcessSignature.Nodes[0].Selected = true;

            this.txt_initStartDate.Text = string.Empty;
            this.txt_finedataStartDate.Text = string.Empty;
            this.ddl_StartDate.SelectedIndex = 0;
            this.ddl_StartDate_SelectedIndexChanged(null, null);
            this.txtNotesSturtup.Text = string.Empty;

            this.txt_object.Text = string.Empty;
            this.txt_initIdDoc.Text = string.Empty;
            this.txt_fineIdDoc.Text = string.Empty;
            this.txt_initIdDoc.ReadOnly = false;
            this.txt_fineIdDoc.Visible = false;
            this.LtlAIdDoc.Visible = false;
            this.LtlDaIdDoc.Visible = false;

            this.txt_initCompletitionDate.Text = string.Empty;
            this.txt_finedataCompletitionDate.Text = string.Empty;
            this.ddl_CompletitionDate.SelectedIndex = 0;
            this.ddl_CompletitionDate_SelectedIndexChanged(null, null);

            this.txt_initInterruptionDate.Text = string.Empty;
            this.txt_finedataInterruptionDate.Text = string.Empty;
            this.ddl_InterruptionDate.SelectedIndex = 0;
            this.ddl_InterruptionDate_SelectedIndexChanged(null, null);
            this.txtNotesInterruption.Text = string.Empty;

            this.opCLOSED.Selected = true;
            this.opIN_EXEC.Selected = true;
            this.opSTOPPED.Selected = true;
            this.opTRUNCATED.Checked = false;

            this.UpFilters.Update();
        }

        #endregion

        #region TreeView

        protected void TreeSignatureProcess_SelectedNodeChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "reallowOp", "reallowOp();", true);
        }

        protected void TreeSignatureProcess_Collapsed(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "reallowOp", "reallowOp();", true);
        }

        private void TreeviewProcesses_Bind(List<ProcessoFirma> listSignatureProcesses)
        {
            try
            {
                foreach (ProcessoFirma p in listSignatureProcesses)
                {
                    this.AddNode(p);
                }
                this.TreeProcessSignature.DataBind();
                this.TreeProcessSignature.CollapseAll();
                this.TreeProcessSignature.Nodes[0].Selected = true;
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return;
            }
        }

        private TreeNode AddNode(ProcessoFirma p)
        {
            TreeNode root = new TreeNode();
            root.Text = p.nome;
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

        #region GridView

        private void GridViewResult_Bind()
        {
            this.gridViewResult.DataSource = this.ListaIstanzaProcessiFirmaFiltered;
            this.gridViewResult.DataBind();
            this.monitoringProcessesResultCount.Text = Utils.Languages.GetLabelFromCode("monitoringProcessesResultCount", UserManager.GetUserLanguage());
            if (this.ListaIstanzaProcessiFirmaFiltered != null)
            {
                this.monitoringProcessesResultCount.Text = this.monitoringProcessesResultCount.Text.Replace("{1}", this.RecordCount.ToString());
            }
            else
            {
                this.monitoringProcessesResultCount.Text = this.monitoringProcessesResultCount.Text.Replace("{1}", "0");
            }
            this.BuildGridNavigator();
            this.HighlightSelectedRow();
            this.UpnlNumeroInstanceStarted.Update();
            this.UpPnlGridView.Update();
        }

        protected void BuildGridNavigator()
        {
            try
            {
                this.plcNavigator.Controls.Clear();

                int countPage = this.PageCount;

                //int val = this.RecordCount % this.PageSize;
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
                    if (this.SelectedPage > 6) startFrom = this.SelectedPage - 5;

                    int endTo = 10;
                    if (this.SelectedPage > 6) endTo = this.SelectedPage + 5;
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
                        if (i == this.SelectedPage)
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

        protected void gridViewResult_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    IstanzaProcessoDiFirma istanzaProcesso = e.Row.DataItem as IstanzaProcessoDiFirma;
                    if (istanzaProcesso.docAll != null && istanzaProcesso.docAll.Equals(ALLEGATO))
                    {
                        (e.Row.FindControl("BtnDocument") as CustomImageButton).ImageUrl = "../Images/Icons/ico_user_attachment.png";
                        (e.Row.FindControl("BtnDocument") as CustomImageButton).ImageUrlDisabled = "../Images/Icons/ico_user_attachment_disabled.png";
                        (e.Row.FindControl("BtnDocument") as CustomImageButton).OnMouseOutImage = "../Images/Icons/ico_user_attachment.png";
                        (e.Row.FindControl("BtnDocument") as CustomImageButton).OnMouseOverImage = "../Images/Icons/ico_user_attachment_hover.png";
                    }
                    else
                    {
                        (e.Row.FindControl("BtnDocument") as CustomImageButton).ImageUrl = "../Images/Icons/ico_previous_details.png";
                        (e.Row.FindControl("BtnDocument") as CustomImageButton).ImageUrlDisabled = "../Images/Icons/ico_previous_details_disabled.png";
                        (e.Row.FindControl("BtnDocument") as CustomImageButton).OnMouseOutImage = "../Images/Icons/ico_previous_details.png";
                        (e.Row.FindControl("BtnDocument") as CustomImageButton).OnMouseOverImage = "../Images/Icons/ico_previous_details_hover.png";
                    }
                }
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return;
            }
        }

        protected void gridViewResult_PreRender(object sender, EventArgs e)
        {

        }

        protected void gridViewResult_ItemCreated(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gridViewResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "viewLinkObject")
            {
                //int rowIndex = Convert.ToInt32(e.CommandArgument);
                string idProfile = (((e.CommandSource as Control).Parent.Parent as GridViewRow).FindControl("idDocumento") as Label).Text;

                if (CheckACL(idProfile))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "parent.fra_main.ajaxDialogModal('RevocationAclIndex', 'warning', '','',null,null,'')", true);
                    return;
                }

                SchedaDocumento schedaDocumento = UIManager.DocumentManager.getDocumentDetails(this, idProfile, idProfile);
                InfoUtente infoUtente = UIManager.UserManager.GetInfoUser();
                string language = UIManager.UserManager.GetUserLanguage();

                #region navigation
                List<Navigation.NavigationObject> navigationList = Navigation.NavigationUtils.GetNavigationList();
                Navigation.NavigationObject actualPage = new Navigation.NavigationObject();
                actualPage.IdObject = idProfile;
                if (!string.IsNullOrEmpty(this.grid_pageindex.Value))
                {
                    this.SelectedPage = int.Parse(this.grid_pageindex.Value);
                }
                actualPage.NumPage = this.SelectedPage.ToString();
                actualPage.PageSize = this.gridViewResult.PageCount.ToString();

                actualPage.NamePage = Navigation.NavigationUtils.GetNamePage(Navigation.NavigationUtils.NamePage.MANAGEMENT_MONITORING_PROCESSES.ToString(), string.Empty);
                actualPage.Link = Navigation.NavigationUtils.GetLink(Navigation.NavigationUtils.NamePage.MANAGEMENT_MONITORING_PROCESSES.ToString(), true, this.Page);
                actualPage.CodePage = Navigation.NavigationUtils.NamePage.MANAGEMENT_MONITORING_PROCESSES.ToString();

                actualPage.Page = "MONITORINGPROCESSES.ASPX";
                navigationList.Add(actualPage);
                Navigation.NavigationUtils.SetNavigationList(navigationList);
                #endregion
                UIManager.DocumentManager.setSelectedRecord(schedaDocumento);
                Response.Redirect("~/Document/Document.aspx");
            }
        }

        protected void gridViewResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.gridViewResult.PageIndex = e.NewPageIndex;
                GridViewResult_Bind();
                this.UpPnlGridView.Update();
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return;
            }
        }

        protected void HighlightSelectedRow()
        {

            if (this.gridViewResult.Rows.Count > 0 && !this.SelectedRow.Equals("-1") && !string.IsNullOrEmpty(this.SelectedRow))
            {
                this.gridViewResult.SelectRow(Convert.ToInt32(this.SelectedRow));
                GridViewRow gvRow = this.gridViewResult.SelectedRow;
                foreach (GridViewRow GVR in this.gridViewResult.Rows)
                {
                    if (GVR == gvRow)
                    {
                        GVR.CssClass += " selectedrow";
                    }
                    else
                    {
                        GVR.CssClass = GVR.CssClass.Replace(" selectedrow", "");
                    }
                }
            }
        }

        protected string GetCompletitionDate(IstanzaProcessoDiFirma istanza)
        {
            string result = string.Empty;
            if (istanza.statoProcesso.Equals(TipoStatoProcesso.CLOSED))
            {
                result = istanza.dataChiusura;
            }
            return result;
        }

        protected string GetInterruptionDate(IstanzaProcessoDiFirma istanza)
        {
            string result = string.Empty;
            if (istanza.statoProcesso.Equals(TipoStatoProcesso.STOPPED))
            {
                result = istanza.dataChiusura;
            }
            return result;
        }

        protected string GetState(IstanzaProcessoDiFirma istanza)
        {
            string result = string.Empty;
            if (!istanza.statoProcesso.Equals(TipoStatoProcesso.CLOSED) && !istanza.statoProcesso.Equals(TipoStatoProcesso.CLOSED_WITH_CUT) && istanza.istanzePassoDiFirma != null && istanza.istanzePassoDiFirma.Length > 0)
            {
                string currentStep = Utils.Languages.GetLabelFromCode("MonitoringProcesseNumStep", UIManager.UserManager.GetUserLanguage()) + " " + istanza.istanzePassoDiFirma[0].numeroSequenza.ToString();
                result = Utils.Languages.GetLabelFromCode(istanza.statoProcesso.ToString(), UIManager.UserManager.GetUserLanguage()) + " (" + currentStep + ")";
            }
            else
            {
                result = Utils.Languages.GetLabelFromCode(istanza.statoProcesso.ToString(), UIManager.UserManager.GetUserLanguage());
            }
            return result;
        }

        protected string GetIdDocNumProto(IstanzaProcessoDiFirma istanza)
        {
            string result = string.Empty;

            if(!string.IsNullOrEmpty(istanza.NumeroProtocollo))
                result = "<span style=\"color:Red; font-weight:bold;\">" + istanza.NumeroProtocollo + "<br />" + istanza.DataProtocollazione + "</span>";
            else if(!string.IsNullOrEmpty(istanza.SegnaturaRepertorio))
                result = "<span style=\"color:Red; font-weight:bold;\">" + istanza.SegnaturaRepertorio + "<br />" + istanza.DataCreazione + "</span>";
            else result = "<span style=\"font-weight:bold;\">" + istanza.docNumber + "<br />" + istanza.DataCreazione + "</span>";

            return result;
        }

        protected string GetObject(IstanzaProcessoDiFirma istanza)
        {
            string result = string.Empty;
            string tmp = istanza.oggetto.Substring(istanza.oggetto.Length - 1);
            if (tmp.Equals("0"))
            { 
                //Non si possiedono diritti di visibilità su documento
                result = Utils.Languages.GetLabelFromCode("MonitoringProcessNoVisibilityObject", UIManager.UserManager.GetUserLanguage());
                //result = "<font color='#808080'>" + result + "</font>";
            }
            else
            {
                result = istanza.oggetto.Replace(istanza.oggetto.Substring(istanza.oggetto.Length - 2), "");
            }
            return result;
        }

        private bool CheckACL(string docnumber)
        {
            bool result = false;
            SchedaDocumento schedaDoc = new SchedaDocumento();
            schedaDoc.systemId = docnumber;
            DocumentManager.setSelectedRecord(schedaDoc);
            result = DocumentManager.CheckRevocationAcl();
            DocumentManager.setSelectedRecord(null);
            return result;
        }
        #endregion
    }
}