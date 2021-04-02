using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.IO;

using GTGrimServer.Utils;
using GTGrimServer.Sony;
using GTGrimServer.Models;

namespace GTGrimServer.Helpers
{
    /// <summary>
    /// Handles logging made by the game for the server to keep track of what the player is doing.
    /// </summary>
    [ApiController]
    [Route("/ap/locale")]
    [Produces("application/xml")]
    public class LocaleController : ControllerBase
    {
        private readonly ILogger<LocaleController> _logger;

        public LocaleController(ILogger<LocaleController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<GrimResult> Post()
        {
            GrimRequest requestReq = await GrimRequest.Deserialize(Request.Body);
            if (requestReq is null)
            {
                // Handle
                return null;
            }

            if (requestReq.Command.Equals("servertime.get"))
                return GetServerTime();
            else if (requestReq.Command.Equals("language.set"))
            {
                return ProcessLanguage(requestReq);
            }
                

            return null;
        }

        private GrimResult GetServerTime()
            => GrimResult.FromString(DateTime.Now.ToRfc3339String());

        private GrimResult ProcessLanguage(GrimRequest gRequest)
        {
            if (!gRequest.TryGetParameterByKey("language", out GrimRequestParam param))
                return null;

            return GrimResult.FromString(param.Text.ToLower());
        }

    }
}
