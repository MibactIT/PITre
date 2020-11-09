using System;
using System.Xml;
using System.Data;
using System.Globalization;
using log4net;

namespace BusinessLogic.Interoperabilit�
{
	/// <summary></summary>
	public class InteroperabilitaControlloRicevute
	{
        private static ILog logger = LogManager.GetLogger(typeof(InteroperabilitaControlloRicevute));
		/// <summary></summary>
		/// <param name="path"></param>
		/// <param name="filename"></param>
		/// <param name="reg"></param>
		/// <param name="mailId"></param>
		/// <param name="mailAddress"></param>
		/// <param name="logger"></param>
		/// <returns></returns>
        public static bool processaXmlConferma(string path, string filename, DocsPaVO.utente.Registro reg, string mailId, string mailAddress, out string moreError)
		{
            //Verifico la validit� della segnatura con Xsd

            bool isSignatureValid = interoperabilita.InteroperabilitaEccezioni.isSignatureValid(System.IO.Path.Combine(path, filename));
            if (!isSignatureValid)
                throw new System.Xml.Schema.XmlSchemaException();

			XmlDocument doc =new XmlDocument();
			InteropResolver my = new InteropResolver();
            XmlTextReader xtr = new XmlTextReader(System.IO.Path.Combine(path, filename)) { Namespaces = false };
			xtr.WhitespaceHandling = WhitespaceHandling.None;
			XmlValidatingReader xvr = new XmlValidatingReader(xtr);
			xvr.ValidationType = System.Xml.ValidationType.DTD;
			xvr.EntityHandling = System.Xml.EntityHandling.ExpandCharEntities;
			xvr.XmlResolver = my;
            moreError = string.Empty;

			try
			{ 
				doc.Load(xvr);
			}
			catch(System.Xml.Schema.XmlSchemaException e)
			{
				logger.Error("La mail viene sospesa perche' il  file confermaRicezione.xml non e' valido. Eccezione:"+e.Message);
                moreError = "La mail viene sospesa perche' il  file confermaRicezione.xml non e' valido. Eccezione:" + e.Message;
				logger.Debug("La mail viene sospesa perche' il  file confermaRicezione.xml non e' valido. Eccezione:"+e.Message);

                if (InteroperabilitaUtils.MailElaborata(mailId, "D"))
				{
					logger.Debug("Sospensione eseguita");
				}
				else
				{
					logger.Debug("Sospensione non eseguita");
				}

				return false;
			}
			catch(Exception e)
			{
                logger.Error("La mail viene sospesa. Eccezione:" + e.Message);
                moreError = "La mail viene sospesa. Eccezione:" + e.Message;
				logger.Debug("La mail viene sospesa. Eccezione:"+e.Message);

                if (InteroperabilitaUtils.MailElaborata(mailId, "U"))
				{
					logger.Debug("Sospensione eseguita");
				}
				else
				{
					logger.Debug("Sospensione non eseguita");
				}
				
				return false;
			}
			finally
			{
				xvr.Close();
				xtr.Close();
			}

			try
			{
				CultureInfo ci = new CultureInfo("it-IT");
				string[] formati={"yyyy-MM-dd"};

                XmlElement elIdentificatore, elIdentificatoreMitt;
                string codiceAmministrazione, codiceAOO, numeroRegistrazione, codiceAmministrazioneMitt, codiceAOOMitt, numeroRegistrazioneMitt;
                DateTime dataRegistrazione, dataRegistrazioneMitt;

                if (!string.IsNullOrEmpty(doc.DocumentElement.NamespaceURI))
                {
                    try
                    {
                        XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(doc.NameTable);
                        xmlnsManager.AddNamespace("p", doc.DocumentElement.NamespaceURI);

                        elIdentificatore = (XmlElement)doc.DocumentElement.SelectSingleNode("p:Identificatore", xmlnsManager);
                        codiceAmministrazione = elIdentificatore.SelectSingleNode("p:CodiceAmministrazione", xmlnsManager).InnerText.Trim();
                        codiceAOO = elIdentificatore.SelectSingleNode("p:CodiceAOO", xmlnsManager).InnerText.Trim();
                        numeroRegistrazione = elIdentificatore.SelectSingleNode("p:NumeroRegistrazione", xmlnsManager).InnerText.Trim();
                        dataRegistrazione = DateTime.ParseExact(elIdentificatore.SelectSingleNode("p:DataRegistrazione", xmlnsManager).InnerText.Trim(), formati, ci.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);

                        //info sul messaggio
                        elIdentificatoreMitt = (XmlElement)doc.DocumentElement.SelectSingleNode("p:MessaggioRicevuto/p:Identificatore", xmlnsManager);
                        codiceAmministrazioneMitt = elIdentificatoreMitt.SelectSingleNode("p:CodiceAmministrazione", xmlnsManager).InnerText.Trim();
                        codiceAOOMitt = elIdentificatoreMitt.SelectSingleNode("p:CodiceAOO", xmlnsManager).InnerText.Trim();
                        numeroRegistrazioneMitt = elIdentificatoreMitt.SelectSingleNode("p:NumeroRegistrazione", xmlnsManager).InnerText.Trim();
                        dataRegistrazioneMitt = DateTime.ParseExact(elIdentificatore.SelectSingleNode("p:DataRegistrazione", xmlnsManager).InnerText, formati, ci.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                    }
                    catch(Exception ex)
                    {
                        codiceAmministrazione = string.Empty;
                        codiceAOO = string.Empty;
                        numeroRegistrazione = string.Empty;
                        codiceAmministrazioneMitt = string.Empty;
                        codiceAOOMitt = string.Empty;
                        numeroRegistrazioneMitt = string.Empty;
                        dataRegistrazione = new DateTime();
                        dataRegistrazioneMitt = new DateTime();
                        logger.Error("La mail viene scartata. ");
                        moreError = "La mail viene scartata.";
                        logger.Debug("La mail viene scartata. Il formato del file conferma.xml non � valido. Non � possibile associarlo al documento.");
                        return false;
                    }

                }
                else
                {
                    try
                    {
                        elIdentificatore = (XmlElement)doc.DocumentElement.SelectSingleNode("Identificatore");
                        codiceAmministrazione = elIdentificatore.SelectSingleNode("CodiceAmministrazione").InnerText.Trim();
                        codiceAOO = elIdentificatore.SelectSingleNode("CodiceAOO").InnerText.Trim();
                        numeroRegistrazione = elIdentificatore.SelectSingleNode("NumeroRegistrazione").InnerText.Trim();
                        dataRegistrazione = DateTime.ParseExact(elIdentificatore.SelectSingleNode("DataRegistrazione").InnerText.Trim(), formati, ci.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);

                        //info sul messaggio
                        elIdentificatoreMitt = (XmlElement)doc.DocumentElement.SelectSingleNode("MessaggioRicevuto/Identificatore");
                        codiceAmministrazioneMitt = elIdentificatoreMitt.SelectSingleNode("CodiceAmministrazione").InnerText.Trim();
                        codiceAOOMitt = elIdentificatoreMitt.SelectSingleNode("CodiceAOO").InnerText.Trim();
                        numeroRegistrazioneMitt = elIdentificatoreMitt.SelectSingleNode("NumeroRegistrazione").InnerText.Trim();
                        dataRegistrazioneMitt = DateTime.ParseExact(elIdentificatore.SelectSingleNode("DataRegistrazione").InnerText, formati, ci.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                    }
                    catch (Exception ex)
                    {
                        codiceAmministrazione = string.Empty;
                        codiceAOO = string.Empty;
                        numeroRegistrazione = string.Empty;
                        codiceAmministrazioneMitt = string.Empty;
                        codiceAOOMitt = string.Empty;
                        numeroRegistrazioneMitt = string.Empty;
                        dataRegistrazione = new DateTime();
                        dataRegistrazioneMitt = new DateTime();
                        logger.Error("La mail viene scartata. Il formato del file conferma.xml non � valido. Non � possibile associarlo al documento.");
                        moreError = "La mail viene scartata. Il formato del file conferma.xml non � valido. Non � possibile associarlo al documento.";
                        logger.Debug("La mail viene scartata. Il formato del file conferma.xml non � valido. Non � possibile associarlo al documento.");
                        return false;
                    }

                }
				
				//si trova il numero del documento
				logger.Debug("Ricerca id del profilo...");

                string idProf = Interoperabilit�.InteroperabilitaUtils.findIdProfile(codiceAOOMitt, numeroRegistrazioneMitt, dataRegistrazioneMitt.Year);
				logger.Debug("idProfile="+idProf);

				if(idProf==null)
				{
                    logger.Debug("La mail viene sospesa: il documento indicato non � stato trovato");
                    moreError = "La mail viene sospesa: il documento indicato non � stato trovato";
					logger.Debug("La mail viene sospesa: il documento indicato non � stato trovato");
                    if (InteroperabilitaUtils.MailElaborata(mailId, "U"))
					{
						logger.Debug("Sospensione eseguita");
					}
					else
					{
						logger.Debug("Sospensione non eseguita");
					};
					return false;
				}

				//si esegue l'update della tabella stato invio
				if(codiceAOO!= null && !codiceAOO.Equals("") && codiceAmministrazione!=null && !codiceAmministrazione.Equals(""))
				{
					logger.Debug("Update della tabella stato invio: idProfile="+idProf+", CodiceAOO="+codiceAOO+", CodiceAmm="+codiceAmministrazione+", data="+dataRegistrazione.ToString("dd/MM/yyyy"));
					
					bool res_update=updateStatoInvio(idProf,codiceAOO,codiceAmministrazione,dataRegistrazione.ToString("dd/MM/yyyy"),numeroRegistrazione,dataRegistrazione.Year);
					if(!res_update)
					{
                        logger.Debug("La mail viene sospesa: non e' stato eseguito l'update del profilo");
                        moreError = "La mail viene sospesa: non e' stato eseguito l'update del profilo";
						logger.Debug("La mail viene sospesa: non e' stato eseguito l'update del profilo");
                        if (InteroperabilitaUtils.MailElaborata(mailId, "U"))
						{
							logger.Debug("Sospensione eseguita");
						}
						else
						{
							logger.Debug("Sospensione non eseguita");
						}
						return false;
					}
				}
				else
				{
                    logger.Debug("L'update della tabella profile non pu� essere eseguito: codiceAOO o codiceAmministrazione nullo");
                    moreError = "La mail viene sospesa: non e' stato eseguito l'update del profilo";
					logger.Debug("L'update della tabella profile non pu� essere eseguito: codiceAOO o codiceAmministrazione nullo");
				}

				return true;
			}
			catch(Exception e)
			{
                logger.Error("La mail viene scartata. Eccezione: " + e.ToString());
                moreError = "La mail viene scartata. Eccezione: " + e.ToString();
				logger.Debug("La mail viene scartata. Eccezione: "+e.ToString()); 
				return false;	
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="idProf"></param>
		/// <param name="codiceAOO"></param>
		/// <param name="codiceAmministrazione"></param>
		/// <param name="data"></param>
		/// <param name="numeroRegistrazione"></param>
		/// <param name="anno"></param>
		/// <param name="logger"></param>
		/// <returns></returns>
		private static bool updateStatoInvio(string idProf, string codiceAOO,string codiceAmministrazione, string data, string numeroRegistrazione,int anno)
		{
			bool result = false;

			try
			{				
				#region Codice Commentato
				/*
				string updateString="UPDATE DPA_STATO_INVIO SET ";
				updateString=updateString+"VAR_PROTO_DEST='"+numeroRegistrazione+"/"+codiceAOO+"/"+anno+"',";
                updateString=updateString+"DTA_PROTO_DEST="+DocsPaWS.Utils.dbControl.toDate(data,false);
	
				updateString=updateString+" WHERE ID_PROFILE="+idProf+" AND VAR_CODICE_AOO='"+codiceAOO+"' AND VAR_CODICE_AMM='"+codiceAmministrazione+"'";
				logger.Debug(updateString);
				db.executeNonQuery(updateString);
				*/
				#endregion

				DocsPaDB.Query_DocsPAWS.Interoperabilita obj = new DocsPaDB.Query_DocsPAWS.Interoperabilita();
				result=	obj.updStatoInvio(idProf,codiceAOO,codiceAmministrazione,data,numeroRegistrazione,anno);
			}
			catch(Exception e)
			{
				logger.Error("Eccezione: " + e.Message);

				result = false;
			}

			return result;
		}
	
	
	
		public static bool processaRicevutaConferma(DocsPaVO.Interoperabilita.RicevutaRitorno ricevuta, out string message)
		{
			message=string.Empty;
			try
			{
				CultureInfo ci = new CultureInfo("it-IT");
				string[] formati={"dd/MM/yyyy","yyyy-MM-dd","DD/MM/YYYY hh:mm:ss","DD/MM/YYYY hh.mm.ss","DD/MM/YYYY HH.mm.ss","DD/MM/YYYY HH:mm:ss"};
				
				string codiceAmministrazione = ricevuta.codAmm;
				string codiceAOO = ricevuta.codAOO;
				string numeroRegistrazione = ricevuta.numeroRegistrazione;
				DateTime dataRegistrazione = DateTime.ParseExact(ricevuta.dataRegistrazione,formati,ci.DateTimeFormat,DateTimeStyles.AllowWhiteSpaces);
				
				//info sul messaggio
				string codiceAmministrazioneMitt = ricevuta.codAmm_Mitt;
				string codiceAOOMitt = ricevuta.codAOO_Mitt;
				string numeroRegistrazioneMitt = ricevuta.numeroRegistr_Mitt;
				DateTime dataRegistrazioneMitt = DateTime.ParseExact(ricevuta.dataRegistr_Mitt,formati,ci.DateTimeFormat,DateTimeStyles.AllowWhiteSpaces);
				
				//si trova il numero del documento
				logger.Debug("Ricerca id del profilo...");

                string idProf = Interoperabilit�.InteroperabilitaUtils.findIdProfile(codiceAOOMitt, numeroRegistrazioneMitt, dataRegistrazioneMitt.Year);
				logger.Debug("idProfile="+idProf);

				if(idProf==null)
				{
					logger.Debug("Documento mittente non trovato"); 
					message="Documento mittente non trovato";
					return false;
				}

				//si esegue l'update della tabella stato invio
				if(codiceAOO!= null && !codiceAOO.Equals("") && codiceAmministrazione!=null && !codiceAmministrazione.Equals(""))
				{
					logger.Debug("Update della tabella stato invio: idProfile="+idProf+", CodiceAOO="+codiceAOO+", CodiceAmm="+codiceAmministrazione+", data="+dataRegistrazione.ToString("dd/MM/yyyy"));
					
					bool res_update=updateStatoInvio(idProf,codiceAOO,codiceAmministrazione,dataRegistrazione.ToString("dd/MM/yyyy"),numeroRegistrazione,dataRegistrazione.Year);
					if(!res_update)
					{ 
						logger.Debug("Errore: non e' stato eseguito l'update del profilo"); 
						message="Si � verificato un errore nell'aggiornamento della ricevuta di ritorno";
						return false;
					}
				}
				else
				{
					logger.Debug("L'update della tabella profile non pu� essere eseguito: codiceAOO o codiceAmministrazione nullo");
					message="Si � verificato un errore: dati mancanti ";
					return false;
				}

				return true;
			}
			catch(Exception e)
			{
                logger.Error("Si � verificato un problema nella ricevuta di ritorno. Eccezione: " + e.ToString()); 
				return false;	
			}
		}


	}
}
