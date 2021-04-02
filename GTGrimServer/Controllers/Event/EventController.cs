using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.IO;

using GTGrimServer.Sony;

namespace GTGrimServer.Helpers
{
    /// <summary>
    /// Handles logging made by the game for the server to keep track of what the player is doing.
    /// </summary>
    [ApiController]
    [Route("/[controller]")]
    [Produces("application/xml")]
    public class EventController : ControllerBase
    {
        private readonly ILogger<EventController> _logger;

        public EventController(ILogger<EventController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("{server}/{fileName}")]
        public async Task GetImageFile(string server, string fileName)
        {
            if (fileName.EndsWith(".png") || fileName.EndsWith(".img"))
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            string eventListFile = $"Resources/event/{server}/{fileName}";
            if (!System.IO.File.Exists(eventListFile))
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            using var fs = System.IO.File.OpenRead(eventListFile);
            await fs.CopyToAsync(Response.Body);
        }

        [HttpGet]
        [Route("{server}/setting.xml")]
        public async Task GetSettings(string server)
        {
            string settingsFile = $"Resources/event/{server}/setting.xml";
            if (!System.IO.File.Exists(settingsFile))
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            using var fs = System.IO.File.OpenRead(settingsFile);
            await fs.CopyToAsync(Response.Body);
        }


        [HttpGet]
        [Route("{server}/event_list.xml")]
        public async Task GetOnlineEventList(string server)
        {
            string eventListFile = $"Resources/event/{server}/event_list.xml";
            if (!System.IO.File.Exists(eventListFile))
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            using var fs = System.IO.File.OpenRead(eventListFile);
            await fs.CopyToAsync(Response.Body);
        }

        [HttpGet]
        [Route("{server}/event_{folderId:int}.xml")]
        public async Task GetOnlineEvent(string server, int folderId)
        {
            string eventFile = $"Resources/event/{server}/event_{folderId}.xml";
            if (!System.IO.File.Exists(eventFile))
            {
                // Note: The game will try 5 times, if missing
                Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            using var fs = System.IO.File.OpenRead(eventFile);
            await fs.CopyToAsync(Response.Body);
        }

    }
}
