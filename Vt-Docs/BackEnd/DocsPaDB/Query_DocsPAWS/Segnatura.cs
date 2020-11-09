using DocsPaVO.documento;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsPaDB.Query_DocsPAWS
{
    public class Segnatura : DBProvider
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DettaglioSegnatura DettaglioSegnatura_Select(string SystemIdProfile)
        {
            DettaglioSegnatura _dettaglio = null;
            try
            {
                _logger.Info("START");
                DocsPaUtils.Query q = DocsPaUtils.InitQuery.getInstance().getQuery("S_GET_DETTAGLIO_SEGNATURA_PROFILE");
                q.setParam("id_profile", SystemIdProfile);
                string command = q.getSQL();
                _logger.Debug("QUERY - " + command);
                DataSet ds = null;
                if (!this.ExecuteQuery(out ds, command))
                {
                    throw new Exception(this.LastExceptionMessage);
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _logger.DebugFormat("La query ha restituito {0} risultati", ds.Tables[0].Rows.Count);
                        DataRow row = ds.Tables[0].Rows[0];
                        _dettaglio = new DettaglioSegnatura()
                        {
                            IsPermanenteProtocollo = row.Field<string>("CHA_PERMANENTE_PROTOCOLLO"),
                            IsPermanenteRepertorio = row.Field<string>("CHA_PERMANENTE_REPERTORIO"),
                            IsPermanenteNP = row.Field<string>("CHA_PERMANENTE_NP"),
                            ProfileID = row.Field<int>("PROFILE_ID").ToString(),
                            SegnaturaNP = row.Field<string>("VAR_SEGNATURA_NP"),
                            SegnaturaProtocollo = row.Field<string>("VAR_SEGNATURA_PROTOCOLLO"),
                            SegnaturaRepertorio = row.Field<string>("VAR_SEGNATURA_REPERTORIO"),
                            SystemId = row.Field<int>("SYSTEM_ID").ToString(),
                            Segnato = row.Field<string>("CHA_SEGNATO"),
                            VersionId = row.Field<int>("VERSION_ID").ToString()
                        };
                    }
                    else if (ds.Tables[0].Rows.Count == 0)
                    {
                        _logger.Debug("la query non ha restituito nessun risultato");
                    }
                    else
                    {
                        throw new Exception("La query non ha restituito i risultati attesi");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw ex;
            }
            finally
            {
                _logger.Info("END");
            }
            return _dettaglio;
        }

        public void DettaglioSegnatura_Insert(DettaglioSegnatura dettaglioSegnatura)
        {
            _logger.Info("START");
            try
            {
                this.BeginTransaction();
                DocsPaUtils.Query _query = DocsPaUtils.InitQuery.getInstance().getQuery("I_DETTAGLI_SEGNATURA_PROFILE");
                _query.setParam("id_profile", dettaglioSegnatura.ProfileID);
                _query.setParam("id_versione", dettaglioSegnatura.VersionId);//VERSION_ID
                _query.setParam("segnatura_protocollo", dettaglioSegnatura.SegnaturaProtocollo ?? String.Empty);
                _query.setParam("segnatura_repertorio", dettaglioSegnatura.SegnaturaRepertorio ?? String.Empty);
                _query.setParam("segnatura_np", dettaglioSegnatura.SegnaturaNP ?? String.Empty);
                _query.setParam("is_permenente_protocollo", "1".Equals(dettaglioSegnatura.IsPermanenteProtocollo) ? "1" : "0");
                _query.setParam("is_permenente_repertorio", "1".Equals(dettaglioSegnatura.IsPermanenteRepertorio) ? "1" : "0");
                _query.setParam("is_permenente_np", "1".Equals(dettaglioSegnatura.IsPermanenteNP) ? "1" : "0");
                _query.setParam("is_segnato", "1".Equals(dettaglioSegnatura.Segnato) ? "1" : "0");
                string command = _query.getSQL();
                _logger.Debug("QUERY - " + command);
                if (!this.ExecuteNonQuery(command))
                {
                    throw new Exception(this.LastExceptionMessage);
                }


                _query = DocsPaUtils.InitQuery.getInstance().getQuery("U_PROFILE_UPDATE_SEGNATURA");
                _query.setParam("segnatura", dettaglioSegnatura.ToString());
                _query.setParam("system_id", dettaglioSegnatura.ProfileID);
                command = _query.getSQL();
                _logger.Debug("QUERY - " + command);
                if (!this.ExecuteNonQuery(command))
                {
                    throw new Exception(this.LastExceptionMessage);
                }




                this.CommitTransaction();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                this.RollbackTransaction();
                throw ex;
            }
            finally
            {
                _logger.Info("END");
            }
        }

        public void DettaglioSegnatura_Update_SetSegnato(string systemIdProfile)
        {
            _logger.Info("START");

            try
            {
                DocsPaUtils.Query _query = DocsPaUtils.InitQuery.getInstance().getQuery("U_DETTAGLIO_SEGNATURA_SET_SEGNATO");
                _query.setParam("id_profile", systemIdProfile);
                string command = _query.getSQL();
                _logger.Debug("QUERY - " + command);
                if (!this.ExecuteNonQuery(command))
                {
                    throw new Exception(this.LastExceptionMessage);
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw ex;
            }

            _logger.Info("END");
        }

        public void DettaglioSegnatura_Update_SetSegnato(DettaglioSegnatura dettaglio)
        {
            _logger.Info("START");
            try
            {
                DocsPaUtils.Query q = DocsPaUtils.InitQuery.getInstance().getQuery("U_DETTAGLIO_SEGNATURA_SET_SEGNATO");
                q.setParam("id_profile", dettaglio.ProfileID);
                string command = q.getSQL();
                _logger.Debug("QUERY - " + command);
                if (!this.ExecuteNonQuery(command))
                {
                    throw new Exception(this.LastExceptionMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw ex;
            }
            _logger.Info("END");
        }



        #region POSITION

        public DettaglioSegnaturaPosition DettaglioSegnaturaPosition_Select(string idProfile)
        {
            DettaglioSegnaturaPosition _dettaglio = null;
            try
            {
                _logger.Info("START");
                DocsPaUtils.Query q = DocsPaUtils.InitQuery.getInstance().getQuery("S_GET_DETTAGLIOSEGNATURAPOSITION");
                q.setParam("profile_id", idProfile);
                string command = q.getSQL();
                _logger.Debug("QUERY - " + command);
                DataSet ds = null;
                if (!this.ExecuteQuery(out ds, command))
                {
                    throw new Exception(this.LastExceptionMessage);
                }
                else
                {
                    _logger.DebugFormat("La query ha restituito {0} risultati", ds.Tables[0].Rows.Count);
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        _dettaglio = new DettaglioSegnaturaPosition()
                        {
                            ProfileID = row.Field<int>("PROFILE_ID").ToString(),
                            SegnaturaPosition = row.Field<string>("SEGNATURA_POSITION")
                        };
                    }
                    else if (ds.Tables[0].Rows.Count == 0)
                    {
                        _logger.Debug("la query non ha restituito nessun risultato");
                    }
                    else
                    {
                        throw new Exception("La query non ha restituito i risultati attesi");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw ex;
            }
            finally
            {
                _logger.Info("END");
            }
            return _dettaglio;
        }

        public void DettaglioSegnaturaPosition_Update_SePosition(DettaglioSegnaturaPosition dettaglio)
        {
            _logger.Info("START");
            try
            {
                DocsPaUtils.Query q = DocsPaUtils.InitQuery.getInstance().getQuery("U_DETTAGLIO_SEGNATURA_SET_POSITION");
                q.setParam("id_profile", dettaglio.ProfileID);
                q.setParam("position", dettaglio.SegnaturaPosition);
                string command = q.getSQL();
                _logger.Debug("QUERY - " + command);
                if (!this.ExecuteNonQuery(command))
                {
                    throw new Exception(this.LastExceptionMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw ex;
            }
            _logger.Info("END");
        }

        public void DettaglioSegnaturaPosition_Insert(DettaglioSegnaturaPosition dettaglio)
        {
            _logger.Info("START");
            try
            {

                DocsPaUtils.Query _query = DocsPaUtils.InitQuery.getInstance().getQuery("I_DETTAGLIOSEGNATURAPOSITION_INSERT");
                _query.setParam("id_profile", dettaglio.ProfileID);
                _query.setParam("position", dettaglio.SegnaturaPosition ?? String.Empty);
                string command = _query.getSQL();
                _logger.Debug("QUERY - " + command);
                if (!this.ExecuteNonQuery(command))
                {
                    throw new Exception(this.LastExceptionMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw ex;
            }
            finally
            {
                _logger.Info("END");
            }
        }

        #endregion


        #region REPERTORIO

        public bool SegnaturaRepertorio_IsPermanente(string docNumber)
        {
            _logger.Info("START");
            bool _result = true;
            try
            {
                DocsPaUtils.Query _query = DocsPaUtils.InitQuery.getInstance().getQuery("S_IS_SEGNATURA_REPERTORIO_PERMANENTE");
                _query.setParam("doc_number", docNumber);
                string command = _query.getSQL();
                _logger.Debug("QUERY - " + command);
                DataSet ds = null;
                if (!this.ExecuteQuery(out ds, command))
                {
                    throw new Exception(this.LastExceptionMessage);
                }
                else
                {
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        _logger.DebugFormat("La query ha restituito {0} risultati", ds.Tables[0].Rows.Count);
                        DataRow row = ds.Tables[0].Rows[0];
                        _result = row.Field<string>("IS_PERMANENTE").Equals("1");
                    }
                    else if (ds.Tables[0].Rows.Count == 0)
                    {
                        _logger.Debug("la query non ha restituito nessun risultato");
                        _result = false;
                    }
                    else
                    {
                        throw new Exception("La query non ha restituito i risultati attesi");
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            _logger.Info("END");

            return _result;

        }

        public DettaglioSegnaturaRepertorio GetSegnaturaRepertorio(string docnumber)
        {
            DettaglioSegnaturaRepertorio dettaglioSegnaturaRepertorio = null;
            DocsPaUtils.Query _query;
            IDataReader reader = null;
            try
            {
                _query = DocsPaUtils.InitQuery.getInstance().getQuery("S_GET_SEGNATURA_REPERTORIO_SIMPLE");
                _query.setParam("docnumber", docnumber.ToString());
                string commandText = _query.getSQL();
                _logger.Debug(commandText);

                reader = ExecuteReader(commandText);
                while (reader.Read())
                {
                    dettaglioSegnaturaRepertorio = new DettaglioSegnaturaRepertorio()
                    {
                        DocNumber = docnumber,
                        Anno = DocsPaUtils.Data.DataReaderHelper.GetValue<int?>(reader, "ANNO", true),
                        Classifica = DocsPaUtils.Data.DataReaderHelper.GetValue<string>(reader, "CLASSIFICA", true),
                        CodiceUO = DocsPaUtils.Data.DataReaderHelper.GetValue<string>(reader, "CODICE_UO", true),
                        Contatore = DocsPaUtils.Data.DataReaderHelper.GetValue<string>(reader, "CONTATORE", true),
                        DataAnnullamento = DocsPaUtils.Data.DataReaderHelper.GetValue<DateTime?>(reader, "DATA_ANNULLAMENTO", true),
                        DataInserimento = DocsPaUtils.Data.DataReaderHelper.GetValue<DateTime?>(reader, "DATA_INSERIMENTO", true),
                        FormatoContatore = DocsPaUtils.Data.DataReaderHelper.GetValue<string>(reader, "FORMATO_CONTATORE", true),
                        IdAooRf = DocsPaUtils.Data.DataReaderHelper.GetValue<int?>(reader, "ID_AOO_RF", true),
                        Onnicomprensiva = DocsPaUtils.Data.DataReaderHelper.GetValue<string>(reader, "ONNICOMPENSIVA", true),
                        Tipologia = DocsPaUtils.Data.DataReaderHelper.GetValue<string>(reader, "TIPOLOGIA", true),
                        IsPermanente = DocsPaUtils.Data.DataReaderHelper.GetValue<string>(reader, "IS_PERMANENTE", true)
                    };
                    break;
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                dettaglioSegnaturaRepertorio = null;
            }
            return dettaglioSegnaturaRepertorio;
        }


        #endregion
    }
}
