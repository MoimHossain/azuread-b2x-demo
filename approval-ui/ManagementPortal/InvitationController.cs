using AadBxApprovals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ManagementPortal
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationController : ControllerBase
    {
        private readonly ILogger<InvitationController> logger;

        public InvitationController(ILogger<InvitationController> logger)
        {
            this.logger = logger;
        }
        // GET: api/<InvitationController>
        [HttpGet]
        public async Task<IEnumerable<PayloadEntity>> Get()
        {
            return await TableStorage.GetAllAsync(this.logger);
        }


        // POST api/<InvitationController>
        [HttpPost("approve")]
        public async Task<object> PostApproveAsync([FromBody] InviteChangePayload payload)
        {
            await TableStorage.ChangeStatusAsync(this.logger, payload.Email, ApprovalState.Approved);

            return new { Ok = true };
        }

        [HttpPost("reject")]
        public async Task<object> PostRejectAsync([FromBody] InviteChangePayload payload)
        {
            await TableStorage.ChangeStatusAsync(this.logger, payload.Email, ApprovalState.Denied);

            return new { Ok = true };
        }

        public class InviteChangePayload
        {
            public string Email { get; set; }
        }
    }
}
