using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DocsPAWA.AdminTool.Gestione_DiagrammiStato
{
    public partial class AssociazioneStatiScadenze : System.Web.UI.Page
    {
        DataSet ds;

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if(Diagramma != null)
                {
                    lbl_titolo.Text = "DIAGRAMMA : " + Diagramma.DESCRIZIONE;
                    this.LoadGrid();
                }
            }
        }

        protected void dg_AssStatiScadenze_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            List<DocsPaWR.AssStatoScadenza> list = new List<DocsPaWR.AssStatoScadenza>();
            try
            {
                for (int i = 0; i < dg_AssStatiScadenze.Items.Count; i++)
                {
                    bool sospensivo = (dg_AssStatiScadenze.Items[i].Cells[2].FindControl("ChkSospensivo") as CheckBox).Checked;
                    bool interruttivo = (dg_AssStatiScadenze.Items[i].Cells[3].FindControl("ChkInterruttivo") as CheckBox).Checked;
                    string termini;
                    int val;
                    if(sospensivo)
                    {
                        termini = (dg_AssStatiScadenze.Items[i].Cells[4].FindControl("txt_termini") as TextBox).Text;
                        if(!Int32.TryParse(termini, out val))
                        {
                            RegisterStartupScript("sospNonInt", "<script>alert('Valore non valido per il campo Termini'); </script>");
                            return;
                        }
                        else if(val<=0)
                        {
                            RegisterStartupScript("sospNonValido", "<script>alert('Valore non valido per il campo Termini'); </script>");
                            return;
                        }
                        else
                        {
                            list.Add(new DocsPaWR.AssStatoScadenza() { IdStato = dg_AssStatiScadenze.Items[i].Cells[0].Text, TerminiScadenza = termini, Tipo = "S" });
                        }
                    }
                    else if(interruttivo)
                    {
                        termini = (dg_AssStatiScadenze.Items[i].Cells[4].FindControl("txt_termini") as TextBox).Text;
                        if (!Int32.TryParse(termini, out val))
                        {
                            RegisterStartupScript("intNonInt", "<script>alert('Valore non valido per il campo Termini'); </script>");
                            return;
                        }
                        else if (val <= 0)
                        {
                            RegisterStartupScript("intNonValido", "<script>alert('Valore non valido per il campo Termini'); </script>");
                            return;
                        }
                        else
                        {
                            list.Add(new DocsPaWR.AssStatoScadenza() { IdStato = dg_AssStatiScadenze.Items[i].Cells[0].Text, TerminiScadenza = termini, Tipo = "I" });
                        }
                    }
                }

                if (!DiagrammiManager.SetAssociazioneStatoScadenza(Diagramma.SYSTEM_ID.ToString(), list, this))
                    throw new Exception();

                RegisterStartupScript("assOk", "<script>OkAndClose();</script>");

            }
            catch(Exception ex)
            {
                RegisterStartupScript("assError", "<script>alert('Si è verificato un errore nel salvataggio della configurazione'); </script>");
            }
        }

        protected void LoadGrid()
        {
            if (Diagramma != null && Diagramma.STATI != null)
            {
                this.InitDataSet();
                DataRow row;

                List<DocsPaWR.AssStatoScadenza> list = DiagrammiManager.GetAssociazioneStatoScadenza(Diagramma.SYSTEM_ID.ToString(), this);

                foreach (DocsPaWR.Stato stato in Diagramma.STATI)
                {
                    row = ds.Tables["STATI"].NewRow();
                    row["STATO"] = stato.DESCRIZIONE;
                    row["ID_STATO"] = stato.SYSTEM_ID.ToString();

                    if(list != null && list.Count > 0)
                    {
                        DocsPaWR.AssStatoScadenza item = list.Where(i => i.IdStato == stato.SYSTEM_ID.ToString()).FirstOrDefault();
                        if (item != null)
                        {
                            row["TERMINI"] = item.TerminiScadenza;
                            row["TIPO_STATO"] = item.Tipo;
                        }
                    }

                    ds.Tables["STATI"].Rows.Add(row);
                }

                DataView dv = ds.Tables["STATI"].DefaultView;
                dg_AssStatiScadenze.DataSource = dv;
                dg_AssStatiScadenze.DataBind();
            }
        }
        
        protected void InitDataSet()
        {
            ds = new System.Data.DataSet();
            ds.Tables.Add("STATI");

            DataColumn dc = new DataColumn("STATO");
            ds.Tables["STATI"].Columns.Add(dc);

            dc = new DataColumn("ID_STATO");
            ds.Tables["STATI"].Columns.Add(dc);

            dc = new DataColumn("TIPO_STATO");
            ds.Tables["STATI"].Columns.Add(dc);

            dc = new DataColumn("TERMINI");
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
        #endregion

        protected void ChkSospensivo_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox item = sender as CheckBox;

            if (item.Checked)
            {
                for (int i = 0; i < dg_AssStatiScadenze.Items.Count; i++)
                {
                    CheckBox chkSospensivo = dg_AssStatiScadenze.Items[i].Cells[2].FindControl("ChkSospensivo") as CheckBox;
                    CheckBox chkInterruttivo = dg_AssStatiScadenze.Items[i].Cells[3].FindControl("ChkInterruttivo") as CheckBox;
                    TextBox txt = dg_AssStatiScadenze.Items[i].Cells[4].FindControl("txt_termini") as TextBox;

                    if (chkSospensivo.ClientID != item.ClientID)
                    {
                        //chkSospensivo.Checked = false;
                        //txt.Enabled = chkInterruttivo.Checked;
                        txt.Enabled = chkSospensivo.Checked;
                    }
                    else
                    {
                        chkInterruttivo.Checked = false;
                        txt.Enabled = true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < dg_AssStatiScadenze.Items.Count; i++)
                {
                    CheckBox chkSospensivo = dg_AssStatiScadenze.Items[i].Cells[2].FindControl("ChkSospensivo") as CheckBox;
                    CheckBox chkInterruttivo = dg_AssStatiScadenze.Items[i].Cells[3].FindControl("ChkInterruttivo") as CheckBox;
                    TextBox txt = dg_AssStatiScadenze.Items[i].Cells[4].FindControl("txt_termini") as TextBox;
                    txt.Enabled = chkSospensivo.Checked;
                }
            }

        }

        protected void ChkInterruttivo_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox item = sender as CheckBox;

            if (item.Checked)
            {
                for (int i = 0; i < dg_AssStatiScadenze.Items.Count; i++)
                {
                    CheckBox chkSospensivo = dg_AssStatiScadenze.Items[i].Cells[2].FindControl("ChkSospensivo") as CheckBox;
                    CheckBox chkInterruttivo = dg_AssStatiScadenze.Items[i].Cells[3].FindControl("ChkInterruttivo") as CheckBox;
                    TextBox txt = dg_AssStatiScadenze.Items[i].Cells[4].FindControl("txt_termini") as TextBox;

                    if (chkInterruttivo.ClientID != item.ClientID)
                    {
                        chkInterruttivo.Checked = false;
                        txt.Enabled = chkSospensivo.Checked;
                    }
                    else
                    {
                        chkSospensivo.Checked = false;
                        txt.Enabled = true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < dg_AssStatiScadenze.Items.Count; i++)
                {
                    CheckBox chkSospensivo = dg_AssStatiScadenze.Items[i].Cells[2].FindControl("ChkSospensivo") as CheckBox;
                    TextBox txt = dg_AssStatiScadenze.Items[i].Cells[4].FindControl("txt_termini") as TextBox;
                    txt.Enabled = chkSospensivo.Checked;
                }
            }
        }

        protected bool IsCheckedSosp(object val)
        {
            return val.ToString().ToUpper().Equals("S");
        }

        protected bool IsCheckedInt(object val)
        {
            return val.ToString().ToUpper().Equals("I");
        }
    }
}