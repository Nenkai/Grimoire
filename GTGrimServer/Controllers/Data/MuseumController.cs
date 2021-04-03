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
    public class MuseumController : ControllerBase
    {
        private readonly ILogger<MuseumController> _logger;

        public MuseumController(ILogger<MuseumController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("{server}/{region}/museum_{fileRegion}_l_{listId}.xml")]
        public async Task GetMuseumList(string server, string region, string fileRegion, int listId)
        {
            string museumListFile = $"Resources/data2/museum/{server}/{region}/museum_{fileRegion}_l_{listId}.xml";
            if (!System.IO.File.Exists(museumListFile))
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            using var fs = System.IO.File.OpenRead(museumListFile);
            await fs.CopyToAsync(Response.Body);
        }
    }
}
