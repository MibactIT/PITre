using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MODELS = PdfService.Model;

namespace PdfService
{
    public class PdfServiceProvider<T> : IPdfServiceProvider where T: IPdfServiceProvider, new()
    {
        private IPdfServiceProvider _pdfProvider;
        public PdfServiceProvider()
        {
            this._pdfProvider = new T();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pdfStreamFile"></param>
        /// <param name="pageNumber">Numero della pagina (prima pagina = 1 [default])</param>
        /// <returns></returns>
        public MODELS.PdfPageInfo GetPdfPageInfo(Stream pdfStreamFile, int pageNumber = 1)
        {
            return this._pdfProvider.GetPdfPageInfo(pdfStreamFile, pageNumber);
        }
    }
}
