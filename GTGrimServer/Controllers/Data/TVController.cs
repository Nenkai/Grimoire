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
    [Route("/")] // Images grab the /data/ endpoint rather than /data2/
    [Produces("application/xml")]
    public class TVController : ControllerBase
    {
        private readonly ILogger<TVController> _logger;
        private readonly GameServerOptions _gameServerOptions;

        public TVController(IOptions<GameServerOptions> options, ILogger<TVController> logger)
        {
            _logger = logger;
            _gameServerOptions = options.Value;
        }

        [HttpGet]
        [Route("/data2/[controller]/{server}/{region}/tv2_{fileRegion}_{arg}.xml")]
        public async Task GetTV(string server, string region, string fileRegion, string arg)
        {
            if (arg.Equals("root"))
                await GetCategoryRoot(server, region, fileRegion);
            else if (arg.StartsWith("l_") && arg.Length > 2 && int.TryParse(arg.AsSpan(2), out int listId))
            {
                await GetTVList(server, region, fileRegion, listId);
            }
            else if (arg.StartsWith("s_") && arg.Length > 2 && int.TryParse(arg.AsSpan(2), out int setId))
            {
                await GetTVSet(server, region, fileRegion, setId);
            }
            else if (arg.Length > 0 && int.TryParse(arg, out int itemId))
                await GetTVItem(server, region, fileRegion, itemId);
            else
            {
                // Handle issue
            }
        }

        [HttpGet]
        [Route("/data/[controller]/common/tv2_l_{type}_{fileId:int}.img")]
        public async Task GetTVListImage(string type, int fileId)
        {
            string tvListImageFile = $"data/tv/common/tv2_l_{type}_{fileId}.img";
            await this.SendFile(_gameServerOptions.XmlResourcePath, tvListImageFile);
        }

        public async Task GetCategoryRoot(string server, string region, string region2)
        {
            string tvRootFile = $"data2/tv/{server}/{region}/tv2_{region2}_root.xml";
            await this.SendFile(_gameServerOptions.XmlResourcePath, tvRootFile);
        }

        public async Task GetTVList(string server, string region, string fileRegion, int listId)
        {
            string tvListFile = $"data2/tv/{server}/{region}/tv2_{fileRegion}_l_{listId}.xml";
            await this.SendFile(_gameServerOptions.XmlResourcePath, tvListFile);
        }

        public async Task GetTVSet(string server, string region, string fileRegion, int setId)
        {
            string tvSetFile = $"data2/tv/{server}/{region}/tv2_{fileRegion}_s_{setId}.xml";
            await this.SendFile(_gameServerOptions.XmlResourcePath, tvSetFile);
        }

        public async Task GetTVItem(string server, string region, string fileRegion, int itemId)
        {
            string tvItemFile = $"data2/tv/{server}/{region}/tv2_{fileRegion}_{itemId}.xml";
            await this.SendFile(_gameServerOptions.XmlResourcePath, tvItemFile);
        }

    }
}
