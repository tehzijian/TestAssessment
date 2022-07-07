using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UF.AssessmentProject.Helper
{
    public class DataDictionary
    {
        /// <summary>
        /// response Result
        ///    failed = 0,
        ///    success = 1,
        ///    pending = 2
        /// </summary>
        public enum responseResult
        {
            /// <summary>
            /// Failed
            /// </summary> 
            failed = 0,
            success = 1 
        }

        public class ErrorMessage
        {
            /// <summary>
            /// Failed
            /// </summary> 
            //Signature Mismatch
            public const string sigMismatch = "Access Denied!";
            //totalamount
            public const string totalamount = "Invalid Total Amount.";
            //timestamp
            public const string timestamp = "Expired.";
            public const string invalidTimeStamp = "Invalid TimeStamp.";
            //Mandatory Parameter
            public const string parameterPartnerkey = "partnerkey is Required.";
            public const string parameterPartnerrefno = "partnerrefno is Required.";
            public const string parameterTimestamp = "timestamp is Required.";
            public const string parameterSig = "sig is Required.";
            public const string parameterPartnerPassword = "partnerpassword is Required.";
            public const string businessLogicTotalAmount = "total amount in negative.";
            public const string businessLogicQty = "qty must more than 1 and less than 5.";
            public const string businessLogicUnitprice = "unit price in negative";
            //Exception Server Error
            public const string exceptionServerError = "exception Server Error";
        }

        public class ValidMessage
        {
            /// <summary>
            /// Failed
            /// </summary> 
            //
            public const string RequestValid = "Request data is valid.";
            //Exception_Server_Error = 0,
            //success = 1
        }
    }
}
