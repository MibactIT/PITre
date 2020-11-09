﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NttDataWA.UIManager;
using NttDataWA.DocsPaWR;
using NttDatalLibrary;

namespace NttDataWA.Popup
{
    public partial class StatisticsSignatureProcess : System.Web.UI.Page
    {
        #region Properties

        private string IdProcesso
        {
            get
            {
                return (String)HttpContext.Current.Session["IdProcesso"];
            }
        }

        private List<IstanzaProcessoDiFirma> ListaIstanzaProcessiFirma
        {
            get
            {
                if (HttpContext.Current.Session["ListaIstanzaProcessiFirma"] != null)
                    return (List<IstanzaProcessoDiFirma>)HttpContext.Current.Session["ListaIstanzaProcessiFirma"];
                else
                    return null;
            }
            set
            {
                HttpContext.Current.Session["ListaIstanzaProcessiFirma"] = value;
            }
        }

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

        #endregion

        #region Standard method
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ClearSession();
                this.InitializeLanguage();
                this.InitializaPage();
            }
            SetAjaxAddressBook();
            RefreshScript();
        }

        private void ClearSession()
        {
            HttpContext.Current.Session.Remove("ListaIstanzaProcessiFirma");
            HttpContext.Current.Session.Remove("ListaIstanzaProcessiFirmaFiltered");
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

            this.ListaIstanzaProcessiFirma = LibroFirmaManager.GetIstanzaProcessoDiFirmaByIdProcesso(IdProcesso);
            this.ListaIstanzaProcessiFirmaFiltered = new List<IstanzaProcessoDiFirma>();
            this.ListaIstanzaProcessiFirmaFiltered.AddRange(this.ListaIstanzaProcessiFirma);
            GridViewResult_Bind();

            if (this.ListaIstanzaProcessiFirma != null && this.ListaIstanzaProcessiFirma.Count > 0)
            {
                this.StatisticsSignatureProcessAddFilter.Enabled = true;
            }
            else
            {
                this.StatisticsSignatureProcessAddFilter.Enabled = false;
            }
        }

        private void InitializeLanguage()
        {
            string language = UIManager.UserManager.GetUserLanguage();
            this.StatisticsSignatureProcessAddFilter.Text = Utils.Languages.GetLabelFromCode("StatisticsSignatureProcessAddFilter", language);
            this.StatisticsSignatureProcessRemoveFilter.Text = Utils.Languages.GetLabelFromCode("StatisticsSignatureProcessRemoveFilter", language);
            this.StatisticsSignatureProcessClose.Text = Utils.Languages.GetLabelFromCode("StatisticsSignatureProcessClose", language);
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
            this.ltlProponente.Text = Utils.Languages.GetLabelFromCode("StatisticsSignatureProcessProponent", language);
            this.chkProponenteExtendHistoricized.Text = Utils.Languages.GetLabelFromCode("chkProponenteExtendHistoricized", language);
        }

        private void RefreshScript()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "refreshSelect", "refreshSelect();", true);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "refreshTipsy", "tooltipTipsy();", true);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "refreshPicker", "DatePicker('" + UIManager.UserManager.GetLanguageData() + "');", true);
        }


        protected void SetAjaxAddressBook()
        {
            string dataUser = UIManager.RoleManager.GetRoleInSession().systemId;
            Registro reg = RegistryManager.GetRegistryInSession();
            if (reg == null)
            {
                reg = RoleManager.GetRoleInSession().registri[0];
                RegistryManager.SetRegistryInSession(reg);
            }
            dataUser = dataUser + "-" + reg.systemId;

            string callType = "CALLTYPE_PROTO_IN";
            this.RapidProponente.ContextKey = dataUser + "-" + UIManager.UserManager.GetUserInSession().idAmministrazione + "-" + callType;
        }
        #endregion

        #region Event Button

        protected void StatisticsSignatureProcessClose_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "function", "reallowOp();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "closeMask", "if (parent.fra_main) {parent.fra_main.closeAjaxModal('StatisticsSignatureProcess', '');} else {parent.closeAjaxModal('StatisticsSignatureProcess', '');};", true);
        }

        protected void StatisticsSignatureProcessAddFilter_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "function", "<script>reallowOp();</script>", false);
            try
            {
                this.AddFilter();
                this.gridViewResult.PageIndex = 0;
                GridViewResult_Bind();
                this.StatisticsSignatureProcessRemoveFilter.Enabled = true;
                this.UpPnlGridView.Update();
                this.UpPnlButtons.Update();
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return;
            }
        }

        protected void StatisticsSignatureProcessRemoveFilter_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "function", "<script>reallowOp();</script>", false);
            try
            {
                ClearFilter();
                this.ListaIstanzaProcessiFirmaFiltered.Clear();
                this.ListaIstanzaProcessiFirmaFiltered.AddRange(this.ListaIstanzaProcessiFirma);
                GridViewResult_Bind();
                this.StatisticsSignatureProcessRemoveFilter.Enabled = false;
                this.UpPnlGridView.Update();
                this.UpPnlButtons.Update();
                this.UpPnlFilter.Update();
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

        private void AddFilter()
        {

            this.ListaIstanzaProcessiFirmaFiltered = new List<IstanzaProcessoDiFirma>(); ;
            List<IstanzaProcessoDiFirma> listTemp = new List<IstanzaProcessoDiFirma>();
            listTemp.AddRange(this.ListaIstanzaProcessiFirma);
            #region DATA DI AVVIO

            if (!string.IsNullOrEmpty(this.txt_initStartDate.Text) && !string.IsNullOrEmpty(this.txt_finedataStartDate.Text))
            {
                listTemp = (from istanza in listTemp
                            where Utils.utils.verificaIntervalloDateSenzaOra(Utils.dateformat.ConvertToDate(istanza.dataAttivazione).ToShortDateString(), txt_initStartDate.Text) &&
                                       Utils.utils.verificaIntervalloDateSenzaOra(this.txt_finedataStartDate.Text, Utils.dateformat.ConvertToDate(istanza.dataAttivazione).ToShortDateString())
                                select istanza).ToList();
                ListaIstanzaProcessiFirmaFiltered.AddRange(listTemp);
            }
            else
                if (!string.IsNullOrEmpty(this.txt_initStartDate.Text))
                {
                    listTemp = (from istanza in listTemp
                                where Utils.dateformat.ConvertToDate(istanza.dataAttivazione).ToShortDateString().Equals(this.txt_initStartDate.Text)
                                    select istanza).ToList();
                    ListaIstanzaProcessiFirmaFiltered.AddRange(listTemp);
                }

            #endregion

            #region NOTE DI AVVIO

            if (!string.IsNullOrEmpty(this.txtNotesSturtup.Text))
            {
                listTemp = (from istanza in listTemp
                            where istanza.NoteDiAvvio.ToLower().Contains(this.txtNotesSturtup.Text.ToLower())
                            select istanza).ToList();
                this.ListaIstanzaProcessiFirmaFiltered.Clear();
                ListaIstanzaProcessiFirmaFiltered.AddRange(listTemp);
            }

            #endregion

            #region PROPONENTE

            if (!string.IsNullOrEmpty(this.idProponente.Value))
            {
                listTemp = (from istanza in listTemp
                            where istanza.UtenteProponente.systemId.Equals(this.idProponente.Value) || istanza.RuoloProponente.systemId.Equals(this.idProponente.Value)
                            select istanza).ToList();
                this.ListaIstanzaProcessiFirmaFiltered.Clear();
                ListaIstanzaProcessiFirmaFiltered.AddRange(listTemp);
            }

            #endregion

            #region DATA CONCLUSIONE

            if (!string.IsNullOrEmpty(this.txt_initCompletitionDate.Text) && !string.IsNullOrEmpty(this.txt_finedataCompletitionDate.Text))
            {
                listTemp = (from istanza in listTemp
                            where !string.IsNullOrEmpty(istanza.dataChiusura) && Utils.utils.verificaIntervalloDateSenzaOra(Utils.dateformat.ConvertToDate(istanza.dataChiusura).ToShortDateString(), txt_initCompletitionDate.Text) &&
                                       Utils.utils.verificaIntervalloDateSenzaOra(this.txt_finedataCompletitionDate.Text, Utils.dateformat.ConvertToDate(istanza.dataChiusura).ToShortDateString())
                            select istanza).ToList();
                this.ListaIstanzaProcessiFirmaFiltered.Clear();
                ListaIstanzaProcessiFirmaFiltered.AddRange(listTemp);
            }
            else
                if (!string.IsNullOrEmpty(this.txt_initCompletitionDate.Text))
                {
                    listTemp = (from istanza in listTemp
                                where !string.IsNullOrEmpty(istanza.dataChiusura) && Utils.dateformat.ConvertToDate(istanza.dataChiusura).ToShortDateString().Equals(this.txt_initCompletitionDate.Text)
                                select istanza).ToList();
                    this.ListaIstanzaProcessiFirmaFiltered.Clear();
                    ListaIstanzaProcessiFirmaFiltered.AddRange(listTemp);
                }

            #endregion

            #region DATA INTERRUZIONE

            if (!string.IsNullOrEmpty(this.txt_initInterruptionDate.Text) && !string.IsNullOrEmpty(this.txt_finedataInterruptionDate.Text))
            {
                listTemp = (from istanza in listTemp
                            where !string.IsNullOrEmpty(istanza.dataChiusura) && Utils.utils.verificaIntervalloDateSenzaOra(Utils.dateformat.ConvertToDate(istanza.dataChiusura).ToShortDateString(), this.txt_initInterruptionDate.Text) &&
                                       Utils.utils.verificaIntervalloDateSenzaOra(this.txt_finedataInterruptionDate.Text, Utils.dateformat.ConvertToDate(istanza.dataChiusura).ToShortDateString())
                            select istanza).ToList();
                this.ListaIstanzaProcessiFirmaFiltered.Clear();
                ListaIstanzaProcessiFirmaFiltered.AddRange(listTemp);
            }
            else
                if (!string.IsNullOrEmpty(this.txt_initInterruptionDate.Text))
                {
                    listTemp = (from istanza in listTemp
                                where !string.IsNullOrEmpty(istanza.dataChiusura) && Utils.dateformat.ConvertToDate(istanza.dataChiusura).ToShortDateString().Equals(this.txt_initInterruptionDate.Text)
                                select istanza).ToList();
                    this.ListaIstanzaProcessiFirmaFiltered.Clear();
                    ListaIstanzaProcessiFirmaFiltered.AddRange(listTemp);
                }

            #endregion

            #region NOTE INTERRUZIONE

            if (!string.IsNullOrEmpty(this.txtNotesInterruption.Text))
            {
                listTemp = (from istanza in listTemp
                            where istanza.MotivoRespingimento.ToLower().Contains(this.txtNotesInterruption.Text.ToLower())
                            select istanza).ToList();
                this.ListaIstanzaProcessiFirmaFiltered.Clear();
                ListaIstanzaProcessiFirmaFiltered.AddRange(listTemp);
            }

            #endregion

            #region STATO
            if (this.opCLOSED.Selected || this.opIN_EXEC.Selected || this.opSTOPPED.Selected)
            {
                this.ListaIstanzaProcessiFirmaFiltered.Clear();
                if (this.opCLOSED.Selected)
                {
                    this.ListaIstanzaProcessiFirmaFiltered.AddRange((from istanza in listTemp
                                where istanza.statoProcesso.Equals(TipoStatoProcesso.CLOSED)
                                select istanza).ToList());
                }
                if (this.opIN_EXEC.Selected)
                {
                    this.ListaIstanzaProcessiFirmaFiltered.AddRange((from istanza in listTemp
                                where istanza.statoProcesso.Equals(TipoStatoProcesso.IN_EXEC)
                                select istanza).ToList());
                }
                if (this.opSTOPPED.Selected)
                {
                    this.ListaIstanzaProcessiFirmaFiltered.AddRange((from istanza in listTemp
                                                                     where istanza.statoProcesso.Equals(TipoStatoProcesso.STOPPED)
                                                                     select istanza).ToList());
                }
            }

            this.ListaIstanzaProcessiFirmaFiltered = (from e in ListaIstanzaProcessiFirmaFiltered orderby Utils.dateformat.ConvertToDate(e.dataAttivazione) descending select e).ToList<IstanzaProcessoDiFirma>();
            #endregion

        }

        private void ClearFilter()
        {
            this.txt_initStartDate.Text = string.Empty;
            this.txt_finedataStartDate.Text = string.Empty;
            this.ddl_StartDate.SelectedIndex = 0;
            this.ddl_StartDate_SelectedIndexChanged(null, null);
            this.txtNotesSturtup.Text = string.Empty;


            this.txt_initCompletitionDate.Text = string.Empty;
            this.txt_finedataCompletitionDate.Text = string.Empty;
            this.ddl_CompletitionDate.SelectedIndex = 0;
            this.ddl_CompletitionDate_SelectedIndexChanged(null, null);

            this.txt_initInterruptionDate.Text = string.Empty;
            this.txt_finedataInterruptionDate.Text = string.Empty;
            this.ddl_InterruptionDate.SelectedIndex = 0;
            this.ddl_InterruptionDate_SelectedIndexChanged(null, null);
            this.txtNotesInterruption.Text = string.Empty;

            this.txtCodiceProponente.Text = string.Empty;
            this.txtDescrizioneProponente.Text = string.Empty;
            this.idProponente.Value = string.Empty;

            this.opCLOSED.Selected = true;
            this.opIN_EXEC.Selected = true;
            this.opSTOPPED.Selected = true;

            this.UpdPnlProponente.Update();
        }

        protected void TxtCode_OnTextChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "function", "<script>reallowOp();</script>", false);
            try
            {
                CustomTextArea caller = sender as CustomTextArea;
                string codeAddressBook = caller.Text;

                if (!string.IsNullOrEmpty(codeAddressBook))
                {
                    RubricaCallType calltype = RubricaCallType.CALLTYPE_CORR_INT_NO_UO;
                    if (this.chkProponenteExtendHistoricized.Checked)
                    {
                        calltype = RubricaCallType.CALLTYPE_CORR_INT_EST_CON_DISABILITATI;
                    }
                    else
                    {
                        calltype = RubricaCallType.CALLTYPE_CORR_INT_EST;
                    }
                    Corrispondente corr = null;
                    corr = UIManager.AddressBookManager.getCorrispondenteRubrica(codeAddressBook, calltype);

                    if (corr == null)
                    {
                        switch (caller.ID)
                        {
                            case "txtCodiceProponente":
                                this.txtCodiceProponente.Text = string.Empty;
                                this.txtDescrizioneProponente.Text = string.Empty;
                                this.idProponente.Value = string.Empty;
                                this.UpdPnlProponente.Update();
                                break;
                        }

                        string msg = "ErrorTransmissionCorrespondentNotFound";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ajaxDialogModal", "if (parent.fra_main) {parent.fra_main.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'warning', '');} else {parent.ajaxDialogModal('" + msg.Replace("'", @"\'") + "', 'warning', '');}", true);
                    }
                    else
                    {
                        switch (caller.ID)
                        {
                            case "txtCodiceProponente":
                                this.txtCodiceProponente.Text = corr.codiceRubrica;
                                this.txtDescrizioneProponente.Text = corr.descrizione;
                                this.idProponente.Value = corr.systemId;
                                this.UpdPnlProponente.Update();
                                break;
                        }
                    }
                }
                else
                {
                    switch (caller.ID)
                    {
                        case "txtCodiceProponente":
                            this.txtCodiceProponente.Text = string.Empty;
                            this.txtDescrizioneProponente.Text = string.Empty;
                            this.idProponente.Value = string.Empty;
                            this.UpdPnlProponente.Update();
                            break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return;
            }
        }

        protected void chkProponenteExtendHistoricized_Click(object sender, EventArgs e)
        {
            string dataUser = RoleManager.GetRoleInSession().systemId;
            Registro reg = RegistryManager.GetRegistryInSession();
            if (reg == null)
            {
                reg = RoleManager.GetRoleInSession().registri[0];
            }
            dataUser = dataUser + "-" + reg.systemId;
            string callType = string.Empty;

            if (this.chkProponenteExtendHistoricized.Checked)
            {
                callType = "CALLTYPE_CORR_INT_EST_CON_DISABILITATI";
            }
            else
            {
                callType = "CALLTYPE_CORR_INT_EST";
            }
            this.RapidProponente.ContextKey = dataUser + "-" + UIManager.UserManager.GetUserInSession().idAmministrazione + "-" + callType;

            this.UpdPnlProponente.Update();
        }

        protected void ImgAddressBookProponente_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (this.chkProponenteExtendHistoricized.Checked)
                {
                    this.CallType = RubricaCallType.CALLTYPE_CORR_INT_EST_CON_DISABILITATI;
                }
                else
                {
                    this.CallType = RubricaCallType.CALLTYPE_CORR_INT_EST;
                }
                HttpContext.Current.Session["AddressBook.from"] = "F_X_X_S_4";
                //OpenAddressBookFromFilter = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "AddressBook", "parent.ajaxModalPopupAddressBook();", true);
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return;
            }
        }


        protected void btnAddressBookPostback_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "function", "<script>reallowOp();</script>", false);
                List<NttDataWA.Popup.AddressBook.CorrespondentDetail> atList = (List<NttDataWA.Popup.AddressBook.CorrespondentDetail>)HttpContext.Current.Session["AddressBook.At"];
                List<NttDataWA.Popup.AddressBook.CorrespondentDetail> ccList = (List<NttDataWA.Popup.AddressBook.CorrespondentDetail>)HttpContext.Current.Session["AddressBook.Cc"];
                string addressBookCallFrom = HttpContext.Current.Session["AddressBook.from"].ToString();

                switch (addressBookCallFrom)
                {                 
                    case "F_X_X_S_4":
                        {
                            if (atList != null && atList.Count > 0)
                            {
                                NttDataWA.Popup.AddressBook.CorrespondentDetail corrInSess = atList[0];
                                Corrispondente tempCorrSingle;
                                if (!corrInSess.isRubricaComune)
                                    tempCorrSingle = UIManager.AddressBookManager.GetCorrespondentBySystemId(atList[0].SystemID);
                                else
                                    tempCorrSingle = UIManager.AddressBookManager.getCorrispondenteByCodRubricaRubricaComune(corrInSess.CodiceRubrica);

                                this.txtCodiceProponente.Text = tempCorrSingle.codiceRubrica;
                                this.txtDescrizioneProponente.Text = tempCorrSingle.descrizione;
                                this.idProponente.Value = tempCorrSingle.systemId;
                                this.UpdPnlProponente.Update();
                            }
                        }
                        break;
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

        #endregion

        #region GridView

        private void GridViewResult_Bind()
        {
            this.gridViewResult.DataSource = this.ListaIstanzaProcessiFirmaFiltered;
            this.gridViewResult.DataBind();
        }

        protected void gridViewResult_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gridViewResult_PreRender(object sender, EventArgs e)
        {

        }

        protected void gridViewResult_ItemCreated(Object sender, GridViewRowEventArgs e)
        {

        }

        protected void gridViewResult_RowCommand(Object sender, GridViewCommandEventArgs e)
        {

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

        protected string GetProponent(IstanzaProcessoDiFirma istanza)
        {
            return istanza.UtenteProponente.descrizione + " (" + istanza.RuoloProponente.descrizione + ")";
        }

        protected string GetCompletitionDate(IstanzaProcessoDiFirma istanza)
        {
            string result = string.Empty;
            if (istanza.statoProcesso.Equals(TipoStatoProcesso.CLOSED))
            {
                result = Utils.dateformat.ConvertToDate(istanza.dataChiusura).ToShortDateString();
            }
            return result;
        }

        protected string GetInterruptionDate(IstanzaProcessoDiFirma istanza)
        {
            string result = string.Empty;
            if (istanza.statoProcesso.Equals(TipoStatoProcesso.STOPPED))
            {
                result = Utils.dateformat.ConvertToDate(istanza.dataChiusura).ToShortDateString();
            }
            return result;
        }

        protected string GetState(IstanzaProcessoDiFirma istanza)
        {
            return Utils.Languages.GetLabelFromCode(istanza.statoProcesso.ToString(), UIManager.UserManager.GetUserLanguage());
        }
        #endregion
    }
}