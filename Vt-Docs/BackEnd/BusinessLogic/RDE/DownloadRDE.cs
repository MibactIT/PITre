using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using DocsPaVO.Rde;
using System.IO;
using System.IO.Compression;
using Ionic.Zip;
using System.Web;
using System.Xml.Linq;
using System.Xml;
using log4net;
using System.Configuration;

namespace BusinessLogic.RDE
{
    public class DownloadRDE : ConfigRde
    {
        private static ILog logger = LogManager.GetLogger(typeof(DownloadRDE));

        public void ReadConfig()
        {
            try
            {
                string xmlfile = File.ReadAllText("config.xml");
                setValues((ConfigRde)xmlfile.Deserialize<ConfigRde>());
            }
            catch
            {
                //setNewValues();

                //string xmlfile = this.Serialize();
                //File.WriteAllText("config.xml", xmlfile);
            }
        }
        public void setValues(ConfigRde entry)
        {
            this.ArrivoLabel = entry.ArrivoLabel;
            this.InternoLabel = entry.InternoLabel;
            this.PartenzaLabel = entry.PartenzaLabel;

            if (this.EnforceSecurity)
            {
                if (entry.SecurityString != calkHash(entry))
                {
                    this.CodiceStringaProtEmerg = "§§§§§EEEEERRRRRROOOOOOORRRRRRR§§§";
                    return;
                }
            }

            this.CodiceClassificaDefault = entry.CodiceClassificaDefault;
            this.CodiceRegistro = entry.CodiceRegistro;
            this.CodiceAmministrazione = entry.CodiceAmministrazione;
            this.CodiceStringaProtEmerg = entry.CodiceStringaProtEmerg;
        }
        public void setNewValues()
        {
            this.ArrivoLabel = "Arrivo";
            this.InternoLabel = "Interno";
            this.PartenzaLabel = "Partenza";
            this.CodiceClassificaDefault = "CC_DEF";
            this.CodiceAmministrazione = "AMM_DEF";
            this.CodiceRegistro = "REG_DEF";
            this.CodiceStringaProtEmerg = "MiBACT|{0}|{1}|{2}-{3}";

            this.SecurityString = calkHash(this);
        }

        public string calkHash(ConfigRde entry)
        {

            if (!entry.EnforceSecurity)
                return "-";

            string retval =
                entry.ArrivoLabel +
                entry.InternoLabel +
                entry.PartenzaLabel +
                entry.CodiceClassificaDefault +
                entry.CodiceAmministrazione +
                entry.CodiceRegistro +
                entry.CodiceStringaProtEmerg;

            return sha256_hash(retval);
        }

        public static String sha256_hash(String value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }
            return Sb.ToString();
        }

        private static XmlDocument GetXmlDocument(XDocument document)
        {
            using (XmlReader xmlReader = document.CreateReader())
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlReader);
                if (document.Declaration != null)
                {
                    XmlDeclaration dec = xmlDoc.CreateXmlDeclaration(document.Declaration.Version,
                        document.Declaration.Encoding, document.Declaration.Standalone);
                    xmlDoc.InsertBefore(dec, xmlDoc.FirstChild);
                }
                return xmlDoc;
            }
        }

        public void addConfigXMLFile_toSetupZipped(ConfigRde entry, out byte[] outByte)
        {
            string xmlFile = ConfigurationManager.AppSettings["DOWNLOAD_RDE_PATH"] + @"\Config.xml";
            string zippedFile = ConfigurationManager.AppSettings["DOWNLOAD_RDE_PATH"] + @"\RdeTool.zip";

            outByte = null;

            try
            {
                //*--- Sostituisco i valori per personalizzare il file xml di controllo -----------------
                //XDocument doc = XDocument.Load(xmlFile);
                XDocument doc = XDocument.Parse(File.ReadAllText(xmlFile));

                foreach (XElement cell in doc.Elements("Config"))
                {
                    if (cell.Element("CodiceAmministrazione").Value == "AMM_DEF")
                    {
                        cell.Element("CodiceAmministrazione").Value = entry.CodiceAmministrazione;
                    }
                    if (cell.Element("CodiceClassificaDefault").Value == "CC_DEF")
                    {
                        cell.Element("CodiceClassificaDefault").Value = entry.CodiceClassificaDefault;
                    }
                    if (cell.Element("CodiceRegistro").Value == "REG_DEF")
                    {
                        cell.Element("CodiceRegistro").Value = entry.CodiceRegistro;
                    }
                    if (cell.Element("SecurityString").Value == "20a84c916ae5790da761c64e3b0a3040ddcfb1c55c4ef35e4bfd9a3bf76cf4c3")
                    {
                        cell.Element("SecurityString").Value = entry.SecurityString;
                    }
                }
                //*---------------------------------------------------------------------------------------

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc = GetXmlDocument(doc);

                MemoryStream resFilestream = new MemoryStream();

                xmlDoc.Save(resFilestream);

                resFilestream.Flush();//Adjust this if you want read your data 
                resFilestream.Position = 0;

                if (resFilestream != null)
                {
                    byte[] ba = new byte[resFilestream.Length];
                    resFilestream.Read(ba, 0, ba.Length);

                    using (ZipFile zip = ZipFile.Read(zippedFile))
                    {
                        MemoryStream zipStream = new MemoryStream();
                        //Aggiunge dal template .zip il file config.xml --------
                        zip.AddEntry("Config.xml", ba);
                        zip.Save(zipStream);
                        zipStream.Position = 0;
                        //*-----------------------------------------------------

                        //Memorizza il file da scaricare... --------------------
                        //outByte = Encoding.ASCII.GetBytes(zip.ToString());
                        outByte = zipStream.ToArray();
                        //*-----------------------------------------------------

                        //*--- Ritoglie dal template .zip il file config.xml ---
                        zip.RemoveEntry("Config.xml");
                        zip.Save();
                        //*-----------------------------------------------------
                    }
                }
            }
            catch (Exception e)
            {
                logger.Debug(e.Message);
                logger.Debug("Errore in BusinessLogic.RDE.DownloadRDE  - metodo: addConfigXMLFile_toSetupZipped ", e);
            }
        }
    }
}