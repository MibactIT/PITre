using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DocsPAWA.AdminTool.Gestione_ProfDinamicaFasc
{
    public partial class ReplicaInAoo : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.Button btn_conferma;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LoadAmministrazioni();
            }
        }

        private void LoadAmministrazioni()
        {
            // prende tutte le amm.ni disponibili
            DocsPAWA.AdminTool.Manager.AmministrazioneManager amm = new DocsPAWA.AdminTool.Manager.AmministrazioneManager();
            amm.ListaAmministrazioni();

            DocsPAWA.DocsPaWR.InfoAmministrazione[] listaAmministrazioni = (DocsPAWA.DocsPaWR.InfoAmministrazione[]) amm.getListaAmministrazioni().ToArray(typeof(DocsPAWA.DocsPaWR.InfoAmministrazione));
            if (listaAmministrazioni != null && listaAmministrazioni.Length > 0)
            {
                this.dg_listaAoo.DataSource = listaAmministrazioni;
                this.dg_listaAoo.DataBind();
            }
        }

        protected void btn_conferma_Click(object sender, EventArgs e)
        {
            List<string> idAmministrazioniSelezionate = new List<string>();

            try
            {
                for (int i = 0; i < this.dg_listaAoo.Items.Count; i++)
                {
                    string idAmm = dg_listaAoo.Items[i].Cells[0].Text;
                    bool selected = (dg_listaAoo.Items[i].FindControl("cbx_seleziona") as CheckBox).Checked;
                    if(selected)
                        idAmministrazioniSelezionate.Add(idAmm);
                }

                if (idAmministrazioniSelezionate.Count > 0)
                {
                    DocsPAWA.DocsPaWR.Templates template = (DocsPAWA.DocsPaWR.Templates)Session["templateSelPerReplicaFasc"];
                    if (!ProfilazioneFascManager.ReplicaTipoFascicolo(idAmministrazioniSelezionate, template.ID_TIPO_FASC))
                        throw new Exception();

                    RegisterStartupScript("associazioneOk", "<script>alert('Replica effettuata con successo'); </script>");
                    return;
                }
                else
                {
                    RegisterStartupScript("selezionaAmministrazione", "<script>alert('Selezionare le amministrazioni per cui si andrà a replicare la tipologia!'); </script>");
                    return;
                }

                RegisterStartupScript("associazioneOk", "<script>alert('Replica effettuata con successo'); </script>");
                return;
            }
            catch (Exception ex)
            {
                RegisterStartupScript("replicaInAmmErrore", "<script>alert('Si è verificato un errore durante la replica'); </script>");
                return;
            }
        }
    }
}