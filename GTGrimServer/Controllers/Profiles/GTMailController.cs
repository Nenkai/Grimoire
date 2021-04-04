using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

using GTGrimServer.Config;
using GTGrimServer.Utils;
using GTGrimServer.Models;

namespace GTGrimServer.Controllers
{
    /// <summary>
    /// Provides news to the player.
    /// </summary>
    [ApiController]
    [Route("/ap/[controller]")]
    [Produces("application/xml")]
    public class GTMailController : ControllerBase
    {
        private readonly ILogger<GTMailController> _logger;
        private readonly GameServerOptions _gameServerOptions;

        public GTMailController(IOptions<GameServerOptions> options, ILogger<GTMailController> logger)
        {
            _logger = logger;
            _gameServerOptions = options.Value;
        }

        [HttpPost]
        public async Task<ActionResult> Post()
        {
            GrimRequest requestReq = await GrimRequest.Deserialize(Request.Body);
            if (requestReq is null)
            {
                // Handle
                return null;
            }

            if (requestReq.Command.Equals("mail.getlist"))
            {
                return GetMail(requestReq);
            }
            else
                _logger.LogDebug($"Received unimplemented mail command: {requestReq.Command}");

            return Ok();
        }

        public ActionResult GetMail(GrimRequest request)
        {
            var result = new GTMail()
            {
                From = 0,
                To = 0,
                FromNickname = "-- from --",
                ToNickname = "-- to --",
                Body = "-- body --",
                MailId = 0,
                Subject = "-- subject --",
                CreateTime = DateTime.Now.ToRfc3339String()
            };

            return Ok(result);
        }
    }
}