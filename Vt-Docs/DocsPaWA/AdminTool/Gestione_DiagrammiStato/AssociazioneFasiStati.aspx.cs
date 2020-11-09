using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DocsPAWA.AdminTool.Gestione_DiagrammiStato
{
    public partial class AssociazioneFasiStati : System.Web.UI.Page
    {

        protected System.Data.DataSet ds;

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if(Diagramma != null)
                {
                    lbl_titolo.Text = "DIAGRAMMA : " + Diagramma.DESCRIZIONE;
                    this.CaricaDdlFasi();
                    this.CaricaDdlTipiDoc();
                    this.LoadGrid();
                }
            }
        }

        protected void CaricaDdlFasi()
        {
            List<DocsPaWR.Phases> list = DiagrammiManager.GetFasi(this);
            if(list != null)
            {
                this.ListaFasi = list;
            }
        }

        protected void CaricaDdlTipiDoc()
        {
            string[] amministrazione = ((string)Session["AMMDATASET"]).Split('@');
            string codiceAmministrazione = amministrazione[0];
            string idAmministrazione = DocsPAWA.Utils.getIdAmmByCod(codiceAmministrazione, this);

            List<DocsPaWR.Templates> list = ProfilazioneDocManager.GetTipiDocProcedimentali(idAmministrazione, this);
            if(list != null)
            {
                this.ListaTipiDocProcedimentali = list;
            }
        }

        protected void LoadGrid()
        {
            if(Diagramma != null && Diagramma.STATI != null)
            {
                this.InitDataSet();
                DataRow row;

                List<DocsPaWR.AssPhaseStatoDiagramma> listAss = DiagrammiManager.GetFasiStatiDiagramma(Diagramma.SYSTEM_ID.ToString(), this);

                foreach(DocsPaWR.Stato stato in Diagramma.STATI)
                {
                    row = ds.Tables["STATI"].NewRow();
                    row["STATO"] = stato.DESCRIZIONE;
                    row["ID_STATO"] = stato.SYSTEM_ID.ToString();
                    string idFase;
                    string idTipoDoc;
                    try
                    {
                        idFase = listAss.Where(s => s.STATO.SYSTEM_ID == stato.SYSTEM_ID).FirstOrDefault().PHASE.SYSTEM_ID;
                        idTipoDoc = listAss.Where(s => s.STATO.SYSTEM_ID == stato.SYSTEM_ID).FirstOrDefault().ID_TIPO_DOC;
                    }
                    catch(Exception)
                    {
                        idFase = string.Empty;
                        idTipoDoc = string.Empty;
                    }

                    row["ID_FASE"] = idFase;
                    row["ID_TIPO_DOC"] = idTipoDoc;
                    ds.Tables["STATI"].Rows.Add(row);
                }

                DataView dv = ds.Tables["STATI"].DefaultView;
                //dv.Sort = "oggetto ASC, descrizione ASC";
                dg_AssStatiFasi.DataSource = ds;
                dg_AssStatiFasi.DataBind();
            }
        }

        protected void InitDataSet()
        {
            ds = new System.Data.DataSet();
            ds.Tables.Add("STATI");

            DataColumn dc = new DataColumn("STATO");
            ds.Tables["STATI"].Columns.Add(dc);

            dc = new DataColumn("ID_FASE");
            ds.Tables["STATI"].Columns.Add(dc);

            dc = new DataColumn("ID_STATO");
            ds.Tables["STATI"].Columns.Add(dc);

            dc = new DataColumn("ID_TIPO_DOC");
            ds.Tables["STATI"].Columns.Add(dc);

        }

        #region Session
        public DocsPaWR.DiagrammaStato Diagramma
        {
            get
            {
                return (Session["DiagrammaStato"] as DocsPaWR.DiagrammaStato);
            }
        }

        public List<DocsPaWR.Phases> ListaFasi
        {
            get
            {
                return (Session["ListaFasi"] as List<DocsPaWR.Phases>);
            }
            set
            {
                Session["ListaFasi"] = value;
            }
        }

        public List<DocsPaWR.Templates> ListaTipiDocProcedimentali
        {
            get
            {
                return (Session["ListaTipiDocProcedimentali"] as List<DocsPaWR.Templates>);
            }
            set
            {
                Session["ListaTipiDocProcedimentali"] = value;
            }
        }
        #endregion

        protected void dg_AssStatiFasi_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList ddl = (e.Item.Cells[1].FindControl("ddlFasi") as DropDownList);
                DropDownList ddl2 = (e.Item.Cells[4].FindControl("ddlTipoDoc") as DropDownList);
                ddl.Items.Add(new ListItem() { Text = "--", Value = "0" });
                ddl2.Items.Add(new ListItem() { Text = "--", Value = "0" });
                if(ListaFasi != null)
                {
                    foreach(DocsPaWR.Phases item in ListaFasi)
                    {
                        ddl.Items.Add(new ListItem() { Text = item.DESCRIZIONE, Value = item.SYSTEM_ID });
                    }

                    string idFase = (e.Item.Cells[2].FindControl("hid_fase") as HiddenField).Value;

                    foreach(ListItem item in ddl.Items)
                    {
                        if(item.Value == idFase)
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                }
                if(ListaTipiDocProcedimentali != null)
                {
                    foreach(DocsPaWR.Templates item in ListaTipiDocProcedimentali)
                    {
                        ddl2.Items.Add(new ListItem() { Text = item.DESCRIZIONE, Value = item.SYSTEM_ID.ToString() });
                    }

                    string idTipoDoc = (e.Item.Cells[5].FindControl("hid_tipo_doc") as HiddenField).Value;

                    foreach(ListItem item in ddl2.Items)
                    {
                        if(item.Value == idTipoDoc)
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                }
            }
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            List<DocsPaWR.AssPhaseStatoDiagramma> list = new List<DocsPaWR.AssPhaseStatoDiagramma>();

            try
            {
                for (int i = 0; i < dg_AssStatiFasi.Items.Count; i++)
                {
                    DocsPaWR.AssPhaseStatoDiagramma item = new DocsPaWR.AssPhaseStatoDiagramma();
                    string idFase = (dg_AssStatiFasi.Items[i].Cells[1].FindControl("ddlFasi") as DropDownList).SelectedValue;
                    string idTipoDoc = (dg_AssStatiFasi.Items[i].Cells[4].FindControl("ddlTipoDoc") as DropDownList).SelectedValue;
                    string idStato = dg_AssStatiFasi.Items[i].Cells[3].Text;


                    item.STATO = new DocsPaWR.Stato() { SYSTEM_ID = Convert.ToInt32(idStato) };
                    item.PHASE = new DocsPaWR.Phases() { SYSTEM_ID = idFase };
                    item.ID_TIPO_DOC = (!string.IsNullOrEmpty(idTipoDoc) && idTipoDoc != "0") ? idTipoDoc : string.Empty;
                    list.Add(item);
                  
                }

                if (!DiagrammiManager.SetFasiStatiDiagramma(list, Diagramma.SYSTEM_ID.ToString(), this))
                    throw new Exception();

                RegisterStartupScript("associazioneOk", "<script>alert('Associazione effettuata con successo'); </script>");
                return;
            }
            catch(Exception ex)
            {
                RegisterStartupScript("associazioneError", "<script>alert('Si è verificato un errore nell''associazione degli stati al diagramma delle fasi'); </script>");
                return;
            }
        }
    }
}