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
    /// <summary>
    /// Provides news to the player.
    /// </summary>
    [ApiController]
    [Route("/[controller]")]
    [Produces("application/xml")]
    public class NewsController : ControllerBase
    {
        private readonly ILogger<NewsController> _logger;
        private readonly GameServerOptions _gameServerOptions;

        public NewsController(IOptions<GameServerOptions> options, ILogger<NewsController> logger)
        {
            _logger = logger;
            _gameServerOptions = options.Value;
        }

        [HttpGet]
        [Route("{serverId}/{region}/{fileId:int}.xml")]
        public async Task GetNews(string serverId, string region, int fileId)
        {
            string newsFile = $"/news/{serverId}/{region}/{fileId}.xml";
            await this.SendFile(_gameServerOptions.XmlResourcePath, newsFile);
        }

        [HttpGet]
        [Route("{serverId}/{region}/l{category_id1:int}_{category_id2:int}.xml")]
        public async Task GetNewsList(string serverId, string region, int category_id1, int category_id2)
        {
            string newsListFile = $"/news/{serverId}/{region}/l{category_id1}_{category_id2}.xml";
            await this.SendFile(_gameServerOptions.XmlResourcePath, newsListFile);
        }

        /// <summary>
        /// For GT5
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{serverId}/{region}/root.xml")]
        public async Task GetCategoryRoot(string serverId, string region)
        {
            string newsCategoryRootFile = $"/news/{serverId}/{region}/root.xml";

            // Note: The game will try 5 times, if missing
            await this.SendFile(_gameServerOptions.XmlResourcePath, newsCategoryRootFile);
        }

        /// <summary>
        /// For GT5
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("common/{newsId}/{imageId}.jpg")]
        public async Task GetNewsImage(int newsId, int imageId)
        {
            string newsFile = $"/news/common/{newsId}/{imageId}.jpg";
            await this.SendFile(_gameServerOptions.XmlResourcePath, newsFile);
        }
    }
}
