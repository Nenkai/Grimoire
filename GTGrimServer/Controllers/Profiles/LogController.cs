using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.IO;

using GTGrimServer.Controllers;
using GTGrimServer.Services;
using GTGrimServer.Sony;
using GTGrimServer.Filters;

namespace GTGrimServer.Helpers
{
    /// <summary>
    /// Handles logging made by the game for the server to keep track of what the player is doing.
    /// </summary>
    [ApiController]
    [PDIClient]
    [Authorize]
    [Route("/log/{server}/")]
    [Produces("application/xml")]
    public class LogController : GrimControllerBase
    {
        private readonly ILogger<LogController> _logger;

        public LogController(PlayerManager playerManager, ILogger<LogController> logger)
            : base(playerManager)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Get(string server)
        {
            if (!Request.Headers.TryGetValue("X-gt-log", out var hVal))
                return BadRequest();

            if (Player is null)
            {
                _logger.LogWarning("Could not get current player");
                return BadRequest();
            }

            string argStr = hVal.FirstOrDefault();
            if (argStr.StartsWith("ADHOC"))
            {
                _logger.LogInformation("[{name}] received ADHOC crash: {argStr}", Player.Data.PSNUserId, argStr);
                return Ok();
            }

            string[] args = argStr.Split(':');
            LogHelper.Humanify(args);

            string humanified = string.Join(':', args);
            _logger.LogInformation("[{name}] Log: {args}", Player.Data.PSNUserId, humanified);

            return Ok();
        }

    }
}
