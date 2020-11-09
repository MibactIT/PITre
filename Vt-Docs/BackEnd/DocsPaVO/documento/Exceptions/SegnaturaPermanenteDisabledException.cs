using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsPaVO.documento.Exceptions
{
    public class SegnaturaPermanenteDisabledException: Exception
    {
        public SegnaturaPermanenteDisabledException() : base() { }

        public SegnaturaPermanenteDisabledException(string message) : base(message) { }

        public SegnaturaPermanenteDisabledException(string message, Exception inner) : base(message, inner) { }
    }
}
