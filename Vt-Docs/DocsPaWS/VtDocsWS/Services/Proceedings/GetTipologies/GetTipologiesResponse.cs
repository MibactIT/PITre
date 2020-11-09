using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace VtDocsWS.Services.Proceedings.GetTipologies
{
    [DataContract]
    public class GetTipologiesResponse : Response
    {
        [DataMember]
        public String[] Typologies
        {
            get;
            set;
        }
    }
}