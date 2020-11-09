using PdfService.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsposePdfService
{
    public class AsposePdfServiceProvider : PdfService.IPdfServiceProvider
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static Aspose.Pdf.License _license = new Aspose.Pdf.License();
        private static bool _isLicensed = false;

        /// <summary>
        /// Richaimare il seguente metodo prima di ogni operazione
        /// </summary>
        private static void _CheckLicense()
        {
            if (!_isLicensed)
            {
                string license = "<License><Data><LicensedTo>NTTData Italia</LicensedTo><EmailTo>roberto.bresciani@nttdata.com</EmailTo><LicenseType>Developer OEM</LicenseType><LicenseNote>Limited to 1 developer, unlimited physical locations</LicenseNote><OrderID>160914101103</OrderID><UserID>48528</UserID><OEM>This is a redistributable license</OEM><Products><Product>Aspose.Total for .NET</Product></Products><EditionType>Enterprise</EditionType><SerialNumber>2aad30f8-4ac2-493b-ae51-572bc1a96820</SerialNumber><SubscriptionExpiry>20171101</SubscriptionExpiry><LicenseVersion>3.0</LicenseVersion><LicenseInstructions>http://www.aspose.com/corporate/purchase/license-instructions.aspx</LicenseInstructions></Data><Signature>Na+lOX38Zfw8kwUC779jeFZEH/XbZMNEqY7rZStQQikMwT0ZLia9x9MKRQ1ZJtEDKCSJigtxjYoNd/a7F0D4cq+YDfyy861g4HuVr+92UeaV4RI3NqC2D4o2djruLUjxHpZLUmXO4wbqrmFPFlNfMbkzXQ9Blu/4n4/MDjPgsG8=</Signature></License>";

                byte[] byteArray = Encoding.ASCII.GetBytes(license);
                MemoryStream stream = new MemoryStream(byteArray);
                _license.SetLicense(stream);
                _isLicensed = true;
            }
        }

        public PdfPageInfo GetPdfPageInfo(Stream pdfStreamFile, int pageNumber = 1)
        {
            AsposePdfServiceProvider._logger?.Info("START");
            PdfPageInfo _fileInfo = null;
            try
            {
                AsposePdfServiceProvider._CheckLicense();
                Aspose.Pdf.Document _pdf = new Aspose.Pdf.Document(pdfStreamFile);
                Aspose.Pdf.Page _page = _pdf?.Pages[pageNumber > 0 && pageNumber <= _pdf.Pages.Count ? pageNumber : 1]; // index base 1 //
                _fileInfo = new PdfPageInfo
                {
                    PageHeight = _pdf.PageInfo.Height,
                    PageWidth = _pdf.PageInfo.Width,
                    Margin = new PdfMarginInfo()
                    {
                        Bottom =_pdf.PageInfo.Margin.Bottom,
                        Top = _pdf.PageInfo.Margin.Top,
                        Left = _pdf.PageInfo.Margin.Left,
                        Right = _pdf.PageInfo.Margin.Right
                    },
                    Rectangle = new PdfPageRectangle
                    {
                        Height = _page.Rect.Height,
                        Width = _page.Rect.Width
                    }
                };
            }
            catch(Exception ex)
            {
                AsposePdfServiceProvider._logger?.Error(ex.Message, ex);
                _fileInfo = null;
            }
            AsposePdfServiceProvider._logger?.Info("INFO");
            return _fileInfo;
        }
    }
}
