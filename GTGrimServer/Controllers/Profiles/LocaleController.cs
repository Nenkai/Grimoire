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

using GTGrimServer.Utils;
using GTGrimServer.Sony;
using GTGrimServer.Models.Xml;
using GTGrimServer.Config;
using GTGrimServer.Filters;

namespace GTGrimServer.Helpers
{
    /// <summary>
    /// Handles logging made by the game for the server to keep track of what the player is doing.
    /// </summary>
    [ApiController]
    [PDIClient]
    [Route("/ap/locale")]
    [Produces("application/xml")]
    public class LocaleController : ControllerBase
    {
        private readonly ILogger<LocaleController> _logger;
        private readonly GameServerOptions _gsOptions;

        public LocaleController(IOptions<GameServerOptions> options, ILogger<LocaleController> logger)
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
                case "servertime.get":
                    return OnGetServerTime(requestReq);
                case "language.set":
                    return OnSetLanguage(requestReq);
            }

            _logger.LogDebug("Received unimplemented locale command: {command}", requestReq.Command);

            return BadRequest();
        }

        private ActionResult OnGetServerTime(GrimRequest gRequest)
        { 
            if (_gsOptions.GameType != GameType.GT5)
            {
                _logger.LogWarning("Got servertime.get request for non GT5");
                return BadRequest();
            }

            var result = GrimResult.FromDateTimeRfc3339(DateTime.Now);
            return Ok(result);
        }

        private ActionResult OnSetLanguage(GrimRequest gRequest)
        {
            if (!gRequest.TryGetParameterByKey("language", out GrimRequestParam param))
            {
                _logger.LogWarning("Got missing language parameter for language.set");
                return BadRequest();
            }

            return Ok(GrimResult.FromString(param.Text.ToLower()));
        }

    }
}
