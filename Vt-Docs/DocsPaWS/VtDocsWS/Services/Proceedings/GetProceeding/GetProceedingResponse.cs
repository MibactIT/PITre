using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace VtDocsWS.Services.Proceedings.GetProceeding
{
    [DataContract]
    public class GetProceedingResponse : Response
    {
        [DataMember]
        public Domain.Phase[] Phases { get; set; }

        [DataMember]
        public Domain.Project Proceeding { get; set; }

        [DataMember]
        public Domain.DocInProceeding[] Documents { get; set; }

        [DataMember]
        public String DueDate { get; set; }

        [DataMember]
        public String ClosureDate { get; set; }

        [DataMember]
        public String PersonInCharge { get; set; }

        [DataMember]
        public String Duration { get; set; }

        [DataMember]
        public int Status { get; set; }

        [DataMember]
        public bool Redirected { get; set; }

        [DataMember]
        public String IdAOO { get; set; }

        [DataMember]
        public string Typology { get; set; }
    }
}