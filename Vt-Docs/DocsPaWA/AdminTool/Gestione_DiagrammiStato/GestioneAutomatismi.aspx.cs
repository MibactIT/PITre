using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DocsPAWA.AdminTool.Gestione_DiagrammiStato
{
    public partial class GestioneAutomatismi : System.Web.UI.Page
    {
        private DocsPAWA.DocsPaWR.Stato statoSelezionato = new DocsPAWA.DocsPaWR.Stato();
        private DocsPaWR.Stato[] statiSuccessivi;
        protected System.Data.DataSet ds;
        private string idAmministrazione;

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AMMDATASET"] == null)
            {
                RegisterStartupScript("NoProfilazione", "<script>alert('Attenzione selezionare un\\'amministrazione !'); document.location = '../Gestione_Homepage/Home.aspx';</script>");
                return;
            }
            // ---------------------------------------------------------------

            string[] amministrazione = ((string)Session["AMMDATASET"]).Split('@');
            string codiceAmministrazione = amministrazione[0];
            idAmministrazione = DocsPAWA.Utils.getIdAmmByCod(codiceAmministrazione, this);

            statoSelezionato = (DocsPAWA.DocsPaWR.Stato)Session["statoPerAutomatismi"];
            statiSuccessivi = Session["statiSuccessiviAutomatismi"] as DocsPaWR.Stato[];

            if (!IsPostBack)
            {
                if (Session["statoPerAutomatismi"] != null)
                {
                    statoSelezionato = (DocsPAWA.DocsPaWR.Stato)Session["statoPerAutomatismi"];
                    statiSuccessivi = Session["statiSuccessiviAutomatismi"] as DocsPaWR.Stato[];
                    this.lbl_titolo.Text = "STATO: " + statoSelezionato.DESCRIZIONE;
                    this.LoadGrid();

                    this.CaricaStatiSuccessivi();
                    this.CaricaRagioni();
                    this.CaricaTipologieDoc();
                    this.CaricaTipoEventi();
                }
            }
        }

        protected void btn_add_Click(object sender, EventArgs e)
        {
            this.resetDdl();
            this.pnlAddAutomatismo.Visible = true;
            this.btn_add.Enabled = false;
            this.btn_close.Enabled = false;
        }

        protected void btn_undo_Click(object sender, EventArgs e)
        {
            this.resetDdl();
            this.pnlAddAutomatismo.Visible = false;
            this.btn_add.Enabled = true;
            this.btn_close.Enabled = true;
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            if (this.ddl_eventi.SelectedValue == "0")
            {
                RegisterStartupScript("NoTipoEvento", "<script>alert('Selezionare un tipo evento');</script>");
                return;
            }

            try
            {
                DocsPaWR.CambioStatoAutomatico item = new DocsPaWR.CambioStatoAutomatico();
                item.IdStatoIniziale = ((DocsPAWA.DocsPaWR.Stato)Session["statoPerAutomatismi"]).SYSTEM_ID.ToString();
                item.IdStatoFinale = this.ddl_stati_successivi.SelectedValue;
                item.IdAmm = idAmministrazione;
                item.TipoEvento = new DocsPaWR.EventoCambioStato() { Id = this.ddl_eventi.SelectedValue };

                if (!this.ddl_ragione.SelectedValue.Equals("0"))
                    item.Ragione = new DocsPaWR.RagioneTrasmissione() { systemId = this.ddl_ragione.SelectedValue };
                else
                    item.Ragione = null;
                if (!this.ddl_tipo_doc.SelectedValue.Equals("0"))
                    item.Tipologia = new DocsPaWR.Templates() { SYSTEM_ID = Convert.ToInt32(this.ddl_tipo_doc.SelectedValue) };
                else
                    item.Tipologia = null;

                if (!DiagrammiManager.CreaCambioStatoAutomatico(item, this))
                    throw new Exception();

                this.pnlAddAutomatismo.Visible = false;
                this.btn_add.Enabled = true;
                this.btn_close.Enabled = true;
                this.resetDdl();
                this.LoadGrid();
                this.CaricaStatiSuccessivi();

            }
            catch (Exception ex)
            {
                RegisterStartupScript("ErroreInserimento", "<script>alert('Si è verificato un errore nell\'inserimento del cambio di stato automatico');</script>");
            }


        }

        protected void ddl_eventi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.ddl_eventi.SelectedItem.Text.ToUpper().Contains("TRASMISSIONE"))
            {
                this.ddl_ragione.Enabled = true;
                this.ddl_tipo_doc.Enabled = true;
            }
            else if(this.ddl_eventi.SelectedItem.Text.ToUpper().Contains("MEZZO"))
            {
                this.ddl_ragione.Enabled = false;
                this.ddl_tipo_doc.Enabled = true;
            }
            else if(this.ddl_eventi.SelectedItem.Text.ToUpper().Contains("TERMINI"))
            {
                this.ddl_ragione.Enabled = false;
                this.ddl_tipo_doc.Enabled = false;
            }
        }

        private void resetDdl()
        {
            this.ddl_eventi.SelectedValue = "0";
            this.ddl_ragione.SelectedValue = "0";
            this.ddl_tipo_doc.SelectedValue = "0";
            this.ddl_ragione.Enabled = true;
            this.ddl_tipo_doc.Enabled = true;
        }

        #endregion

        #region Popolamento ddl

        protected void CaricaStatiSuccessivi()
        {
            this.ddl_stati_successivi.Items.Clear();
            if(Session["statiSuccessiviAutomatismi"] != null) 
            {
                DocsPaWR.Stato[] statiSuccessivi = Session["statiSuccessiviAutomatismi"] as DocsPaWR.Stato[];
                if(statiSuccessivi != null && statiSuccessivi.Count() > 0)
                {
                    foreach(DocsPaWR.Stato stato in statiSuccessivi)
                    {
                        if(!this.checkStato(stato.SYSTEM_ID.ToString()))
                            this.ddl_stati_successivi.Items.Add(new ListItem() { Text = stato.DESCRIZIONE, Value = stato.SYSTEM_ID.ToString() });
                    }

                    if (this.ddl_stati_successivi.Items.Count == 0)
                        this.ddl_stati_successivi.Enabled = false;
                }
                else
                {
                    this.ddl_stati_successivi.Enabled = false;
                }
            }
            else
            {
                this.ddl_stati_successivi.Enabled = false;
            }

            this.btn_save.Enabled = this.ddl_stati_successivi.Enabled;
        }

        protected void CaricaRagioni()
        {
            DocsPaWR.OrgRagioneTrasmissione[] ragioni = this.GetRagioniTrasmissione();
            if(ragioni != null && ragioni.Count() > 0)
            {
                this.ddl_ragione.Items.Add(new ListItem() { Text = string.Empty, Value = "0" });
                foreach(DocsPaWR.OrgRagioneTrasmissione ragione in ragioni)
                {
                    this.ddl_ragione.Items.Add(new ListItem() { Text = ragione.Codice, Value = ragione.ID });
                }
            }
            else
            {
                this.ddl_ragione.Enabled = false;
            }
        }

        protected void CaricaTipologieDoc()
        {
            ArrayList tipologie = ProfilazioneDocManager.getTemplates(idAmministrazione, this);
            if(tipologie != null && tipologie.Count > 0)
            {
                this.ddl_tipo_doc.Items.Add(new ListItem() { Text = string.Empty, Value = "0" });
                foreach(DocsPaWR.Templates template in tipologie)
                {
                    this.ddl_tipo_doc.Items.Add(new ListItem() { Text = template.DESCRIZIONE, Value = template.SYSTEM_ID.ToString() });
                }
            }
        }

        protected void CaricaTipoEventi()
        {
            List<DocsPaWR.EventoCambioStato> eventi = DiagrammiManager.GetEventiCambioStato(this);
            if(eventi != null && eventi.Count > 0)
            {
                this.ddl_eventi.Items.Add(new ListItem() { Text = string.Empty, Value = "0" });
                foreach(DocsPaWR.EventoCambioStato evento in eventi)
                {
                    this.ddl_eventi.Items.Add(new ListItem() { Text = evento.Descrizione, Value = evento.Id });
                }
            }
        }

        private DocsPAWA.DocsPaWR.OrgRagioneTrasmissione[] GetRagioniTrasmissione()
        {
            AmmUtils.WebServiceLink ws = new AmmUtils.WebServiceLink();
            return ws.GetRagioniTrasmissione(idAmministrazione);
        }

        private bool checkStato(string id)
        {
            bool result = false;
            if(ds != null && ds.Tables["AUTOMATISMI"] != null && ds.Tables["AUTOMATISMI"].Rows.Count > 0)
            {
                foreach(System.Data.DataRow row in ds.Tables["AUTOMATISMI"].Rows)
                {
                    if(row["ID_STATO"].ToString() == id)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        #endregion

        #region Grid

        protected void LoadGrid()
        {
            this.InitDataSet();
            System.Data.DataRow row;

            List<DocsPaWR.CambioStatoAutomatico> list = DiagrammiManager.GetCambiAutomaticiStato(statoSelezionato.SYSTEM_ID.ToString(), this);
            if(list != null)
            {
                foreach(DocsPaWR.CambioStatoAutomatico item in list)
                {
                    row = ds.Tables["AUTOMATISMI"].NewRow();

                    row["ID_STATO"] = item.IdStatoFinale;
                    row["ID_EVENTO"] = item.TipoEvento.Id;
                    row["DESC_EVENTO"] = item.TipoEvento.Descrizione;

                    if(item.Ragione != null)
                    {
                        row["ID_RAGIONE"] = item.Ragione.systemId;
                        row["DESC_RAGIONE"] = item.Ragione.descrizione;
                    }
                    if(item.Tipologia != null)
                    {
                        row["ID_TIPOLOGIA"] = item.Tipologia.ID_TIPO_ATTO;
                        row["DESC_TIPOLOGIA"] = item.Tipologia.DESCRIZIONE;
                    }

                    DocsPaWR.Stato statoSucc = this.GetStatoById(item.IdStatoFinale);
                    if (statoSucc != null)
                        row["STATO"] = statoSucc.DESCRIZIONE;

                    ds.Tables["AUTOMATISMI"].Rows.Add(row);
                }

                System.Data.DataView dv = ds.Tables["AUTOMATISMI"].DefaultView;
                this.dg_cambiStatoAutomatici.DataSource = dv;
                this.dg_cambiStatoAutomatici.DataBind();
            }
        }

        protected void InitDataSet()
        {
            ds = new System.Data.DataSet();
            ds.Tables.Add("AUTOMATISMI");

            System.Data.DataColumn dc = new System.Data.DataColumn("ID_STATO");
            ds.Tables["AUTOMATISMI"].Columns.Add(dc);

            dc = new System.Data.DataColumn("STATO");
            ds.Tables["AUTOMATISMI"].Columns.Add(dc);

            dc = new System.Data.DataColumn("ID_EVENTO");
            ds.Tables["AUTOMATISMI"].Columns.Add(dc);

            dc = new System.Data.DataColumn("DESC_EVENTO");
            ds.Tables["AUTOMATISMI"].Columns.Add(dc);

            dc = new System.Data.DataColumn("ID_TIPOLOGIA");
            ds.Tables["AUTOMATISMI"].Columns.Add(dc);

            dc = new System.Data.DataColumn("DESC_TIPOLOGIA");
            ds.Tables["AUTOMATISMI"].Columns.Add(dc);

            dc = new System.Data.DataColumn("ID_RAGIONE");
            ds.Tables["AUTOMATISMI"].Columns.Add(dc);

            dc = new System.Data.DataColumn("DESC_RAGIONE");
            ds.Tables["AUTOMATISMI"].Columns.Add(dc);

        }

        private DocsPaWR.Stato GetStatoById(string id)
        {
            DocsPaWR.Stato stato = null;

            if(statiSuccessivi != null && statiSuccessivi.Count() > 0)
            {
                foreach(DocsPaWR.Stato item in statiSuccessivi)
                {
                    if(item.SYSTEM_ID.ToString() == id)
                    {
                        stato = item;
                        break;
                    }
                        
                }
            }

            return stato;
        }



        #endregion

        protected void dg_cambiStatoAutomatici_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            switch(e.CommandName.ToUpper())
            {
                case "DELETE":
                    string idStatoIniziale = ((DocsPAWA.DocsPaWR.Stato)Session["statoPerAutomatismi"]).SYSTEM_ID.ToString();
                    string idStatoFinale = e.Item.Cells[0].Text;
                    DocsPaWR.CambioStatoAutomatico item = new DocsPaWR.CambioStatoAutomatico() { IdStatoIniziale = idStatoIniziale, IdStatoFinale = idStatoFinale };
                    bool result = DiagrammiManager.EliminaCambioStatoAutomatico(item, this);
                    if(result)
                    {
                        this.resetDdl();
                        this.LoadGrid();
                        this.CaricaStatiSuccessivi();
                    }
                    else
                    {
                        RegisterStartupScript("ErroreRimozione", "<script>alert('Si è verificato un errore nella rimozione del cambio di stato automatico');</script>");
                    }
                    break;
            }
        }
    }
}