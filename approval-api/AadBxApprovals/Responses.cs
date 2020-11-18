

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace AadBxApprovals
{
    public class Responses
    {
        public static OkObjectResult ContinueResponse
        {
            get
            {
                return new OkObjectResult(new
                {
                    version = "1.0.0",
                    action = "Continue"
                });
            }
        }

        public static OkObjectResult PendingReviewResponse
        {
            get
            {
                return new OkObjectResult(new
                {
                    version = "1.0.0",
                    action = "ShowBlockPage",
                    userMessage = "We are processing your account. It's pending for approval in backoffice. You'll be notified when your request has been approved.",
                    code = "CLOUDOVEN-APPROVAL-PENDING"
                });
            }
        }

        public static OkObjectResult PendingReviewRepeatResponse
        {
            get
            {
                return new OkObjectResult(new
                {
                    version = "1.0.0",
                    action = "ShowBlockPage",
                    userMessage = "Your request is registered and we are busy processing your account. It's pending for approval in backoffice. You'll be notified when your request has been approved.",
                    code = "CLOUDOVEN-APPROVAL-PENDING"
                });
            }
        }

        public static OkObjectResult DeniedResponse
        {
            get
            {
                return new OkObjectResult(new
                {
                    version = "1.0.0",
                    action = "ShowBlockPage",
                    userMessage = "Your request is denied. Sorry, please contact your administrator and take it from there.",
                    code = "CLOUDOVEN-APPROVAL-PENDING"
                });
            }
        }
    }
}
