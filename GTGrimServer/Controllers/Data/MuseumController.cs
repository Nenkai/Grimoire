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
    [Route("/")]
    [PDIClient]
    [Produces("application/xml")]
    public class MuseumController : ControllerBase
    {
        private readonly ILogger<MuseumController> _logger;
        private readonly GameServerOptions _gameServerOptions;

        public MuseumController(IOptions<GameServerOptions> options, ILogger<MuseumController> logger)
        {
            _logger = logger;
            _gameServerOptions = options.Value;
        }

        [HttpGet]
        [Route("/data2/[controller]/{server}/{region}/museum_{fileRegion}_{arg}.xml")]
        public async Task GetMuseum(string server, string region, string fileRegion, string arg)
        {
            if (arg.StartsWith("l_") && arg.Length > 2 && int.TryParse(arg.AsSpan(2), out int listId))
                await GetMuseumList(server, region, fileRegion, listId);
            else if (arg.Length > 0 && int.TryParse(arg, out int itemId))
                await GetMuseumItem(server, region, fileRegion, itemId);
            else
            {
                // Handle issue
            }
            
        }

        [HttpGet]
        [Route("/data/[controller]/{server}/{region}/museum_{fileRegion}_{imageId:int}.img")]
        public void GetMuseumImage(string server, string region, string fileRegion, int imageId)
        {
            return;
        }

        private async Task GetMuseumList(string server, string region, string fileRegion, int listId)
        {
            string museumListFile = $"data2/museum/{server}/{region}/museum_{fileRegion}_l_{listId}.xml";
            await this.SendFile(_gameServerOptions.XmlResourcePath, museumListFile);
        }

        public async Task GetMuseumItem(string server, string region, string fileRegion, int itemId)
        {
            string museumItemFile = $"data2/museum/{server}/{region}/museum_{fileRegion}_{itemId}.xml";
            await this.SendFile(_gameServerOptions.XmlResourcePath, museumItemFile);
        }
    }
}
