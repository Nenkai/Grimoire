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

namespace GTGrimServer.Controllers
{
    [ApiController]
    [Route("/ap/ticket/login/{pfsVersion}/")]
    [Produces("application/xml")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(ILogger<AuthenticationController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<TicketResult> Post(ulong pfsVersion)
        {
            _logger.LogDebug("Got auth request ticket with PFS version: {pfsVersion}", pfsVersion);

            if (Request.ContentLength == 0)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return null;
            }

            byte[] buf = new byte[(int)Request.ContentLength];
            await Request.Body.ReadAsync(buf, 0, buf.Length);

            Ticket ticket = Ticket.FromBuffer(buf);

            var resp = new TicketResult()
            {
                Result = "1",
                Nickname = "Nenkai",
                UserId = ticket.UserId,
                UserNumber = "0",
                ServerTime = DateTime.Now,
            };

            return resp;
        }

        [XmlRoot("result")]
        public class TicketResult
        {
            /// <summary>
            /// Whether it succeeded.
            /// </summary>
            [XmlElement(ElementName = "result")]
            public string Result { get; set; }

            /// <summary>
            /// PSN User ID.
            /// </summary>
            [XmlElement(ElementName = "user_id")]
            public ulong UserId { get; set; }

            /// <summary>
            /// Nickname.
            /// </summary>
            [XmlElement(ElementName = "nickname")]
            public string Nickname { get; set; }

            /// <summary>
            /// User Number.
            /// </summary>
            [XmlElement(ElementName = "user_no")]
            public string UserNumber { get; set; }

            /// <summary>
            /// Server Time.
            /// </summary>
            [XmlElement(ElementName = "server_time")]
            public DateTime ServerTime { get; set; }
        }
    }
}
