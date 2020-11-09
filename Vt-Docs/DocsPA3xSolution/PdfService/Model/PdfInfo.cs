using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfService.Model
{
    public class PdfPageInfo
    {
        public double PageHeight { get; set; }
        public double PageWidth { get; set; }
        public PdfMarginInfo Margin { get; set; }
        public PdfPageRectangle Rectangle { get; set; }


    }
}
