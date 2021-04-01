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
    [Route("/init/regionlist.xml")]
    [Produces("application/xml")]
    public class RegionListController : ControllerBase
    {
        private readonly ILogger<RegionListController> _logger;

        public RegionListController(ILogger<RegionListController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task Get()
        {
            string regionListFile = "Resources/regionlist.xml";
            if (!System.IO.File.Exists(regionListFile))
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            using var fs = System.IO.File.OpenRead(regionListFile);
            await fs.CopyToAsync(Response.Body);
        }
    }
}
