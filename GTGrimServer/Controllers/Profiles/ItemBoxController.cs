using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.AspNetCore.Mvc.Formatters.Xml.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

using GTGrimServer.Config;
using GTGrimServer.Utils;
using GTGrimServer.Models;

namespace GTGrimServer.Controllers
{
    /// <summary>
    /// Provides news to the player.
    /// </summary>
    [ApiController]
    [Route("/ap/[controller]")]
    [Produces("application/xml")]
    public class ItemBoxController : ControllerBase
    {
        private readonly ILogger<ItemBoxController> _logger;
        private readonly GameServerOptions _gameServerOptions;

        public ItemBoxController(IOptions<GameServerOptions> options, ILogger<ItemBoxController> logger)
        {
            _logger = logger;
            _gameServerOptions = options.Value;
        }

        [HttpPost]
        public async Task<ActionResult> Post()
        {
            GrimRequest requestReq = await GrimRequest.Deserialize(Request.Body);
            if (requestReq is null)
            {
                // Handle
                return null;
            }

            if (requestReq.Command.Equals("itembox.getlist"))
            {
                return GetItemList(requestReq);
            }
            else
                _logger.LogDebug($"Received unimplemented itembox command: {requestReq.Command}");

            return Ok();
        }

        public ActionResult GetItemList(GrimRequest request)
        {
            var result = new ItemBoxList();
            return new XmlResult(result);
        }
    }
}