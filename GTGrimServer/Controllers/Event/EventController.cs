using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.IO;

using GTGrimServer.Sony;
using GTGrimServer.Utils;
using GTGrimServer.Config;

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
        private readonly GameServerOptions _gameServerOptions;

        public EventController(IOptions<GameServerOptions> options, ILogger<EventController> logger)
        {
            _logger = logger;
            _gameServerOptions = options.Value;
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

            string eventListFile = $"event/{server}/{fileName}";
            await this.SendFile(_gameServerOptions.XmlResourcePath, eventListFile);
        }

        [HttpGet]
        [Route("{server}/setting.xml")]
        public async Task GetSettings(string server)
        {
            string settingsFile = $"event/{server}/setting.xml";
            await this.SendFile(_gameServerOptions.XmlResourcePath, settingsFile);
        }


        [HttpGet]
        [Route("{server}/event_list.xml")]
        public async Task GetOnlineEventList(string server)
        {
            string eventListFile;
            if (_gameServerOptions.GameType == "GT5")
                eventListFile = $"event/{server}/event_list_gt5.xml";
            else
                eventListFile = $"event/{server}/event_list.xml";

            await this.SendFile(_gameServerOptions.XmlResourcePath, eventListFile);
            
        }

        [HttpGet]
        [Route("{server}/event_{folderId:int}.xml")]
        public async Task GetOnlineEvent(string server, int folderId)
        {
            string eventFile = $"event/{server}/event_{folderId}.xml";
            await this.SendFile(_gameServerOptions.XmlResourcePath, eventFile);
        }

    }
}
