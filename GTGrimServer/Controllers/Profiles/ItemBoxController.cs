using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
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
using GTGrimServer.Utils;
using GTGrimServer.Models;
using GTGrimServer.Filters;

namespace GTGrimServer.Controllers
{
    /// <summary>
    /// Provides news to the player.
    /// </summary>
    [ApiController]
    [PDIClient]
    [Route("/ap/[controller]")]
    [Produces("application/xml")]
    public class ItemBoxController : ControllerBase
    {
        private readonly ILogger<ItemBoxController> _logger;
        private readonly GameServerOptions _gsOptions;

        public ItemBoxController(IOptions<GameServerOptions> options, ILogger<ItemBoxController> logger)
        {
            _logger = logger;
            _gsOptions = options.Value;
        }

        [HttpPost]
        public async Task<ActionResult> Post()
        {
            GrimRequest requestReq = await GrimRequest.Deserialize(Request.Body);
            if (requestReq is null)
                return BadRequest();

            switch (requestReq.Command)
            {
                case "itembox.getlist":
                    return OnGetItemList(requestReq);
            }

            _logger.LogDebug("Received unimplemented itembox command: {command}", requestReq.Command);

            return BadRequest();
        }

        public ActionResult OnGetItemList(GrimRequest request)
        {
            var result = new ItemBoxList();
            return Ok(result);
        }
    }
}