

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AadBxApprovals
{
    public static class Functions
    {
        [FunctionName("CheckApproval")]
        public static async Task<IActionResult> CheckApproval(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            var email = ((Newtonsoft.Json.Linq.JValue)(data?.email)).Value.ToString();

            if (!string.IsNullOrWhiteSpace(email))
            {
                var payload = await TableStorage.GetAsync(email, log);
                if (payload == null)
                {   // never registered before- first time user
                    await TableStorage.AddAsync(email);
                    return Responses.PendingReviewResponse;
                }
                else if (payload.State == ApprovalState.Pending)
                {
                    return Responses.PendingReviewRepeatResponse;

                }
                else if(payload.State == ApprovalState.Denied)
                {
                    return Responses.DeniedResponse;
                }
            }
            return Responses.ContinueResponse;
        }

        [FunctionName("RequestApproval")]
        public static async Task<IActionResult> RequestApproval(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            var email = ((Newtonsoft.Json.Linq.JValue)(data?.email)).Value.ToString();

            if (!string.IsNullOrWhiteSpace(email))
            {
                var payload = await TableStorage.GetAsync(email, log);               
            }
            return Responses.ContinueResponse;
        }
    }
}
