using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.IO;

namespace GTGrimServer.Controllers
{
    [ApiController]
    [Route("/init/{region}/serverlist.xml")]
    [Produces("application/xml")]
    public class ServerListController : ControllerBase
    {
        private readonly ILogger<ServerListController> _logger;

        public ServerListController(ILogger<ServerListController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task Get(string region)
        {
            string serverListFile = region == "_default" ? "Resources/serverlist.xml" : $"Resources/{region}/serverlist.xml";
            if (!System.IO.File.Exists(serverListFile))
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            using var fs = System.IO.File.OpenRead(serverListFile);
            await fs.CopyToAsync(Response.Body);
        }
    }
}
