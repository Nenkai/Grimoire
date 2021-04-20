using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
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
using GTGrimServer.Filters;
using GTGrimServer.Models.Xml;

namespace GTGrimServer.Controllers
{
    /// <summary>
    /// Provides news to the player.
    /// </summary>
    [ApiController]
    [PDIClient]
    [Authorize]
    [Route("/ap/[controller]")]
    [Produces("application/xml")]
    public class GTMailController : ControllerBase
    {
        private readonly ILogger<GTMailController> _logger;
        private readonly GameServerOptions _gsOptions;

        public GTMailController(IOptions<GameServerOptions> options, ILogger<GTMailController> logger)
        {
            _logger = logger;
            _gsOptions = options.Value;
        }

        [HttpPost]
        public async Task<ActionResult> Post()
        {
            GrimRequest requestReq = await GrimRequest.Deserialize(Request.Body);
            if (requestReq is null)
                return BadRequest();

            switch (requestReq.Command)
            {
                case "mail.getlist":
                    return OnGetMail(requestReq);
                case "mail.send":
                    return OnSendMail(requestReq);
            }

            _logger.LogDebug($"Received unimplemented mail command: {requestReq.Command}");

            return BadRequest();
        }

        private ActionResult OnGetMail(GrimRequest request)
        {
            if (!request.TryGetParameterByKey("mail_id", out var mailId))
            {
                _logger.LogWarning($"Got get mail request with missing 'mail_id' parameter");
                return BadRequest();
            }

            if (!request.TryGetParameterByKey("by", out var sortType))
            {
                _logger.LogWarning($"Got get mail request with missing 'by' parameter");
                return BadRequest();
            }

            var result = new Mail()
            {
                FromUsername = "PSN_Name_Author",
                ToUsername = "-- PSN_Name_Destination",
                FromNickname = "-- from nickname --",
                ToNickname = "-- to nickname --",
                Body = "-- body --",
                MailId = 0,
                Subject = "-- subject --",
                CreateTime = DateTime.Now
            };

            return Ok(result);
        }

        private ActionResult OnSendMail(GrimRequest request)
        {
            if (!request.TryGetParameterByKey("to", out var toParam))
            {
                _logger.LogWarning($"Got get mail send request with missing 'to' parameter");
                return BadRequest();
            }
            else if (!request.TryGetParameterByKey("subject", out var subjectParam))
            {
                _logger.LogWarning($"Got get mail send request with missing 'subject' parameter");
                return BadRequest();
            }
            else if (!request.TryGetParameterByKey("body", out var bodyParam))
            {
                _logger.LogWarning($"Got get mail send request with missing 'body' parameter");
                return BadRequest();
            }
            else if (!request.TryGetParameterByKey("mail_id", out var mailIdParam))
            {
                _logger.LogWarning($"Got get mail send request with missing 'mail_id' parameter");
                return BadRequest();
            }

            // Mail list with 1 is sent
            var mailList = new MailList()
            {
                Mails = new List<Mail>()
                {
                    new Mail(),
                }
            };

            return Ok();
        }
    }
}