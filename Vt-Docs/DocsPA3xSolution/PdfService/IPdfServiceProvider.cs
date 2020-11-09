using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfService
{
    public interface IPdfServiceProvider
    {
        Model.PdfPageInfo GetPdfPageInfo(Stream pdfStreamFile, int pageNumber = 1);
    }
}
