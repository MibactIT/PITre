using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsPaVO.documento
{
    public class DettaglioSegnaturaPosition
    {
        public string SystemId { get; set; }
        public string ProfileID { get; set; }

        public string SegnaturaPosition { get; set; } // TOP,BOTTOM,LEFT,RIGHT
    }
}
