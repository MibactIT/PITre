using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using DocsPaVO.Procedimento;
using log4net;

namespace DocsPaDB.Query_DocsPAWS
{
    public class Procedimenti : DBProvider
    {

        private ILog logger = LogManager.GetLogger(typeof(Procedimenti));

        #region Insert
        public bool InsertDoc(string idProject, string idDoc, string idCorrGlobali, bool isDocPrincipale, string idProcedimento, bool visualizzato)
        {
            logger.Debug("BEGIN");
            bool result = false;

            try
            {
                DocsPaUtils.Query query = DocsPaUtils.InitQuery.getInstance().getQuery("I_DPA_PROCEDIMENTI");
                query.setParam("idProject", idProject);
                query.setParam("idProfile", idDoc);
                query.setParam("idCorrGlobali", idCorrGlobali);
                query.setParam("idProcedimento", idProcedimento);
                if (isDocPrincipale)
                {
                    query.setParam("docPrinc", "1");
                    query.setParam("visualizzato", "1");
                }
                else
                {
                    query.setParam("docPrinc", "0");
                    query.setParam("visualizzato", visualizzato ? "1" : "0");
                }
                string command = query.getSQL();
                logger.Debug("QUERY: " + command);

                if (!ExecuteNonQuery(command))
                {
                    throw new Exception(this.LastExceptionMessage);
                }
                result = true;
            }
            catch (Exception ex)
            {
                logger.Debug("Errore in Procedimenti>InsertDoc ", ex);
            }

            logger.Debug("END");
            return result;
        }

        public bool InsertFaseProcedimento(string idProject, string idStato)
        {
            bool result = false;
            logger.Debug("BEGIN");

            try
            {
                DocsPaUtils.Query query = DocsPaUtils.InitQuery.getInstance().getQuery("I_DPA_PROCEDIMENTI_FASI");
                query.setParam("id_project", idProject);
                query.setParam("id_stato", idStato);

                string command = query.getSQL();
                logger.Debug("QUERY: " + command);

                if (!this.ExecuteNonQuery(command))
                {
                    throw new Exception(this.LastExceptionMessage);
                }
                result = true;
            }
            catch (Exception ex)
            {
                logger.Debug("Errore in Procedimenti>InsertFaseProcedimento", ex);
            }

            logger.Debug("END");
            return result;
        }
        #endregion

        #region Update
        public bool UpdateStato(string idDoc, string idProject)
        {
            logger.Debug("BEGIN");
            bool result = false;

            try
            {
                DocsPaUtils.Query query = DocsPaUtils.InitQuery.getInstance().getQuery("U_DPA_PROCEDIMENTI");
                string whereCond = " ID_PROFILE=" + idDoc;
                if (!string.IsNullOrEmpty(idProject))
                {
                    whereCond = whereCond + " AND ID_PROJECT=" + idProject;
                }
                query.setParam("cond", whereCond);

                string command = query.getSQL();
                logger.Debug("QUERY: " + command);

                if (!ExecuteNonQuery(command))
                {
                    throw new Exception(this.LastExceptionMessage);
                }

                result = true;
            }
            catch (Exception ex)
            {
                logger.Debug("Errore in Procedimenti>UpdateStato ", ex);
            }

            logger.Debug("END");
            return result;
        }

        public bool UpdateDocReindirizzamento(string oldIdProject, string newIdProject, string newIdReg)
        {
            logger.Debug("BEGIN");
            bool result = false;

            try
            {
                DocsPaUtils.Query query = DocsPaUtils.InitQuery.getInstance().getQuery("U_DPA_PROCEDIMENTI_REINDIRIZZAMENTO");
                query.setParam("old_id_project", oldIdProject);
                query.setParam("new_id_project", newIdProject);
                query.setParam("new_id_aoo", newIdReg);
                string command = query.getSQL();

                logger.Debug("QUERY - " + query);
                if (!this.ExecuteNonQuery(command))
                    throw new Exception(this.LastExceptionMessage);

                result = true;
            }
            catch(Exception ex)
            {
                logger.Debug(ex);
                result = false;
            }

            logger.Debug("END");
            return result;
        }
        #endregion

        #region Select
        public bool IsDocVisualizzato(string idProcedimento, string idDoc)
        {
            logger.Debug("BEGIN");
            bool result = false;
            try
            {
                DocsPaUtils.Query query = DocsPaUtils.InitQuery.getInstance().getQuery("S_GET_STATO_VIS_DOC_PROC");
                query.setParam("id_procedimento", idProcedimento);
                query.setParam("id_doc", idDoc);
                string command = query.getSQL();
                logger.Debug("QUERY: " + command);

                string retVal = string.Empty;
                if (!this.ExecuteScalar(out retVal, command))
                {
                    throw new Exception(this.LastExceptionMessage);
                }
                result = (retVal == "1");

            }
            catch (Exception ex)
            {
                logger.Debug("Errore in Procedimenti>IsDocVisualizzato ", ex);
            }
            logger.Debug("END");
            return result;
        }

        public bool IsFolderInProceeding(string idFolder)
        {
            logger.Debug("BEGIN");
            bool result = false;

            try
            {
                DocsPaUtils.Query query = DocsPaUtils.InitQuery.getInstance().getQuery("S_IS_FOLDER_IN_PROCEEDING");
                query.setParam("idFolder", idFolder);

                string command = query.getSQL();
                logger.Debug("QUERY: " + command);

                string str = string.Empty;
                int counter;
                if (!this.ExecuteScalar(out str, command))
                {
                    throw new Exception(this.LastExceptionMessage);
                }

                if (Int32.TryParse(str, out counter))
                {
                    result = counter > 0;
                }
            }
            catch (Exception ex)
            {
                logger.Debug("Errore in Procedimenti>IsFolderInProceeding", ex);
            }

            logger.Debug("END");
            return result;
        }

        public List<Procedimento> GetProcedimentiNonVisualizzati(string idCorrGlobali)
        {
            logger.Debug("BEGIN");
            List<Procedimento> retVal = new List<Procedimento>();

            try
            {
                DocsPaUtils.Query query = DocsPaUtils.InitQuery.getInstance().getQuery("S_PROC_GET_DOC_NON_VISUALIZZATI");
                query.setParam("idCorrGlobali", idCorrGlobali);

                string command = query.getSQL();
                logger.Debug("QUERY: " + command);

                DataSet ds = new DataSet();
                if (this.ExecuteQuery(out ds, command))
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            Procedimento item = new Procedimento();
                            item.Id = row["ID_PROJECT"] != DBNull.Value ? row["ID_PROJECT"].ToString() : string.Empty;
                            item.IdEsterno = row["ID_ESTERNO"] != DBNull.Value ? row["ID_ESTERNO"].ToString() : string.Empty;
                            item.Descrizione = row["DESCRIPTION"] != DBNull.Value ? row["DESCRIPTION"].ToString() : string.Empty;
                            item.Documenti = row["UNREAD_DOCS"] != DBNull.Value ? new DocumentoProcedimento[Convert.ToInt32(row["UNREAD_DOCS"])] : new DocumentoProcedimento[0];
                            retVal.Add(item);
                        }
                    }
                }
                else
                {
                    throw new Exception(this.LastExceptionMessage);
                }
            }
            catch (Exception ex)
            {
                logger.Debug("Errore in Procedimenti>GetProcedimentiNonVisualizzati ", ex);
                retVal = null;
            }

            logger.Debug("END");
            return retVal;
        }

        public Procedimento GetProcedimentoByIdFascicolo(string idFascicolo)
        {
            logger.Debug("BEGIN");
            Procedimento proc = new Procedimento();

            try
            {
                DocsPaUtils.Query query = DocsPaUtils.InitQuery.getInstance().getQuery("S_GET_PROCEDIMENTO");
                query.setParam("fromCond", string.Empty);
                query.setParam("whereCond", "A.ID_PROJECT=" + idFascicolo);
                string command = query.getSQL();

                logger.Debug("QUERY: " + command);
                DataSet ds = new DataSet();

                if (!this.ExecuteQuery(out ds, command))
                {
                    throw new Exception(this.LastExceptionMessage);
                }
                else
                {
                    if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        proc.Id = ds.Tables[0].Rows[0]["ID_PROJECT"].ToString();
                        proc.IdEsterno = ds.Tables[0].Rows[0]["ID_ESTERNO"].ToString();
                        proc.Autore = ds.Tables[0].Rows[0]["ID_CORR_GLOBALI"].ToString();
                        proc.Descrizione = ds.Tables[0].Rows[0]["DESCRIPTION"].ToString();
                        proc.Documenti = new DocumentoProcedimento[ds.Tables[0].Rows.Count];
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            proc.Documenti[i] = new DocumentoProcedimento()
                            {
                                Id = ds.Tables[0].Rows[i]["ID_PROFILE"].ToString(),
                                DataVisualizzazione = ds.Tables[0].Rows[i]["DATA_VISUALIZZAZIONE"].ToString()
                            };

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Debug("Errore in Procedimenti>GetProcedimentoByIdFascicolo ", ex);
                proc = null;
            }

            logger.Debug("END");
            return proc;
        }

        public Procedimento GetProcedimentoByIdFolder(string idFolder)
        {
            logger.Debug("BEGIN");
            Procedimento proc = new Procedimento();

            try
            {
                DocsPaUtils.Query query = DocsPaUtils.InitQuery.getInstance().getQuery("S_GET_PROCEDIMENTO");
                query.setParam("fromCond", ", PROJECT B");
                query.setParam("whereCond", " A.ID_PROJECT=B.ID_FASCICOLO AND B.SYSTEM_ID=" + idFolder);
                string command = query.getSQL();

                logger.Debug("QUERY: " + command);
                DataSet ds = new DataSet();

                if (!this.ExecuteQuery(out ds, command))
                {
                    throw new Exception(this.LastExceptionMessage);
                }
                else
                {
                    if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        proc.Id = ds.Tables[0].Rows[0]["ID_PROJECT"].ToString();
                        proc.IdEsterno = ds.Tables[0].Rows[0]["ID_ESTERNO"].ToString();
                        proc.Autore = ds.Tables[0].Rows[0]["ID_CORR_GLOBALI"].ToString();
                        proc.Descrizione = ds.Tables[0].Rows[0]["DESCRIPTION"].ToString();
                        proc.Documenti = new DocumentoProcedimento[ds.Tables[0].Rows.Count];
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            proc.Documenti[i] = new DocumentoProcedimento()
                            {
                                Id = ds.Tables[0].Rows[i]["ID_PROFILE"].ToString(),
                                DataVisualizzazione = ds.Tables[0].Rows[i]["DATA_VISUALIZZAZIONE"].ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Debug("Errore in Procedimenti>GetProcedimentoByIdFolder ", ex);
                proc = null;
            }

            logger.Debug("END");
            return proc;
        }

        public Procedimento GetProcedimentoByIdEsterno(string idProcedimento)
        {
            logger.Debug("BEGIN");
            Procedimento proc = new Procedimento();
            proc.Id = "0";

            try
            {
                DocsPaUtils.Query query = DocsPaUtils.InitQuery.getInstance().getQuery("S_GET_PROCEDIMENTO");
                query.setParam("fromCond", string.Empty);
                query.setParam("whereCond", "A.ID_ESTERNO=" + idProcedimento);
                string command = query.getSQL();

                logger.Debug("QUERY: " + command);
                DataSet ds = new DataSet();

                if (!this.ExecuteQuery(out ds, command))
                {
                    throw new Exception(this.LastExceptionMessage);
                }
                else
                {
                    if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        proc.Id = ds.Tables[0].Rows[0]["ID_PROJECT"].ToString();
                        proc.IdEsterno = ds.Tables[0].Rows[0]["ID_ESTERNO"].ToString();
                        proc.Autore = ds.Tables[0].Rows[0]["ID_CORR_GLOBALI"].ToString();
                        proc.Descrizione = ds.Tables[0].Rows[0]["DESCRIPTION"].ToString();
                        proc.Documenti = new DocumentoProcedimento[ds.Tables[0].Rows.Count];
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            proc.Documenti[i] = new DocumentoProcedimento()
                            {
                                Id = ds.Tables[0].Rows[i]["ID_PROFILE"].ToString(),
                                DataVisualizzazione = ds.Tables[0].Rows[i]["DATA_VISUALIZZAZIONE"].ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Debug("Errore in Procedimenti>GetProcedimentoByIdEsterno", ex);
                proc = null;
            }

            logger.Debug("END");
            return proc;
        }

        public Procedimento GetProcedimentoByIdDoc(string idDoc)
        {
            logger.Debug("BEGIN");
            Procedimento proc = new Procedimento();

            try
            {
                DocsPaUtils.Query query = DocsPaUtils.InitQuery.getInstance().getQuery("S_GET_PROCEDIMENTO");
                query.setParam("fromCond", string.Empty);
                query.setParam("whereCond", "A.ID_PROFILE=" + idDoc);
                string command = query.getSQL();

                logger.Debug("QUERY: " + command);
                DataSet ds = new DataSet();

                if (!this.ExecuteQuery(out ds, command))
                {
                    throw new Exception(this.LastExceptionMessage);
                }
                else
                {
                    if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        proc.Id = ds.Tables[0].Rows[0]["ID_PROJECT"].ToString();
                        proc.IdEsterno = ds.Tables[0].Rows[0]["ID_ESTERNO"].ToString();
                        proc.Autore = ds.Tables[0].Rows[0]["ID_CORR_GLOBALI"].ToString();
                        proc.Descrizione = ds.Tables[0].Rows[0]["DESCRIPTION"].ToString();
                        proc.Documenti = new DocumentoProcedimento[1];
                        proc.Documenti[0] = new DocumentoProcedimento()
                        {
                            Id = ds.Tables[0].Rows[0]["ID_PROFILE"].ToString(),
                            DataVisualizzazione = ds.Tables[0].Rows[0]["DATA_VISUALIZZAZIONE"].ToString()
                        };
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Debug("Errore in Procedimenti>GetProcedimentoByIdDoc", ex);
                proc = null;
            }

            logger.Debug("END");
            return proc;
        }

        public string GetIdIstanzaProcedimento(string idProject)
        {
            logger.Debug("BEGIN");
            string result = string.Empty;

            try
            {
                DocsPaUtils.Query query = DocsPaUtils.InitQuery.getInstance().getQuery("S_GET_PROCEDIMENTO_ISTANZA");
                query.setParam("id_project", idProject);

                string command = query.getSQL();
                logger.Debug("QUERY - " + query);

                if (!this.ExecuteScalar(out result, command))
                    throw new Exception(this.LastExceptionMessage);
            }
            catch(Exception ex)
            {
                logger.Debug("Errore in GetIdIstanzaProcedimento - ", ex);
                result = string.Empty;
            }

            return result;
        }

        public string GetIdTemplateDocByDescProcedimento(string descrizione, string idAmm)
        {
            logger.Debug("BEGIN");
            string result = string.Empty;

            try
            {
                DocsPaUtils.Query query = DocsPaUtils.InitQuery.getInstance().getQuery("S_GET_TEMPLATE_DOC_ID_BY_DESCRIZIONE");
                query.setParam("descrizione", descrizione.Replace("'", "''"));
                query.setParam("id_amm", idAmm);
                string command = query.getSQL();

                logger.Debug("QUERY - " + query);
                if (!this.ExecuteScalar(out result, command))
                    throw new Exception(this.LastExceptionMessage);
            }
            catch (Exception ex)
            {
                logger.Debug("Errore in GetIdTemplateDocByDescProcedimento - ", ex);
                result = null;
            }

            logger.Debug("END");
            return result;
        }

        public string GetIdTemplateFascByDescProcedimento(string descrizione, string idAmm)
        {
            logger.Debug("BEGIN");
            string result = string.Empty;

            try
            {
                DocsPaUtils.Query query = DocsPaUtils.InitQuery.getInstance().getQuery("S_GET_TEMPLATE_FASC_ID_BY_DESCRIZIONE");
                query.setParam("descrizione", descrizione.Replace("'", "''"));
                query.setParam("id_amm", idAmm);
                string command = query.getSQL();

                logger.Debug("QUERY - " + command);
                if (!this.ExecuteScalar(out result, command))
                    throw new Exception(this.LastExceptionMessage);
            }
            catch(Exception ex)
            {
                logger.Debug("Errore in GetIdTemplateFascByDescProcedimento - ", ex);
                result = null;
            }

            logger.Debug("END");
            return result;
        }

        public EsitoProcedimento GetEsitoProcedimento(string idFascicolo)
        {
            logger.Debug("BEGIN");
            EsitoProcedimento esito = EsitoProcedimento.InCorso;

            try
            {
                DocsPaUtils.Query query = DocsPaUtils.InitQuery.getInstance().getQuery("S_ESITO_PROCEDIMENTO");
                query.setParam("id_project", idFascicolo);
                string command = query.getSQL();

                logger.Debug("QUERY: " + command);
                string result = string.Empty;

                if (!this.ExecuteScalar(out result, command))
                {
                    throw new Exception(this.LastExceptionMessage);
                }

                if (!string.IsNullOrEmpty(result))
                {
                    if (result == "1")
                    {
                        esito = EsitoProcedimento.Positivo;
                    }
                    else
                    {
                        esito = EsitoProcedimento.Negativo;
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Debug("Errore in Procedimenti>GetEsitoProcedimento ", ex);
            }


            logger.Debug("END");
            return esito;
        }

        public string GetIdPerCambioStato(string evento, string idOggetto)
        {
            logger.Debug("BEGIN");
            string result = string.Empty;

            try
            {
                DocsPaUtils.Query query = DocsPaUtils.InitQuery.getInstance().getQuery("S_PROCEDIMENTO_CAMBIO_STATO");

                query.setParam("evento", evento);
                query.setParam("id_obj", idOggetto);

                string command = query.getSQL();
                logger.Debug("QUERY: " + query);

                if (!this.ExecuteScalar(out result, command))
                    throw new Exception(this.LastExceptionMessage);
            }
            catch (Exception ex)
            {
                logger.Debug("Errore in Procedimenti>GetIdPerCambioStato", ex);
            }

            logger.Debug("END");
            return result;
        }

        public string[] GetTipiProcedimentoAmministrazione(string idAmm)
        {
            logger.Debug("BEGIN");
            string[] result = null;

            try
            {
                DocsPaUtils.Query query = DocsPaUtils.InitQuery.getInstance().getQuery("S_GET_PROCEDIMENTI_AMM");
                query.setParam("id_amm", idAmm);

                string command = query.getSQL();
                logger.Debug("QUERY: " + command);
                DataSet ds = new DataSet();

                if (!this.ExecuteQuery(out ds, command))
                {
                    throw new Exception(this.LastExceptionMessage);
                }
                else
                {
                    if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        result = new string[ds.Tables[0].Rows.Count];
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            result[i] = ds.Tables[0].Rows[i]["VAR_DESC_FASC"].ToString() + "_" + ds.Tables[0].Rows[i]["ID_PROC"].ToString();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Debug("Errore in Procedimenti>GetProcedimentiAmministrazione", ex);
                result = null;
            }

            logger.Debug("END");
            return result;
        }

        public List<DettaglioProcedimento> GetProcedimentiReport(List<DocsPaVO.filtri.FiltroRicerca> filters)
        {
            logger.Debug("BEGIN");
            List<DettaglioProcedimento> list = new List<DettaglioProcedimento>();

            try
            {
                DocsPaUtils.Query query = DocsPaUtils.InitQuery.getInstance().getQuery("S_GET_PROCEDIMENTI_REPORT");
                if (filters != null && filters.Count > 0)
                {
                    string filterString = this.SetFilters(filters);
                    query.setParam("filters", filterString);
                }
                else
                {
                    query.setParam("filters", string.Empty);
                }

                string command = query.getSQL();
                logger.Debug("QUERY: " + command);

                DataSet ds = new DataSet();

                if (!this.ExecuteQuery(out ds, command))
                {
                    throw new Exception(this.LastExceptionMessage);
                }
                else
                {
                    if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        string lastId = string.Empty;
                        List<StatoProcedimento> stati = null;

                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            DettaglioProcedimento dp = null;

                            if (row["ID_PROJECT"].ToString() != lastId)
                            {
                                if (!string.IsNullOrEmpty(lastId))
                                {
                                    if (stati != null)
                                    {
                                        list.Last().Stati = new StatoProcedimento[stati.Count];
                                        for (int i = 0; i < stati.Count; i++)
                                        {
                                            list.Last().Stati[i] = new StatoProcedimento()
                                            {
                                                IdStato = stati[i].IdStato,
                                                DescrizioneStato = stati[i].DescrizioneStato,
                                                IdFase = stati[i].IdFase,
                                                DescrizioneFase = stati[i].DescrizioneFase,
                                                StatoIniziale = stati[i].StatoIniziale,
                                                StatoFinale = stati[i].StatoFinale,
                                                DataStato = stati[i].DataStato
                                            };
                                        }
                                    }
                                }

                                lastId = row["ID_PROJECT"].ToString();
                                dp = new DettaglioProcedimento();
                                dp.IdProject = row["ID_PROJECT"].ToString();
                                dp.Descrizione = row["DESC_PROJECT"].ToString();
                                dp.IdTipoAtto = row["ID_TIPO_FASC"].ToString();
                                dp.DescrizioneTipoAtto = row["VAR_DESC_FASC"].ToString();
                                list.Add(dp);

                                stati = new List<StatoProcedimento>();
                            }

                            stati.Add(new StatoProcedimento()
                            {
                                IdStato = row["ID_STATO"].ToString(),
                                DescrizioneStato = row["DESC_STATO"].ToString(),
                                IdFase = row["ID_FASE"].ToString(),
                                DescrizioneFase = row["DESC_FASE"].ToString(),
                                StatoIniziale = row["STATO_INIZIALE"].ToString().Equals("1"),
                                StatoFinale = row["STATO_FINALE"].ToString().Equals("1"),
                                DataStato = Convert.ToDateTime(row["DTA_START"])
                            });
                        }

                        // Popolo l'ultimo elemento che altrimenti rimarrebbe null
                        list.Last().Stati = new StatoProcedimento[stati.Count];
                        for (int i = 0; i < stati.Count; i++)
                        {
                            list.Last().Stati[i] = new StatoProcedimento()
                            {
                                IdStato = stati[i].IdStato,
                                DescrizioneStato = stati[i].DescrizioneStato,
                                IdFase = stati[i].IdFase,
                                DescrizioneFase = stati[i].DescrizioneFase,
                                StatoIniziale = stati[i].StatoIniziale,
                                StatoFinale = stati[i].StatoFinale,
                                DataStato = stati[i].DataStato
                            };
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Errore in Procedimenti>GetProcedimentoReport", ex);
                list = null;
            }

            logger.Debug("END");
            return list;
        }

        public List<DocsPaVO.utente.Registro> GetAOOAssociateProcedimento(string template, string idAmm)
        {
            logger.Debug("BEGIN");
            List<DocsPaVO.utente.Registro> list = new List<DocsPaVO.utente.Registro>();

            try
            {
                DocsPaUtils.Query query = DocsPaUtils.InitQuery.getInstance().getQuery("S_AOO_PROCEDIMENTI");
                query.setParam("template", template.Replace("'", "''"));
                query.setParam("id_amm", idAmm);
                string command = query.getSQL();
                logger.Debug("QUERY - " + command);

                DataSet ds;
                if (!this.ExecuteQuery(out ds, command))
                    throw new Exception(this.LastExceptionMessage);

                if(ds != null && ds.Tables[0] != null)
                {
                    foreach(DataRow row in ds.Tables[0].Rows)
                    {
                        DocsPaVO.utente.Registro item = new DocsPaVO.utente.Registro();
                        item.systemId = row["SYSTEM_ID"].ToString();
                        item.codice = row["VAR_CODICE"].ToString();
                        item.descrizione = row["VAR_DESC_REGISTRO"].ToString();
                        item.idAmministrazione = row["ID_AMM"].ToString();

                        list.Add(item);
                    }
                }
                else
                {
                    list = null;
                }
            }
            catch(Exception ex)
            {
                logger.Debug("Errore in GetAOOAssociateProcedimento - ", ex);
                list = null;
            }

            logger.Debug("END");
            return list;
        }

        public bool CheckProcedimentoReindirizzato(string idProject, out string newId, out string newIdReg)
        {
            logger.Debug("BEGIN");
            bool result = false;

            newId = string.Empty;
            newIdReg = string.Empty;

            try
            {
                DocsPaUtils.Query query = DocsPaUtils.InitQuery.getInstance().getQuery("S_PROCEDIMENTO_REINDIRIZZATO");
                query.setParam("id_project", idProject);
                string command = query.getSQL();
                

                logger.Debug("QUERY - " + command);
                DataSet ds = new DataSet();
                if(!this.ExecuteQuery(out ds, command))
                    throw new Exception(this.LastExceptionMessage);

                if(ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    newId = row["ID_REDIRECT"].ToString();
                    newIdReg = row["ID_AOO_REDIRECT"].ToString();

                    result = newId != "0";
                }
                else
                {
                    result = false;
                }              
            }
            catch(Exception ex)
            {
                logger.Debug("Errore in CheckProcedimentoReindirizzato - ", ex);
                newId = string.Empty;
                newIdReg = string.Empty;
            }

            logger.Debug("END");
            return result;
        }

        public bool CheckStatoChiusuraProcedimento(string idStato)
        {
            logger.Debug("BEGIN");
            bool result = false;

            try
            {

                DocsPaUtils.Query query = DocsPaUtils.InitQuery.getInstance().getQuery("S_CHECK_STATO_CHIUSURA_PROCEDIMENTO");
                query.setParam("id_stato", idStato);

                string command = query.getSQL();
                string retVal = string.Empty;
                logger.Debug("QUERY - " + command);

                if (!this.ExecuteScalar(out retVal, command))
                    throw new Exception(this.LastExceptionMessage);

                if (!string.IsNullOrEmpty(retVal) && retVal.Trim() == idStato)
                    result = true;

            }
            catch(Exception ex)
            {
                logger.Debug("Errore in CheckStatoChiusuraProcedimento - ", ex);
                result = false;
            }

            logger.Debug("END");
            return result;
        }

        #region Amministrazione
        public List<String> GetTipologieDocumento(String[] AOO)
        {
            logger.Debug("BEGIN");
            List<String> list = new List<String>();

            try
            {
                DocsPaUtils.Query query = DocsPaUtils.InitQuery.getInstance().getQuery("S_PORTALE_AMM_GET_TIPI_DOC");
                string filtro = this.SetFiltroAOO(AOO);
                query.setParam("aoo", filtro);
                query.setParam("num_aoo", AOO.Count().ToString());
                string command = query.getSQL();
                logger.Debug("QUERY - " + command);
                DataSet ds;

                if (!this.ExecuteQuery(out ds, command))
                    throw new Exception(this.LastExceptionMessage);

                if(ds != null && ds.Tables[0] != null)
                {
                    foreach(DataRow row in ds.Tables[0].Rows)
                    {
                        list.Add(row["DESCRIZIONE"].ToString());
                    }
                }
            }
            catch(Exception ex)
            {
                logger.Debug("Errore in GetTipologieDocumento - ", ex);
                list = null;
            }

            logger.Debug("END");
            return list;
        }

        public List<String> GetTipologieFascicolo(String[] AOO)
        {
            logger.Debug("BEGIN");
            List<String> list = new List<String>();

            try
            {
                DocsPaUtils.Query query = DocsPaUtils.InitQuery.getInstance().getQuery("S_PORTALE_AMM_GET_TIPI_FASC");
                string filtro = this.SetFiltroAOO(AOO);
                query.setParam("aoo", filtro);
                query.setParam("num_aoo", AOO.Count().ToString());
                string command = query.getSQL();
                logger.Debug("QUERY - " + command);
                DataSet ds;

                if (!this.ExecuteQuery(out ds, command))
                    throw new Exception(this.LastExceptionMessage);

                if (ds != null && ds.Tables[0] != null)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        list.Add(row["DESCRIZIONE"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Debug("Errore in GetTipologieFascicolo - ", ex);
                list = null;
            }

            logger.Debug("END");
            return list;
        }

        public List<DocsPaVO.utente.Registro> GetAOO()
        {
            logger.Debug("BEGIN");
            List<DocsPaVO.utente.Registro> list = null;

            try
            {
                list = new List<DocsPaVO.utente.Registro>();

                DocsPaUtils.Query query = DocsPaUtils.InitQuery.getInstance().getQuery("S_PORTALE_AMM_GET_AOO");
                string command = query.getSQL();

                DataSet ds;

                if (!this.ExecuteQuery(out ds, command))
                    throw new Exception(this.LastExceptionMessage);

                if(ds != null && ds.Tables[0] != null)
                {
                    foreach(DataRow row in ds.Tables[0].Rows)
                    {
                        DocsPaVO.utente.Registro reg = new DocsPaVO.utente.Registro();
                        reg.systemId = row["SYSTEM_ID"].ToString();
                        reg.codRegistro = row["VAR_CODICE"].ToString();
                        reg.descrizione = row["VAR_DESC_REGISTRO"].ToString();
                        reg.codAmministrazione = row["VAR_CODICE_AMM"].ToString();
                        reg.idAmministrazione = row["ID_AMM"].ToString();

                        list.Add(reg);
                    }
                }
            }
            catch(Exception ex)
            {
                logger.Debug("Errore in GetAOO - " + ex.Message);
            }

            logger.Debug("END");
            return list;
        }
        #endregion

        #endregion

        #region Delete
        public bool DeleteDocProcedimento(string idFascicolo)
        {
            logger.Debug("BEGIN");
            bool result = false;

            try
            {
                DocsPaUtils.Query query = DocsPaUtils.InitQuery.getInstance().getQuery("D_DPA_PROCEDIMENTI_REINDIRIZZAMENTO");
                query.setParam("id_project", idFascicolo);

                string command = query.getSQL();
                logger.Debug("QUERY - " + command);

                if (!this.ExecuteNonQuery(command))
                    throw new Exception(this.LastExceptionMessage);

                result = true;
            }
            catch(Exception ex)
            {
                logger.Debug(ex);
                result = false;
            }

            logger.Debug("END");
            return result;
        }
        #endregion

        #region Metodi privati
        private string SetFilters(List<DocsPaVO.filtri.FiltroRicerca> filters)
        {
            string filterString = string.Empty;
            string dbType = System.Configuration.ConfigurationManager.AppSettings["DBType"];

            foreach (DocsPaVO.filtri.FiltroRicerca f in filters)
            {
                switch (f.argomento.ToUpper())
                {
                    case "ID_TIPO_FASC":
                        filterString = filterString + " AND A.ID_TIPO_FASC = " + f.valore;
                        break;
                    case "ANNO":
                        if (!string.IsNullOrEmpty(f.valore))
                        {
                            if (dbType.ToUpper().Equals("SQL"))
                            {
                                filterString = filterString + " AND EXISTS (SELECT 'X' FROM DPA_ASS_PROCEDIMENTI_FASI Y1, DPA_STATI Y2 WHERE Y1.ID_STATO=Y2.SYSTEM_ID AND Y2.Stato_iniziale='1' AND Y1.ID_PROJECT=A.SYSTEM_ID AND YEAR(Y1.DTA_START)= " + f.valore + ") ";
                            }
                            else
                            {
                                filterString = filterString + " AND EXISTS (SELECT 'X' FROM DPA_ASS_PROCEDIMENTI_FASI Y1, DPA_STATI Y2 WHERE Y1.ID_STATO=Y2.SYSTEM_ID AND Y2.Stato_iniziale='1' AND Y1.ID_PROJECT=A.SYSTEM_ID AND TO_CHAR(Y1.DTA_START, 'YYYY)='" + f.valore + "') ";
                            }
                        }
                        break;
                    case "ID_AMM":

                        break;
                }
            }


            return filterString;
        }

        private string SetFiltroAOO(String[] list)
        {
            string filter = string.Empty;

            if(list != null && list.Count() > 0)
            {
                string listString = string.Empty;
                for (int i=0; i<list.Count(); i++)
                {
                    listString = listString + list[i];
                    if (i < list.Count() - 1)
                        listString = listString + ",";
                }

                //filter = string.Format(" AND B.SYSTEM_ID IN ({0})", listString);
                filter = listString;
            }

            return filter;
        }
        #endregion


    }
}
