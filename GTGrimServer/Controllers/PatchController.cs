using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

using GTGrimServer.Filters;
using GTGrimServer.Config;
using GTGrimServer.Utils;

namespace GTGrimServer.Controllers
{
    [ApiController]
    [Route("/[controller]/")]
    [Produces("application/xml")]
    [PDIClient]
    public class PatchController : ControllerBase
    {
        private readonly ILogger<PatchController> _logger;
        private readonly GameServerOptions _gameServerOptions;

        public PatchController(IOptions<GameServerOptions> options, ILogger<PatchController> logger)
        {
            _logger = logger;
            _gameServerOptions = options.Value;
        }

        [HttpGet]
        [Route("patchinfo.xml")]
        public async Task Get(string region)
        {
            string serverListFile = "patch/patchinfo.xml";
            await this.SendFile(_gameServerOptions.XmlResourcePath, serverListFile);
        }
    }
}
