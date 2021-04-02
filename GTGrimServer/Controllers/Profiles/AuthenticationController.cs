using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.IO;

using GTGrimServer.Models;
using GTGrimServer.Utils;
using GTGrimServer.Sony;

namespace GTGrimServer.Controllers
{
    [ApiController]
    [Route("/ap/ticket/login/{pfsVersion}/")]
    [Produces("application/xml")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(ILogger<AuthenticationController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<TicketResult> Post(ulong pfsVersion)
        {
            if (Request.ContentLength == 0)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return null;
            }

            byte[] buf = new byte[(int)Request.ContentLength];
            await Request.Body.ReadAsync(buf, 0, buf.Length);

            Ticket ticket = Ticket.FromBuffer(buf);

            _logger.LogDebug("Ticket Auth -> PFS: {pfsVersion} | OnlineID:{OnlineId} | Region: {Region}", pfsVersion, ticket.OnlineId, ticket.Region);

            var resp = new TicketResult()
            {
                Result = "1",
                Nickname = ticket.OnlineId,
                UserId = ticket.UserId,
                UserNumber = "0",
                ServerTime = DateTime.Now.ToRfc3339String(),
            };

            return resp;
        }
    }
}
