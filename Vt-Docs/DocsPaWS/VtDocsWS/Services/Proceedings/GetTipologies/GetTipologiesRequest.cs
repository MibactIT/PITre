using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace VtDocsWS.Services.Proceedings.GetTipologies
{
    [DataContract]
    public class GetTipologiesRequest : Request
    {
        [DataMember]
        public String ObjectType
        {
            get;
            set;
        }

        [DataMember]
        public String[] AOO
        {
            get;
            set;
        }
    }
}