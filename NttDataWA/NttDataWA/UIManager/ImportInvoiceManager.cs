using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using NttDataWA.DocsPaWR;
//using NttDataWA.DocsPaFatturazioneWR;
using NttDataWA.Utils;
using log4net;

namespace NttDataWA.UIManager
{
    public class ImportInvoiceManager
    {
        private static ILog logger = LogManager.GetLogger(typeof(ImportInvoiceManager));
        //private static DocsPaFatturazioneWR.DocsPaFatturazioneWS ws = new DocsPaFatturazioneWS();
        private static DocsPaWR.DocsPaWebService ws = ProxyManager.GetWS();
        private static string InvoiceNamespace = System.Configuration.ConfigurationManager.AppSettings["NAMESPACE_FATTURAPA"];


        // METODI TEMPORANEI PER TEST
        public static DocsPaWR.FileDocumento getPDF()
        {
            DocsPaWR.FileDocumento fileDoc = new DocsPaWR.FileDocumento();
            Byte[] bites = System.IO.File.ReadAllBytes("C:\\test_invoice.pdf");
            fileDoc.cartaceo = false;
            fileDoc.content = bites;
            fileDoc.estensioneFile = "pdf";
            fileDoc.fullName = "esempio_fattura.pdf";
            fileDoc.length = bites.Length;
            fileDoc.name = "esempio_fattura.pdf";
            fileDoc.nomeOriginale = "esempio_fattura.pdf";
            fileDoc.path = "";

            return fileDoc;

        }

        public static string getFattura(string idFattura)
        {
            
            ws.Timeout = System.Threading.Timeout.Infinite;
            try
            {
                //string invoice = ws.GetFatturaXML(ImportInvoiceManager.getInfoUtente(), idFattura);
                string invoice = ws.GetFatturaXML(UserManager.GetInfoUser(), idFattura);
                if (string.IsNullOrEmpty(invoice) || invoice.Equals("NotFound"))
                {
                    // fattura non trovata
                    return "NotFound";
                }
                else if (invoice.Equals("KO"))
                {
                    // errore nella ricerca
                    return "KO";
                }
                else
                {
                    XmlDocument XmlDoc = new XmlDocument();
                    XmlDoc.LoadXml(invoice);
                    SetSessionValue("invoiceXML", XmlDoc);

                    XmlNamespaceManager xmlnsMan = new XmlNamespaceManager(XmlDoc.NameTable);
                    //xmlnsMan.AddNamespace("p", "http://www.fatturapa.gov.it/sdi/fatturapa/v1.0");
                    //xmlnsMan.AddNamespace("p", "http://www.fatturapa.gov.it/sdi/fatturapa/v1.1");
                    xmlnsMan.AddNamespace("p", InvoiceNamespace);

                    XmlNode node = XmlDoc.SelectSingleNode("p:FatturaElettronica/FatturaElettronicaHeader/DatiTrasmissione/CodiceDestinatario", xmlnsMan);
                    if (string.IsNullOrEmpty(node.InnerText.Trim()))
                    {
                        return "NO_IPA";
                    }
                    
                    // Produco il file XML da utilizzare per l'anteprima
                    // contiene il riferimento al foglio di stile XSL
                    

                    //XmlDocument previewInvoice = new XmlDocument();
                    //string decl = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                    //string pi = "<?xml-stylesheet type=\"text/xsl\" href=\"http://localhost/nttdatawa/importDati/fatturapa_v1.0.xsl\"?>";
                    //string previewXml = invoice.Replace(decl, decl + "\n" + pi);
                    //previewInvoice.LoadXml(previewXml);

                    //previewInvoice.Save("C:\\Users\\utente\\temp\\test_invoice.xml");

                    

                    //Byte[] bytes = Encoding.UTF8.GetBytes(previewInvoice.OuterXml);
                    //DocsPaWR.FileDocumento fileDoc = new DocsPaWR.FileDocumento();

                    //fileDoc.cartaceo = false;
                    //fileDoc.contentType = "text/xml";
                    //fileDoc.content = bytes;
                    //fileDoc.length = bytes.Length;
                    DocsPaWR.FileDocumento fileDoc = getInvoicePreview(invoice);
                    SetSessionValue("invoicePreview", fileDoc);

                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return "KO";
            }

        }

        public static DocsPaWR.FileDocumento getFileDocFattura()
        {
            try
            {
                DocsPaWR.FileDocumento retVal = new DocsPaWR.FileDocumento();
                XmlDocument XmlDoc = (XmlDocument)GetSessionValue("invoiceXML");
                if (XmlDoc != null)
                {
                    // recupero il numero fattura
                    XmlNamespaceManager xmlnsMan = new XmlNamespaceManager(XmlDoc.NameTable);
                    //xmlnsMan.AddNamespace("p", "http://www.fatturapa.gov.it/sdi/fatturapa/v1.0");
                    //xmlnsMan.AddNamespace("p", "http://www.fatturapa.gov.it/sdi/fatturapa/v1.1");
                    xmlnsMan.AddNamespace("p", InvoiceNamespace);

                    XmlNode node = XmlDoc.SelectSingleNode("p:FatturaElettronica/FatturaElettronicaBody/DatiGenerali/DatiGeneraliDocumento/Numero", xmlnsMan);
                    string idFattura = node.InnerText;

                    // content
                    Byte[] bytes = Encoding.UTF8.GetBytes(XmlDoc.OuterXml);

                    // parametri fileDocumento
                    retVal.cartaceo = false;
                    retVal.content = bytes;
                    retVal.contentType = "text/xml";
                    retVal.estensioneFile = "xml";
                    retVal.fullName = "Invoice_No" + idFattura + ".xml";
                    retVal.length = bytes.Length;
                    retVal.name = "Invoice_No" + idFattura + ".xml";
                    retVal.nomeOriginale = "Invoice_No" + idFattura + ".xml";
                    retVal.path = "";
                }
                else
                {
                    retVal = null;
                }
                return retVal;
            }
            catch (Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return null;
            }


        }

        public static bool uploadFattura()
        {

            bool retVal = false;

            try
            {
                XmlDocument docXML = (XmlDocument)GetSessionValue("invoiceXML");
                string invoice = string.Empty;
                if (docXML != null)
                {
                    invoice = docXML.OuterXml;
                    retVal = ws.SendFattura(invoice, UserManager.GetInfoUser(), RoleManager.GetRoleInSession().idGruppo);
                }
            }
            catch (Exception ex)
            {
                retVal = false;
                UIManager.AdministrationManager.DiagnosticError(ex);
            }

            return retVal;

        }

        public static bool updateParams(string rifAmm, string strIdDoc, string codGic, string posFin, string strDesc, string strQuant, string strPrezUni, string strPrezTot, string strAliquot, string optional1, string optional2, string optional3, string optional4, string optional5, string optional6)
        {
            bool result = true;

            try
            {
                // aggiorno XML fattura
                XmlDocument invoice = (XmlDocument)GetSessionValue("invoiceXML");
                string invoiceText = string.Empty;
                if (invoice != null)
                {
                    XmlNamespaceManager xmlnsMan = new XmlNamespaceManager(invoice.NameTable);
                    //xmlnsMan.AddNamespace("p", "http://www.fatturapa.gov.it/sdi/fatturapa/v1.0");
                    //xmlnsMan.AddNamespace("p", "http://www.fatturapa.gov.it/sdi/fatturapa/v1.1");
                    xmlnsMan.AddNamespace("p", InvoiceNamespace);

                    if (!string.IsNullOrEmpty(rifAmm))
                    {
                        XmlNode node = invoice.SelectSingleNode("p:FatturaElettronica/FatturaElettronicaHeader/CedentePrestatore/RiferimentoAmministrazione", xmlnsMan);
                        node.InnerText = rifAmm;
                    }
                    if (!string.IsNullOrEmpty(strIdDoc))
                    {
                        XmlNode node1 = invoice.SelectSingleNode("p:FatturaElettronica/FatturaElettronicaBody/DatiGenerali/DatiContratto/IdDocumento", xmlnsMan);
                        node1.InnerText = strIdDoc;
                    }
                    if (!string.IsNullOrEmpty(codGic))
                    {
                        XmlNode node2 = invoice.SelectSingleNode("p:FatturaElettronica/FatturaElettronicaBody/DatiGenerali/DatiContratto/CodiceCIG", xmlnsMan);
                        node2.InnerText = codGic;
                    }

                                        
                    if ((!string.IsNullOrEmpty(optional1)) || (!string.IsNullOrEmpty(optional2)))
                    {
                        XmlNode DatiOrdineAcquisto = invoice.SelectSingleNode("p:FatturaElettronica/FatturaElettronicaBody/DatiGenerali/DatiOrdineAcquisto", xmlnsMan);

                        //2.1.2.4  
                        if (!string.IsNullOrEmpty(optional1))
                        {
                            XmlNode beforeNode = invoice.SelectSingleNode("p:FatturaElettronica/FatturaElettronicaBody/DatiGenerali/DatiOrdineAcquisto/IdDocumento", xmlnsMan);

                            XmlElement nodoOptional1 = invoice.CreateElement("NumItem");
                            nodoOptional1.InnerText = optional1;
                            DatiOrdineAcquisto.InsertAfter(nodoOptional1,beforeNode);
                            //DatiOrdineAcquisto.AppendChild(nodoOptional1);
                        }

                        //2.1.2.5
                        if (!string.IsNullOrEmpty(optional2))
                        {
                            XmlNode afterNode = invoice.SelectSingleNode("p:FatturaElettronica/FatturaElettronicaBody/DatiGenerali/DatiOrdineAcquisto/CodiceCUP", xmlnsMan);
                            XmlNode nodeOpt2 = invoice.CreateElement("CodiceCommessaConvenzione");
                            nodeOpt2.InnerText = optional2;
                            DatiOrdineAcquisto.InsertBefore(nodeOpt2,afterNode);
                            //DatiOrdineAcquisto.AppendChild(nodeOpt2);
                        }
                    }
                    
                    if (!string.IsNullOrEmpty(optional3) || !string.IsNullOrEmpty(optional4))
                    {
                        XmlNode node_DettaglioLinee = invoice.SelectSingleNode("p:FatturaElettronica/FatturaElettronicaBody/DatiBeniServizi/DettaglioLinee", xmlnsMan);
                        //2.2.1.7
                        if (!string.IsNullOrEmpty(optional3))
                        {
                            XmlNode beforeNode = invoice.SelectSingleNode("p:FatturaElettronica/FatturaElettronicaBody/DatiBeniServizi/DettaglioLinee/UnitaMisura", xmlnsMan);
                            XmlElement nodoOptional3 = invoice.CreateElement("DataInizioPeriodo");
                            nodoOptional3.InnerText = optional3;
                            node_DettaglioLinee.InsertAfter(nodoOptional3,beforeNode);
                            //node_DettaglioLinee.AppendChild(nodoOptional3);
                        }
                        //2.2.1.8 
                        if (!string.IsNullOrEmpty(optional4))
                        {
                            XmlNode afterNode = invoice.SelectSingleNode("p:FatturaElettronica/FatturaElettronicaBody/DatiBeniServizi/DettaglioLinee/PrezzoUnitario", xmlnsMan);
                            XmlElement nodoOptional4 = invoice.CreateElement("DataFinePeriodo");
                            nodoOptional4.InnerText = optional4;
                            node_DettaglioLinee.InsertBefore(nodoOptional4, afterNode);
                            //node_DettaglioLinee.AppendChild(nodoOptional4);
                        }
                    }
                    //2.1.3.5
                    if (!string.IsNullOrEmpty(optional5))
                    {
                        XmlNode beforeNode = invoice.SelectSingleNode("p:FatturaElettronica/FatturaElettronicaBody/DatiGenerali/DatiContratto/IdDocumento", xmlnsMan);
                        XmlNode DatiContratto = invoice.SelectSingleNode("p:FatturaElettronica/FatturaElettronicaBody/DatiGenerali/DatiContratto", xmlnsMan);
                        XmlNode nodeOpt5 = invoice.CreateElement("CodiceCommessaConvenzione");
                        nodeOpt5.InnerText = optional5;
                        DatiContratto.InsertAfter(nodeOpt5, beforeNode);
                        //DatiContratto.AppendChild(nodeOpt5);
                    }

                    if (!string.IsNullOrEmpty(posFin)){
                        XmlNode node_DataFattura = invoice.SelectSingleNode("p:FatturaElettronica/FatturaElettronicaBody/DatiGenerali/DatiGeneraliDocumento/Data", xmlnsMan);
                        string dataFAttura = node_DataFattura.InnerText;
                        string annoFattura = dataFAttura.Substring(0,4);

                        XmlNode node_DettaglioLinee = invoice.SelectSingleNode("p:FatturaElettronica/FatturaElettronicaBody/DatiBeniServizi/DettaglioLinee", xmlnsMan);
                        XmlElement altriDatiGestionali = invoice.CreateElement("AltriDatiGestionali");
                        XmlElement tipoDato = invoice.CreateElement("TipoDato");
                        tipoDato.InnerText = "POS_FIN";
                        altriDatiGestionali.AppendChild(tipoDato);

                        XmlElement riferimentoTesto = invoice.CreateElement("RiferimentoTesto");
                        riferimentoTesto.InnerText = posFin;
                        altriDatiGestionali.AppendChild(riferimentoTesto);

                        XmlElement riferimentoNumero = invoice.CreateElement("RiferimentoNumero");
                        riferimentoNumero.InnerText = "0.00";
                        altriDatiGestionali.AppendChild(riferimentoNumero);

                        XmlElement riferimentoData = invoice.CreateElement("RiferimentoData");
                        riferimentoData.InnerText = dataFAttura;
                        altriDatiGestionali.AppendChild(riferimentoData);

                        node_DettaglioLinee.AppendChild(altriDatiGestionali);

                        XmlElement altriDatiGestionali2 = invoice.CreateElement("AltriDatiGestionali");
                        XmlElement tipoDato2 = invoice.CreateElement("TipoDato");
                        tipoDato2.InnerText = "ANNO_FIN";
                        altriDatiGestionali2.AppendChild(tipoDato2);

                        XmlElement riferimentoTesto2 = invoice.CreateElement("RiferimentoTesto");
                        riferimentoTesto2.InnerText = annoFattura;
                        altriDatiGestionali2.AppendChild(riferimentoTesto2);

                        XmlElement riferimentoNumero2 = invoice.CreateElement("RiferimentoNumero");
                        riferimentoNumero2.InnerText = "0.00";
                        altriDatiGestionali2.AppendChild(riferimentoNumero2);

                        XmlElement riferimentoData2 = invoice.CreateElement("RiferimentoData");
                        riferimentoData2.InnerText = dataFAttura;
                        altriDatiGestionali2.AppendChild(riferimentoData2);

                        node_DettaglioLinee.AppendChild(altriDatiGestionali2);
                    }

                    //2.2.1.15
                    if (!string.IsNullOrEmpty(optional6))
                    {
                        XmlNode beforeNode = invoice.SelectSingleNode("p:FatturaElettronica/FatturaElettronicaBody/DatiBeniServizi/DettaglioLinee/AliquotaIVA", xmlnsMan);
                        XmlNode nodeOpt6 = invoice.SelectSingleNode("p:FatturaElettronica/FatturaElettronicaBody/DatiBeniServizi/DettaglioLinee", xmlnsMan);

                        XmlElement nodoOptional6 = invoice.CreateElement("RiferimentoAmministrazione");
                        nodoOptional6.InnerText = optional6;
                        nodeOpt6.InsertAfter(nodoOptional6, beforeNode);
                        //nodeOpt6.AppendChild(nodoOptional6);
                    }

                    if (!string.IsNullOrEmpty(strDesc) && !string.IsNullOrEmpty(strQuant) &&
                        !string.IsNullOrEmpty(strPrezUni) && !string.IsNullOrEmpty(strPrezTot) &&
                        !string.IsNullOrEmpty(strAliquot))
                    {
                        try
                        {
                            strPrezUni = strPrezUni.Replace("-", ""); strPrezTot = strPrezTot.Replace("-", "");
                            strPrezUni = strPrezUni.Replace("+", ""); strPrezTot = strPrezUni.Replace("+", "");

                            XmlNodeList listLinee = invoice.SelectNodes("p:FatturaElettronica/FatturaElettronicaBody/DatiBeniServizi/DettaglioLinee", xmlnsMan);
                            string numLinea = (listLinee.Count + 1).ToString();
                            XmlNode ultimoDettaglio = listLinee.Item(listLinee.Count - 1);

                            float pTotale;
                            float unitarioPrec;
                            float totalePrec;
                            string totale1;
                            string totale2;

                            //System.Globalization.NumberStyles style = System.Globalization.NumberStyles.AllowDecimalPoint;
                            //System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.CreateSpecificCulture("fr-FR");

                            float.TryParse(strPrezTot, out pTotale);

                            XmlNode nodoUnitarioPrec = ultimoDettaglio.SelectSingleNode("PrezzoUnitario", xmlnsMan);

                            float.TryParse(nodoUnitarioPrec.InnerText.Replace(".",","), out unitarioPrec);
                            totale1 = (unitarioPrec + pTotale).ToString().Replace(",",".");
                            if (!totale1.Contains('.'))
                                totale1 = totale1 + ".00";
                            nodoUnitarioPrec.InnerText = totale1;

                            XmlNode nodoTotalePrec = ultimoDettaglio.SelectSingleNode("PrezzoTotale", xmlnsMan);
                            float.TryParse(nodoTotalePrec.InnerText.Replace(".", ","), out totalePrec);
                            totale2 = (totalePrec + pTotale).ToString();
                            if (!totale2.Contains('.'))
                                totale2 = totale2 + ".00";
                            nodoTotalePrec.InnerText = totale2;

                            //XmlNode nodoTotaleImponibile = invoice.SelectSingleNode("p:FatturaElettronica/FatturaElettronicaBody/DatiBeniServizi/DatiRiepilogo/ImponibileImporto", xmlnsMan);
                            //float.TryParse(nodoTotaleImponibile.InnerText.Replace(".", ","), out totaleImponibile);
                            //nodoTotaleImponibile.InnerText = (totaleImponibile + pTotale).ToString();

                            //XmlNode nodoTotaleGenerale = invoice.SelectSingleNode("p:FatturaElettronica/FatturaElettronicaBody/DatiPagamento/DettaglioPagamento/ImportoPagamento", xmlnsMan);
                            //float.TryParse(nodoTotaleGenerale.InnerText.Replace(".", ","), out totaleGenerale);
                            //nodoTotaleGenerale.InnerText = (totaleGenerale + pTotale).ToString();

                            XmlElement dettaglioLinee = invoice.CreateElement("DettaglioLinee");

                            XmlElement numeroLinea = invoice.CreateElement("NumeroLinea");
                            numeroLinea.InnerText = numLinea;
                            XmlElement descrizione = invoice.CreateElement("Descrizione");
                            descrizione.InnerText = strDesc;
                            XmlElement quantita = invoice.CreateElement("Quantita");
                            quantita.InnerText = strQuant.Replace(",", ".") + ".00";
                            XmlElement prezzoUnitario = invoice.CreateElement("PrezzoUnitario");
                            prezzoUnitario.InnerText = "-" + (strPrezUni.Replace(",", "."));
                            XmlElement prezzoTotale = invoice.CreateElement("PrezzoTotale");
                            prezzoTotale.InnerText = "-" + (strPrezTot.Replace(",", "."));
                            XmlElement aliquotaIVA = invoice.CreateElement("AliquotaIVA");
                            aliquotaIVA.InnerText = strAliquot.Replace(",", ".") + ".00";

                            dettaglioLinee.AppendChild(numeroLinea);
                            dettaglioLinee.AppendChild(descrizione);
                            dettaglioLinee.AppendChild(quantita);
                            dettaglioLinee.AppendChild(prezzoUnitario);
                            dettaglioLinee.AppendChild(prezzoTotale);
                            dettaglioLinee.AppendChild(aliquotaIVA);

                            XmlNode datiBeniServizi = invoice.SelectSingleNode("p:FatturaElettronica/FatturaElettronicaBody/DatiBeniServizi", xmlnsMan);
                            datiBeniServizi.InsertAfter(dettaglioLinee, ultimoDettaglio);
                        }
                        catch
                        {
                            result = false;
                        }
                    }

                    if (result)
                    {
                        SetSessionValue("invoiceXML", invoice);
                        invoiceText = invoice.OuterXml;
                    }
                }
                else
                {
                    result = false;
                }

                // aggiorno preview
                if (!string.IsNullOrEmpty(invoiceText))
                {
                    DocsPaWR.FileDocumento preview = getInvoicePreview(invoiceText);
                    SetSessionValue("invoicePreview", preview);
                }
                else
                {
                    result = false;
                }

                return result;       
            }
            catch (Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return false;
            }

        }

        private static DocsPaWR.FileDocumento getInvoicePreview(string invoice)
        {
            try
            {
                string urlXSL = System.Configuration.ConfigurationManager.AppSettings["FATTURAPA_XSL_URL"];

                XmlDocument previewInvoice = new XmlDocument();
                string decl = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                string pi = "<?xml-stylesheet type=\"text/xsl\" href=\"" + urlXSL + "\"?>";
                string previewXml = invoice.Replace(decl, decl + "\n" + pi);
                previewInvoice.LoadXml(previewXml);

                Byte[] bytes = Encoding.UTF8.GetBytes(previewInvoice.OuterXml);
                DocsPaWR.FileDocumento fileDoc = new DocsPaWR.FileDocumento();

                fileDoc.cartaceo = false;
                fileDoc.contentType = "text/xml";
                fileDoc.content = bytes;
                fileDoc.length = bytes.Length;

                return fileDoc;
            }
            catch (Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return null;
            }
        }

        #region Session functions
        /// <summary>
        /// Reperimento valore da sessione
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        private static System.Object GetSessionValue(string sessionKey)
        {
            try
            {
                return System.Web.HttpContext.Current.Session[sessionKey];
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
                return null;
            }
        }

        /// <summary>
        /// Impostazione valore in sessione
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="sessionValue"></param>
        private static void SetSessionValue(string sessionKey, object sessionValue)
        {
            try
            {
                System.Web.HttpContext.Current.Session[sessionKey] = sessionValue;
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
            }
        }

        /// <summary>
        /// Rimozione chiave di sessione
        /// </summary>
        /// <param name="sessionKey"></param>
        private static void RemoveSessionValue(string sessionKey)
        {
            try
            {
                System.Web.HttpContext.Current.Session.Remove(sessionKey);
            }
            catch (System.Exception ex)
            {
                UIManager.AdministrationManager.DiagnosticError(ex);
            }
        }

        //private static DocsPaFatturazioneWR.InfoUtente getInfoUtente()
        //{
        //    try
        //    {
        //        Utente user = HttpContext.Current.Session["userData"] as Utente;
        //        DocsPaWR.Ruolo role = HttpContext.Current.Session["userRole"] as DocsPaWR.Ruolo;

        //        DocsPaFatturazioneWR.InfoUtente infoUtente = new DocsPaFatturazioneWR.InfoUtente();
        //        if (user != null && role != null)
        //        {
        //            infoUtente.idPeople = user.idPeople;
        //            infoUtente.dst = user.dst;
        //            infoUtente.idAmministrazione = user.idAmministrazione;
        //            infoUtente.userId = user.userId;
        //            infoUtente.sede = user.sede;
        //            if (user.urlWA != null)
        //                infoUtente.urlWA = user.urlWA;
        //            if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session["userDelegato"] != null)
        //                infoUtente.delegato = HttpContext.Current.Session["userDelegato"] as DocsPaFatturazioneWR.InfoUtente;

        //            infoUtente.idCorrGlobali = role.systemId;
        //            infoUtente.idGruppo = role.idGruppo;

        //            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["CODICE_APPLICAZIONE"]))
        //                infoUtente.codWorkingApplication = System.Configuration.ConfigurationManager.AppSettings["CODICE_APPLICAZIONE"].ToString();
        //        }

        //        return infoUtente;

        //    }
        //    catch (System.Exception ex)
        //    {
        //        UIManager.AdministrationManager.DiagnosticError(ex);
        //        return null;
        //    }
        //}

        #endregion

    }


}