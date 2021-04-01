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

namespace GTGrimServer.Helpers
{
    /// <summary>
    /// Handles logging made by the game for the server to keep track of what the player is doing.
    /// </summary>
    [ApiController]
    [Route("/log/{server}/")]
    [Produces("application/xml")]
    public class LogController : ControllerBase
    {
        private readonly ILogger<LogController> _logger;

        public LogController(ILogger<LogController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Get(string server)
        {
            if (!Request.Headers.TryGetValue("X-gt-log", out var hVal))
                return NoContent();

            string argStr = hVal.FirstOrDefault();
            string[] args = argStr.Split(':');
            LogHelper.Humanify(args);

            string humanified = string.Join(':', args);
            _logger.LogDebug(humanified);

            return Ok();
        }

    }
}
