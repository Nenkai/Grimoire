using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.IO;

using GTGrimServer.Sony;
using GTGrimServer.Models;

namespace GTGrimServer.Helpers
{
    /// <summary>
    /// Handles logging made by the game for the server to keep track of what the player is doing.
    /// </summary>
    [ApiController]
    [Route("/ap/[controller]/")]
    [Produces("application/xml")]
    public class ActionLogController : ControllerBase
    {
        private readonly ILogger<LogController> _logger;

        public ActionLogController(ILogger<LogController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> Get(string server)
        {
            GrimRequest requestReq = await GrimRequest.Deserialize(Request.Body);
            if (requestReq is null)
            {
                // Handle
                return null;
            }

            if (requestReq.Command.Equals("actionlog.putActionLog")) // getActionLogListPath - see requestActionLogList in gtmode.ad
            {
                // 6 params, returns a path
                return Ok(GrimResult.FromString("test.txt"));
            }
            else
            {
                _logger.LogDebug("Got unimplemented actionlog call: {command}", requestReq.Command);
            }

            return NoContent();
        }

    }
}
