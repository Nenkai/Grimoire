using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.IO;

using GTGrimServer.Controllers;
using GTGrimServer.Filters;
using GTGrimServer.Config;
using GTGrimServer.Models;
using GTGrimServer.Utils;
using GTGrimServer.Sony;

namespace GTGrimServer.Controllers.Profiles
{
    [ApiController]
    [PDIClient]
    [Route("/ap/ticket/login/{pfsVersion}/")]
    [Produces("application/xml")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly GameServerOptions _gsOptions;

        public AuthenticationController(IOptions<GameServerOptions> gsOptions, ILogger<AuthenticationController> logger)
        {
            _logger = logger;
            _gsOptions = gsOptions.Value;
        }

        [HttpPost]
        public async Task<ActionResult> Post(ulong pfsVersion)
        {
            PFSType pfsType = (PFSType)pfsVersion;
            _logger.LogDebug("Login request from {host} with PFS: ", Request.Host, pfsType);

            if (!EnsureVersion(pfsType))
                return Forbid();

            Ticket ticket;
            try
            {
                byte[] buf = new byte[(int)Request.ContentLength];
                await Request.Body.ReadAsync(buf, 0, buf.Length);
                ticket = Ticket.FromBuffer(buf);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not read NP ticket provided by client");
                return BadRequest();
            }

            _logger.LogDebug("Got auth request - NP_Ticket-> PFS: {pfsVersion} | OnlineID: {OnlineId} | Region: {Region}", pfsVersion, ticket.OnlineId, ticket.Region);

            var resp = new TicketResult()
            {
                Result = "1",
                Nickname = ticket.OnlineId,
                UserId = ticket.UserId,
                UserNumber = "0",
                ServerTime = DateTime.Now.ToRfc3339String(),
            };

            return Ok(resp);
        }

        /// <summary>
        /// Checks if the game version provided is suitable for the server.
        /// </summary>
        /// <param name="pfs"></param>
        /// <returns></returns>
        private bool EnsureVersion(PFSType pfs)
        {
            if (_gsOptions.GameType == GameType.GT6)
            {
                if (pfs < PFSType.GT6_V1_22)
                {
                    _logger.LogInformation("Client Fail: Received PFS type {type} - expected 1.22.", pfs);
                    return false;
                }
            }

            return true;
        }

        private bool VerifyTicket(Ticket ticket)
        {
            return true;
        }
    }
}
