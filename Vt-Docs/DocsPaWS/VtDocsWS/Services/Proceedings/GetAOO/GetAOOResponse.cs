using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace VtDocsWS.Services.Proceedings.GetAOO
{
    [DataContract]
    public class GetAOOResponse : Response
    {
        [DataMember]
        public Domain.Register[] AOO
        {
            get;
            set;
        }

    }
}