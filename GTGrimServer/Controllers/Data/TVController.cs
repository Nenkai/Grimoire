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
    [Route("/data2/[controller]")]
    [Produces("application/xml")]
    public class TVController : ControllerBase
    {
        private readonly ILogger<TVController> _logger;

        public TVController(ILogger<TVController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        [Route("{server}/{region}/tv2_{fileRegion}_{arg}.xml")]
        public async Task GetTV(string server, string region, string fileRegion, string arg)
        {
            if (arg.Equals("root"))
                await GetCategoryRoot(server, region, fileRegion);
            else if (arg.StartsWith("l_") && arg.Length > 2 && int.TryParse(arg.AsSpan(2), out int listId))
                await GetTVList(server, region, fileRegion, listId);
            else if (arg.Length > 0 && int.TryParse(arg, out int itemId))
                await GetTVItem(server, region, fileRegion, itemId);
            else
            {
                // Handle issue
            }
        }

        public async Task GetCategoryRoot(string server, string region, string region2)
        {
            string tvListFile = $"Resources/data2/tv/{server}/{region}/tv2_{region2}_root.xml";
            if (!System.IO.File.Exists(tvListFile))
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            using var fs = System.IO.File.OpenRead(tvListFile);
            await fs.CopyToAsync(Response.Body);

        }

        public async Task GetTVList(string server, string region, string fileRegion, int listId)
        {
            string tvListFile = $"Resources/data2/tv/{server}/{region}/tv2_{fileRegion}_l_{listId}.xml";
            if (!System.IO.File.Exists(tvListFile))
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            using var fs = System.IO.File.OpenRead(tvListFile);
            await fs.CopyToAsync(Response.Body);
        }

        public async Task GetTVItem(string server, string region, string fileRegion, int itemId)
        {
            string tvItemFile = $"Resources/data2/tv/{server}/{region}/tv2_{fileRegion}_{itemId}.xml";
            if (!System.IO.File.Exists(tvItemFile))
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            using var fs = System.IO.File.OpenRead(tvItemFile);
            await fs.CopyToAsync(Response.Body);
        }
    }
}
