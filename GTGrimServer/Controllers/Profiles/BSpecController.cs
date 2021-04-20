using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

using GTGrimServer.Config;
using GTGrimServer.Services;
using GTGrimServer.Models;
using GTGrimServer.Models.Xml;
using GTGrimServer.Filters;

namespace GTGrimServer.Controllers
{
    /// <summary>
    /// Remote B-Spec handler.
    /// </summary>
    [ApiController]
    [PDIClient]
    [Authorize]
    [Route("/ap/[controller]")]
    [Produces("application/xml")]
    public class BSpecController : GrimControllerBase
    {
        private readonly ILogger<BSpecController> _logger;
        private readonly GameServerOptions _gsOptions;

        public BSpecController(PlayerManager players, IOptions<GameServerOptions> options, ILogger<BSpecController> logger)
            : base(players)
        {
            _logger = logger;
            _gsOptions = options.Value;
        }

        [HttpPost]
        public async Task<ActionResult> Post()
        {
            var player = Player;
            if (player is null)
            {
                _logger.LogWarning("Could not get current player for host {host}", Request.Host);
                return Unauthorized();
            }

            if (_gsOptions.GameType != GameType.GT5)
            {
                _logger.LogWarning("Got a bspec remote request on a GT6 server from host: {host}", Request.Host);
                return Unauthorized();
            }

            GrimRequest gRequest = await GrimRequest.Deserialize(Request.Body);
            if (gRequest is null)
            {
                // Handle
                var badReq = GrimResult.FromInt(-1);
                return BadRequest(badReq);
            }

            _logger.LogDebug("<- {command}", gRequest.Command);

            switch (gRequest.Command)
            {
                case "bspec.carlist":
                    return await OnRequestCarList(player, gRequest);
            }

            _logger.LogDebug("Received unimplemented bspec call: {command}", gRequest.Command);
            var res = GrimResult.FromInt(-1);
            return BadRequest(res);
        }

        private async Task<ActionResult> OnRequestCarList(Player player, GrimRequest request)
        {
            var result = new ItemBoxList();
            return Ok(result);
        }
    }
}