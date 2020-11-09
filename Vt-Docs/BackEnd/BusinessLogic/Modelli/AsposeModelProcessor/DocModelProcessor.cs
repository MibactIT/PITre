using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using log4net;
using Aspose.Words;
using DocsPaVO.documento;
using DocsPaVO.utente;
using Aspose.Words.Properties;
using System.Collections;

namespace BusinessLogic.Modelli.AsposeModelProcessor
{
    public class DocModelProcessor : BaseDocModelProcessor
    {
        private static ILog logger = LogManager.GetLogger(typeof(DocModelProcessor));

        private License license;

        public DocModelProcessor()
        {
            license = new License();
            DocsPaDB.Query_DocsPAWS.ClientSideModelProcessor csmp = new DocsPaDB.Query_DocsPAWS.ClientSideModelProcessor();

            byte[] licenseContent = csmp.GetLicense("ASPOSE");
            if (licenseContent != null)
            {
                System.IO.MemoryStream licenseStream = new System.IO.MemoryStream(licenseContent, 0, licenseContent.Length);
                license.SetLicense(licenseStream);
                licenseStream.Close();
            }
        }

        protected override DocsPaVO.Modelli.ModelKeyValuePair[] GetModelKeyValuePairs(DocsPaVO.utente.InfoUtente infoUtente, DocsPaVO.documento.SchedaDocumento schedaDocumento)
        {
            logger.Debug("BEGIN");
            logger.Debug("DOCNUMBER = " + schedaDocumento.docNumber);
            logger.Debug("IDAMM = " + infoUtente.idAmministrazione);
            string tipoAtto = string.Empty;
            if (schedaDocumento.tipologiaAtto != null)
            {
                tipoAtto = schedaDocumento.tipologiaAtto.descrizione;
            }
            logger.Debug("TIPOLOGIA = " + tipoAtto);

            ArrayList listaCampi = this.GetOggettiProfilazione(schedaDocumento.docNumber, infoUtente.idAmministrazione, tipoAtto);

            base.FetchCommonFields(listaCampi, infoUtente, schedaDocumento);

            List<DocsPaVO.Modelli.ModelKeyValuePair> list = new List<DocsPaVO.Modelli.ModelKeyValuePair>();

            foreach (string[] items in listaCampi)
            {
                if (items != null)
                {
                    logger.DebugFormat("Key: {0} - Value: {1}", items[0], items[1]);
                }
                else
                {
                    logger.Debug("ITEM=NULL");
                }
                DocsPaVO.Modelli.ModelKeyValuePair pair = new DocsPaVO.Modelli.ModelKeyValuePair();
                pair.Key = items[0];
                pair.Value = items[1];
                list.Add(pair);
            }

            logger.Debug("END");
            return list.ToArray();
        }

        public override DocsPaVO.Modelli.ModelResponse ProcessModel(DocsPaVO.Modelli.ModelRequest request)
        {
            throw new NotImplementedException();
        }

        public void ConvertToPdfAsync(byte[] content, DocsPaVO.documento.FileRequest fileReq, DocsPaVO.utente.InfoUtente infoUtente)
        {
            logger.Debug("BEGIN");
            AsyncCallback callback = new AsyncCallback(CallBack);
            ConvertToPdfDelegate conv = new ConvertToPdfDelegate(ConvertToPdf);
            conv.BeginInvoke(content, fileReq, infoUtente, callback, conv);
        }

        private delegate void ConvertToPdfDelegate(byte[] content, DocsPaVO.documento.FileRequest fileReq, DocsPaVO.utente.InfoUtente infoUtente);

        private static void CallBack(IAsyncResult result)
        {
            var del = result.AsyncState as ConvertToPdfDelegate;
            if (del != null)
            {
                del.EndInvoke(result);
            }
        }

        private void ConvertToPdf(byte[] content, DocsPaVO.documento.FileRequest fileReq, DocsPaVO.utente.InfoUtente infoUtente)
        {
            logger.Debug("BEGIN");

            MemoryStream ms = new MemoryStream(content);
            Document doc = new Document(ms);

            using (MemoryStream pdfStream = new MemoryStream())
            {
                doc.Save(pdfStream, SaveFormat.Pdf);

                if (pdfStream != null && pdfStream.Length > 0)
                {
                    using (DocsPaDB.TransactionContext transaction = new DocsPaDB.TransactionContext())
                    {
                        try
                        {
                            DocsPaVO.documento.FileRequest convertedFileReq = new DocsPaVO.documento.FileRequest();

                            convertedFileReq.cartaceo = false;
                            convertedFileReq.daAggiornareFirmatari = false;
                            convertedFileReq.descrizione = "Documento converito in PDF lato server";
                            convertedFileReq.docNumber = fileReq.docNumber;

                            convertedFileReq = BusinessLogic.Documenti.VersioniManager.addVersion(convertedFileReq, infoUtente, false);

                            DocsPaVO.documento.FileDocumento convertedFileDoc = new DocsPaVO.documento.FileDocumento();
                            convertedFileDoc.cartaceo = false;
                            convertedFileDoc.content = pdfStream.ToArray();
                            convertedFileDoc.contentType = "application/pdf";
                            convertedFileDoc.estensioneFile = "PDF";
                            convertedFileDoc.fullName = fileReq.fileName + ".pdf";
                            convertedFileDoc.length = convertedFileDoc.content.Length;
                            convertedFileDoc.name = fileReq.fileName + ".pdf";
                            convertedFileDoc.nomeOriginale = fileReq.fileName + ".pdf";
                            convertedFileDoc.path = "";

                            DocsPaVO.documento.FileRequest result = Documenti.FileManager.putFile(convertedFileReq, convertedFileDoc, infoUtente);

                            if (result == null)
                            {
                                throw new Exception();
                            }

                            transaction.Complete();

                        }
                        catch (Exception ex)
                        {
                            logger.Debug("Errore nella conversione PDF Aspose - ", ex);

                        }
                    }

                }

            }

            logger.Debug("END");

        }

        public byte[] GetProcessedTemplate(DocsPaVO.utente.InfoUtente infoUtente, DocsPaVO.documento.SchedaDocumento sd)
        {
            byte[] content = null;
            System.IO.MemoryStream stream = new System.IO.MemoryStream();

            // Percorso modello
            string pathModel = sd.template.PATH_MODELLO_1;

            try
            {
                // Caricamento licenza
                DocsPaDB.Query_DocsPAWS.ClientSideModelProcessor csmp = new DocsPaDB.Query_DocsPAWS.ClientSideModelProcessor();

                Aspose.Words.License lic = new License();
                byte[] licenseContent = csmp.GetLicense("ASPOSE");

                if (licenseContent != null)
                {
                    System.IO.MemoryStream licenseStream = new System.IO.MemoryStream(licenseContent, 0, licenseContent.Length);
                    lic.SetLicense(licenseStream);
                    licenseStream.Close();
                }

                //Lettura modello
                Document doc = new Document(pathModel);
                // Estrazione campi
                logger.Debug("BEGIN");
                DocsPaVO.Modelli.ModelKeyValuePair[] commonFields = this.GetModelKeyValuePairs(infoUtente, sd);

                foreach (DocsPaVO.Modelli.ModelKeyValuePair kvp in commonFields)
                {
                    doc.Range.Replace(this.FormatFieldName(kvp.Key), kvp.Value, new Aspose.Words.Replacing.FindReplaceOptions());
                }

                // Individuazione formato

                SaveFormat frm = new SaveFormat();
                switch (sd.template.PATH_MODELLO_1_EXT.ToUpper())
                {
                    case "ODT":
                        frm = SaveFormat.Odt;
                        break;
                    default:
                        throw new Exception("Formato estensione file non gestito!");
                        break;
                }

                doc.Save(stream, frm);
                content = stream.ToArray();

            }
            catch (Exception ex)
            {
                logger.DebugFormat("{0}\r\n{1}", ex.Message, ex.StackTrace);
                content = null;
            }

            return content;
        }

        private ArrayList GetOggettiProfilazione(string docNumber, string idAmministrazione, string tipoAtto)
        {
            DocsPaVO.ProfilazioneDinamica.Templates template = BusinessLogic.ProfilazioneDinamica.ProfilazioneDocumenti.getTemplateDettagli(docNumber);
            ArrayList listaChiaviValori = new ArrayList();

            if (template != null && template.ELENCO_OGGETTI != null)
            {
                for (int i = 0; i < template.ELENCO_OGGETTI.Count; i++)
                {
                    DocsPaVO.ProfilazioneDinamica.OggettoCustom oggettoCustom = (DocsPaVO.ProfilazioneDinamica.OggettoCustom)template.ELENCO_OGGETTI[i];

                    if (oggettoCustom != null)
                    {
                        string[] itemToAdd = null;

                        switch (oggettoCustom.TIPO.DESCRIZIONE_TIPO)
                        {
                            case "Corrispondente":
                                DocsPaVO.utente.Corrispondente corr = BusinessLogic.Utenti.UserManager.getCorrispondenteBySystemID(oggettoCustom.VALORE_DATABASE);


                                itemToAdd = new string[8] { "", "", "", "", "", "", "", "" };
                                itemToAdd[0] = oggettoCustom.DESCRIZIONE;
                                if (corr != null)
                                {
                                    itemToAdd[1] = corr.descrizione;
                                    DocsPaVO.utente.Corrispondente corrIndirizzo = BusinessLogic.Utenti.UserManager.getDettagliIndirizzoCorrispondente(oggettoCustom.VALORE_DATABASE);
                                    if (corrIndirizzo != null)
                                    {
                                        //
                                        oggettoCustom.INDIRIZZO += corr.descrizione + Environment.NewLine + corrIndirizzo.indirizzo + Environment.NewLine +
                                            corrIndirizzo.cap + '-' + corrIndirizzo.citta + '-' + corrIndirizzo.localita;
                                        itemToAdd[3] = oggettoCustom.INDIRIZZO;
                                        oggettoCustom.TELEFONO += corr.descrizione + Environment.NewLine + corrIndirizzo.telefono1 + '-' + corrIndirizzo.telefono2;
                                        itemToAdd[6] = oggettoCustom.TELEFONO;
                                        oggettoCustom.INDIRIZZO_TELEFONO += oggettoCustom.INDIRIZZO + Environment.NewLine + corrIndirizzo.telefono1 +
                                           '-' + corrIndirizzo.telefono2;
                                        itemToAdd[7] = oggettoCustom.INDIRIZZO_TELEFONO;

                                    }

                                }


                                itemToAdd[2] = oggettoCustom.TIPO.DESCRIZIONE_TIPO;
                                //itemToAdd[3] = 
                                itemToAdd[4] = oggettoCustom.ANNO;
                                itemToAdd[5] = oggettoCustom.ID_AOO_RF;
                                listaChiaviValori.Add(itemToAdd);

                                break;
                            case "Contatore":
                                string formato = oggettoCustom.FORMATO_CONTATORE;
                                formato = formato.Replace("ANNO", oggettoCustom.ANNO);

                                DocsPaVO.utente.Registro reg = BusinessLogic.Utenti.RegistriManager.getRegistro(oggettoCustom.ID_AOO_RF);
                                if (reg != null && !string.IsNullOrEmpty(reg.codRegistro))
                                    formato = formato.Replace("AOO", reg.codRegistro);

                                DocsPaVO.utente.Registro rf = BusinessLogic.Utenti.RegistriManager.getRegistro(oggettoCustom.ID_AOO_RF);
                                if (rf != null && !string.IsNullOrEmpty(rf.codRegistro))
                                    formato = formato.Replace("RF", rf.codRegistro);

                                formato = formato.Replace("CONTATORE", oggettoCustom.VALORE_DATABASE);

                                itemToAdd = new string[6] { "", "", "", "", "", "" };
                                itemToAdd[0] = oggettoCustom.DESCRIZIONE;
                                itemToAdd[1] = formato;
                                itemToAdd[2] = oggettoCustom.TIPO.DESCRIZIONE_TIPO;
                                itemToAdd[3] = oggettoCustom.FORMATO_CONTATORE;
                                itemToAdd[4] = oggettoCustom.ANNO;
                                //itemToAdd[5] = 
                                listaChiaviValori.Add(itemToAdd);

                                break;
                            case "CasellaDiSelezione":
                                string valore = string.Empty;
                                foreach (string val in oggettoCustom.VALORI_SELEZIONATI)
                                {
                                    if (val != null && val != "")
                                        valore += val + "-";
                                }
                                if (valore.Length > 1)
                                    valore = valore.Substring(0, valore.Length - 1);

                                itemToAdd = new string[6] { "", "", "", "", "", "" };
                                itemToAdd[0] = oggettoCustom.DESCRIZIONE;
                                itemToAdd[1] = valore;
                                itemToAdd[2] = oggettoCustom.TIPO.DESCRIZIONE_TIPO;
                                //itemToAdd[3] = 
                                itemToAdd[4] = oggettoCustom.ANNO;
                                //itemToAdd[5] =
                                listaChiaviValori.Add(itemToAdd);
                                break;
                            default:
                                itemToAdd = new string[6] { "", "", "", "", "", "" };
                                itemToAdd[0] = oggettoCustom.DESCRIZIONE;
                                itemToAdd[1] = oggettoCustom.VALORE_DATABASE;
                                itemToAdd[2] = oggettoCustom.TIPO.DESCRIZIONE_TIPO;
                                //itemToAdd[3] = 
                                itemToAdd[4] = oggettoCustom.ANNO;
                                //itemToAdd[5] =
                                listaChiaviValori.Add(itemToAdd);
                                break;
                        }
                    }
                }
            }

            return listaChiaviValori;
        }

        private string FormatFieldName(string name)
        {
            return "#" + name + "#";
        }
    }
}
