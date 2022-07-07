using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UF.AssessmentProject.Controllers
{
    [Produces("application/json"),
        Route("api/[action]"),
        ApiController]
    [SwaggerTag("Transaction Middleware Controller to keep transactional data in Log Files")]
    public class TransactionController : ControllerBase
    {
        private readonly ILoggerManager logger;
        public TransactionController(ILoggerManager logger)
        {
            this.logger = logger;
        }
        [HttpPost]
        public ActionResult<UF.AssessmentProject.Model.ResponseMessage> submittrxmessage(UF.AssessmentProject.Model.RequestMessage req)
        {
            this.logger.LogInformation("Request: " + Newtonsoft.Json.JsonConvert.SerializeObject(req));
            UF.AssessmentProject.Model.ResponseMessage results = new UF.AssessmentProject.Model.ResponseMessage();
            var currentDateTime = DateTime.UtcNow;
            var reqTimeStamp = new DateTime();
            try
            {
                //Mandatory Parameter
                if (String.IsNullOrEmpty(req.partnerkey))
                {
                    results.errormessage = Helper.DataDictionary.ErrorMessage.parameterPartnerkey;
                    results.success = Helper.DataDictionary.responseResult.failed;
                    this.logger.LogInformation("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(results));
                    return Ok(results);
                }
                if (String.IsNullOrEmpty(req.partnerpassword))
                {
                    results.errormessage = Helper.DataDictionary.ErrorMessage.parameterPartnerPassword;
                    results.success = Helper.DataDictionary.responseResult.failed;
                    this.logger.LogInformation("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(results));
                    return Ok(results);
                }
                if (String.IsNullOrEmpty(req.partnerrefno))
                {
                    results.errormessage = Helper.DataDictionary.ErrorMessage.parameterPartnerrefno;
                    results.success = Helper.DataDictionary.responseResult.failed;
                    this.logger.LogInformation("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(results));
                    return Ok(results);
                }
                if (String.IsNullOrEmpty(req.timestamp))
                {
                    results.errormessage = Helper.DataDictionary.ErrorMessage.parameterTimestamp;
                    results.success = Helper.DataDictionary.responseResult.failed;
                    this.logger.LogInformation("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(results));
                    return Ok(results);
                }
                if (String.IsNullOrEmpty(req.sig))
                {
                    results.errormessage = Helper.DataDictionary.ErrorMessage.parameterSig;
                    results.success = Helper.DataDictionary.responseResult.failed;
                    this.logger.LogInformation("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(results));
                    return Ok(results);
                }

                var sigTimestamp = DateTime.TryParse(req.timestamp, out reqTimeStamp);

                if (sigTimestamp)
                {
                    var formatetTimeStamp = reqTimeStamp.ToString("yyyyMMddHHmmss");

                    // Signature Mismatch
                    var sigMatch = formatetTimeStamp + req.partnerkey + req.partnerrefno + req.totalamount + req.partnerpassword;
                    using (SHA256 hash = SHA256Managed.Create())
                    {
                        Encoding enc = Encoding.UTF8;
                        Byte[] result = hash.ComputeHash(enc.GetBytes(sigMatch));
                        var sigMessage = Convert.ToBase64String(result);
                        if (sigMessage != req.sig)
                        {
                            results.errormessage = Helper.DataDictionary.ErrorMessage.sigMismatch;
                            results.success = Helper.DataDictionary.responseResult.failed;
                            this.logger.LogInformation("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(results));
                            return Ok(results);
                        }
                    }

                    foreach (var item in req.items)
                    {
                        if(item.qty <= 1 || item.qty > 5)
                        {
                            results.errormessage = Helper.DataDictionary.ErrorMessage.businessLogicQty;
                            results.success = Helper.DataDictionary.responseResult.failed;
                            this.logger.LogInformation("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(results));
                            return Ok(results);
                        }

                        if (item.unitprice < 0)
                        {
                            results.errormessage = Helper.DataDictionary.ErrorMessage.businessLogicUnitprice;
                            results.success = Helper.DataDictionary.responseResult.failed;
                            this.logger.LogInformation("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(results));
                            return Ok(results);
                        }
                    }

                    // timestamp
                    double min = (currentDateTime - reqTimeStamp.ToUniversalTime()).TotalMinutes;

                    if (min < 0) min *= -1;

                    if (min >= 5)
                    {
                        results.errormessage = Helper.DataDictionary.ErrorMessage.timestamp;
                        results.success = Helper.DataDictionary.responseResult.failed;
                        this.logger.LogInformation("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(results));
                        return Ok(results);
                    }
                }
                else
                {
                    results.errormessage = Helper.DataDictionary.ErrorMessage.invalidTimeStamp;
                    results.success = Helper.DataDictionary.responseResult.failed;
                    this.logger.LogInformation("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(results));
                    return Ok(results);
                }

                //totalamount
                if (req.items.Count > 0)
                {
                    //Only allow positive value
                    if (req.totalamount < 0)
                    {
                        results.errormessage = Helper.DataDictionary.ErrorMessage.businessLogicTotalAmount;
                        results.success = Helper.DataDictionary.responseResult.failed;
                        this.logger.LogInformation("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(results));
                        return Ok(results);
                    }
                    long calculatedTotalamount = 0;
                    foreach (var item in req.items)
                    {
                        calculatedTotalamount += (item.qty * item.unitprice);
                    }
                    //itemDetails
                    if (calculatedTotalamount != req.totalamount)
                    {
                        results.errormessage = Helper.DataDictionary.ErrorMessage.totalamount;
                        results.success = Helper.DataDictionary.responseResult.failed;
                        this.logger.LogInformation("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(results));
                        return Ok(results);
                    }
                }
            }
            catch (Exception ex)
            {
                results.errormessage = Helper.DataDictionary.ErrorMessage.exceptionServerError;
                results.success = Helper.DataDictionary.responseResult.failed;
                this.logger.LogInformation("Exception: " + Newtonsoft.Json.JsonConvert.SerializeObject(ex));
                return Ok(results);
            }

            results.errormessage = Helper.DataDictionary.ValidMessage.RequestValid;
            results.success = Helper.DataDictionary.responseResult.success;
            this.logger.LogInformation("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(results));
            return Ok(results);
        }
    }
}
