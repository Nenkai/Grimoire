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
    /// Provides a greeting card on logon to the player if provided. (GT5 only).
    /// </summary>
    [ApiController]
    [Authorize]
    [PDIClient]
    [Route("/[controller]/")]
    [Produces("application/xml")]
    public class GreetingController : ControllerBase
    {
        private readonly ILogger<GreetingController> _logger;
        private readonly GameServerOptions _gameServerOptions;

        public GreetingController(IOptions<GameServerOptions> options, ILogger<GreetingController> logger)
        {
            _logger = logger;
            _gameServerOptions = options.Value;
        }

        [HttpGet]
        [Route("{server}/{region}/greeting_list.xml")]
        public async Task GetGreetingList(string server, string region)
        {
            string greetingListFile = $"greeting/{server}/{region}/greeting_list.xml";
            await this.SendFile(_gameServerOptions.XmlResourcePath, greetingListFile);
        }

        [HttpGet]
        [Route("{server}/{region}/{imageName}.img")]
        public async Task GetGreetingCardImage(string server, string region, string imageName)
        {
            string greetingImageFile = $"greeting/{server}/{region}/{imageName}.img";
            await this.SendFile(_gameServerOptions.XmlResourcePath, greetingImageFile);
        }
    }
}
