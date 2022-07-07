using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UF.AssessmentProject.Model.Transaction
{
    public class RequestMessage : Model.RequestMessage
    {
        public string partnerrefno { get; set; }
        public string totalamount { get; set; }
        public itemdetail items { get; set; } 
    }

    public class itemdetail
    {
        public string partneritemref { get; set; }
        public string name { get; set; }

    }

    public class ResponseMessage: Model.ResponseMessage
    {
    }
}
