using PdfService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MODELS = PdfService.Model;

namespace BusinessLogic.Pdf
{
    public class PdfManager
    {
        private static IPdfServiceProvider _pdfServiceProvider;
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static void _init()
        {
            if(PdfManager._pdfServiceProvider == null)
            {
                var _pdfService = ConfigurationManager.AppSettings["PDF_MANAGER"].ToUpper();
                switch (_pdfService)
                {
                    case "ASPOSE":
                        _pdfServiceProvider = new PdfServiceProvider<AsposePdfService.AsposePdfServiceProvider>();
                        break;
                }
            }
        }



        public static MODELS.PdfPageInfo GetPdfPageInfo(Stream pdfStreamFile, int pageNumber = 1)
        {
            PdfManager._logger?.Info("START");
            MODELS.PdfPageInfo _pdfPageInfo;
            try
            {
                PdfManager._init();
                _pdfPageInfo = PdfManager._pdfServiceProvider.GetPdfPageInfo(pdfStreamFile, pageNumber);
            }
            catch(Exception ex)
            {
                PdfManager._logger?.Error(ex.Message, ex);
                _pdfPageInfo = null;
            }
            PdfManager._logger?.Info("END");
            return _pdfPageInfo;
        }


    }
}
