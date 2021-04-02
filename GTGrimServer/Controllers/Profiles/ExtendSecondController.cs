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
    /// Handles requests made by the game to extend the current session.
    /// </summary>
    // Refer to server option 'online.extendsession.interval' for the interval between session extension requests
    [ApiController]
    [Route("/ap/misc/extend/")]
    [Produces("application/xml")]
    public class ExtendSessionController : ControllerBase
    {
        private readonly ILogger<ExtendSessionController> _logger;

        public ExtendSessionController(ILogger<ExtendSessionController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public GrimResult Post()
        {
            _logger.LogInformation($"Got session extend request");
            return GrimResult.FromInt(1);
        }

    }
}
