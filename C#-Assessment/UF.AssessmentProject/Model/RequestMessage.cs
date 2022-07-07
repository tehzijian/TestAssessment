using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UF.AssessmentProject.Helper;

namespace UF.AssessmentProject.Model
{
    public class RequestMessage
    {

        public string partnerkey { get; set; }
        public string partnerrefno { get; set; }
        public long totalamount { get; set; }
        public List<item> items { get; set; }
        public string timestamp { get; set; }
        public string sig { get; set; }

        public string partnerpassword { get; set; }

        /// <summary>
        /// Time in the ISO format. ie. 2014-04-14T12:34:23.00+0800 Preferdably set using RealTimeStamp instead.
        /// </summary>
        /// <example>2020-07-28T12:34:23.00+0800</example>
        /// <returns></returns> 
        //[Required]
        //public string timestamp
        //{
        //    get { return _realTimeStamp.ToUniversalTime().ToString("o"); }
        //    set { _realTimeStamp = DateTime.Parse(value, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.RoundtripKind); }
        //}

        private DateTime _realTimeStamp;
        /// <summary>
        /// When this field is set, automatically converts to the string 
        /// for the timestamp property
        /// </summary> 
        /// <remarks>Internal use</remarks>
        [Newtonsoft.Json.JsonIgnore()]
        [System.Text.Json.Serialization.JsonIgnore()]
        public DateTime RealTimeStamp
        {
            get { return _realTimeStamp; }
        }

    }

    public class item
    {
        public string partneritemref { get; set; }
        public string name { get; set; }
        public int qty { get; set; }
        public long unitprice { get; set; }
    }
     
}
