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
    /// <summary>
    /// Provides news to the player.
    /// </summary>
    [ApiController]
    [Route("/[controller]")]
    [Produces("application/xml")]
    public class NewsController : ControllerBase
    {
        private readonly ILogger<NewsController> _logger;

        public NewsController(ILogger<NewsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("{serverId}/{region}/{fileId:int}.xml")]
        public async Task GetNews(string serverId, string region, int fileId)
        {
            string newsFile = $"Resources/news/{serverId}/{region}/{fileId}.xml";
            if (!System.IO.File.Exists(newsFile))
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            using var fs = System.IO.File.OpenRead(newsFile);
            await fs.CopyToAsync(Response.Body);
        }

        [HttpGet]
        [Route("{serverId}/{region}/l{category_id1:int}_{category_id2:int}.xml")]
        public async Task GetNewsList(string serverId, string region, int category_id1, int category_id2)
        {
            string newsFile = $"Resources/news/{serverId}/{region}/l{category_id1}_{category_id2}.xml";
            if (!System.IO.File.Exists(newsFile))
            {
                // Note: The game will try 5 times, if missing
                Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            using var fs = System.IO.File.OpenRead(newsFile);
            await fs.CopyToAsync(Response.Body);
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
            string newsFile = $"Resources/news/{serverId}/{region}/root.xml";
            if (!System.IO.File.Exists(newsFile))
            {
                // Note: The game will try 5 times, if missing
                Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            using var fs = System.IO.File.OpenRead(newsFile);
            await fs.CopyToAsync(Response.Body);
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
            string newsFile = $"Resources/news/common/{newsId}/{imageId}.jpg";
            if (!System.IO.File.Exists(newsFile))
            {
                // Note: The game will try 5 times, if missing
                Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            using var fs = System.IO.File.OpenRead(newsFile);
            await fs.CopyToAsync(Response.Body);
        }
    }
}
