﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace VtDocsWS.Services.DocumentsAdvanced.CaricaLottoInPi3
{
    [DataContract]
    public class CaricaLottoInPi3Response : Response
    {
        /// <summary>
        /// Dettaglio del documento creato
        /// </summary>
        [DataMember]
        public Domain.Document Document
        {
            get;
            set;
        }
    }
}