using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
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
    /// <summary>
    /// Provides news to the player.
    /// </summary>
    [ApiController]
    [Authorize]
    [PDIClient]
    [Route("/used_car/")]
    [Produces("application/xml")]
    public class UsedCarDealershipController : ControllerBase
    {
        private readonly ILogger<UsedCarDealershipController> _logger;
        private readonly GameServerOptions _gameServerOptions;

        public UsedCarDealershipController(IOptions<GameServerOptions> options, ILogger<UsedCarDealershipController> logger)
        {
            _logger = logger;
            _gameServerOptions = options.Value;
        }

        [HttpGet]
        [Route("{server}/used_car_list.xml")]
        public async Task Get(string server)
        {
            string usedCarListFile = $"used_car/{server}/used_car_list.xml";
            await this.SendFile(_gameServerOptions.XmlResourcePath, usedCarListFile);
        }
    }
}
