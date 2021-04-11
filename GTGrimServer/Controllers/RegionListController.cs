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
    [PDIClient]
    [Route("/init/regionlist.xml")]
    [Produces("application/xml")]
    public class RegionListController : ControllerBase
    {
        private readonly ILogger<RegionListController> _logger;
        private readonly GameServerOptions _gameServerOptions;

        public RegionListController(IOptions<GameServerOptions> options, ILogger<RegionListController> logger)
        {
            _logger = logger;
            _gameServerOptions = options.Value;
        }

        [HttpGet]
        public async Task Get()
        {
            string regionListFile = "regionlist.xml";
            await this.SendFile(_gameServerOptions.XmlResourcePath, regionListFile);
        }
    }
}
