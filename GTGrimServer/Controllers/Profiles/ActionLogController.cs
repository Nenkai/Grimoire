using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.IO;

using GTGrimServer.Filters;
using GTGrimServer.Sony;
using GTGrimServer.Models;

namespace GTGrimServer.Helpers
{
    /// <summary>
    /// Handles logging made by the game for the server to keep track of what the player is doing.
    /// </summary>
    [ApiController]
    [PDIClient]
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
                return BadRequest();

            switch (requestReq.Command)
            {
                case "actionlog.putActionLog":
                    return OnActionPutLog(requestReq);
            }

            _logger.LogDebug("Got unimplemented actionlog call: {command}", requestReq.Command);
            

            return BadRequest();
        }

        // getActionLogListPath - see requestActionLogList in gtmode.ad
        public ActionResult OnActionPutLog(GrimRequest gRequest)
        {
            // 6 params, returns a path
            return Ok(GrimResult.FromString("test.txt"));
        }
    }
}
