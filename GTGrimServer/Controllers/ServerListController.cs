using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

using GTGrimServer.Config;
using GTGrimServer.Utils;

namespace GTGrimServer.Controllers
{
    [ApiController]
    [Route("/init/{region}/serverlist.xml")]
    [Produces("application/xml")]
    public class ServerListController : ControllerBase
    {
        private readonly ILogger<ServerListController> _logger;
        private readonly GameServerOptions _gameServerOptions;

        public ServerListController(IOptions<GameServerOptions> options, ILogger<ServerListController> logger)
        {
            _logger = logger;
            _gameServerOptions = options.Value;
        }

        [HttpGet]
        public async Task Get(string region)
        {
            string serverListFile = region == "_default" ? "serverlist.xml" : $"{region}/serverlist.xml";
            await this.SendFile(_gameServerOptions.XmlResourcePath, serverListFile);
        }
    }
}
